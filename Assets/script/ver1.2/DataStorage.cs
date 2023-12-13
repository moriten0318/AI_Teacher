using System;
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

    // �����ID�������isResponse������MassageData���擾
    public MessageData GetMessageData(int id, bool isResponse)
    {
        /*   ���R�[�h�y�O�̂��߁��z
        if (ResponseMessages.ContainsKey(id))
                {
                    return ResponseMessages[id];
                }
                return new List<MessageData>();*/

        try
        {
            if (ResponseMessages.TryGetValue(id, out List<MessageData> messages))
            {
                foreach (var message in messages)
                {
                    if (message.isResponse == isResponse)
                    {
                        return message; // ��v����MessageData���������ꍇ�A�����Ԃ�
                    }
                }
            }
            // �����Ɉ�v���郁�b�Z�[�W���Ȃ��ꍇ
            Debug.LogError($"�w��ID�̃e�L�X�g�f�[�^��������`(�L;��;�M)ID�F: {id} isResponse: {isResponse}");
            return null;
        }
        catch (Exception ex)
        {
            // ���炩�̃G���[�����������ꍇ�A���̃G���[�����O�ɏo��
            Debug.LogError("Error in GetMessagesData: " + ex.Message);
            return null;
        }
    }
}

public class ResponceVoiceStorage
{///����R�[�i�[�Ŏg���{�C�X�Ǘ��N���X
    public List<string> IntroText = new List<string>
        {
            "����ł́A�����܂ł̓��e�Ŏ���̂���l�͂��܂����H",
            "������Ȃ����Ƃ�����΃��C���Ƀ��b�Z�[�W�𑗐M���Ă��������ˁB",
            "���b�Z�[�W��҂��Ԃ����܂�����A�������l���đ��v�ł���B",
            "����ł́A�������̎���ɓ����Ă����܂��ˁB"
        };
    ///���̃{�C�X�����������X�g
    private Dictionary<int, Voice> IntroVoice = new Dictionary<int, Voice>();

    public void StoreIntroVoice(int id, Voice voice)
    {
        IntroVoice[id] = voice;
    }
    public Voice GetIntroVoice(int id)
    {
        // ����:ID�ɉ������A�ԓ�������Ǔ_���Ƃɋ�؂�ꂽ������Ԃ�
        if (IntroVoice[id]!=null)
        {
            Voice voice = IntroVoice[id];
            return voice;
        }
        else
        {
            Debug.Log("�L�[�������ɑ��݂��܂���: " + id);
            return null;
        }
    }


    private Dictionary<int, List<Voice>> QuestionResponceVoice = new Dictionary<int, List<Voice>>();
    public void StoreResponceVoice(int id, List<Voice> voice)
    {
        QuestionResponceVoice[id] = voice;
    }

    public List<Voice> GetResponceVoice(int id)
    {
        // ����:ID�ɉ������A�ԓ�������Ǔ_���Ƃɋ�؂�ꂽ������Ԃ�
        if (QuestionResponceVoice.TryGetValue(id, out List<Voice> voice))
        {
            return voice;
        }
        else
        {
            Debug.Log("�L�[�������ɑ��݂��܂���: " + id);
            return null;
        }

    }

}



