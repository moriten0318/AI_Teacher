using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDP : MonoBehaviour {

    /// 受信するC＃側のスクリプト
 static UdpClient udp;
 IPEndPoint remoteEP = null;
 int i = 0;
 // Use this for initialization
 void Start () {
  int LOCA_LPORT = 50007;

  udp = new UdpClient(LOCA_LPORT);
  udp.Client.ReceiveTimeout = 2000;//データのタイムアウト=2秒
 }

// Update is called once per frame
 void Update ()
 {
 IPEndPoint remoteEP = null;
 byte[] data = udp.Receive(ref remoteEP);///受信したデータをbyte配列dataに格納
 string text = Encoding.UTF8.GetString(data);///dataをtextに入れる
 Debug.Log(text);
 }
}