using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private int i;

    // Update is called once per frame
    void Update()
    {
        i = (i + 1) % 359;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, i));
    }
}
