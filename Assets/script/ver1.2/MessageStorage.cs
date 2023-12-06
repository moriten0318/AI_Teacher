using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
