using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoicevoxBridge;
using System.Threading.Tasks;

public class MainManager : MonoBehaviour
{

    public JSONmanager _JSON;
    public Voicemanager _Voice;
    public BlackBoardManager _BB;
    public Receiver _receiver;

    private LessonDataStorage _storage = new LessonDataStorage();
    private VoiceDataStorage _Vstorage = new VoiceDataStorage();
    private QuestionSceneVoiceStorage _QVstorage = new QuestionSceneVoiceStorage();

    [SerializeField] VOICEVOX voicevox;///VOICEVOX�X�N���v�g�A�^�b�`�����I�u�W�F�N�g������

    public GameObject _TextNode;/// �������鎚��Prefab�̂��߂̕ϐ��錾
    public GameObject _Text_parent;///textnode�̐e�v�f�p�ϐ�
    public Scrollbar _ScrollBar;

    private List<Texture2D> textureList;///�X���C�h�摜���X�g

    private int CurrentSectionIndex=0;///main���[�v���K�肷��C���f�b�N�X
    //bool isPlayingSection = false;

    



    void Start()
    {
        textureList=_BB.GetTextureList();
        _storage.StoreSpeech(_JSON.LoadSpeech());

        Generate_AllSpeechVoice();///�S���b�̉��������J�n
        _ScrollBar = _ScrollBar.GetComponent<Scrollbar>();
        _ScrollBar.value = 1;
    }

    void Update()
    {
        _ScrollBar.value = 0;
    }

    async Task Generate_AllSpeechVoice()
    {
        // fQuestionIntrotext �̂��ׂĂ̗v�f�ɑ΂��ď������s��
        for (int i = 0; i < _QVstorage.IntroText.Length; i++)
        {
            string txt = _QVstorage.IntroText[i];
            // �����𐶐����A�ۑ�
            Voice voice = await _Voice.CreateOneVoice(txt);
            _QVstorage.StoreIntroVoice(voice);
        }
        Debug.Log("����R�[�i�[����������������");

        int index = 0;
        while (true)
        {
            // _storage.GetLessonData(index) �� null �Ȃ烋�[�v�I��
            var lessonData = _storage.GetLessonData(index);
            if (lessonData == null)
            {
                break;
            }

            // �����𐶐����A�ۑ�
            List<Voice> voices = await _Voice.CreateVoiceDate(index, lessonData);
            _Vstorage.StoreVoicelist(index, voices);
            // �C���f�b�N�X�𑝂₷
            index++;
        }

        Debug.Log("�����S��������");
    }




    public async Task play_section(int index)
    {///�P�Z�N�V�������Đ����郋�[�v

       
        List<string> SplitText = _storage.GetLessonData(index);
        List<Voice> CurrentVoices = _Vstorage.GetSectionVoiceList(index);

        ///�Z�N�V�����̔��b���e���ׂ�ׂ璝���
        int num = 0;
        while (num < CurrentVoices.Count)
        {
            _BB.UpdateBlackBoard(index+1, textureList);///�X���C�h�̍X�V(�X���C�h�͂P������������Ă�̂Œ���!)
            Create_captions(SplitText[num]);///��������
            await voicevox.Play(CurrentVoices[num]);///�����Đ�
            await Task.Delay(500);
            num++;


         ///�����܂�
        }
        ///����R�[�i�[
        await QuestionScene();

        await Task.Delay(2500); // 5000�~���b�ҋ@
        Debug.Log("�Z�N�V�����I��");

        _receiver.StartTrackingSection();
    }


    /// <summary>
    /// �񓚕���Split����@�������Đ�����
    /// </summary>
    /// <returns></returns>
    private async Task QuestionScene()
    {

        for (int i = 0; i < 3; i++)
        {
            int n = _receiver.RandomID_ThisSection();
            string responstxt = _receiver.GetMessage(n, true);
            Debug.Log(responstxt);

            if (!string.IsNullOrEmpty(responstxt))
            {
                Create_captions(responstxt);
                await voicevox.PlayOneShot(_Voice.speaker, responstxt);
            }
            else
            {
                Debug.LogError("Voice is null for ID: " + n);
            }
        }
    }

    private void Create_captions(string text)
    {
        Destroy_Node();
        GameObject instantiatedNode_Prefab = Instantiate(_TextNode, _Text_parent.transform);///Prefab����
        Transform parent01Transform = instantiatedNode_Prefab.transform;///��������Prefab��Text�̐e�v�f�Ȃ̂ŁA�e��Transform�Ƃ��Ď擾
        Transform parent02Transform = parent01Transform.Find("Board");
        Transform childTransform = parent02Transform.Find("Text");

        TextMeshProUGUI _blackboard_Node = childTransform.GetComponent<TextMeshProUGUI>();

        string addtext = text.Replace("�u", "").Replace("�v", "").Replace("\\n", "");

        // addtext����łȂ����`�F�b�N���Ă���ǉ�
        if (!string.IsNullOrEmpty(addtext))
        {
            _blackboard_Node.text = addtext;
        }       
    }
    private void Destroy_Node()
    {
        foreach (Transform child in _Text_parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Main()
    {
        Mainroop();
    }


    public async Task Mainroop()
    {
        while (CurrentSectionIndex < _JSON.jsondata.Length)
        {
            await play_section(CurrentSectionIndex);
            CurrentSectionIndex++;
        }
    }
}


