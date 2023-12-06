using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageStorage : MonoBehaviour
{
    // IDをキーとして、メッセージのリストを保存
    private Dictionary<int, List<MessageData>> messagesById = new Dictionary<int, List<MessageData>>();

    // メッセージを追加
    public void AddMessage(MessageData message)
    {
        if (!messagesById.ContainsKey(message.id))
        {
            messagesById[message.id] = new List<MessageData>();
        }
        messagesById[message.id].Add(message);
    }

    // 特定のIDのメッセージを取得
    public List<MessageData> GetMessagesById(int id)
    {
        if (messagesById.ContainsKey(id))
        {
            return messagesById[id];
        }
        return new List<MessageData>();
    }
}
