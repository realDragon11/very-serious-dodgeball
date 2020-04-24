using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject head;

    private float speed = 150f;

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    private void Walk()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 horizontal = head.transform.right * x;
        Vector3 vertical = head.transform.forward * z;

        Vector3 move = horizontal + vertical;

        rb.velocity = (move * speed * Time.deltaTime) + new Vector3(0, rb.velocity.y, 0);
    }
}
