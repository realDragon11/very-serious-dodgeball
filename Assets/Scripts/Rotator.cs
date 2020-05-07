using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private int i, j;

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0, 2) > 0) i = (i + 1) % 359;
        else j = (j + 1) % 359;

        transform.rotation = Quaternion.Euler(new Vector3(0, i, j));
    }
}
