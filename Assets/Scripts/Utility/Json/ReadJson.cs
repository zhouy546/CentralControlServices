using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadJson : MonoBehaviour
{

    private JsonData itemDate;

    private string jsonString;

    public void Awake()
    {
    //    EventCenter.AddListener(EventDefine.resetCurrentLou, resetCurrentLou);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(INI());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator INI()
    {
        yield return StartCoroutine(readProjectSerial());

        yield return StartCoroutine(readJson());

    }

    IEnumerator readProjectSerial()
    {
        string spath = Application.streamingAssetsPath + "/ProjectSerial.json";
        FileInfo info = new FileInfo(spath);

        if (!info.Exists)
        {
            Debug.Log("未找到配置文件");

        }
        else
        {
            WWW www = new WWW(spath);

            yield return www;

            string _jsonString = System.Text.Encoding.UTF8.GetString(www.bytes);

            JsonMapper.ToObject(www.text);

            JsonData _itemDate = JsonMapper.ToObject(_jsonString.ToString());

            for (int i = 0; i < _itemDate["ProjectorSerial"].Count; i++)
            {
                string name = _itemDate["ProjectorSerial"][i]["name"].ToString();

                string open = _itemDate["ProjectorSerial"][i]["open"].ToString();

                string close = _itemDate["ProjectorSerial"][i]["close"].ToString();

                string read = _itemDate["ProjectorSerial"][i]["read"].ToString();

                string receiveon = _itemDate["ProjectorSerial"][i]["receiveon"].ToString();

                string receiveoff = _itemDate["ProjectorSerial"][i]["receiveoff"].ToString();

                string powerok = _itemDate["ProjectorSerial"][i]["powerok"].ToString();

                int port =int.Parse(_itemDate["ProjectorSerial"][i]["port"].ToString());

                ValueSheet.ProjectorCMD.Add(name, new ProjectorSerial_JSON(name, open, close, read, port, receiveon, receiveoff, powerok));
            }
        }
    }

    IEnumerator readJson()
    {
        string spath = Application.streamingAssetsPath + "/information.json";
        FileInfo info = new FileInfo(spath);
        if (!info.Exists)
        {
            Debug.Log("未找到配置文件，重新生成默认");
            MainCtr.instance.createDefaultCanvas();
            EventCenter.Broadcast(EventDefine.inifromNoJson);
        }
        else
        {
            Debug.Log("找到配置文件，从Json载入");
           

            WWW www = new WWW(spath);

            yield return www;

            jsonString = System.Text.Encoding.UTF8.GetString(www.bytes);

            JsonMapper.ToObject(www.text);

            itemDate = JsonMapper.ToObject(jsonString.ToString());

            List<Floor_JSON> jsonfloor = new List<Floor_JSON>();
            for (int i = 0; i < itemDate[0]["floors"].Count; i++)
            {
              
                int pageindex = i;
              
                Floor_JSON floor_JSON = new Floor_JSON(pageindex);

                floor_JSON.bgUrl = itemDate[0]["floors"][i]["bgUrl"].ToString();

                for (int k = 0; k < itemDate[0]["floors"][i]["centralControlDevices"].Count; k++)
                {
                    string ip = itemDate[0]["floors"][i]["centralControlDevices"][k]["ip"].ToString();

                    string PCDeviceIP = itemDate[0]["floors"][i]["centralControlDevices"][k]["PCDeviceIP"].ToString();

                    int DelayedSetStateus = int.Parse(itemDate[0]["floors"][i]["centralControlDevices"][k]["DelayedSetStateus"].ToString());
                    int deviceType = int.Parse(itemDate[0]["floors"][i]["centralControlDevices"][k]["deviceType"].ToString());
                    int x = int.Parse(itemDate[0]["floors"][i]["centralControlDevices"][k]["x"].ToString());
                    int y = int.Parse(itemDate[0]["floors"][i]["centralControlDevices"][k]["y"].ToString());
                    string Mname = itemDate[0]["floors"][i]["centralControlDevices"][k]["Name"].ToString();

                    //Debug.Log(Mname);
                    string LightID = itemDate[0]["floors"][i]["centralControlDevices"][k]["LightID"].ToString();

                    string ProjectorSerial = itemDate[0]["floors"][i]["centralControlDevices"][k]["ProjectorSerial"].ToString();
                    CentralControlDevice_JSON centralControlDevice_JSON = new CentralControlDevice_JSON(ip, PCDeviceIP, DelayedSetStateus, deviceType, x, y, Mname, LightID, ProjectorSerial);
                    floor_JSON.centralControlDevices.Add(centralControlDevice_JSON);
                }
                jsonfloor.Add(floor_JSON);
            }

            ValueSheet.ReadJsoncentralcontrolServices = new CentralControlServices_JSON(jsonfloor);

            EventCenter.Broadcast(EventDefine.inifromJson);
        }
     

        EventCenter.Broadcast(EventDefine.ini);
    }


    public void resetCurrentLou()
    {
      //  Debug.Log("I am running the function SetCurrentLou but dont running inside if");

        //Debug.Log(ValueSheet.louCtrs.Count);
        //if (ValueSheet.louCtrs.Count != 0)
        //{
        //    //Debug.Log("I am running the function SetCurrentLou");
        //    ValueSheet.currentLouCtr = ValueSheet.louCtrs[ValueSheet.louCtrs.Count-1];
        //}
    }

    public void SetLouLinkedList()
    {

    }

  
}
