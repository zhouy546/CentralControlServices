using MyUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Text;

public class HeartbeatSystem : MonoBehaviour
{
   [SerializeField]
    private CentralControlDevice mdevice;

    Threadtcp tcp_thread;

  [SerializeField]
    private int currentFloor;
    [SerializeField]
    private int currentDevice;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener(EventDefine.ini, SendBeat);

        EventCenter.AddListener(EventDefine.HeartBeatGoNext, SendBeat);

        EventCenter.AddListener(EventDefine.HeartBeatGoNext, updateGoNextDeviceAndFloorIndex);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            SendBeat();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            tcp_thread.t.Abort();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Button J Pressed");
            CentralControlDevice device = ValueSheet.centralcontrolServices.floors[0].centralControlDevices[3];
            Threadtcp tcp_thread = new Threadtcp(device.PCDeviceIP, 5000, ValueSheet.LEDCmd[2], device, true);
            tcp_thread.sendHexString();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            CentralControlDevice device = ValueSheet.centralcontrolServices.floors[0].centralControlDevices[4];

            string s = device.LightID + " " + ValueSheet.LightCmd[2];

            //byte[] bytes = { 03,06,00,01,00,00 };
            string output = CRC.CRCCalc(s);

            string send = s + " " + output;

            Debug.Log("键盘i Press发送" + send);

            tcp_thread = new Threadtcp("192.168.0.7", 28010, send, device, false);

            tcp_thread.sendHexString();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            CentralControlDevice device = ValueSheet.centralcontrolServices.floors[0].centralControlDevices[5];

            string s = device.LightID + " " + ValueSheet.LightCmd[2];

            //byte[] bytes = { 03,06,00,01,00,00 };
            string output = CRC.CRCCalc(s);

            string send = s + " " + output;

            Debug.Log("键盘o Press发送" + send);

            tcp_thread = new Threadtcp("192.168.0.7", 28010, send, device, false);

            tcp_thread.sendHexString();

        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CentralControlDevice device = ValueSheet.centralcontrolServices.floors[0].centralControlDevices[6];

            string s = device.LightID + " " + ValueSheet.LightCmd[2];

            //byte[] bytes = { 03,06,00,01,00,00 };
            string output = CRC.CRCCalc(s);

            string send = s + " " + output;

            Debug.Log("键盘p Press发送" + send);

            tcp_thread = new Threadtcp("192.168.0.7", 28010, send, device, false);

            tcp_thread.sendHexString();

        }
    }
     
    private void OnApplicationQuit()
    {
        if (tcp_thread != null)
        {
            tcp_thread.t.Abort();
        }
    }


    void SendBeat()
    {
        Debug.Log("running");

        CentralControlDevice device = ValueSheet.centralcontrolServices.floors[currentFloor].centralControlDevices[currentDevice];


        if (!Utility.checkIp(device.PCDeviceIP))
        {
            device.status = 4;//等于Fault状态域名不准确的
        }
            UpdateTCPLoop(device);

    }


    private void UpdateTCPLoop(CentralControlDevice _device)
    {
        EventCenter.AddListener<CentralControlDevice, string>(EventDefine.HeartbeatTcpResult, dealwithTCPResult);
        if (_device.deviceType == DeviceType.多媒体服务器)
        {
            tcp_thread = new Threadtcp(_device.PCDeviceIP, 3000, ValueSheet.MediaServerCmd[2], _device, true);
            tcp_thread.sendDefaultString();
        }
        else if (_device.deviceType == DeviceType.LED电柜)
        {
            //Debug.Log("心跳包给配电柜发送的IP值："+_device.PCDeviceIP);

                tcp_thread = new Threadtcp(_device.PCDeviceIP, 5000, ValueSheet.LEDCmd[2], _device, true);
                tcp_thread.sendHexString();

        }
        else if (_device.deviceType == DeviceType.灯光)
        {
            Debug.Log("心跳包给灯光照明发送的IP值：" + _device.PCDeviceIP);


            string s = _device.LightID + " " + ValueSheet.LightCmd[2];

            string output = CRC.CRCCalc(s);

            string send = s + " " + output;

            Debug.Log("心跳包给灯光照明发送的值：" + send);

            tcp_thread = new Threadtcp(_device.PCDeviceIP, 28010, send, _device, true);

            tcp_thread.sendHexString();

        }
    }



    private void updateGoNextDeviceAndFloorIndex()
    {


        if (currentDevice < ValueSheet.centralcontrolServices.floors[currentFloor].centralControlDevices.Count - 1)
        {
            currentDevice++;
        }
        else
        {
            currentDevice = 0;
            if (currentFloor < ValueSheet.centralcontrolServices.floors.Count - 1)
            {
                Debug.Log("心跳包增加楼层");

                currentFloor++;

                if (ValueSheet.centralcontrolServices.floors[currentFloor].centralControlDevices.Count == 0)
                {
                    currentFloor = 0;
                }

            }
            else
            {
                currentFloor = 0;
            }
            currentDevice = 0;
        }
    }


    private void dealwithTCPResult(CentralControlDevice device, string s)
    {
        if (device.deviceType == DeviceType.多媒体服务器)
        {
            UpdateMediaServerDeviceSataus(device,s);
        }else if(device.deviceType == DeviceType.LED电柜)
        {
            UpdateLEDServerDeviceSataus(device, s);
        }
        else if (device.deviceType == DeviceType.灯光)
        {
            UpdateLightServerDeviceSataus(device, s);
        }

        EventCenter.RemoveListener<CentralControlDevice, string>(EventDefine.HeartbeatTcpResult, dealwithTCPResult);

        Debug.Log("GoNext");
        EventCenter.Broadcast(EventDefine.HeartBeatGoNext);

    }


    private void UpdateLightServerDeviceSataus(CentralControlDevice device, string s)
    {
        if (s.Length >= 10)
        {
           s = s.Substring(2, 8);       
        }
        Debug.Log("curString:_" + s);
        Debug.Log("心跳包状态：" + s + "    " + "ip地址：" + device.ip + "  Name" + device.MName + "  " + device.status.ToString());
        device.status = Utility.convertLightServerStatus(s);
    }

    private void UpdateLEDServerDeviceSataus(CentralControlDevice device, string s)
    {
    //    Debug.Log("配电柜返回值:"+ s);
        device.status = Utility.convertLEDServerStatus(s);
   //     Debug.Log("心跳包状态：" + s + "    " + "ip地址：" + device.ip + "  Name" + device.MName + "  " + device.status.ToString());

    }


    private void UpdateMediaServerDeviceSataus(CentralControlDevice device,string s)
    {

        device.status = Utility.convertMediaServerStatus(s);
      //  Debug.Log("心跳包状态："+s+"    " +"ip地址："+device.ip+"  Name" +device.MName + "  "+ device.status.ToString());

    }


}
