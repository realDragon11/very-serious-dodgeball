using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    public GameObject ball;
    public float strength = 10;
    public static bool hasBall = false;

    private bool throwing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && !throwing && hasBall)
        {
            StartCoroutine(Throw());
            throwing = true;
            hasBall = false;
        }
    }

    private IEnumerator Throw()
    {
        GameObject instance = Instantiate(ball, transform.position, Quaternion.identity);
        Rigidbody rb = instance.GetComponent<Rigidbody>();
        Vector3 direction = transform.position - transform.parent.transform.position;
        rb.velocity = direction * strength;
        yield return new WaitForSeconds(2);
        throwing = false;
    }
}
