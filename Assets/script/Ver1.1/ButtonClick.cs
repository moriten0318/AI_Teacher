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
            thisbutton.gameObject.SetActive(false); // �{�^����UI���\���ɂ���
        }
/*        if (BG != null)
        {
            BG.gameObject.SetActive(false); // BG��UI���\���ɂ���
        }*/


    }
}
