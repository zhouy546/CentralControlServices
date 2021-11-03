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
        StartCoroutine(readJson());
    }

    // Update is called once per frame
    void Update()
    {
        
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
                    CentralControlDevice_JSON centralControlDevice_JSON = new CentralControlDevice_JSON(ip, PCDeviceIP, DelayedSetStateus, deviceType, x, y, Mname, LightID);
                    floor_JSON.centralControlDevices.Add(centralControlDevice_JSON);
                }
                jsonfloor.Add(floor_JSON);
            }

            ValueSheet.ReadJsoncentralcontrolServices = new CentralControlServices_JSON(jsonfloor);

            EventCenter.Broadcast(EventDefine.inifromJson);
        }
      



        //Debug.Log(ValueSheet.ReadJsoncentralcontrolServices.floors[0].centralControlDevices[0].Name);

        //for (int i = 0; i < itemDate["Level"][0].Count; i++)
        //{
        //    List<Node> nodes = new List<Node>();


        //    string id = itemDate["Level"][0][i]["id"].ToString();
        //    string LouImageurl = itemDate["Level"][0][i]["LouImageurl"].ToString();

        //    Debug.Log("ID" + id);
        //    for (int J = 0; J < itemDate["Level"][0][i]["nodes"].Count; J++)
        //    {
        //        JsonData NodeJsonData = itemDate["Level"][0][i]["nodes"][J];
        //        string nodeID = NodeJsonData["id"].ToString();
        //        string PosX = NodeJsonData["PosX"].ToString();
        //        string PosY = NodeJsonData["PosY"].ToString();
        //        string imageUrl = NodeJsonData["imageUrl"].ToString();
        //        string imageTitle = NodeJsonData["imageTitle"].ToString();
        //        Debug.Log("NodeID:" + nodeID + "___" + "POSX:" + PosX+"___" + "PosY:" + PosY+"___" + "imageUrl" + imageUrl+"___"+  "imageTitle" + imageTitle);


        //        Node TEMPNode =new  Node(int.Parse(nodeID), new Vector2(float.Parse(PosX), float.Parse(PosY)), imageUrl, imageTitle);
        //        nodes.Add(TEMPNode);
        //    }
        //    Lou lou = new Lou(int.Parse(id), nodes.ToArray(), LouImageurl);

        //    ValueSheet.lous.Add(lou);

        //}


        ////MiscJson
        //string MiscJsonPath = Application.streamingAssetsPath + "/MiscJson.json";
        //WWW Miscwww = new WWW(MiscJsonPath);

        //yield return www;

        //jsonString = System.Text.Encoding.UTF8.GetString(Miscwww.bytes);

        //JsonMapper.ToObject(www.text);

        //itemDate = JsonMapper.ToObject(jsonString.ToString());

        //bool isReadFromStreamingAssets =Utility.StringToBool( itemDate["Misc"][0]["isReadFromStreamingAssets"].ToString());

        //Debug.Log(isReadFromStreamingAssets);

        //ValueSheet.MiscData.isReadFromStreamingAssets = isReadFromStreamingAssets;

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
