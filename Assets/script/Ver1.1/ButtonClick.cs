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
            Destroy(thisbutton); // �{�^����UI���\���ɂ���
        }
        /*        if (BG != null)
                {
                    BG.gameObject.SetActive(false); // BG��UI���\���ɂ���
                }*/
    }

    public void BottunToActive()
    {
        // ���̃��\�b�h���Ŏg�p���镔��
        if (thisbutton != null)
        {
            // index��0�ŁA����thisbutton�����݂���ꍇ�Ɏ��s���鏈��
            thisbutton.SetActive(true);
        }
    }

}
