using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;
using YunqiLibrary;

public class TcpClientTest : MonoBehaviour
{

    public delegate void CallBack<T>(T arg);

    public event CallBack<string> TCPCallBackevent;

    private delegate string ConnectSocketDelegate(IPEndPoint ipep, Socket sock);

    struct IpAndPort
    {
        public string Ip;
        public string Port;
    }

    // Start is called before the first frame update
    void Start()
    {
        TCPCallBackevent += debugText;
        Thread t = new Thread(ConnectTcp);

        t.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void debugText(string s)
    {
        Debug.Log(s);
    }


    public string  md5(string val)
    {


        string cl = val;
        //string pwd = "";
        MD5 md5 = MD5.Create(); //实例化一个md5对像
                                // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        return Convert.ToBase64String(s);

    }


    public void ConnectTcp()
    {
        string result = string.Empty;

        IPAddress ip = IPAddress.Parse("192.168.0.8");

        IPEndPoint ipep = new IPEndPoint(ip, 4352);//IP和端口

        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        ConnectSocketDelegate connect = ConnectSocket;

        IAsyncResult asyncResult = connect.BeginInvoke(ipep, clientSocket, null, null);

        bool connectSuccess = asyncResult.AsyncWaitHandle.WaitOne(1000, false);

        result = Receive(clientSocket, 2000); //5*2 seconds timeout.

        Debug.Log("Receive：" + result);

        string password = "Panasonic";



        result =md5(result.Split(' ')[2]).ToString()+password + "%1POWR 0";

        byte[] bytes = Encoding.Default.GetBytes(result);

        string str ="";

        for (int i = 0; i < bytes.Length; i++)
        {
            str += bytes[i].ToString("X2");
        }
        Debug.Log(str);

        byte[] b = MyUtility.Utility.strToToHexByte(str);

        clientSocket.Send(b);


        Thread.Sleep(500);

        result = ReceiveLEDHex(clientSocket, 2000); //5*2 seconds timeout.
                                                    // Debug.Log("Receive：" + result)             
        Thread.Sleep(500);

        if (result.Length > 0)
        {
            TCPCallBackevent.Invoke(result);
        }

        Debug.Log(result);
    }


    private string Receive(Socket socket, int timeout)
    {
        string result = string.Empty;
        socket.ReceiveTimeout = timeout;
        List<byte> data = new List<byte>();
        byte[] buffer = new byte[1024];
        int length = 0;
        try
        {
            while ((length = socket.Receive(buffer)) > 0)
            {
                for (int j = 0; j < length; j++)
                {
                    data.Add(buffer[j]);
                }
                if (length < buffer.Length)
                {
                    break;
                }
            }
        }
        catch { }
        if (data.Count > 0)
        {
            result = Encoding.Default.GetString(data.ToArray(), 0, data.Count);
        }

        return result;
    }

    private string ReceiveLEDHex(Socket socket, int timeout)
    {
        string result = string.Empty;
        socket.ReceiveTimeout = timeout;
        List<byte> data = new List<byte>();
        byte[] buffer = new byte[1024];
        int length = 0;
        try
        {
            while ((length = socket.Receive(buffer)) > 0)
            {
                for (int j = 0; j < length; j++)
                {
                    data.Add(buffer[j]);
                }
                if (length < buffer.Length)
                {
                    break;
                }
            }
        }
        catch { }
        if (data.Count > 0)
        {
            byte[] bytes = data.ToArray();


            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    result += bytes[i].ToString("X2");
                }
            }
        }

        return result;
    }

    private string ConnectSocket(IPEndPoint ipep, Socket sock)
    {
        string exmessage = "";
        try
        {
            sock.Connect(ipep);
        }
        catch (System.Exception ex)
        {
            exmessage = ex.Message;
        }
        finally
        {
        }

        return exmessage;
    }

    private string ReceiveLEDHex(Socket socket)
    {
        string result = string.Empty;
        List<byte> data = new List<byte>();
        byte[] buffer = new byte[1024];
        int length = 0;
        try
        {
            while ((length = socket.Receive(buffer)) > 0)
            {
                for (int j = 0; j < length; j++)
                {
                    data.Add(buffer[j]);
                }
                if (length < buffer.Length)
                {
                    break;
                }
            }
        }
        catch { }
        if (data.Count > 0)
        {
            byte[] bytes = data.ToArray();


            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    result += bytes[i].ToString("X2");
                }
            }
        }

        return result;
    }

}
