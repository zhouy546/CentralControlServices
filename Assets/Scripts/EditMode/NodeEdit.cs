using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeEdit : MonoBehaviour
{
    public InputField nameInputField;

    public InputField IpaddressInputField;

    public Dropdown dropdownField;

    public Dropdown LightdropdownField;

    public void OnDisable()
    {
        
    }

    private void OnEnable()
    {
        getValue();
        if (ValueSheet.currentCentralControlDevice.deviceType == DeviceType.灯光)
        {
            LightdropdownField.gameObject.SetActive(true);
            foreach (var option in LightdropdownField.options)
            {
                string optionStr = option.text.Split('-')[1];
                if (optionStr == ValueSheet.currentCentralControlDevice.LightID)
                {
                    LightdropdownField.value = LightdropdownField.options.IndexOf(option);
                }
            }
        }
        else
        {
            LightdropdownField.gameObject.SetActive(false);
        }
     
    }

    public void DisableUI()
    {
        this.gameObject.SetActive(false);
    }

    public void DeleteNodeBtn()
    {
        ValueSheet.currentFloor.centralControlDevices.Remove(ValueSheet.currentCentralControlDevice);

        Destroy(ValueSheet.currentCentralControlDevice.gameObject, 0.5F);

        DisableUI();
    }

    public void SubmitBtn() {
        setValue();

        DisableUI();
    }
    

    public void getValue()
    {
        nameInputField.text = ValueSheet.currentCentralControlDevice.MName;

        IpaddressInputField.text = ValueSheet.currentCentralControlDevice.ip;

        if(ValueSheet.currentCentralControlDevice.deviceType == DeviceType.多媒体服务器)
        {
            dropdownField.value = 0;
        }
        else if (ValueSheet.currentCentralControlDevice.deviceType == DeviceType.LED电柜)
        {
            dropdownField.value = 2;
        }
        else if (ValueSheet.currentCentralControlDevice.deviceType == DeviceType.投影)
        {
            dropdownField.value = 1;
        }
        else if (ValueSheet.currentCentralControlDevice.deviceType == DeviceType.灯光)
        {
            dropdownField.value = 3;
        }
    }

    private void setValue()
    {
        ValueSheet.currentCentralControlDevice.name= ValueSheet.currentCentralControlDevice.MName = nameInputField.text;
        
        ValueSheet.currentCentralControlDevice.ip = IpaddressInputField.text;
        if (ValueSheet.currentCentralControlDevice.deviceType== DeviceType.多媒体服务器)
        {
            string _ip = ValueSheet.currentCentralControlDevice.ip;

            ValueSheet.currentCentralControlDevice.PCDeviceIP = _ip.Split('.')[0] + "." + _ip.Split('.')[1] + "." + (int.Parse(_ip.Split('.')[2]) + 19).ToString() + "." + _ip.Split('.')[3];

        }

        if (dropdownField.value == 0)
        {
            ValueSheet.currentCentralControlDevice.deviceType = DeviceType.多媒体服务器;
        }
        else if (dropdownField.value == 2)
        {

            ValueSheet.currentCentralControlDevice.deviceType = DeviceType.LED电柜;

            ValueSheet.currentCentralControlDevice.PCDeviceIP= ValueSheet.currentCentralControlDevice.ip;
        }

        else if (dropdownField.value == 1)
        {

            ValueSheet.currentCentralControlDevice.deviceType = DeviceType.投影;

            ValueSheet.currentCentralControlDevice.PCDeviceIP = ValueSheet.currentCentralControlDevice.ip;

        }
        else if (dropdownField.value == 3)
        {

            ValueSheet.currentCentralControlDevice.deviceType = DeviceType.灯光;

            ValueSheet.currentCentralControlDevice.PCDeviceIP = ValueSheet.currentCentralControlDevice.ip;

        }
    }

    public void OnLightDropDonwValueChange()
    {
        Debug.Log("dropdownField.value: " + LightdropdownField.value);
        Debug.Log("对应字符串: " + LightdropdownField.options[LightdropdownField.value].text);
        ValueSheet.currentCentralControlDevice.LightID = LightdropdownField.options[LightdropdownField.value].text.Split('-')[1];
    }
}
