using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject head;
    public static bool running = false;

    private float defaultSpeed = 150f, moreSpeed;

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    private void Walk()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            running = true;
            moreSpeed = 2;
        }
        else
        {
            running = false;
            moreSpeed = 1;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 horizontal = head.transform.right * x;
        Vector3 vertical = head.transform.forward * z;

        Vector3 move = horizontal + vertical;

        rb.velocity = (move * defaultSpeed * moreSpeed * Time.deltaTime) + new Vector3(0, rb.velocity.y, 0);
    }
}
