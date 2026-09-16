using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftorRightRealorfake : MonoBehaviour
{
    bool leftIsReal;

    void Start()
    {
        leftIsReal = Random.Range(0, 2) == 0;

        if (leftIsReal)
        {
            Debug.Log("Left image is REAL");
            Debug.Log("Right image is AI");
        }
        else
        {
            Debug.Log("Left image is AI");
            Debug.Log("Right image is REAL");
        }
    }
}