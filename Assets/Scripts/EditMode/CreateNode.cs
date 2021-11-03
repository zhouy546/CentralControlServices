using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener(EventDefine.NewNode, newNode);
    }

    public void newNode()
    {
        CentralControlDevice _device = new CentralControlDevice();

        _device.ini("03", DeviceType.多媒体服务器, "多媒体服务", "192.168.1.1*", 500, 500, MainCtr.instance.sprites[0]);

        GameObject GCentralControlDevice = Instantiate(MainCtr.instance.G_CentralControlDevice,ValueSheet.currentFloor.transform);

        GCentralControlDevice.AddComponent<CentralControlDevice>();

        CentralControlDevice TempCentralControlDevice = GCentralControlDevice.GetComponent<CentralControlDevice>();

        TempCentralControlDevice.ini(_device.LightID, _device.deviceType, _device.MName, _device.ip, _device.x, _device.y, _device.sprite);

        ValueSheet.currentFloor.centralControlDevices.Add(TempCentralControlDevice);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
