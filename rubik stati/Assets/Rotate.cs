using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public int k = 10;
    private int i = 0;
    public float speed = 8;
    private int ok = 0;
    private bool start = false;
    Quaternion startRotation;
    Quaternion endRotation;
    float rotationProgress = -1;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("mouse 0"))
        {
            start = true;
        }
        if (Input.GetKeyDown("mouse 1") && start == false)
        {
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
        }
        if (start && i < k)
        {
            if (rotationProgress == -1)
            {
                int randomSide = Random.Range(0, 6);

                childPivot = GetChildWithName(sides[randomSide]);
                childTrigger = GetChildWithName(triggers[randomSide]);

                List<GameObject> cubes = childTrigger.GetComponent<RotateTrigger>().objects;

                foreach (GameObject cube in cubes)
                {
                    cube.transform.parent = childPivot.transform;
                }
                // Here we cache the starting and target rotations
                startRotation = childPivot.transform.rotation;

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

                // This starts the rotation, but you can use a boolean flag if it's clearer for you
                rotationProgress = 0;
            }


            if (rotationProgress < 1 && rotationProgress >= 0)
            {
                rotationProgress += Time.deltaTime * speed;

                // Here we assign the interpolated rotation to transform.rotation
                // It will range from startRotation (rotationProgress == 0) to endRotation (rotationProgress >= 1)
                childPivot.transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
                if (rotationProgress >= 1)
                {
                    childPivot.transform.rotation = endRotation;
                }
            }
            else
            {
                rotationProgress = -1;
                i++;
            }
        }
        if(i >= k && start == true)
        {
            start = false;
            foreach(string side in triggers)
            {
                Debug.Log(side);
                GameObject part = GetChildWithName(side);
                Debug.Log(part.GetComponent<RotateTrigger>().sideCount);
                if(part.GetComponent<RotateTrigger>().sideCount == 4)
                {
                    ok++;
                }
            }
            if (ok == 3)
            {
                Debug.Log("A kockanak 3 oldalan 4 az ertek!");
            }
            else
            {
                Debug.Log("A kocka nem keveredett elonyosen");
            }
            ok = 0;
            i = 0;
        }
    }
}