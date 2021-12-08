using MyUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(OpenAllDevices());
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(CloseDevice());
        }
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

        foreach (CentralControlDevice device in ValueSheet.currentFloor.centralControlDevices)
        {
            ValueSheet.currentCentralControlDevice = device;

            if (device.deviceType == DeviceType.灯光) {

                device.CloseDevice();
                yield return new WaitForSeconds(2.5f);
            }


        }

    }

    private IEnumerator IETurnOnFloorLight()
    {

        foreach (CentralControlDevice device in ValueSheet.currentFloor.centralControlDevices)
        {
            ValueSheet.currentCentralControlDevice = device;

            if (device.deviceType == DeviceType.灯光) { 
                device.OpenDevice();
                yield return new WaitForSeconds(2.5f);
            }

      


        }

    }
    private IEnumerator OpenAllDevices()
    {
        foreach (floor _floor in ValueSheet.centralcontrolServices.floors)
        {
            foreach (CentralControlDevice device in _floor.centralControlDevices)
            {
                ValueSheet.currentCentralControlDevice = device;

                device.OpenDevice();

                yield return new WaitForSeconds(2.5f);


            }
        }
    }

    private IEnumerator CloseDevice()
    {
        foreach (floor _floor in ValueSheet.centralcontrolServices.floors)
        {
            foreach (CentralControlDevice device in _floor.centralControlDevices)
            {
                ValueSheet.currentCentralControlDevice = device;

                device.CloseDevice();

                yield return new WaitForSeconds(2.5f);


            }
        }
    }
}
