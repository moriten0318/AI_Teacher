using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public GameObject thisbutton;
/*    public GameObject BG;*/

    private void Start()
    {

    }

    public void OnButtonClick()
    {
        if (thisbutton != null)
        {
            thisbutton.gameObject.SetActive(false); // ボタンのUIを非表示にする
        }
/*        if (BG != null)
        {
            BG.gameObject.SetActive(false); // BGのUIを非表示にする
        }*/


    }
}
