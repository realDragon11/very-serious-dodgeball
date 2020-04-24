using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintHighest : MonoBehaviour
{
    private float highest;
    private bool printed = false;
    // Start is called before the first frame update
    void Start()
    {
        highest = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(transform.position.y <= 1.4 && !printed)
        {
            print(transform.position);
            printed = true;
        }*/
    }
}
