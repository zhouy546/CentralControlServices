using MyUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Threadtcp {

    public CentralControlDevice centralControlDevice;
    public string host;
    public int port;
    public string data;
    public bool isHeartbeat;
    public Thread t;


    public Threadtcp(string _host, int _port, string _data, bool _IsHeartbeat = false)
    {
        host = _host;
        port = _port;
        data = _data;
        isHeartbeat = _IsHeartbeat;


    }

    public Threadtcp(string _host, int _port, string _data, CentralControlDevice _centralControlDevice, bool _IsHeartbeat = false)
    {
        centralControlDevice = _centralControlDevice;
        host = _host;
        port = _port;
        data = _data;
        isHeartbeat = _IsHeartbeat;


    }



    private static Encoding encode = Encoding.Default;

    public void sendDefaultString()
    {
        t = new Thread(new ThreadStart(Send));
        t.Start();
    }

    public void sendHexString()
    {
        t = new Thread(new ThreadStart(SendHexByte));
        t.Start();
    }



    private void SendHexByte()
    {
      

        string result = string.Empty;
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        if (isHeartbeat)
        {
            Thread.Sleep(ValueSheet.heartBeatSendWaitTime);
        }

        try
        {
            clientSocket.Connect(host, port);
            Debug.Log("Try Connect " + host + "port: " + port);


        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            BoardEvent(centralControlDevice, "null");

            t.Abort();
            return;
        }

         byte[] b = Utility.strToToHexByte(data);
        clientSocket.Send(b);
        // Debug.Log("Send：" + data);
        result = ReceiveLEDHex(clientSocket, ValueSheet.TcpReceiveWaitTime); //5*2 seconds timeout.
                                                    // Debug.Log("Receive：" + result);

        Thread.Sleep(500);
        DestroySocket(clientSocket);

        BoardEvent(centralControlDevice, result);
        t.Abort();
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    private void Send()
    {
        string result = string.Empty;
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        if (isHeartbeat)
        {
            Thread.Sleep(ValueSheet.heartBeatSendWaitTime);
        }

        try
        {
            clientSocket.Connect(host, port);
            Debug.Log("Try Connect "+host+"port: "+port);


        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            BoardEvent(centralControlDevice, "null");

            t.Abort();
            return;
        }
        clientSocket.Send(encode.GetBytes(data));
       // Debug.Log("Send：" + data);
        result = Receive(clientSocket, ValueSheet.TcpReceiveWaitTime); //5*2 seconds timeout.
                                             // Debug.Log("Receive：" + result);

   
        DestroySocket(clientSocket);

        BoardEvent(centralControlDevice, result);
        t.Abort();
    }


    private void BoardEvent(CentralControlDevice device,string result)
    {
        if (!isHeartbeat)//不是心跳包TCP
        {
            EventCenter.Broadcast(EventDefine.TCPResult, result);
        }
        else//是心跳包TCP
        {
            EventCenter.Broadcast(EventDefine.HeartbeatTcpResult, device, result);
        }
    }


    /// <summary>
    /// 接收数据
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    private  string Receive(Socket socket, int timeout)
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
            result = encode.GetString(data.ToArray(), 0, data.Count);
        }

        return result;
    }

    private  string ReceiveLEDHex(Socket socket, int timeout)
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
            Debug.Log("HEX running");
            byte[] bytes = data.ToArray();

           
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    result += bytes[i].ToString("X2");
                }
            }
            Debug.Log("result是：" + result);
            //string s="";
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    result += bytes[i];
            //}

            //Debug.Log("result是：" + result);
        }

        return result;
    }


    /// <summary>
    /// 销毁Socket对象
    /// </summary>
    /// <param name="socket"></param>
    private static void DestroySocket(Socket socket)
    {
        if (socket.Connected)
        {
            socket.Shutdown(SocketShutdown.Both);
        }
        socket.Close();
    }
}
