using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using VoicevoxBridge;

public class MessageStorage : MonoBehaviour
{
    // ID���L�[�Ƃ��āA���b�Z�[�W�̃��X�g��ۑ�
    private Dictionary<int, List<MessageData>> messagesById = new Dictionary<int, List<MessageData>>();

    // ���b�Z�[�W��ǉ�
    public void AddMessage(MessageData message)
    {
        if (!messagesById.ContainsKey(message.id))
        {
            messagesById[message.id] = new List<MessageData>();
        }
        messagesById[message.id].Add(message);
    }

    // �����ID�̃��b�Z�[�W���擾
    public List<MessageData> GetMessagesById(int id)
    {
        if (messagesById.ContainsKey(id))
        {
            return messagesById[id];
        }
        return new List<MessageData>();
    }
}

public class VoiceStorage : MonoBehaviour
{
    private Dictionary<int, Voice> voiceById = new Dictionary<int, Voice>();

    public void StoreVoice(int id, Voice voice)
    {
        voiceById[id] = voice;
    }

    public Voice GetVoice(int id)
    {
        if (voiceById.TryGetValue(id, out Voice voice))
        {
            return voice;
        }
        return null;
    }
}
