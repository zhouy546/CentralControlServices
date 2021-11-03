using MyUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDevice 
{
    public static CentralControlDevice temp;


    private static void dealwithTCPResult(string s)
    {
        UpdateDeviceSataus(s);
        EventCenter.RemoveListener<string>(EventDefine.TCPResult, dealwithTCPResult);

    }

    public static void openLightServer(string _ip, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_ip))
        {
            temp = _centralControlDevice;

            EventCenter.AddListener<string>(EventDefine.TCPResult, dealwithTCPResult);

            string str = temp.LightID + " " + ValueSheet.LightCmd[0];
            string sendstr = str+" "+ CRC.CRCCalc(str);

            Threadtcp tcp_thread = new Threadtcp(_ip, 28010, sendstr, false);
            tcp_thread.sendHexString();

        }
    }

    public static void closeLightServer(string _ip, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_ip))
        {

            temp = _centralControlDevice;

            EventCenter.AddListener<string>(EventDefine.TCPResult, dealwithTCPResult);

            string str = temp.LightID + " " + ValueSheet.LightCmd[1];
            string sendstr = str + " " + CRC.CRCCalc(str);
            Threadtcp tcp_thread = new Threadtcp(_ip, 28010, sendstr, false);
            tcp_thread.sendHexString();
        }
    } 

    private static void UpdateDeviceSataus(string s)
    {
        if (s.Length >= 10) {

           string str =  s.Substring(2, 10);

           temp.status = Utility.convertLightServerStatus(str);

        }
        else
        {
            temp.status = 4;
        }


    }
}
