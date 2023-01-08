using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSimulation : MonoBehaviour
{
    public int k = 10;
    private int i = 0;
    public float speed = 0.03F;
    private int ok = 0;
    private int jo = 0;
    private int n = 0;
    private bool start = false;
    Quaternion endRotation;
    GameObject childPivot;
    GameObject childTrigger;

    private readonly string[] sides = { "sideRight", "sideLeft", "sideUp", "sideDown", "sideFront", "sideBack" };
    private readonly string[] triggers = { "RightTrigger", "LeftTrigger", "UpTrigger", "DownTrigger", "FrontTrigger", "BackTrigger" };

    GameObject GetChildWithName(string name)
    {

        Transform childTrans = transform.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown("mouse 2") && start == false)
        {
            start = true;
            StartCoroutine(Simulate());
        }
    }

    IEnumerator Simulate()
    {
        while (true)
        {
            if (i < k)
            {
                int randomSide = Random.Range(0, 6);

                childPivot = GetChildWithName(sides[randomSide]);
                childTrigger = GetChildWithName(triggers[randomSide]);

                yield return new WaitForSeconds(speed);
                List<GameObject> cubes = childTrigger.GetComponent<RotateTrigger>().objects;

                foreach (GameObject cube in cubes)
                {
                    cube.transform.parent = childPivot.transform;
                }

                if (randomSide == 0 || randomSide == 1)
                {
                    endRotation = Quaternion.Euler(childPivot.transform.rotation.eulerAngles.x, childPivot.transform.rotation.eulerAngles.y, childPivot.transform.rotation.eulerAngles.z) * Quaternion.Euler(90 * (Random.Range(0, 2) < 0.5 ? -1 : 1), 0, 0);
                }
                else if (randomSide == 2 || randomSide == 3)
                {
                    endRotation = Quaternion.Euler(childPivot.transform.rotation.eulerAngles.x, childPivot.transform.rotation.eulerAngles.y, childPivot.transform.rotation.eulerAngles.z) * Quaternion.Euler(0, 90 * (Random.Range(0, 2) < 0.5 ? -1 : 1), 0);
                }
                else
                {
                    endRotation = Quaternion.Euler(childPivot.transform.rotation.eulerAngles.x, childPivot.transform.rotation.eulerAngles.y, childPivot.transform.rotation.eulerAngles.z) * Quaternion.Euler(0, 0, 90 * (Random.Range(0, 2) < 0.5 ? -1 : 1));
                }
                childPivot.transform.rotation = endRotation;
                i++;
            }
            if (i >= k)
            {
                yield return new WaitForSeconds(speed);
                foreach (string side in triggers)
                {
                    GameObject part = GetChildWithName(side);
                    int sideCount = part.GetComponent<RotateTrigger>().sideCount;
                    if (sideCount == 4)
                    {
                        ok++;
                    }
                }
                if (ok == 3)
                {
                    jo++;
                }
                n++;
                ok = 0;
                i = 0;

                foreach (string side in triggers)
                {
                    childTrigger = GetChildWithName(side);
                    List<GameObject> cubes = childTrigger.GetComponent<RotateTrigger>().objects;
                    foreach (GameObject cube in cubes)
                    {
                        cube.GetComponent<SavePosition>().ResetPos();
                        cube.transform.parent = transform;
                    }
                }
                Debug.Log((float)jo / n);
            }
        }
    }
}
