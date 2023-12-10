using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoicevoxBridge;




public class LessonDataStorage
{///授業のテキストデータを扱うクラス
    private Dictionary<int, List<string>> LessonSpeech = new Dictionary<int, List<string>>();

    public void StoreSpeech(Dictionary<int, List<string>> newdic)
    {
        LessonSpeech = newdic;
    }

    public List<string> GetLessonData(int index)
    {
        // 引数:INDEXに応じた、1セクションの発話が句読点ごとに区切られたリストを返す
        //１セクション分の発話のリストを返す
        if (LessonSpeech.TryGetValue(index, out var speechList))
        {
            return speechList;
        }
        return null;
    }
}

public class VoiceDataStorage
{///教師の発話のボイスリスト
    private Dictionary<int, List<Voice>> AllLessonVoice = new Dictionary<int, List<Voice>>();

    public void StoreVoicelist(int index, List<Voice> voicelist )
    {
        AllLessonVoice[index] = voicelist;
    }

    public List<Voice> GetSectionVoiceList(int index)
    {
        // 引数:INDEXに応じた、1セクションの発話が句読点ごとに区切られた音声のリストを返す
        if (AllLessonVoice.TryGetValue(index, out var voicelist))
        {
            return voicelist;
        }
        return null;
    }
}

public class ResponceMessageStorage
{    // IDをキーとして、Pythonから送られたJSON形式のメッセージのリストを保存するクラス
    private Dictionary<int, List<MessageData>> ResponseMessages = new Dictionary<int, List<MessageData>>();

    // 新しいメッセージが追加されたときに通知するためのイベントハンドラの定義
    public delegate void MessageAddedHandler(MessageData message);
    public event MessageAddedHandler MessageAdded; ///MessageAddedイベント

    // メッセージを追加
    public void AddMessage(MessageData message)
    {
        if (!ResponseMessages.ContainsKey(message.id))
        {
            ResponseMessages[message.id] = new List<MessageData>();
        }
        ResponseMessages[message.id].Add(message);

        // イベントを発火させる
        MessageAdded?.Invoke(message);
    }

    // 特定のIDのメッセージを取得
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
{///質問コーナーで使うボイス管理クラス
    private List<string> IntroTextList = new List<string>
        {
            "それでは、ここまでの内容で質問のある人はいますか？",
            "分からないことがあればラインにメッセージを送信してくださいね。",
            "それでは、いくつか質問に答えていきますね。"
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
        // 引数:INDEXに応じた、1セクションの発話が句読点ごとに区切られた音声を返す
        if (QuestionResponceVoice[index]!=null)
        {
            Voice voice = QuestionResponceVoice[index];
            return voice;
        }
        else
        {
            Debug.Log("nullやで笑");
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



