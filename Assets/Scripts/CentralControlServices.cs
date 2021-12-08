using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YunqiLibrary;
using MyUtility;
public class CentralControlServices : MonoBehaviour
{

    public List<floor> floors = new List<floor>();

    public TCP_Client btntcp;

    private void Start()
    {
        btntcp = new TCP_Client(btnTCPCallBack);
    }

    private void btnTCPCallBack(string s)
    {

        if (ValueSheet.currentCentralControlDevice.deviceType == DeviceType.LED电柜)
        {
            ValueSheet.currentCentralControlDevice.status = MyUtility.Utility.convertLEDServerStatus(s);
        }
        else if (ValueSheet.currentCentralControlDevice.deviceType == DeviceType.多媒体服务器)
        {
            ValueSheet.currentCentralControlDevice.status = MyUtility.Utility.convertMediaServerStatus(s);

        }
        else if (ValueSheet.currentCentralControlDevice.deviceType == DeviceType.投影)
        {
            ValueSheet.currentCentralControlDevice.status = MyUtility.Utility.convertProjectorServerStatus(s, ValueSheet.currentCentralControlDevice);

        }
        else if (ValueSheet.currentCentralControlDevice.deviceType == DeviceType.灯光)
        {
            if (s.Length >= 10)
            {
                s = s.Substring(2, 8);
            }

            ValueSheet.currentCentralControlDevice.status = MyUtility.Utility.convertLightServerStatus(s);

        }



    }

}
