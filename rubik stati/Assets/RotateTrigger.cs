using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrigger : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>(); // stores the objects currently in contact with the trigger

    public int sideCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            objects.Add(other.gameObject);
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Textures"))
        {
            sideCount++;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            objects.Remove(other.gameObject); // Remove the Object from the List
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Textures"))
        {
            sideCount--;
        }
    }
}
