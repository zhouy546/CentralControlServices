using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeatCtr : MonoBehaviour
{
    public static HeartBeatCtr instance;

    public bool isHoldHeartBeats;

    public GameObject heartBeat;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        EventCenter.AddListener(EventDefine.ini, INI);
    }

    public void INI()
    {
        StartCoroutine(ini());
    }

    private IEnumerator ini()
    {
        yield return new WaitForSeconds(2f);

        foreach (floor floor in ValueSheet.centralcontrolServices.floors)
        {
            foreach (CentralControlDevice device in floor.centralControlDevices)
            {
                ValueSheet.devices.Add(device);
            }
        }

        List<CentralControlDevice> tempdevices = new List<CentralControlDevice>();

        for (int i = 0; i < ValueSheet.devices.Count; i++)
        {
            tempdevices.Add(ValueSheet.devices[i]);
            if ((i+1) % 10 == 0)
            {
                List<CentralControlDevice> tempnewdevice = new List<CentralControlDevice>();

                tempnewdevice.AddRange(tempdevices);

                HeartbeatSystem M_HeartbeatSystem = Instantiate(heartBeat, this.transform).GetComponent<HeartbeatSystem>();

                M_HeartbeatSystem.beatsLoopDevices = tempnewdevice;

                tempdevices.Clear();

                yield return new WaitForSeconds(0.5f);

                //M_HeartbeatSystem.INI();
            }
        }

        if (tempdevices.Count > 0)
        {
            HeartbeatSystem heartbeatSystem = Instantiate(heartBeat, this.transform).GetComponent<HeartbeatSystem>();

            heartbeatSystem.beatsLoopDevices = tempdevices;

            yield return new WaitForSeconds(0.5f);

            //heartbeatSystem.INI();
        }

        


        //foreach (floor floor in ValueSheet.centralcontrolServices.floors)
        //{
        //    List<CentralControlDevice> tempDevices = new List<CentralControlDevice>();
        //    foreach (CentralControlDevice device in floor.centralControlDevices)
        //    {
        //        tempDevices.Add(device);
        //    }
        //    ValueSheet.centralControlDevices.Add(tempDevices);

        //    HeartbeatSystem heartbeatSystem =  Instantiate(heartBeat,this.transform).GetComponent<HeartbeatSystem>();

        //    heartbeatSystem.beatsLoopDevices = tempDevices;
        //}
        yield return new WaitForSeconds(1f);
        EventCenter.Broadcast(EventDefine.HeartBeatStart);
    }

}
