using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePosition : MonoBehaviour
{
    private Vector3 spawnPos;
    private Quaternion spawnRot;
    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;
        spawnRot = transform.rotation;
    }

    public void ResetPos()
    {
        transform.position = spawnPos;
        transform.rotation = spawnRot;
    }
}
