using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System;

namespace MyUtility {

    public static class Utility
    {
        public static float Maping(float value, float inputMin, float inputMax, float outputMin, float outputMax, bool clamp)
        {
            float outVal = ((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin);

            if (clamp)
            {
                if (outputMax < outputMin)
                {
                    if (outVal < outputMax) outVal = outputMax;
                    else if (outVal > outputMin) outVal = outputMin;
                }
                else
                {
                    if (outVal > outputMax) outVal = outputMax;
                    else if (outVal < outputMin) outVal = outputMin;
                }
            }


            return outVal;
        }

        public static DeviceType GetDeviceTypebyInt(int i)
        {
            if (i == 0)
            {
                return DeviceType.多媒体服务器;
            }
            else if (i == 1)
            {
                return DeviceType.投影;
            }
            else if (i == 2)
            {
                return DeviceType.LED电柜;
            }
            else if (i == 3)
            {
                return DeviceType.灯光;
            }

            return DeviceType.多媒体服务器;
        }

        //public static IEnumerator GetTexture(string url)
        //{
        //    WWW www = new WWW(url);
        //    yield return www;
        //    if (www.isDone && www.error == null)
        //    {
        //        Texture2D img = www.texture;
        //        ValueSheet.tempImage = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));

        //    }
        //}


        //异步获取外部图片
        //IEnumerator GetText(string url)
        //{
        //    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        //    {
        //        yield return uwr.SendWebRequest();

        //        if (uwr.result != UnityWebRequest.Result.Success)
        //        {
        //            Debug.Log(uwr.error);
        //        }
        //        else
        //        {
        //            // Get downloaded asset bundle
        //            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

        //            texture2s.Add(texture);
        //        }
        //    }
        //}


        //    async void Start()
        //{
        //    _texture = await Utility.GetRemoteTexture(_imageUrl);
        //    sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));

        //}
        /// <summary>
        /// 使用次方法前 需要在Start前加async    async void ini()
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<Texture2D> GetRemoteTexture(string url)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                // begin request:
                var asyncOp = www.SendWebRequest();

                // await until it's done: 
                while (asyncOp.isDone == false)
                    await Task.Delay(1000 / 30);//30 hertz

                // read results:
                if (www.isNetworkError || www.isHttpError)
                {
                    // log error:
#if DEBUG
                Debug.Log($"{www.error}, URL:{www.url}");
#endif

                    // nothing to return on error:
                    return null;
                }
                else
                {
                    // return valid results:
                    return DownloadHandlerTexture.GetContent(www);
                }
            }
        }

        public static bool checkIp(string ipStr)
        {
            IPAddress ip;
            if (IPAddress.TryParse(ipStr, out ip))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //十六进制字符串转byte数组
        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

//状态0关机； 状态1正在开机；状态2正在关机；状态3开机；

        public static int convertProjectorServerStatus(string s, CentralControlDevice device)
        {
            if (s == ValueSheet.ProjectorCMD[device.ProjectSerial].receiveoff)
            {
                return 0;
            }
            else if (s == ValueSheet.ProjectorCMD[device.ProjectSerial].receiveon)
            {
                return 3;
            }
            else if (s == ValueSheet.ProjectorCMD[device.ProjectSerial].powerok)
            {
                if (device.status == 3)
                {
                    return 2;
                }
                if (device.status == 0)
                {
                    return 1;
                }

            }

            return 4;
        }

        public static int convertMediaServerStatus(string s)
        {
            if (s == ValueSheet.MediaServerReceiveCmd[0])
            {
                return 3;
            }
            else if (s == ValueSheet.MediaServerReceiveCmd[1])
            {
                return 2;
            }
            else if (s == ValueSheet.MediaServerReceiveCmd[2])
            {
                return 1;
            }
            else if (s == ValueSheet.MediaServerReceiveCmd[3])
            {
                return 0;
            }
            return 4;
        }

        public static int convertLEDServerStatus(string s)
        {
            if (s == ValueSheet.ByTenLEDReceiveCmd[2])
            {
                return 3;
            }
            else if (s == ValueSheet.ByTenLEDReceiveCmd[3])
            {
                return 0;
            }
            else if (s == ValueSheet.ByTenLEDReceiveCmd[0])
            {
                return 3;
            }
            else if (s == ValueSheet.ByTenLEDReceiveCmd[1])
            {
                return 0;
            }
            return 4;
        }

        public static int convertLightServerStatus(string s)
        {
            if (s == ValueSheet.LightReceiveCmd[0])
            {
                return 3;
            }
            if (s == ValueSheet.LightReceiveCmd[1]) {
                return 0;
            }
            if (s == ValueSheet.LightReceiveCmd[2])
            {
                return 3;
            }
            if (s == ValueSheet.LightReceiveCmd[3])
            {
                return 0;
            }
            return 4;
        }
    }
}

