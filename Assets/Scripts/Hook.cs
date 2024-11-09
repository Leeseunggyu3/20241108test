using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public LineRenderer line;
    public Transform hook;
    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 2;
        line.endWidth = line.startWidth;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);

        if(Input.GetKeyDown(KeyCode.Es)) 
        {
            Camera.main.Scr
        }
    }
}
