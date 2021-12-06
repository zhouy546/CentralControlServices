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
            yield return new WaitForSeconds(2.5f);

            if (device.deviceType == DeviceType.灯光) { device.CloseDevice(); }
            
        }

    }

    private IEnumerator IETurnOnFloorLight()
    {

        foreach (CentralControlDevice device in ValueSheet.currentFloor.centralControlDevices)
        {
            yield return new WaitForSeconds(2.5f);

            if (device.deviceType == DeviceType.灯光) { device.OpenDevice(); }

        }

    }
    private IEnumerator OpenAllDevices()
    {
        foreach (floor _floor in ValueSheet.centralcontrolServices.floors)
        {
            foreach (CentralControlDevice device in _floor.centralControlDevices)
            {
                yield return new WaitForSeconds(2.5f);

                device.OpenDevice();

            }
        }
    }

    private IEnumerator CloseDevice()
    {
        foreach (floor _floor in ValueSheet.centralcontrolServices.floors)
        {
            foreach (CentralControlDevice device in _floor.centralControlDevices)
            {
                yield return new WaitForSeconds(2.5f);

                device.CloseDevice();

            }
        }
    }
}
