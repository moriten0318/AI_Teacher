using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoicevoxBridge;
using System.Threading.Tasks;
using System.Linq;

public class MainManager : MonoBehaviour
{

    public JSONmanager _JSON;
    public Voicemanager _Voice;
    public BlackBoardManager _BB;
    public Receiver _receiver;
    [SerializeField] private ButtonClick bottun;
    [SerializeField] private Blackboard _header;

    private LessonDataStorage _storage = new LessonDataStorage();
    private VoiceDataStorage _Vstorage = new VoiceDataStorage();
    public ResponceMessageStorage _RMStorage = new ResponceMessageStorage();
    public ResponceVoiceStorage _RVStorage = new ResponceVoiceStorage();

    [SerializeField] VOICEVOX voicevox;///VOICEVOX�X�N���v�g�A�^�b�`�����I�u�W�F�N�g������

    public GameObject _TextNode;/// �������鎚��Prefab�̂��߂̕ϐ��錾
    public GameObject _Text_parent;///textnode�̐e�v�f�p�ϐ�
    public Scrollbar _ScrollBar;

    private List<Texture2D> textureList;///�X���C�h�摜���X�g

    private int CurrentSectionIndex=0;///main���[�v���K�肷��C���f�b�N�X
    //bool isPlayingSection = false;

    private int AnswerTimes = 3;//�P��̃Z�N�V�����Ŏ���ɓ������

   

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
        // QuestionIntrotext �̂��ׂĂ̗v�f�ɑ΂��ď������s��
        for (int i = 0; i < _RVStorage.IntroText.Count; i++)
        {
            string txt = _RVStorage.IntroText[i];
            // �����𐶐����A�ۑ�
            Voice voice = await _Voice.CreateOneVoice(txt);
            _RVStorage.StoreIntroVoice(i,voice);
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

            if (index == 0)
            {///�ŏ��̉����������Ƀ{�^����true�ɂ���
                bottun.BottunToActive();
            }

            index++;
        }

