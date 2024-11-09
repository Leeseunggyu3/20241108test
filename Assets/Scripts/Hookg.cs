using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookg : MonoBehaviour
{
    GappingHook garppling;
    public DistanceJoint2D joint2D;

    // Start is called before the first frame update
    void Start()
    {
        garppling = GameObject.Find("Player").GetComponent<GappingHook>();
        joint2D = GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ring"))
        {
            joint2D.enabled = true;
            garppling.isAttach =true;
        }
    }
}
