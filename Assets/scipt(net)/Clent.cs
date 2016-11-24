using UnityEngine;
using System.Collections;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System;
using Newtonsoft.Json;
public class Clent : MonoBehaviour {
    NetworkStream stream;
    ReceiveClient receive;
    public BaseAction_AI_player unitAction;
    byte[] head = new byte[4];
    byte[] mainData;
    public const string SERVER_IP="127.0.0.1";

    // Use this for initialization
    void Start () {

        TcpClient client = new TcpClient();
        client.Connect(SERVER_IP, 4747);
        stream = client.GetStream();
        unitAction = GetComponent<BaseAction_AI_player>();

        receive = new ReceiveClient(client);
        Thread thread = new Thread(receive.receiveThread);
        thread.Start();
        
    }
	
	// Update is called once per frame
	void Update () {
        
        //檢查是否有收到數據---------------------------------------------------------
        if (receive.bufferOk)
        {
          
            int nowindex = 0;
            
            while (receive.buffer.Length-nowindex>4) {//一个封包至少为4byte长度(head)
                Debug.Log(Time.deltaTime);
                Debug.Log("buffer length - nowindex is" + (receive.buffer.Length - nowindex));
                Debug.Log("while start");
                Array.Copy(receive.buffer, nowindex, head, 0, 4);
                Debug.Log("copy1");
                int headLength = byteArray2int(head);//headLength是将head的序列化byte阵列反序列化后得到的int,标记该封包长度

                nowindex += 4;
                if (receive.buffer.Length - nowindex >= headLength)//如果剩下的byte长度比headlength还要短的话,说明接收不完全
                {
                    Debug.Log("enter if");
                    mainData = new byte[headLength];//maindata:byte阵列的序列化ziliao
                    Array.Copy(receive.buffer, nowindex, mainData, 0, headLength);
                    Debug.Log("copy2");
                    nowindex += headLength;
                    /* for (int i = 0; i < mainData.Length; i++)
                     {
                         Debug.Log("i=" + i + mainData[i]);
                     }*/
                    String strData = ToObject(mainData) as String;//strData:将mainData反序列化后的字串
                    Dictionary<int, object> dicData = (Dictionary<int, object>)JsonConvert.DeserializeObject(strData, typeof(Dictionary<int, object>));//dicData:将strData反json化后行成的字典


                    object order;
                    dicData.TryGetValue(1, out order);
                    string orderText = (string)order;
                    if (orderText.Equals("1"))
                    {
                        object forWard;
                        dicData.TryGetValue(2, out forWard);
                        if (forWard.Equals("W"))
                        {
                            Debug.Log("get W");
                            unitAction.move_AI_player(new Vector3(0, 0, 1));
                        }

                    }
                }
            }
            //恢复未接收状态
            receive.bufferOk = false;
        }

        //發送按鍵信息---------------------------------------------------------------
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("getkey-w");
            Dictionary<int, object> data = new Dictionary<int, object>();
            data.Add(1, "1");
            data.Add(2, "W");
            String jsondata = JsonConvert.SerializeObject(data,Formatting.Indented);

            byte[] bytedata = ToByteArray(jsondata);
            /*for(int i = 0; i < bytedata.Length; i++)
            {
                Debug.Log("bytedata"+i+" "+bytedata[i]);
            }*/
            stream.Write(bytedata,0,bytedata.Length);
            Debug.Log("Send ok over is"+receive.programNotOver);
        }
	}
    void OnDestroy()
    {
        Debug.Log("on destory");
        receive.programNotOver = false;
    }

    
    class ReceiveClient
    {
        TcpClient client;
        NetworkStream steam;
        public byte[] buffer = new byte[4096];
        public byte[] temp = new byte[4096];
        int tempnum;//在temp里堆积资料的长度
        public bool bufferOk=false;
        public bool programNotOver=true;
        public ReceiveClient(TcpClient self)
        {
            Debug.Log("thread is open");
            client = self;
            steam = client.GetStream();
           
        }
        public void receiveThread()
        {
            while (programNotOver)
            {   Debug.Log("reading...");
                int num=steam.Read(temp, tempnum, temp.Length);
                Debug.Log("receive "+num+" byte");
                tempnum += num;

                if (!bufferOk)
                {
                    buffer = new byte[num];
                    Debug.Log("In !bufferOK");
                    Array.Copy(temp, buffer, num);
                    Array.Clear(temp, 0, tempnum);
                    tempnum = 0;
                    bufferOk = true;
                    Debug.Log("end !bufferOK");
                }
                
                Debug.Log("read ok");
                Thread.Sleep(100);
                
            }
        }
       
    }
    //序列化方法--------------------------------------------------------------------------
        public static object ToObject(byte[] source)
       {
        Debug.Log("decode start");
           var Formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
           using (var stream = new System.IO.MemoryStream(source))
           {

            Debug.Log("length is"+stream.Length);
               return Formatter.Deserialize(stream);
           }
        Debug.Log("Decode ok");
        }

        public static byte[] ToByteArray(object source)
        {
            var Formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (var stream = new System.IO.MemoryStream())
            {
                Formatter.Serialize(stream, source);
                return stream.ToArray();
            }
        }
    //int与byte互转-----------------------------------------------------------------------
    public static byte[] int2byteArray(int num)
    {
        byte[] result = new byte[4];
        result[0] = (byte)(num >> 24);//取最高8位放到0下標
        result[1] = (byte)(num >> 16);//取次高8为放到1下標
        result[2] = (byte)(num >> 8); //取次低8位放到2下標 
        result[3] = (byte)(num);      //取最低8位放到3下標
        return result;
    }
    public static int byteArray2int(byte[] b)
    {
        byte[] a = new byte[4];
        int i = a.Length - 1, j = b.Length - 1;
        for (; i >= 0; i--, j--)
        {//從b的尾部(即int值的低位)開始copy數據
            if (j >= 0)
                a[i] = b[j];
            else
                a[i] = 0;//如果b.length不足4,則將高位補0
        }
        int v0 = (a[0] & 0xff) << 24;//&0xff將byte值無差異轉成int,避免Java自動類型提升後,會保留高位的符號位
        int v1 = (a[1] & 0xff) << 16;
        int v2 = (a[2] & 0xff) << 8;
        int v3 = (a[3] & 0xff);
        return v0 + v1 + v2 + v3;
    }
}
