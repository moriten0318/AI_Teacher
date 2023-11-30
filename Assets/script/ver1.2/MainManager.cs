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

    [SerializeField] VOICEVOX voicevox;///VOICEVOX�X�N���v�g�A�^�b�`�����I�u�W�F�N�g������

    public GameObject _TextNode;/// �������鎚��Prefab�̂��߂̕ϐ��錾
    public GameObject _Text_parent;///textnode�̐e�v�f�p�ϐ�

    public List<Texture2D> textureList;///�X���C�h�摜���X�g

    public string[] lessons;///�Z�N�V�������Ƃ̋��t�̔��b�̔z��(1�v�f��1�Z�N�V����)
    private string[] SplitText;///��Ǔ_�ŕ�����������
    private string[] tempSplitText;///��Ǔ_�ŕ�����������

    private List<Voice> voicelist = new List<Voice>();///�Đ����鉹�����X�g
    private List<Voice> tempVoices = new List<Voice>();///���̃Z�N�V�����̉������X�g

    private int CurrentSectionIndex=1;

    public Scrollbar _ScrollBar;

    void Start()
    {
        prepare_speech(0);
        _ScrollBar = _ScrollBar.GetComponent<Scrollbar>();
        _ScrollBar.value = 1;
    }

    void Update()
    {
        _ScrollBar.value = 0;
    }

    async void prepare_speech(int index)
    {
        tempVoices = await _Voice.CreateVoiceDate(index);
        tempSplitText = lessons[index].Split(char.Parse("�B"));///��Ǔ_�ŋ�؂���splitText�ɓ����
    }

    public async void play_section(int CurrentSectionIndex)
    {///�P�Z�N�V�������Đ����郋�[�v
        int num = 0;
        SplitText = tempSplitText;
        voicelist = tempVoices;

        prepare_speech(CurrentSectionIndex);

        while (num < voicelist.Count)
        {
            _BB.UpdateBlackBoard(CurrentSectionIndex+1);///�X���C�h�̍X�V
            Create_captions(SplitText[num]);///��������
            await voicevox.Play(voicelist[num]);///�����Đ�
            num++;
        }

    }


    private void Create_captions(string text)
    {

        foreach (string lesson in lessons)
        {
            Debug.Log(lesson);
        }

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
}


