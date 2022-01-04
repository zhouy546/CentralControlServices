using MyUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject DoNotInteractUI;

    public Image barFillImage;
    public void Start()
    {
        EventCenter.AddListener(EventDefine.OnAllStart, OnAllStart);


        EventCenter.AddListener(EventDefine.OnAllEnd, OnAllEnd);
        //EventCenter.AddListener(EventDefine.OffAll,);
    }


    private void OnAllStart()
    {
        HeartBeatCtr.instance.isHoldHeartBeats = true;

        DoNotInteractUI.SetActive(true);
    }

    private void OnAllEnd()
    {
        HeartBeatCtr.instance.isHoldHeartBeats = false;

        DoNotInteractUI.SetActive(false);
    }



    public void Update()
    {

    }


    public void CloseAll()
    {
        StartCoroutine(CloseDevice());
    }

    public void OpenAll()
    {
        StartCoroutine(OpenAllDevices());
    }

    public void TurnOffFloorLight()
    {
        StartCoroutine(IETurnOffFloorLight());
    }

    public void TurnOnFloorLight()
    {
        StartCoroutine(IETurnOnFloorLight());
    }
    private IEnumerator IETurnOffFloorLight()
    {

        EventCenter.Broadcast(EventDefine.OnAllStart);

        float index = 0;

        float deviceNum = 0;
        foreach (CentralControlDevice device in ValueSheet.currentFloor.centralControlDevices)
        {
            if (device.deviceType == DeviceType.灯光)
            {
                deviceNum++;
            }
        }

        foreach (CentralControlDevice device in ValueSheet.currentFloor.centralControlDevices)
        {
            ValueSheet.currentCentralControlDevice = device;

            if (device.deviceType == DeviceType.灯光) {

                index++;

                barFillImage.fillAmount = index / deviceNum;

                device.CloseDevice();
                yield return new WaitForSeconds(2.5f);
            }
        }

        EventCenter.Broadcast(EventDefine.OnAllEnd);

    }

    private IEnumerator IETurnOnFloorLight()
    {

        EventCenter.Broadcast(EventDefine.OnAllStart);


        float index = 0;

        float deviceNum = 0;
        foreach (CentralControlDevice device in ValueSheet.currentFloor.centralControlDevices)
        {
            if (device.deviceType == DeviceType.灯光)
            {
                deviceNum++;
            }
        }

        foreach (CentralControlDevice device in ValueSheet.currentFloor.centralControlDevices)
        {
            ValueSheet.currentCentralControlDevice = device;

            if (device.deviceType == DeviceType.灯光) {

                index++;

                barFillImage.fillAmount = index / deviceNum;

                device.OpenDevice();
                yield return new WaitForSeconds(2.5f);
            }
        }

        EventCenter.Broadcast(EventDefine.OnAllEnd);
    }
    private IEnumerator OpenAllDevices()
    {
        EventCenter.Broadcast(EventDefine.OnAllStart);

        float index = 0;

        foreach (floor _floor in ValueSheet.centralcontrolServices.floors)
        {
            foreach (CentralControlDevice device in _floor.centralControlDevices)
            {
                ValueSheet.currentCentralControlDevice = device;

                index++;

                //Debug.Log(index / FindObjectsOfType<CentralControlDevice>().Length);

                barFillImage.fillAmount = index / FindObjectsOfType<CentralControlDevice>().Length;

                device.OpenDevice();

                yield return new WaitForSeconds(2.5f);


            }
        }
        EventCenter.Broadcast(EventDefine.OnAllEnd);

    }

    private IEnumerator CloseDevice()
    {
        EventCenter.Broadcast(EventDefine.OnAllStart);

        float index = 0;

        foreach (floor _floor in ValueSheet.centralcontrolServices.floors)
        {
            foreach (CentralControlDevice device in _floor.centralControlDevices)
            {
                ValueSheet.currentCentralControlDevice = device;

                index++;

                barFillImage.fillAmount = index / FindObjectsOfType<CentralControlDevice>().Length;

                device.CloseDevice();

                yield return new WaitForSeconds(2.5f);


            }
        }
        EventCenter.Broadcast(EventDefine.OnAllEnd);

    }
}
