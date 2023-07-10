using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoicevoxBridge;

public class voicevox_test : MonoBehaviour
{
 [SerializeField] VOICEVOX voicevox;

public async void PlayVoice()
    {
        int speaker = 1; // Ç∏ÇÒÇæÇ‡ÇÒ Ç†Ç‹Ç†Ç‹
        string text = "ç°ì˙Ç‡ó«Ç¢ìVãCÇ≈Ç∑ÇÀ";
        await voicevox.PlayOneShot(speaker, text);
    }

}
