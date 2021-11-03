using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCtr : MonoBehaviour
{
    public GameObject G_floor;
    public GameObject G_CentralControlDevice;
   

    public Sprite[] sprites;
    public static MainCtr instance;



    public void Awake()
    {
        instance = this;
        EventCenter.AddListener(EventDefine.inifromJson, IniCanvasFromJson);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void IniCanvasFromJson()
    {
        CentralControlServices_JSON ccs_json = ValueSheet.ReadJsoncentralcontrolServices;

        this.gameObject.AddComponent<CentralControlServices>();

        ValueSheet.centralcontrolServices = this.gameObject.GetComponent<CentralControlServices>();
      
        for (int i = 0; i < ccs_json.floors.Count; i++)
        {
            GameObject gfloor = Instantiate(G_floor, this.transform);

            gfloor.AddComponent<floor>();

            floor _floor = gfloor.GetComponent<floor>();

            ValueSheet.centralcontrolServices.floors.Add(_floor);

            //_floor.bgUrl = ccs_json.floors[i].bgUrl;

            if (i == 0)
            {
                ValueSheet.currentFloor = _floor;
            }

            List<CentralControlDevice> centralControlDevices = new List<CentralControlDevice>();

            for (int j = 0; j < ccs_json.floors[i].centralControlDevices.Count; j++)
            {
                string _ip = ccs_json.floors[i].centralControlDevices[j].ip;
                string _PCDeviceIP = ccs_json.floors[i].centralControlDevices[j].PCDeviceIP;
                int _DelayedSetStateus = ccs_json.floors[i].centralControlDevices[j].DelayedSetStateus;
                DeviceType _deviceType = MyUtility.Utility.GetDeviceTypebyInt(ccs_json.floors[i].centralControlDevices[j].deviceType);

               // Debug.Log("读取的设备类型" + _deviceType);
                int _x = ccs_json.floors[i].centralControlDevices[j].x;
                int _y = ccs_json.floors[i].centralControlDevices[j].y;
                string _name = ccs_json.floors[i].centralControlDevices[j].Name;
                string _LightID = ccs_json.floors[i].centralControlDevices[j].LightID.ToString();

                CentralControlDevice device = new CentralControlDevice();

                device.ini(_LightID, _deviceType, _name, _ip, _x, _y, sprites[0]);

                centralControlDevices.Add(device);              
            }
            _floor.ini(i, ccs_json.floors[i].bgUrl, G_CentralControlDevice, centralControlDevices);
        }
        SetValue(ccs_json);
    }

    private void SetValue(CentralControlServices_JSON _ccs_json)
    {
        for (int i = 0; i < _ccs_json.floors.Count; i++)
        {
            ValueSheet.centralcontrolServices.floors[i].pageindex= _ccs_json.floors[i].pageindex;

            ValueSheet.centralcontrolServices.floors[i].bgUrl = _ccs_json.floors[i].bgUrl;

            for (int j = 0; j < _ccs_json.floors[i].centralControlDevices.Count; j++)
            {
                ValueSheet.centralcontrolServices.floors[i].centralControlDevices[j].ip = _ccs_json.floors[i].centralControlDevices[j].ip;
                ValueSheet.centralcontrolServices.floors[i].centralControlDevices[j].PCDeviceIP = _ccs_json.floors[i].centralControlDevices[j].PCDeviceIP;
                ValueSheet.centralcontrolServices.floors[i].centralControlDevices[j].DelayedSetStateus = _ccs_json.floors[i].centralControlDevices[j].DelayedSetStateus;
                ValueSheet.centralcontrolServices.floors[i].centralControlDevices[j].deviceType =MyUtility.Utility.GetDeviceTypebyInt(_ccs_json.floors[i].centralControlDevices[j].deviceType);
                ValueSheet.centralcontrolServices.floors[i].centralControlDevices[j].x = _ccs_json.floors[i].centralControlDevices[j].x;
                ValueSheet.centralcontrolServices.floors[i].centralControlDevices[j].y = _ccs_json.floors[i].centralControlDevices[j].y;
                ValueSheet.centralcontrolServices.floors[i].centralControlDevices[j].MName = _ccs_json.floors[i].centralControlDevices[j].Name;
                ValueSheet.centralcontrolServices.floors[i].centralControlDevices[j].LightID = _ccs_json.floors[i].centralControlDevices[j].LightID;
            }
        }

        setFloorLinkedList(ValueSheet.centralcontrolServices);
    }

    public void setFloorLinkedList(CentralControlServices centralControlServices)
    {
        for (int i = 0; i < centralControlServices.floors.Count; i++)
        {
            if(i+1 == centralControlServices.floors.Count&& i != 0)
            {
                Debug.Log("LinkedList最后一个");
                centralControlServices.floors[i].next = null;
                centralControlServices.floors[i].pervious = centralControlServices.floors[i - 1];
            }
            else if (i + 1 == centralControlServices.floors.Count && i == 0)
            {
                Debug.Log("LinkedList只有一个");
                centralControlServices.floors[i].next = null;
                centralControlServices.floors[i].pervious =null;
            }

            else if (i==0 && (i + 1) != centralControlServices.floors.Count)
            {
                Debug.Log("LinkedList第一个");
                centralControlServices.floors[i].next = centralControlServices.floors[i + 1];
                centralControlServices.floors[i].pervious = null;

            }
            else if(i!=0&& (i + 1) != centralControlServices.floors.Count)
            {
                Debug.Log("LinkedList中间那些");
                centralControlServices.floors[i].next = centralControlServices.floors[i + 1];
                centralControlServices.floors[i].pervious = centralControlServices.floors[i - 1];
            }
        }
    }

    public void createDefaultCanvas()
    {
        this.gameObject.AddComponent<CentralControlServices>();

        ValueSheet.centralcontrolServices = this.gameObject.GetComponent<CentralControlServices>();

        GameObject gfloor = Instantiate(G_floor, this.transform);

        gfloor.AddComponent<floor>();

        floor _floor = gfloor.GetComponent<floor>();

        ValueSheet.currentFloor = _floor;

        ValueSheet.centralcontrolServices.floors.Add(_floor);

        List<CentralControlDevice> centralControlDevices = new List<CentralControlDevice>();

        CentralControlDevice device = new CentralControlDevice();

        device.ini("03", DeviceType.多媒体服务器, "多媒体服务", "192.168.1.1*", 0, 0, sprites[0]);

        centralControlDevices.Add(device);

        //    Debug.Log(centralControlDevices.Count);

        _floor.ini(0, ValueSheet.defaultBGURL, G_CentralControlDevice, centralControlDevices);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
