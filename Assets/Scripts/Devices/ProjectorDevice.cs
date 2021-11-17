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


            ProjectorSerial_JSON projectorSerial_JSON = ValueSheet.ProjectorCMD[temp.ProjectSerial];

            Threadtcp tcp_thread = new Threadtcp(temp.PCDeviceIP, projectorSerial_JSON.port, projectorSerial_JSON.open, temp);

            tcp_thread.sendHexString();

        }
    }

    public static void closeProjectorServer(string _PCDeviceIP, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_PCDeviceIP))
        {

            temp = _centralControlDevice;

            EventCenter.AddListener<string>(EventDefine.TCPResult, dealwithTCPResult);

            ProjectorSerial_JSON projectorSerial_JSON = ValueSheet.ProjectorCMD[temp.ProjectSerial];

            Threadtcp tcp_thread = new Threadtcp(temp.PCDeviceIP, projectorSerial_JSON.port, projectorSerial_JSON.close, temp);

            tcp_thread.sendHexString();
        }

    }

    private static void UpdateDeviceSataus(string s)
    {
        temp.status = Utility.convertProjectorServerStatus(s,temp);
    }
}