        Debug.Log("�����S��������");
    }




    public async Task play_section(int index)
    {///�P�Z�N�V�������Đ����郋�[�v

        _header.Destroy_TextNode();
        List<string> SplitText = _storage.GetLessonData(index);
        List<Voice> CurrentVoices = _Vstorage.GetSectionVoiceList(index);

        ///�Z�N�V�����̔��b���e���ׂ�ׂ璝���
        int num = 0;
        while (num < CurrentVoices.Count)
        {
            _BB.UpdateBlackBoard(index+2, textureList);///�X���C�h�̍X�V(�X���C�h�͂P������������Ă�̂Œ���!)
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


    private async Task QuestionScene()
    {
        _BB.UpdateBlackBoard(1, textureList);

        for (int i = 0; i < _RVStorage.IntroText.Count; i++)
        {
            Create_captions(_RVStorage.IntroText[i]);
            Voice v = _RVStorage.GetIntroVoice(i);
            await voicevox.Play(v, autoReleaseVoice: false); // �����Đ�
        }

        for (int i = 0; i < AnswerTimes; i++)///�O�񎿖�ɓ�����
        {
            bool retry = false;
            do
            {
                //Debug.Log("���݂�i=" + i);
                try
                {
                    Debug.Log($"i={i}�̃��X�|���X��Ԃ��܂�");
                    int n = _receiver.RandomID_ThisSection();
                    Debug.Log($"RandomID_ThisSection����Ԃ��ꂽID: {n}");

                    // RandomID_ThisSection �� -1(�������鎿�₪�P���Ȃ�)��Ԃ����ꍇ�̏���
                    if (n == -1)
                    {
                        _header.Create_TextNode("�������t���c");
                        Debug.LogError("�L����ID��������܂���ł����B");
                        await Task.Delay(5000); // 3�b�ҋ@
                        Voice vo;
                        switch (i)
                        {
                            case 0:
                                vo = await voicevox.CreateVoice(20, "�����t���ł���H���₪�Ȃ��Ȃ��ɐi�񂶂Ⴂ�܂���H");
                                Create_captions("�����t���ł���H���₪�Ȃ��Ȃ��ɐi�񂶂Ⴂ�܂���H");
                                await voicevox.Play(vo);
                                break;
                            case 1:
                                vo = await voicevox.CreateVoice(20, "�R�����g�͂���܂��񂩁H���������҂��܂��傤���H");
                                Create_captions("�R�����g�͂���܂��񂩁H���������҂��܂��傤���H");
                                await voicevox.Play(vo);
                                break;
                            default:
                                vo = await voicevox.CreateVoice(20, "�₵���ł��ˁB���i�񂶂Ⴂ�܂���H");
                                Create_captions("�₵���ł��ˁB���i�񂶂Ⴂ�܂���H");
                                await voicevox.Play(vo);
                                break;
                        }
                        retry = false;
                        break; // �������X�L�b�v���Ď��̔����ɐi��
                    }
                    else 
                    {
                        while (!_receiver.CheckComplete(n))
                        {
                            // �������܂���������Ă��Ȃ��ꍇ
                            Debug.Log($"ID {n} �̉����͂܂���������Ă��܂���B�����ҋ@���܂��B");
                            await Task.Delay(3000); // 3�b�ҋ@
                        }

                        //����������
                        MessageData responceMessagedata = _RMStorage.GetMessageData(n, true);
                        if (responceMessagedata == null)
                        {
                            Debug.Log($"ID {n} ��MessageData��������܂���ł����B");
                            retry = false; // ���[�v���I��������
                            break; // ����try�u���b�N�̏������I�����Afor���[�v�̎��̔����ɐi��
                        }

                        string[] splittext = responceMessagedata.content.Split(char.Parse("�B"));
                        List<string> txtlist = splittext.ToList();
                        //Debug.Log($"�������ꂽ�e�L�X�g���X�g�̃T�C�Y: {txtlist.Count}");

                        List<Voice> voicelist = _RVStorage.GetResponceVoice(n);
                        //Debug.Log($"�擾���ꂽVoice���X�g�̃T�C�Y: {voicelist.Count}");

                        ///���╶��\������
                        MessageData Question = _RMStorage.GetMessageData(n, false);
                        _header.Create_TextNode(Question.content);

                        ///����ւ̉𓚂�\���A�Đ�����
                        // �e�L�X�g���X�g�Ɖ������X�g�̗v�f�����قȂ�ꍇ�͒�������
                        int minCount = Math.Min(txtlist.Count, voicelist.Count);

                        // j�̒l�����X�g�̃T�C�Y���Ɏ��܂��Ă��邱�Ƃ��m�F
                        for (int j = 0; j < minCount; j++)
                        {
                            //Debug.Log($"�e�L�X�g: {txtlist[j]}, Voice�C���f�b�N�X: {j}");
                            Create_captions(txtlist[j]); // ��������

                            // j��voicelist�̃T�C�Y���Ɏ��܂��Ă��邱�Ƃ��m�F
                            if (j < voicelist.Count)
                            {
                                await voicevox.Play(voicelist[j]); // �����Đ�
                            }
                            else
                            {
                                Debug.LogError($"Voice���X�g�̃C���f�b�N�X�͈͊O: �C���f�b�N�X {j}, Voice���X�g�̃T�C�Y {voicelist.Count}");
                                // �K�v�ɉ����Ă����ŃG���[������t�H�[���o�b�N�������s��
                            }
                        }
                        retry = false;
                        await Task.Delay(3000);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"�G���[���������܂���: {ex.Message}");
                    // ��莞�ԑҋ@
                    await Task.Delay(3000); // �����ł�3�b�ҋ@���Ă��܂�
                    retry = true; // �Ď��s�t���O�𗧂Ă�
                }
            } while (retry); // retry��true�̏ꍇ�A�Ď��s����
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


