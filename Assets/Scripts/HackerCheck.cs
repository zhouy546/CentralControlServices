using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using LitJson;
using System;
using System.Runtime.InteropServices;
using System.Text;

public class HackerCheck : MonoBehaviour
{


    [DllImport("Nox2App.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public extern static int NoxFind(uint appID, int[] keyHandle, int[] keyNum);

    [DllImport("Nox2App.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public extern static int NoxGetLastError();

    [DllImport("Nox2App.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public extern static int NoxGetUID(int keyHandle, StringBuilder uid);

    [DllImport("Nox2App.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public extern static int NoxOpen(int keyHandle, byte[] pwd);

    [DllImport("Nox2App.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public extern static int NoxClose(int keyHandle);

    [DllImport("Nox2App.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public extern static int NoxWriteStorage(int keyHandle, int page, byte[] WData);

    [DllImport("Nox2App.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public extern static int NoxReadStorage(int keyHandle, int page, byte[] RData);
    int Rtn;
    int[] keyHandle = new int[8];
    int[] keyNum = new int[8];

    private JsonData itemDate;

    private string jsonString;

    public GameObject checkerObject;
    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_EDITOR

        StartCoroutine(readituidJson());
#endif
    }

    IEnumerator readituidJson()
    {
        string spath = Application.streamingAssetsPath + "/UID.json";

        FileInfo info = new FileInfo(spath);

        Debug.Log("找到配置文件，从Json载入");

        WWW www = new WWW(spath);

        yield return www;

        jsonString = System.Text.Encoding.UTF8.GetString(www.bytes);

        JsonMapper.ToObject(www.text);

        itemDate = JsonMapper.ToObject(jsonString.ToString());

        ValueSheet.appid = itemDate["appid"].ToString();

        ValueSheet.password = itemDate["password"].ToString();

        yield return new WaitForSeconds(1f);

        if (!Check())
        {
            Debug.LogWarning("10秒后关闭");

            checkerObject.SetActive(true);

            yield return new WaitForSeconds(10f);

            Application.Quit();
        }

    }

    bool Check()
    {
        bool b =FindLocker();

        if (!b)
        {
            return b;
        }

        b = OpenLocker();

        if (!b)
        {
            return b;
        }

        b = checkTime();

        if (!b)
        {
            return b;
        }

        return true;
    }

    /// <summary>
    /// 查找锁
    /// </summary>

    private bool FindLocker()
    {
        uint appID = Convert.ToUInt32(ValueSheet.appid, 16);
        Rtn = NoxFind(appID, keyHandle, keyNum);

        Debug.LogWarning(Rtn);
        if (Rtn != 0)
        {

            Debug.LogWarning("没有找到锁！错误码：" + NoxGetLastError());
            return false;
        }
        StringBuilder uid = new StringBuilder(32);
        Rtn = NoxGetUID(keyHandle[0], uid);
        if (Rtn != 0)
        {
            Debug.LogWarning("获取ID失败！错误码：" + NoxGetLastError());
            return false;
        }
        Debug.LogWarning(uid.ToString());
        return true;
    }

    /// <summary>
    /// 打开锁
    /// </summary>
    private bool OpenLocker()
    {
        byte[] pwd = Encoding.UTF8.GetBytes(ValueSheet.password.Trim());
        Rtn = NoxOpen(keyHandle[0], pwd);
        if (Rtn != 0)
        {
            Debug.LogWarning("打开失败！错误码" + NoxGetLastError());
            return false;
        }
        Debug.LogWarning("打开成功");
        return true;
    }

    /// <summary>
    /// 关闭加密锁
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CloseLcoker()
    {
        Rtn = NoxClose(keyHandle[0]);
        if (Rtn != 0)
        {
            Debug.LogWarning("关闭失败！错误码" + NoxGetLastError());
            return;
        }
        Debug.LogWarning("关闭成功");
    }

    private string BatteryRead()
    {
        int page = Convert.ToInt32("1");
        byte[] RData = new byte[64];
        Rtn = NoxReadStorage(keyHandle[0], page, RData);
        if (Rtn != 0)
        {
            Debug.LogWarning("读取失败！错误码：" + NoxGetLastError());
            return "null";
        }
      return Encoding.UTF8.GetString(RData);
    }

    private bool checkTime()
    {
        string str3 = BatteryRead();
        Debug.Log(str3);

        DateTime dt_YouXiao = Convert.ToDateTime(str3.Substring(0, 10)); //到期时间
        DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));//当前时间
        if (dt_YouXiao < dt2)
        {
            Debug.LogWarning("软件到期");
            CloseLcoker();
            return false;
        }
        else
        {

            Debug.LogWarning("校验通过可以运行程序");
            CloseLcoker();
            return true;
        }
    }
     //bool Djah8786ef87fs8ef(out int er)
     //   {
     //       er = 0;
     //       //设置返回默认值
     //       bool ret = false;

     //       long keyNum = 0;
     //       //程序识别号
     //       uint appID = 0x78635467;

     //       //查找加密锁 
     //       long nRet = Nox2AppApis.NoxFind(appID, out long[] keyHandles, ref keyNum);
     //       if (nRet != 0)
     //       {
     //           er = 1;
     //           Console.WriteLine("查找加密狗失败");
     //           return ret;
     //       }
     //       //获取加密锁唯一ID
     //       string szUID = string.Empty;
     //       if (0 != Nox2AppApis.NoxGetUID(keyHandles[0], ref szUID))
     //       {
     //           er = 2;
     //           Console.WriteLine("获取唯一id错误");
     //           return ret;
     //       }
     //       Console.WriteLine("唯一id" + szUID);

     //       //使用用户密码打开加密狗
     //       String userPin;
     //       try
     //       {
     //           er = 3;
     //             userPin = DJ.WindowAPI.DESEncrypt.Decrypt(mConfig.COMM.UID, "Cy9t");
     //       }
     //       catch 
     //       {
     //           ituid.Text = mConfig.COMM.UID;
     //           ituid.Visibility = Visibility.Visible;
     //           return ret;
     //       }
     //       if (0 != Nox2AppApis.NoxOpen(keyHandles[0], userPin))
     //       {
     //           er = 4;
     //           ituid.Text = mConfig.COMM.UID;
     //           ituid.Visibility = Visibility.Visible;
     //           Console.WriteLine("用户密码错误");
     //           Nox2AppApis.NoxClose(keyHandles[0]);
     //           return ret;
     //       }

     //       bool Can = false;
     //       //存储区读
     //       byte[] readDt = new byte[64];
     //       if (0 == Nox2AppApis.NoxReadStorage(keyHandles[0], 0, readDt))
     //       {
     //           string str = System.Text.Encoding.Default.GetString(readDt);

     //           if (str != "")
     //           {
     //               DateTime dt_ChunChu = Convert.ToDateTime(str.Substring(0, 19));//上次打开的时间
     //               DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//当前时间
     //               if (dt_ChunChu <= dt)//判断是否有修改计算机日期的嫌疑
     //               {
     //                   //把当前时间写入到加密狗
     //                   byte[] byteArray3 = System.Text.Encoding.ASCII.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
     //                   if (0 == Nox2AppApis.NoxWriteStorage(keyHandles[0], 0, byteArray3))
     //                   {
     //                       //校验系统日期通过
     //                       Can = true;
     //                   }
     //               }
     //               else
     //               {
     //                   er = 5;
     //                   Console.WriteLine("可能修改了系统日期");
     //                   Nox2AppApis.NoxClose(keyHandles[0]);
     //               }

     //           }
     //           else
     //           {
     //               er = 6;
     //               Console.WriteLine("加密狗存储的上次打开时间是空");
     //               Nox2AppApis.NoxClose(keyHandles[0]);
     //           }
     //       }
     //       else
     //       {
     //           er = 7;
     //           Console.WriteLine("读取加密狗存储内容失败");
     //           Nox2AppApis.NoxClose(keyHandles[0]);
     //       }
     //       if (Can)
     //       {
     //           //读取软件设置到期时间
     //           byte[] readStorage = new byte[64];
     //           if (0 == Nox2AppApis.NoxReadStorage(keyHandles[0], 1, readStorage))
     //           {
     //               string str3 = System.Text.Encoding.Default.GetString(readStorage);
     //               DateTime dt_YouXiao = Convert.ToDateTime(str3.Substring(0, 10)); //到期时间
     //               DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));//当前时间
     //               if (dt_YouXiao < dt2)
     //               {
     //                   er = 8;
     //                   Console.WriteLine("软件到期");
     //                   Nox2AppApis.NoxClose(keyHandles[0]);
     //               }
     //               else
     //               {
     //                   er = 0;
     //                   Console.WriteLine("校验通过可以运行程序");
     //                   Nox2AppApis.NoxClose(keyHandles[0]);
     //                   ret = true;
     //               }
     //           }

     //       }
     //       return ret;
     //   }
}
