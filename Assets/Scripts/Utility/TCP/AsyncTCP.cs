using MyUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncTCP
{
    //public string ip;
    //public int port;
    //public string data;
    //Socket sock;
    //public AsyncTCP()
    //{
    //     sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //}

    //public AsyncTCP(string _ip, int _port)
    //{
    //    ip = _ip;
    //    port = _port;
    //}

    //public AsyncTCP(string _ip, int _port,string _data)
    //{
    //    ip = _ip;
    //    port = _port;
    //    data = _data;
    //}

    //public void setval(string _ip, int _port, string _data)
    //{
    //    ip = _ip;
    //    port = _port;
    //    data = _data;
    //}

    //public async Task HexTestSend()
    //{
    //    bool b = await Connect();

    //    string s =await SendHexData(sock,data);

    //    DestroySocket(sock);
    //}

    //public async Task TestSend()
    //{
    //    bool b = await Connect();

    //    string s = await SendHexData(sock, data);

    //    DestroySocket(sock);
    //}
    //private static void DestroySocket(Socket socket)
    //{
    //    if (socket.Connected)
    //    {
    //        socket.Shutdown(SocketShutdown.Both);
    //    }
    //    socket.Close();
    //}
    //public async Task<bool> Connect()
    //{

    //    IPAddress ipAddress = IPAddress.Parse(ip);
    //    IPEndPoint ipep = new IPEndPoint(ipAddress, port);//IP和端口
       

    //    ConnectSocketDelegate connect = ConnectSocket;
    //    IAsyncResult asyncResult = connect.BeginInvoke(ipep, sock, null, null);

    //    bool connectSuccess = asyncResult.AsyncWaitHandle.WaitOne(ValueSheet.TcpSendWaitTime, false);
    //    if (!connectSuccess)
    //    {
    //        Debug.Log(string.Format("失败！错误信息：{0}", "连接超时"));
    //        return false;
    //    }


    //    string exmessage = connect.EndInvoke(asyncResult);
    //    if (!string.IsNullOrEmpty(exmessage))
    //    {
    //        Debug.Log(string.Format("失败！错误信息：{0}", exmessage));
    //        return false;
    //    }

    //    await Task.Yield();

    //    return true;

    //}


    //public async Task<string> SendData(Socket socket, string data)
    //{
    //    byte[] b = Utility.strToToHexByte(data);
    //    socket.Send(b);//发送信息

    //    string S = Receive(socket, ValueSheet.TcpReceiveWaitTime);

    //    await Task.Yield();
    //    return S;
    //}

    //private string Receive(Socket socket, int timeout)
    //{
    //    string result = string.Empty;
    //    socket.ReceiveTimeout = timeout;


    //    List<byte> data = new List<byte>();
    //    byte[] buffer = new byte[1024];
    //    int length = 0;
    //    try
    //    {
    //        while ((length = socket.Receive(buffer)) > 0)
    //        {
    //            for (int j = 0; j < length; j++)
    //            {
    //                data.Add(buffer[j]);
    //            }
    //            if (length < buffer.Length)
    //            {
    //                break;
    //            }
    //        }
    //    }
    //    catch { }
    //    if (data.Count > 0)
    //    {
    //        Debug.Log("HEX running");
    //        byte[] bytes = data.ToArray();


    //        if (bytes != null)
    //        {
    //            for (int i = 0; i < bytes.Length; i++)
    //            {
    //                result += bytes[i].ToString("X2");
    //            }
    //        }
    //        Debug.Log("result是：" + result);
    //    }

    //    return result;
    //}




    //public async Task<string> SendHexData(Socket socket, string data)
    //{
    //    byte[] b = Utility.strToToHexByte(data);
    //    socket.Send(b);//发送信息

    //    string S = ReceiveHex(socket, ValueSheet.TcpReceiveWaitTime);

    //    await Task.Yield();
    //    return S;
    //}

    //private string ReceiveHex(Socket socket, int timeout)
    //{
    //    string result = string.Empty;
    //    socket.ReceiveTimeout = timeout;


    //    List<byte> data = new List<byte>();
    //    byte[] buffer = new byte[1024];
    //    int length = 0;
    //    try
    //    {
    //        while ((length = socket.Receive(buffer)) > 0)
    //        {
    //            for (int j = 0; j < length; j++)
    //            {
    //                data.Add(buffer[j]);
    //            }
    //            if (length < buffer.Length)
    //            {
    //                break;
    //            }
    //        }
    //    }
    //    catch { }
    //    if (data.Count > 0)
    //    {
    //        Debug.Log("HEX running");
    //        byte[] bytes = data.ToArray();


    //        if (bytes != null)
    //        {
    //            for (int i = 0; i < bytes.Length; i++)
    //            {
    //                result += bytes[i].ToString("X2");
    //            }
    //        }
    //        Debug.Log("result是：" + result);
    //        //string s="";
    //        //for (int i = 0; i < bytes.Length; i++)
    //        //{
    //        //    result += bytes[i];
    //        //}

    //        //Debug.Log("result是：" + result);
    //    }

    //    return result;
    //}

    //private delegate string ConnectSocketDelegate(IPEndPoint ipep, Socket sock);
    //private string ConnectSocket(IPEndPoint ipep, Socket sock)
    //{
    //    string exmessage = "";
    //    try
    //    {
    //        sock.Connect(ipep);
    //    }
    //    catch (System.Exception ex)
    //    {
    //        exmessage = ex.Message;
    //    }
    //    finally
    //    {
    //    }

    //    return exmessage;
    //}

}
