using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopRightBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            string s = TCP_Utility.Send("192.168.20.10", 3000, "start");
            Debug.Log(s);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            string s = TCP_Utility.Send("192.168.20.10", 3000, "stop");
            Debug.Log(s);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            string s = TCP_Utility.Send("192.168.20.10", 3000, "read");
            Debug.Log(s);
        }
    }

    public void openAll()
    {
        Debug.Log("全开");
    }

    public void CloseAll()
    {

    }
}
