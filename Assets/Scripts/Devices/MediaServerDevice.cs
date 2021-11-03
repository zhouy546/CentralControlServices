using MyUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MediaServerDevice 
{
    public static CentralControlDevice temp;

    private static void dealwithTCPResult(string s)
    {
        UpdateDeviceSataus(s);
        EventCenter.RemoveListener<string>(EventDefine.TCPResult, dealwithTCPResult);

    }

    public static void openMediaServer(string _PCDeviceIP, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_PCDeviceIP))
        {
            temp = _centralControlDevice;

            EventCenter.AddListener<string>(EventDefine.TCPResult, dealwithTCPResult);

            Threadtcp tcp_thread = new Threadtcp(_PCDeviceIP, 3000, ValueSheet.MediaServerCmd[0]);
            tcp_thread.sendDefaultString();

        }
    }

    public static void closeMediaServer(string _PCDeviceIP, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_PCDeviceIP))
        {

            temp = _centralControlDevice;

            EventCenter.AddListener<string>(EventDefine.TCPResult, dealwithTCPResult);

            Threadtcp tcp_thread = new Threadtcp(_PCDeviceIP, 3000, ValueSheet.MediaServerCmd[1]);
            tcp_thread.sendDefaultString();
        }

    }

    private static void UpdateDeviceSataus(string s)
    {
        temp.status = Utility.convertMediaServerStatus(s);
    }
}
