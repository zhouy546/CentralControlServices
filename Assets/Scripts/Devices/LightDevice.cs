using MyUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDevice 
{
    public static CentralControlDevice temp;


    public static void openLightServer(string _ip, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_ip))
        {
            temp = _centralControlDevice;

            string str = temp.LightID + " " + ValueSheet.LightCmd[0];

            string sendstr = str+" "+ CRC.CRCCalc(str);

            ValueSheet.centralcontrolServices.btntcp.TCPSenHex(_ip, 28010, sendstr);
        }
    }

    public static void closeLightServer(string _ip, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_ip))
        {

            temp = _centralControlDevice;

            string str = temp.LightID + " " + ValueSheet.LightCmd[1];

            string sendstr = str + " " + CRC.CRCCalc(str);

            ValueSheet.centralcontrolServices.btntcp.TCPSenHex(_ip, 28010, sendstr);

        }
    } 
}
