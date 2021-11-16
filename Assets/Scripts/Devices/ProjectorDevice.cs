using MyUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorDevice : MonoBehaviour
{
    public static CentralControlDevice temp;

    private static void dealwithTCPResult(string s)
    {
        UpdateDeviceSataus(s);
        EventCenter.RemoveListener<string>(EventDefine.TCPResult, dealwithTCPResult);

    }

    public static void openProjectorServer(string _PCDeviceIP, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_PCDeviceIP))
        {
            temp = _centralControlDevice;

            EventCenter.AddListener<string>(EventDefine.TCPResult, dealwithTCPResult);

            Threadtcp tcp_thread = new Threadtcp(_PCDeviceIP, 3000, ValueSheet.MediaServerCmd[0]);
            tcp_thread.sendDefaultString();

        }
    }

    public static void closeProjectorServer(string _PCDeviceIP, CentralControlDevice _centralControlDevice)
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
        temp.status = Utility.convertProjectorServerStatus(s,temp);
    }
}
