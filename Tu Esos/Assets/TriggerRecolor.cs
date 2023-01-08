using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRecolor : MonoBehaviour
{
    public Material color;
    public int k = 0;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MeshRenderer>().material.color != color.color)
        {
            k++;
        }
        other.GetComponent<MeshRenderer>().material = color;
    }
}
