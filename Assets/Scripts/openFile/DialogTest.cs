using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class DialogTest : MonoBehaviour
{


    //public void OpenVideoFile()
    //{
    //    string title = "请选择打开的文件：";
    //    //string msg = string.Empty;
        
    //    string path = FileDialogForWindows.FileDialog(title,2,"");

    //    if (!string.IsNullOrEmpty(path))
    //    {
          
    //       Debug.Log("指定的文件路径为: " + path);

    //    }
    //    else
    //    {
    //        Debug.Log("用户未作选择！");
    //    }
    //}

    public void OpenImageFile()
    {
        string title = "请选择打开的文件：";
        //string msg = string.Empty;

        string path = FileDialogForWindows.FileDialog(title, 3, "");

        if (!string.IsNullOrEmpty(path))
        {

            Debug.Log("指定的文件路径为: " + path);


        }
        else
        {
            Debug.Log("用户未作选择！");
        }
    }

    public void SaveFile(string pathname)
    {
        string title = "请选择保存的位置：";
        //string msg = string.Empty;
        string savepath = FileDialogForWindows.SaveDialog(title, Path.Combine(Application.streamingAssetsPath, pathname));//假如你存rar文件。
        if (!string.IsNullOrEmpty(savepath))
        {
        //   Game_manager.Instance.msgText.text = "保存文件的路径为: " + savepath;

        }
        else
        {
          //  Game_manager.Instance.msgText.text = "用户取消保存！";
        }
    }


}