using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WriteJson : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener(EventDefine.Save, SaveJson);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //public void aveJson()
    //{
    //    StartCoroutine(saveJson());
    //}

    public void SaveJson()
    {
        JsonData jd = new JsonData();


        List<Floor_JSON> jsonfloor = new List<Floor_JSON>();

        foreach (floor mfloor in ValueSheet.centralcontrolServices.floors)
        {

            Floor_JSON floor_JSON = new Floor_JSON(mfloor.pageindex);

            floor_JSON.bgUrl = mfloor.bgUrl;

            foreach (CentralControlDevice device in mfloor.centralControlDevices)
            {

               // Debug.Log(device.Name);
                CentralControlDevice_JSON jsonDevice =  new CentralControlDevice_JSON(device.ip, device.PCDeviceIP, device.DelayedSetStateus, (int)device.deviceType, device.x, device.y
                    , device.MName, device.LightID, device.ProjectSerial);

                floor_JSON.centralControlDevices.Add(jsonDevice);
            }

            jsonfloor.Add(floor_JSON);
        }

        CentralControlServices_JSON centralControlServices_JSON = new CentralControlServices_JSON(jsonfloor);

        //List<Lou> lous = new List<Lou>();


        //List<Node> nodes = new List<Node>();

        //foreach (var louCtr in ValueSheet.louCtrs)
        //{
        //    foreach (var item in louCtr.btnShows)
        //    {
        //        Node node = new Node(item.id, item.pos, item.imageUrl, item.Imagetitle);
        //        nodes.Add(node);
        //    }
        //    Lou Lou1 = new Lou(louCtr.id, nodes.ToArray(), louCtr.LouImageurl);

        //    lous.Add(Lou1);
        //}

        string json = ConvertClassToJsonData(centralControlServices_JSON).ToJson();


        string informationJsonURL =   Application.streamingAssetsPath + "/information.json";



        // CreatJsonFile(miscJson, MiscJsonURL);   

        //// yield return new WaitForSeconds(2f);
        CreatJsonFile(json, informationJsonURL);

    }

    JsonData ConvertClassToJsonData(object obj)
    {
        JsonData temp = new JsonData();

        temp.Add(JsonMapper.ToObject(JsonMapper.ToJson(obj)));
        return temp;
    }

    //public void UpdateJson(string[] Jsonarray)
    //{
    //    string temp = "";
    //    for (int i = 0; i < Jsonarray.Length; i++)
    //    {
    //        temp += Jsonarray[i];
    //    }

    //    string ss2 = Regex.Unescape(temp);
    //    CreatJsonFile(ss2);
    //}

    void CreatJsonFile(string jsonStr,string url)
    {
        string spath = url;
        StringBuilder sb = new StringBuilder();
        StreamWriter sw;
        FileInfo info = new FileInfo(spath);
        if (!info.Exists)
        {
            sw = info.CreateText();
            print("文件不存在，创建数据");
        }
        else
        {
            info.Delete();
            print("文件已经存在，删除数据");
            sw = info.CreateText();
        }

        sw.Write(jsonStr);
        sw.Close();

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
