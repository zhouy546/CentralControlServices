using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeEdit : MonoBehaviour
{
    public InputField nameInputField;

    public InputField IpaddressInputField;

    public Dropdown dropdownField;

    public List<Dropdown>  LPdropdowns = new List<Dropdown>();

    public Dropdown LightdropdownField;

    public Dropdown ProjectorDropDownField;


    public void OnDisable()
    {
        
    }

    private void OnEnable()
    {
        getValue();
        if (ValueSheet.currentCentralControlDevice.deviceType == DeviceType.灯光)
        {
            OndropDown(LightdropdownField);
            foreach (var option in LightdropdownField.options)
            {
                string optionStr = option.text.Split('-')[1];
                if (optionStr == ValueSheet.currentCentralControlDevice.LightID)
                {
                    LightdropdownField.value = LightdropdownField.options.IndexOf(option);
                }
            }
        }
        else if(ValueSheet.currentCentralControlDevice.deviceType == DeviceType.投影)
        {
            OndropDown(ProjectorDropDownField);

            foreach (var option in ProjectorDropDownField.options)
            {
                string optionStr = option.text.Split('-')[1];
                if (optionStr == ValueSheet.currentCentralControlDevice.ProjectSerial)
                {
                    ProjectorDropDownField.value = ProjectorDropDownField.options.IndexOf(option);
                }
            }
        }
        else
        {
            offAllDropDown();
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
            offAllDropDown();
        }
        else if (ValueSheet.currentCentralControlDevice.deviceType == DeviceType.LED电柜)
        {
            dropdownField.value = 2;
            offAllDropDown();
        }
        else if (ValueSheet.currentCentralControlDevice.deviceType == DeviceType.投影)
        {
            dropdownField.value = 1;
            OndropDown(ProjectorDropDownField);
        }
        else if (ValueSheet.currentCentralControlDevice.deviceType == DeviceType.灯光)
        {
            dropdownField.value = 3;
            OndropDown(LightdropdownField);

        }
    }

    public void setValue()
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

            offAllDropDown();
        }
        else if (dropdownField.value == 2)
        {

            ValueSheet.currentCentralControlDevice.deviceType = DeviceType.LED电柜;

            ValueSheet.currentCentralControlDevice.PCDeviceIP= ValueSheet.currentCentralControlDevice.ip;

            offAllDropDown();
        }

        else if (dropdownField.value == 1)
        {

            ValueSheet.currentCentralControlDevice.deviceType = DeviceType.投影;

            ValueSheet.currentCentralControlDevice.PCDeviceIP = ValueSheet.currentCentralControlDevice.ip;

            OndropDown(ProjectorDropDownField);

        }
        else if (dropdownField.value == 3)
        {

            ValueSheet.currentCentralControlDevice.deviceType = DeviceType.灯光;

            ValueSheet.currentCentralControlDevice.PCDeviceIP = ValueSheet.currentCentralControlDevice.ip;

            OndropDown(LightdropdownField);

        }
    }

    public void OnLightDropDonwValueChange()
    {
        Debug.Log("dropdownField.value: " + LightdropdownField.value);
        Debug.Log("对应字符串: " + LightdropdownField.options[LightdropdownField.value].text);
        ValueSheet.currentCentralControlDevice.LightID = LightdropdownField.options[LightdropdownField.value].text.Split('-')[1];
    }

    public void OnProjectorDropDonwValueChange()
    {
        Debug.Log("ProjectorDropDownField.value: " + ProjectorDropDownField.value);
        Debug.Log("对应字符串: " + ProjectorDropDownField.options[ProjectorDropDownField.value].text);
        ValueSheet.currentCentralControlDevice.ProjectSerial = ProjectorDropDownField.options[ProjectorDropDownField.value].text.Split('-')[1];
    }


    private void offAllDropDown()
    {
        foreach (Dropdown item in LPdropdowns)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void OndropDown(Dropdown _dropdown)
    {
        foreach (Dropdown item in LPdropdowns)
        {
            
            if (item == _dropdown)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
