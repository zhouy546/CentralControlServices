using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValueSheet
{
    public static string appid = "";
    public static string password = "";


    public static int reslutionX=1280;
    public static int reslutionY=720;
    public static string ProgramName = "CentralControlServices";

    public static int heartBeatSendWaitTime=2000;
    public static int TcpReceiveWaitTime = 2000;
    public static int TcpSendWaitTime = 1000;
    public static bool EditMode = false;
    public static floor currentFloor;
    public static CentralControlDevice currentCentralControlDevice;
    public static CentralControlServices centralcontrolServices;
    public static CentralControlServices_JSON ReadJsoncentralcontrolServices;
    public static string defaultBGURL = Application.streamingAssetsPath + "/defaultBG.png";
    #region 多媒体服务器收发指令
    public static string[] MediaServerCmd = { "start", "stop", "read" };
    public static string[] MediaServerReceiveCmd = { "running", "stopping", "start" , "stopped" };
    #endregion
    #region LED配电柜收发指令
    public static string[] LEDCmd = { "00000000000601050801FF00"/*on*/, "00000000000601050802FF00"/*off*/, "000000000006010105080001" /*read*/};
    public static string[] ByTenLEDReceiveCmd = { "00000000000601050801FF00"/*发送on后回值*/, "00000000000601050802FF00"/*发送off回值*/, "00000000000401010101"/*发送Read后--开启--状态值*/, "00000000000401010100"/*发送Read后--关闭--状态值*/ };//回值状态
    #endregion
    #region 灯光收发
    public static string[] LightCmd = { "06 00 02 00 01"/*ON*/, "06 00 01 00 00"/*OFF*/,"03 00 03 00 01"/*READ*/};
    public static string[] LightReceiveCmd = { "03020001"/*READ回值开*/, "03020000"/*READ回值关*/, "0600020001"/*发送开后回值*/, "0600010000"/*发送关后回值*/};
    #endregion
    #region 投影机器收发
    public static Dictionary<string, ProjectorSerial_JSON> ProjectorCMD = new Dictionary<string, ProjectorSerial_JSON>();
    #endregion
}




public enum DeviceType { 多媒体服务器,投影,LED电柜,灯光 }

public enum DeviceStatus { 关闭,开启中,开启,关闭中}


public class CentralControlServices_JSON
{
    public List<Floor_JSON> floors = new List<Floor_JSON>();
    public CentralControlServices_JSON(List<Floor_JSON> _floors)
    {
        floors = _floors;
    }
}

public class Floor_JSON
{
    public int pageindex;
    public string bgUrl;
    public List<CentralControlDevice_JSON> centralControlDevices = new List<CentralControlDevice_JSON>();

    public Floor_JSON(int _index)
    {
        pageindex = _index;
    }

    public Floor_JSON(int _pageindex, List<CentralControlDevice_JSON> _centralControlDevices, string _bgUrl)
    {
        pageindex = _pageindex;
        centralControlDevices = _centralControlDevices;
        bgUrl = _bgUrl;
    }
}

public class CentralControlDevice_JSON {

    public string ip;
    public string PCDeviceIP;
    public int DelayedSetStateus;
    public int deviceType;
    public int x;
    public int y;
    public string Name;
    public string LightID;
    public string ProjectorSerial;



    public CentralControlDevice_JSON(string _ip, string _PCDeviceIP, int _DelayedSetStateus, int _deviceType, int _x, int _y, string _Name, string _LightID, string _ProjectorSerial = "PJLink")
    {
        ip = _ip;
        PCDeviceIP = _PCDeviceIP;
        DelayedSetStateus = _DelayedSetStateus;
        deviceType = _deviceType;
        x = _x;
        y = _y;
        Name = _Name;
        LightID = _LightID;
        ProjectorSerial = _ProjectorSerial;
    }
}

public class ProjectorSerial_JSON
{
    public string name;

    public string open;

    public string close;

    public string read;

    public string receiveon;

    public string receiveoff;

    public string powerok;

    public int port;


    public ProjectorSerial_JSON(string _name,string _open, string _close, string _read,int _port,string _receiveon,string _receiveoff,string _powerok)
    {
        name = _name;

        open = _open;

        close = _close;

        read = _read;

        port = _port;

        receiveon = _receiveon;

        receiveoff = _receiveoff;

        powerok = _powerok;
    }
}


