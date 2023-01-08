using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountTriggers : MonoBehaviour
{
    int sum = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("mouse 1"))
        {
            foreach(Transform child in transform)
            {
                sum += child.gameObject.GetComponent<TriggerRecolor>().k;
            }
            Debug.Log("K:");
            Debug.Log(sum);
        }
    }
}
