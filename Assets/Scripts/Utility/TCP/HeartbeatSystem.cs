using MyUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Text;
using YunqiLibrary;
public class HeartbeatSystem : MonoBehaviour
{
   [SerializeField]
    private CentralControlDevice mdevice;

    TCP_Client heartbeatTcp_client;

    // Start is called before the first frame update
    void Start()
    {



        EventCenter.AddListener(EventDefine.ini, INI);

    }

    void INI()
    {
        StartCoroutine(ini());
    }

    private IEnumerator ini()
    {
        yield return new WaitForSeconds(5f);

        heartbeatTcp_client = new TCP_Client(dealwithTCPResult);

        foreach (floor floor in ValueSheet.centralcontrolServices.floors)
        {
            foreach (CentralControlDevice device in floor.centralControlDevices)
            {
                ValueSheet.centralControlDevices.Add(device);
            }
        }
        StartCoroutine(SendBeat());
    }

    private  void Update()
    {

    


    }
    
    IEnumerator SendBeat()
    {


        foreach (CentralControlDevice device in ValueSheet.centralControlDevices)
        {
            if (!MyUtility.Utility.checkIp(device.PCDeviceIP))
            {
                device.status = 4;//等于Fault状态域名不准确的
            }

            mdevice = device;

            UpdateTCPLoop(device);

            yield return new WaitForSeconds(3f);
        }

        StartCoroutine(SendBeat());

    }


    private void UpdateTCPLoop(CentralControlDevice _device)
    {
        if (_device.deviceType == DeviceType.多媒体服务器)
        {
            heartbeatTcp_client.TCPSend(_device.PCDeviceIP, 3000, ValueSheet.MediaServerCmd[2]);
        }
        else if (_device.deviceType == DeviceType.LED电柜)
        {
            heartbeatTcp_client.TCPSenHex(_device.PCDeviceIP, 5000, ValueSheet.LEDCmd[2]);

        }
        else if (_device.deviceType == DeviceType.灯光)
        {
            string s = _device.LightID + " " + ValueSheet.LightCmd[2];

            string output = CRC.CRCCalc(s);

            string send = s + " " + output;

            heartbeatTcp_client.TCPSenHex(_device.PCDeviceIP, 28010, send);

        }
        else if (_device.deviceType == DeviceType.投影)
        {

            heartbeatTcp_client.TCPSenHex(_device.PCDeviceIP, ValueSheet.ProjectorCMD[_device.ProjectSerial].port, ValueSheet.ProjectorCMD[_device.ProjectSerial].read);
        }
    }

    private void dealwithTCPResult(string s)
    {
       // Debug.Log("心跳包返回值： "+s);

        if (mdevice.deviceType == DeviceType.多媒体服务器)
        {
            UpdateMediaServerDeviceSataus(mdevice, s);
        }else if(mdevice.deviceType == DeviceType.LED电柜)
        {
            UpdateLEDServerDeviceSataus(mdevice, s);
        }
        else if (mdevice.deviceType == DeviceType.灯光)
        {
            UpdateLightServerDeviceSataus(mdevice, s);
        }
        else if (mdevice.deviceType == DeviceType.投影)
        {
            UpdateProjectorServerDeviceSataus(mdevice, s);
        }
    }


    private void UpdateLightServerDeviceSataus(CentralControlDevice device, string s)
    {
        if (s.Length >= 10)
        {
           s = s.Substring(2, 8);       
        }
        device.status = MyUtility.Utility.convertLightServerStatus(s);
    }

    private void UpdateLEDServerDeviceSataus(CentralControlDevice device, string s)
    {
        device.status = MyUtility.Utility.convertLEDServerStatus(s);
    }


    private void UpdateMediaServerDeviceSataus(CentralControlDevice device,string s)
    {
        device.status = MyUtility.Utility.convertMediaServerStatus(s);
    }

    private void UpdateProjectorServerDeviceSataus(CentralControlDevice device, string s)
    {
        device.status = MyUtility.Utility.convertProjectorServerStatus(s,device);
    }


}
