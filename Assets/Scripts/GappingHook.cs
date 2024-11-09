using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GappingHook : MonoBehaviour
{
    public LineRenderer line;
    public Transform hook;
    Vector2 mousedir;
    public bool isHookActive;
    public bool isLineMax;
    void Start()
    {
        line.positionCount = 2;
        line.endWidth = line.startWidth =0.05f;
        line.SetPosition(0,transform.position);
        line.SetPosition(1,hook.position);
        line.useWorldSpace= true;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);

        if(Input.GetKeyDown(KeyCode.E) && !isHookActive)
        {
            hook.position = transform.position; 
            mousedir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            isHookActive = true;
        }

        if(isHookActive && !isLineMax)
        {
            hook.Translate(mousedir.normalized * Time.deltaTime * 10);

            if(Vector2.Distance(transform.position,hook.position)>5)
            {
                isLineMax = true;    
            }
        }
    }
}
