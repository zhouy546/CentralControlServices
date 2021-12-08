using MyUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MediaServerDevice 
{
    public static CentralControlDevice temp;


    public static void openMediaServer(string _PCDeviceIP, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_PCDeviceIP))
        {
            temp = _centralControlDevice;

            ValueSheet.centralcontrolServices.btntcp.TCPSend(_PCDeviceIP, 3000, ValueSheet.MediaServerCmd[0]);

        }
    }

    public static void closeMediaServer(string _PCDeviceIP, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_PCDeviceIP))
        {

            temp = _centralControlDevice;

            ValueSheet.centralcontrolServices.btntcp.TCPSend(_PCDeviceIP, 3000, ValueSheet.MediaServerCmd[1]);

        }

    }

}
