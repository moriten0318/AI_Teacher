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
            Destroy(thisbutton); // ボタンのUIを非表示にする
        }
        /*        if (BG != null)
                {
                    BG.gameObject.SetActive(false); // BGのUIを非表示にする
                }*/
    }

    public void BottunToActive()
    {
        // 他のメソッド内で使用する部分
        if (thisbutton != null)
        {
            // indexが0で、かつthisbuttonが存在する場合に実行する処理
            thisbutton.SetActive(true);
        }
    }

}
