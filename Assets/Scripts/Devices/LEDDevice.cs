using MyUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDDevice 
{
    public static CentralControlDevice temp;


    private static void dealwithTCPResult(string s)
    {
        UpdateDeviceSataus(s);
        EventCenter.RemoveListener<string>(EventDefine.TCPResult, dealwithTCPResult);

    }

    public static void openLEDServer(string _ip, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_ip))
        {
            temp = _centralControlDevice;

            EventCenter.AddListener<string>(EventDefine.TCPResult, dealwithTCPResult);

            Threadtcp tcp_thread = new Threadtcp(_ip, 5000, ValueSheet.LEDCmd[0], false);

            tcp_thread.sendHexString();

        }
    }

    public static void closeLEDServer(string _ip, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_ip))
        {

            temp = _centralControlDevice;

            EventCenter.AddListener<string>(EventDefine.TCPResult, dealwithTCPResult);

            Threadtcp tcp_thread = new Threadtcp(_ip, 5000, ValueSheet.LEDCmd[1], false);

            tcp_thread.sendHexString();
        }
    }

    private static void UpdateDeviceSataus(string s)
    {
        temp.status = Utility.convertLEDServerStatus(s);
    }
}
