using MyUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDDevice 
{
    public static CentralControlDevice temp;



    public static void openLEDServer(string _ip, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_ip))
        {
            temp = _centralControlDevice;

            ValueSheet.centralcontrolServices.btntcp.TCPSenHex(_ip, 5000, ValueSheet.LEDCmd[0]);

        }
    }

    public static void closeLEDServer(string _ip, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_ip))
        {

            temp = _centralControlDevice;

            ValueSheet.centralcontrolServices.btntcp.TCPSenHex(_ip, 5000, ValueSheet.LEDCmd[1]);

        }
    }

}
