using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoicevoxBridge;




public class LessonDataStorage
{///���Ƃ̃e�L�X�g�f�[�^�������N���X
    private Dictionary<int, List<string>> LessonSpeech = new Dictionary<int, List<string>>();

    public void StoreSpeech(Dictionary<int, List<string>> newdic)
    {
        LessonSpeech = newdic;
    }

    public List<string> GetLessonData(int index)
    {
        // ����:INDEX�ɉ������A1�Z�N�V�����̔��b����Ǔ_���Ƃɋ�؂�ꂽ���X�g��Ԃ�
        //�P�Z�N�V�������̔��b�̃��X�g��Ԃ�
        if (LessonSpeech.TryGetValue(index, out var speechList))
        {
            return speechList;
        }
        return null;
    }
}

public class VoiceDataStorage
{///���t�̔��b�̃{�C�X���X�g
    private Dictionary<int, List<Voice>> AllLessonVoice = new Dictionary<int, List<Voice>>();

    public void StoreVoicelist(int index, List<Voice> voicelist )
    {
        AllLessonVoice[index] = voicelist;
    }

    public List<Voice> GetSectionVoiceList(int index)
    {
        // ����:INDEX�ɉ������A1�Z�N�V�����̔��b����Ǔ_���Ƃɋ�؂�ꂽ�����̃��X�g��Ԃ�
        if (AllLessonVoice.TryGetValue(index, out var voicelist))
        {
            return voicelist;
        }
        return null;
    }
}

public class ResponceMessageStorage
{    // ID���L�[�Ƃ��āAPython���瑗��ꂽJSON�`���̃��b�Z�[�W�̃��X�g��ۑ�����N���X
    private Dictionary<int, List<MessageData>> ResponseMessages = new Dictionary<int, List<MessageData>>();

    // �V�������b�Z�[�W���ǉ����ꂽ�Ƃ��ɒʒm���邽�߂̃C�x���g�n���h���̒�`
    public delegate void MessageAddedHandler(MessageData message);
    public event MessageAddedHandler MessageAdded; ///MessageAdded�C�x���g

    // ���b�Z�[�W��ǉ�
    public void AddMessage(MessageData message)
    {
        if (!ResponseMessages.ContainsKey(message.id))
        {
            ResponseMessages[message.id] = new List<MessageData>();
        }
        ResponseMessages[message.id].Add(message);

        // �C�x���g�𔭉΂�����
        MessageAdded?.Invoke(message);
    }

    // �����ID�̃��b�Z�[�W���擾
    public List<MessageData> GetMessagesById(int id)
    {
        if (ResponseMessages.ContainsKey(id))
        {
            return ResponseMessages[id];
        }
        return new List<MessageData>();
    }
}

public class QuestionSceneVoiceStorage
{///����R�[�i�[�Ŏg���{�C�X�Ǘ��N���X
    private List<string> IntroTextList = new List<string>
        {
            "����ł́A�����܂ł̓��e�Ŏ���̂���l�͂��܂����H",
            "������Ȃ����Ƃ�����΃��C���Ƀ��b�Z�[�W�𑗐M���Ă��������ˁB",
            "����ł́A����������ɓ����Ă����܂��ˁB"
        };
    private Dictionary<int, Voice> IntroVoice = new Dictionary<int, Voice>();

    public void StoreIntroVoice(int id, Voice voice)
    {
        IntroVoice[id] = voice;
    }


    private Dictionary<int, Voice> QuestionResponceVoice = new Dictionary<int, Voice>();
    public void StoreResponceVoice(int id, Voice voice)
    {
        QuestionResponceVoice[id] = voice;
    }

    public Voice GetResponceVoice(int index)
    {
        // ����:INDEX�ɉ������A1�Z�N�V�����̔��b����Ǔ_���Ƃɋ�؂�ꂽ������Ԃ�
        if (QuestionResponceVoice[index]!=null)
        {
            Voice voice = QuestionResponceVoice[index];
            return voice;
        }
        else
        {
            Debug.Log("null��ŏ�");
            return null;
        }

    }

   public Voice GetVoice(int id)
    {
        if (QuestionResponceVoice.TryGetValue(id, out Voice voice))
        {
            return voice;
        }
        return null;
    }
}



