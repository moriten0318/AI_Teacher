using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoicevoxBridge;

public class voicevox_test : MonoBehaviour
{
 [SerializeField] VOICEVOX voicevox;

public async void PlayVoice()
    {
        int speaker = 1; // ���񂾂��� ���܂���
        string text = "�������ǂ��V�C�ł���";
        await voicevox.PlayOneShot(speaker, text);
    }

}
