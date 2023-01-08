using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTu : MonoBehaviour
{
    public GameObject TuPrefab;
    public float randomRange = 6f;
    public float size = 10f;
    private int n = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("mouse 0"))
        {
            GameObject tu = Instantiate(TuPrefab, transform, false);
            tu.transform.localPosition = new Vector3(Random.Range(-randomRange, randomRange), 0, Random.Range(-randomRange, randomRange));
            tu.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), -90);
            tu.transform.localScale = new Vector3(0.1f, size, 0.1f);
            n++;
        }
        if(Input.GetKeyDown("mouse 1"))
        {
            Debug.Log("N:");
            Debug.Log(n);
        }
    }
}
