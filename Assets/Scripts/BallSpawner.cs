using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;

    private bool shooting = false;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 adjusted = transform.position + new Vector3(-1f, 1f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!shooting) StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        shooting = true;
        float y = Random.Range(0, 1.0f);

        Vector3 trajectory = transform.position - transform.parent.transform.position;
        trajectory.Normalize();
        //new Vector3(-1f, 1f, 0f).normalized;
        trajectory *= TrajectoryCalcculator.initialSpeed;

        yield return new WaitForSeconds(3f);

        GameObject instance = Instantiate(ball, transform.position, Quaternion.identity);
        Rigidbody rb = instance.GetComponent<Rigidbody>();
        rb.velocity = trajectory;
        
        shooting = false;
    }

    private float PointToDegree(Vector3 start, Vector3 end)
    {
        float x = end.x - start.x;
        float y = end.y - start.y;

        return Mathf.Abs(Mathf.Atan(y / x) * Mathf.Rad2Deg);
    }
}
