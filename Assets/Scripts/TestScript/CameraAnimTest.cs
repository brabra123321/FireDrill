using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimTest : MonoBehaviour
{
    public Transform P_head;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (P_head != null)
        {
            transform.position = P_head.position;
        }
    }
}
