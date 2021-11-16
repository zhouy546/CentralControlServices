using MyUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class floor : MonoBehaviour
{
    // Start is called before the first frame update
    //json part
    public string bgUrl;
    public int pageindex;
    public List<CentralControlDevice> centralControlDevices = new List<CentralControlDevice>();

    //other part
    public floor next;
    public floor pervious;
    

    [SerializeField]
    private Image BgImage;

    public void MonoIni(int _pageindex,string _bgUrl)
    {
        pageindex = _pageindex;

        this.name = pageindex.ToString() + "楼";

        BgImage = this.GetComponent<Image>();

        bgUrl = _bgUrl; 
    }


    public void ini(int _pageindex, string _bgurl,GameObject _GCentralControlDevice, List<CentralControlDevice> _device) {

        MonoIni(_pageindex, _bgurl);

        for (int i = 0; i < _device.Count; i++)
        {
            GameObject GCentralControlDevice =  Instantiate(_GCentralControlDevice, this.transform);

             GCentralControlDevice.AddComponent<CentralControlDevice>();

            CentralControlDevice TempCentralControlDevice = GCentralControlDevice.GetComponent<CentralControlDevice>();

         //   Debug.Log("初始化" + _device[i].deviceType);

            TempCentralControlDevice.ini(_device[i].LightID, _device[i].deviceType, _device[i].MName, _device[i].ip, _device[i].x, _device[i].y, _device[i].sprite, _device[i].ProjectSerial);

            //Debug.Log(TempCentralControlDevice);

            centralControlDevices.Add(TempCentralControlDevice);
        }

        StartCoroutine(setPos(_pageindex));
    }

    IEnumerator setPos(int _pageindex)
    {
        yield return new WaitForSeconds(3f);
        if (_pageindex != 0)
        {
            this.transform.localPosition = new Vector2(1000, 0);
        }

            loadTexture(bgUrl);
    }

    public void Update()
    {

    }

    async public void loadTexture(string _path)
    {
        Texture2D _texture = await Utility.GetRemoteTexture(_path);
        if (_texture != null)
        {
            BgImage.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
        }     
    }

}
