using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CentralControlDevice : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    //jsonData
    public string ip;
    public string PCDeviceIP;
    public int DelayedSetStateus;
    public DeviceType deviceType;
    public int x;
    public int y;
    public string MName;
    public string LightID;
    public string ProjectSerial;
    //localData
    public Sprite sprite;
    public Text HintText;
    public int status=0;//statue=0关闭状态；statue=1开启状态;statue=2正在关闭状态;statue=运行;
    private bool ismove;
    //private bool interactivable=true;
    
 

    public void ini(string _LightID, DeviceType _deviceType, string _name, string _ip, int _x, int _y, Sprite _sprite,string _ProjectSerial= "PJLink") {
        LightID = _LightID;

        if (_deviceType != DeviceType.多媒体服务器)
        {
            PCDeviceIP = ip;
        }
        else
        {
            PCDeviceIP = _ip.Split('.')[0] + "." + _ip.Split('.')[1] + "." + (int.Parse(_ip.Split('.')[2]) + 19).ToString() + "." + _ip.Split('.')[3];
        }

        deviceType = _deviceType;
        MName = _name;
        ip = _ip;
        x = _x;
        y = _y;
        sprite = _sprite;
        ProjectSerial = _ProjectSerial;
    }

    private void iniHintText()
    {
        HintText = this.GetComponentInChildren<Text>();
        HintText.text = ip + "\n" + name;
        HintText.gameObject.SetActive(false);
    }

    private void UpdateHintText()
    {
        HintText.text = ip + "\n" + name;
    }

    private void iniPosition()
    {
        this.transform.position = new Vector2(x, y);
    }

    public void Start()
    {
        StartCoroutine(doingSthAfterCreate());
    }
    //update the shit 不能在线程中修改
    public void Update()
    {
    }

    public IEnumerator doingSthAfterCreate()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.name = MName;
        iniHintText();
        iniPosition();
        StartCoroutine(UpdateTheShit());
    }

    public IEnumerator UpdateTheShit()
    {
        yield return new WaitForSeconds(1f);
     
        GetComponent<Image>().sprite = MainCtr.instance.sprites[status];

        StartCoroutine(UpdateTheShit());
    }



    public void OpenDevice()
    {
        switch (deviceType)
        {
            case DeviceType.多媒体服务器:
               MediaServerDevice.openMediaServer(PCDeviceIP,this);
                break;
            case DeviceType.投影:
                ProjectorDevice.openProjectorServer(ip, this);
                break;
            case DeviceType.LED电柜:
                LEDDevice.openLEDServer(ip, this);
                break;
            case DeviceType.灯光:
                LightDevice.openLightServer(ip,this);
                break;
            default:
                break;
        }
    }

    public void CloseDevice()
    {
        switch (deviceType)
        {
            case DeviceType.多媒体服务器:
              MediaServerDevice.closeMediaServer(PCDeviceIP, this);
                break;
            case DeviceType.投影:
                ProjectorDevice.closeProjectorServer(ip, this);
                break;
            case DeviceType.LED电柜:
                LEDDevice.closeLEDServer(ip, this);
                break;
            case DeviceType.灯光:
                LightDevice.closeLightServer(ip, this);
                break;
            default:
                break;
        }
    }


    #region interaction
    public void OnPointerClick(PointerEventData eventData)
    {
        //在编辑模式下打开DEVICE编辑窗口
        if (ValueSheet.EditMode&&!ismove)
        {
            ValueSheet.currentCentralControlDevice = this;

            EditCtr.instance.turnOnGxiugai();
           // EditCtr.instance.Gxiugai.GetComponent<NodeEdit>().getValue();

        }
        //手动开关设备
        else if (!ValueSheet.EditMode && !ismove)
        {
            Debug.Log("点击控制节点");
            if (status == 0)
            {
                Debug.Log("打开控制设备");

                OpenDevice();
            }
            else if (status == 3)
            {
                Debug.Log("关闭控制设备");

                CloseDevice();
            }
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ValueSheet.EditMode)
        {
            ismove = true;
            ValueSheet.currentCentralControlDevice = this;
            this.transform.position = Input.mousePosition;
            ValueSheet.currentCentralControlDevice.x =(int) this.transform.position.x;
            ValueSheet.currentCentralControlDevice.y = (int)this.transform.position.y;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ValueSheet.EditMode)
        {
            ValueSheet.currentCentralControlDevice = this;

            this.transform.position = Input.mousePosition;
            ValueSheet.currentCentralControlDevice.x = (int)this.transform.position.x;
            ValueSheet.currentCentralControlDevice.y = (int)this.transform.position.y;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ValueSheet.EditMode)
        {
            ValueSheet.currentCentralControlDevice = this;
            ismove = false;
            this.transform.position = Input.mousePosition;
            ValueSheet.currentCentralControlDevice.x = (int)this.transform.position.x;
            ValueSheet.currentCentralControlDevice.y = (int)this.transform.position.y;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HintText.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateHintText();
        HintText.gameObject.SetActive(true);
    }
    #endregion
  
}
