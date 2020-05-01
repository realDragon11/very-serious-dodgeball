using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;
    public Transform shooter;

    private bool shooting = false, loadingShot = false;
    private Vector3 trajectory = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (!shooting) StartCoroutine(Shoot());

        FollowPlayer();
    }

    private void FollowPlayer()
    {
        trajectory = TrajectoryCalcculator.LookToPoint(transform.position);
        shooter.LookAt(transform.position + trajectory);
    }

    private IEnumerator Shoot()
    {
        shooting = true;
        loadingShot = true;

        yield return new WaitForSeconds(3f);
        loadingShot = false;

        float y = Random.Range(0, 1.0f);
        
        trajectory *= TrajectoryCalcculator.initialSpeed;
        GameObject instance = Instantiate(ball, transform.position, Quaternion.identity);
        Rigidbody rb = instance.GetComponent<Rigidbody>();
        rb.velocity = trajectory;
        shooting = false;
    }
}
