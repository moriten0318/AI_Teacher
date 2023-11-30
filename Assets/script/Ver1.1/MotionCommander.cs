using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionCommander : MonoBehaviour
{
    // 変数の宣言

    public bool _motion_flag = true;    ///Trueだったらモーション再生可能
    public GameObject _haru_model;
    public MotionPlayer _motionplayer;

    public AnimationClip idle_animation;

    private void OnValidate()
    {
        _motionplayer = _haru_model.GetComponent<MotionPlayer>();
        Idle_Motion_Play();
    }

    void Start()
    {
        Invoke("Idle_Motion_Play",1f);
    }

    public void Idle_Motion_Play()
    {
        _motionplayer.Play_roopMotion(idle_animation);
        Debug.Log("アイドリング中・・・");
    }
}
