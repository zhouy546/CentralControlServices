using MyUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorDevice : MonoBehaviour
{
    public static CentralControlDevice temp;


    public static void openProjectorServer(string _PCDeviceIP, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_PCDeviceIP))
        {
            temp = _centralControlDevice;

            ProjectorSerial_JSON projectorSerial_JSON = ValueSheet.ProjectorCMD[temp.ProjectSerial];

            ValueSheet.centralcontrolServices.btntcp.TCPSenHex(temp.PCDeviceIP, projectorSerial_JSON.port, projectorSerial_JSON.open);

        }
    }

    public static void closeProjectorServer(string _PCDeviceIP, CentralControlDevice _centralControlDevice)
    {
        if (Utility.checkIp(_PCDeviceIP))
        {

            temp = _centralControlDevice;

            ProjectorSerial_JSON projectorSerial_JSON = ValueSheet.ProjectorCMD[temp.ProjectSerial];

            ValueSheet.centralcontrolServices.btntcp.TCPSenHex(temp.PCDeviceIP, projectorSerial_JSON.port, projectorSerial_JSON.close);

        }

    }

}
