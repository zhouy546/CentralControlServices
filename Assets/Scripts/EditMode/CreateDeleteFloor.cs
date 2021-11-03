using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDeleteFloor : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

        EventCenter.AddListener(EventDefine.CreateFloor, CreateFloor);
        EventCenter.AddListener(EventDefine.DeleteFloor, DeleteFloor);
        EventCenter.AddListener(EventDefine.ChangeFloorBG, ChangeFloorBG);


    }

    private void CreateFloor()
    {
        GameObject gfloor = Instantiate(MainCtr.instance.G_floor, MainCtr.instance.transform);

        gfloor.AddComponent<floor>();

        floor _newfloor = gfloor.GetComponent<floor>();

        floor perviousFloor = ValueSheet.centralcontrolServices.floors[ValueSheet.centralcontrolServices.floors.Count - 1];

        SetupNewFloor(perviousFloor,_newfloor);
    }

    private void SetupNewFloor(floor _perviousFloor,floor _newfloor)
    {
        _perviousFloor.next = _newfloor;

        _newfloor.pervious = _perviousFloor;
        
        ValueSheet.currentFloor = _newfloor;

        ValueSheet.centralcontrolServices.floors.Add(_newfloor);

        _newfloor.MonoIni(ValueSheet.centralcontrolServices.floors.Count - 1, ValueSheet.defaultBGURL);
    }

    private void DeleteFloor()
    {
        if (ValueSheet.centralcontrolServices.floors.Count >= 2)
        {
            Debug.Log("删除"+ValueSheet.currentFloor.name);

            ValueSheet.centralcontrolServices.floors.Remove(ValueSheet.currentFloor);

            if (ValueSheet.currentFloor.pervious != null)
            {
                ValueSheet.currentFloor.pervious.next = ValueSheet.currentFloor.next;
            }
            if (ValueSheet.currentFloor.next != null)
            {
                ValueSheet.currentFloor.next.pervious = ValueSheet.currentFloor.pervious;
            }

            Destroy(ValueSheet.currentFloor.gameObject, 0.2f);

            ValueSheet.currentFloor = ValueSheet.centralcontrolServices.floors[ValueSheet.centralcontrolServices.floors.Count - 1];

            ValueSheet.currentFloor.transform.localPosition = Vector2.zero;
        }

    }

    public void ChangeFloorBG()
    {

        string title = "请选择打开的文件：";
        //string msg = string.Empty;

        string path = FileDialogForWindows.FileDialog(title, 3, "");

        if (!string.IsNullOrEmpty(path))
        {

            Debug.Log("指定的文件路径为: " + path);

            ValueSheet.currentFloor.bgUrl = path;

            ValueSheet.currentFloor.loadTexture(path);
        }
        else
        {
            Debug.Log("用户未作选择！选择默认路径" + ValueSheet.defaultBGURL);

            ValueSheet.currentFloor.bgUrl = ValueSheet.defaultBGURL;

        }

    }

    public void NextFloor()
    {
        if (ValueSheet.currentFloor.next != null)
        {
            ValueSheet.currentFloor.transform.localPosition = new Vector2(1000, 0);

            ValueSheet.currentFloor = ValueSheet.currentFloor.next;

            ValueSheet.currentFloor.transform.localPosition = new Vector2(0, 0);

        }
    }

    public void PerviousFloor()
    {
        if (ValueSheet.currentFloor.pervious != null)
        {
            ValueSheet.currentFloor.transform.localPosition = new Vector2(1000, 0);

            ValueSheet.currentFloor = ValueSheet.currentFloor.pervious;

            ValueSheet.currentFloor.transform.localPosition = new Vector2(0, 0);


        }
    }



}
