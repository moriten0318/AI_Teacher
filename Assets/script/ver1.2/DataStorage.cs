using System;
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

    // 特定のIDかつ特定のisResponseを持つMassageDataを取得
    public MessageData GetMessageData(int id, bool isResponse)
    {
        /*   旧コード【念のため★】
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
                        return message; // 一致するMessageDataを見つけた場合、それを返す
                    }
                }
            }
            // 条件に一致するメッセージがない場合
            Debug.LogError($"指定IDのテキストデータが無いよ～(´;ω;｀)ID：: {id} isResponse: {isResponse}");
            return null;
        }
        catch (Exception ex)
        {
            // 何らかのエラーが発生した場合、そのエラーをログに出力
            Debug.LogError("Error in GetMessagesData: " + ex.Message);
            return null;
        }
    }
}

public class ResponceVoiceStorage
{///質問コーナーで使うボイス管理クラス
    public List<string> IntroText = new List<string>
        {
            "それでは、ここまでの内容で質問のある人はいますか？",
            "分からないことがあればラインにメッセージを送信してくださいね。",
            "メッセージを待つ時間を取りますから、ゆっくり考えて大丈夫ですよ。",
            "それでは、いくつかの質問に答えていきますね。"
        };
    ///↑のボイスが入ったリスト
    private Dictionary<int, Voice> IntroVoice = new Dictionary<int, Voice>();

    public void StoreIntroVoice(int id, Voice voice)
    {
        IntroVoice[id] = voice;
    }
    public Voice GetIntroVoice(int id)
    {
        // 引数:IDに応じた、返答文が句読点ごとに区切られた音声を返す
        if (IntroVoice[id]!=null)
        {
            Voice voice = IntroVoice[id];
            return voice;
        }
        else
        {
            Debug.Log("キーが辞書に存在しません: " + id);
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
        // 引数:IDに応じた、返答文が句読点ごとに区切られた音声を返す
        if (QuestionResponceVoice.TryGetValue(id, out List<Voice> voice))
        {
            return voice;
        }
        else
        {
            Debug.Log("キーが辞書に存在しません: " + id);
            return null;
        }

    }

}



