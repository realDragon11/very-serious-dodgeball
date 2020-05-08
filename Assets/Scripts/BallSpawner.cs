using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;
    public Transform shooter;

    private bool shooting = false, hasBall = false;
    private Vector3 trajectory = Vector3.zero, trajectoryXZ = Vector3.zero;
    private TrajectoryCalcculator tc;

    private void Start()
    {
        tc = GetComponent<TrajectoryCalcculator>();
    }

    // Update is called once per frame
    void Update()
    {
        shooter.LookAt(TrajectoryCalcculator.targetPos);

        if (!shooting) StartCoroutine(Shoot()); 
    }

    private IEnumerator Shoot()
    {
        tc.SelectTrajectory();
        shooting = true;
        trajectory = TrajectoryCalcculator.lookTo.normalized;
        shooter.LookAt(transform.position + trajectory);

        

        float y = Random.Range(0, 1.0f);

        trajectory *= TrajectoryCalcculator.chosen.y;
        GameObject instance = Instantiate(ball, transform.position, Quaternion.identity);
        ball.GetComponent<BallData>().alive = true;
        Rigidbody rb = instance.GetComponent<Rigidbody>();
        rb.velocity = trajectory;
        yield return new WaitForSeconds(3f);
        shooting = false;
        hasBall = false;
    }

    public void ThrowBall()
    {
        if (hasBall && !shooting) StartCoroutine(Shoot());
        else if (!hasBall) print(gameObject.name + " can't throw because it isn't holding a ball.");
    }

    public void PickupBall()
    {
        hasBall = true;
    }
}
