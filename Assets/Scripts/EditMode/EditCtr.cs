using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditCtr : MonoBehaviour
{
    public static EditCtr instance;

    public GameObject hintText;

    public GameObject Gxiugai;

    CombinationKey editKey;
    CombinationKey NewNodeKey;
    CombinationKey SaveKey;
    CombinationKey DeleteFloor;
    CombinationKey CreateFloor;
    CombinationKey ChangeFloorBG;
    public void Awake()
    {
        instance = this;
    }

    public void turnOnGxiugai()
    {
        Gxiugai.SetActive(true);
    }


    // Start is called before the first frame update
    void Start()
    {
        editKey = new CombinationKey(KeyCode.LeftControl, KeyCode.T);
        NewNodeKey = new CombinationKey(KeyCode.LeftControl, KeyCode.N);
        SaveKey = new CombinationKey(KeyCode.LeftControl, KeyCode.S);
        DeleteFloor = new CombinationKey(KeyCode.LeftControl, KeyCode.R);
        CreateFloor = new CombinationKey(KeyCode.LeftControl, KeyCode.F);
        ChangeFloorBG = new CombinationKey(KeyCode.LeftControl, KeyCode.G);
        EventCenter.AddListener(EventDefine.enterEdit, enterEdit);
        EventCenter.AddListener(EventDefine.exitEdit, exitEdit);
    }

    // Update is called once per frame
    void Update()
    {
        if (ValueSheet.EditMode)
        {
            if (NewNodeKey.ClickKey())
            {
                EventCenter.Broadcast(EventDefine.NewNode);
                Debug.Log("新建");
            }
            else if (SaveKey.ClickKey())
            {
                EventCenter.Broadcast(EventDefine.Save);
                Debug.Log("保存");
            }
            else if (CreateFloor.ClickKey())
            {
                EventCenter.Broadcast(EventDefine.CreateFloor);
                Debug.Log("新建楼层");
            }
            else if (DeleteFloor.ClickKey())
            {
                EventCenter.Broadcast(EventDefine.DeleteFloor);
                Debug.Log("删除楼层");
            }
            else if (ChangeFloorBG.ClickKey())
            {
                EventCenter.Broadcast(EventDefine.ChangeFloorBG);
                Debug.Log("更改楼层背景");
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                EventCenter.Broadcast(EventDefine.exitEdit);
                Debug.Log("退出修改模式");
            }else if (Input.GetKeyDown(KeyCode.Delete))
            {
                Debug.Log("删除");
                EventCenter.Broadcast(EventDefine.DeleteNode);
            }
        }
        else
        {
            if (editKey.ClickKey())
            {
                Debug.Log("进入修改模式");
                EventCenter.Broadcast(EventDefine.enterEdit);
            }
        }       
    }

    void enterEdit()
    {
        ValueSheet.EditMode = true;
        hintText.SetActive(true);
    }

    void exitEdit()
    {
        ValueSheet.EditMode = false;
        hintText.SetActive(false);

    }

}

public class CombinationKey
{
    public CombinationKey(KeyCode p, KeyCode a)
    {
        PrimaryKey = p;
        AttachKey = a;
    }

    public CombinationKey() { }
    /// <summary>
    /// 主键
    /// </summary>
    public KeyCode PrimaryKey
    {
        set { primaryKey = value; }
    }
    private KeyCode primaryKey = KeyCode.LeftControl;
    bool bPrimary = false;
    /// <summary>
    /// 附键
    /// </summary>
    public KeyCode AttachKey
    {
        set { attachKey = value; }
    }
    private KeyCode attachKey = KeyCode.E;

    /// <summary>
    /// 按下组合键后，只返回一次真
    /// </summary>
    /// <returns></returns>
    public bool ClickKey()
    {
        if (Input.GetKeyDown(primaryKey))
        {
            bPrimary = true;
        }
        if (Input.GetKeyUp(primaryKey))
        {
            bPrimary = false;
        }
        if (bPrimary)
        {
            if (Input.GetKeyDown(attachKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 按下组合键后，一直返回真，直到松开
    /// </summary>
    /// <returns></returns>
    public bool PressKey()
    {
        if (Input.GetKeyDown(primaryKey))
        {
            bPrimary = true;
        }
        if (Input.GetKeyUp(primaryKey))
        {
            bPrimary = false;
        }
        if (bPrimary)
        {
            if (Input.GetKey(attachKey))
            {
                Debug.Log("d");
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
