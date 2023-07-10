using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoicevoxBridge;

public class voicevox_test : MonoBehaviour
{
 [SerializeField] VOICEVOX voicevox;

public async void PlayVoice()
    {
        int speaker = 1; // ずんだもん あまあま
        string text = "今日も良い天気ですね";
        await voicevox.PlayOneShot(speaker, text);
    }

}
