using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDP : MonoBehaviour {

    /// ��M����C�����̃X�N���v�g
 static UdpClient udp;
 IPEndPoint remoteEP = null;
 // Use this for initialization
 void Start () {
  int LOCA_LPORT = 50007;

  udp = new UdpClient(LOCA_LPORT);
  udp.Client.ReceiveTimeout = 100000;

 }

// Update is called once per frame
 void Update ()
 {
 byte[] data = udp.Receive(ref remoteEP);///��M�����f�[�^��byte�z��data�Ɋi�[
 string text = Encoding.UTF8.GetString(data);///data��text�ɓ����
 Debug.Log(text);
  }
}

