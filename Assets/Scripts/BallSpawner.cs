using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;
    public Transform shooter;

    private bool shooting = false, hasBall = false;
    private Vector3 trajectory = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();

        if (!shooting) StartCoroutine(Shoot()); 
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

    private void FollowPlayer(/*int i*/)
    {
        trajectory = TrajectoryCalcculator.LookToPoint(transform.position, (int)TrajectoryCalcculator.chosen.x);
        shooter.LookAt(transform.position + trajectory);
    }

    private IEnumerator Shoot()
    {
        shooting = true;
        //for (int i = 0; i < 5; i++)
        //{
            //FollowPlayer();
            
            yield return new WaitForSeconds(3f);

            float y = Random.Range(0, 1.0f);

            trajectory *= TrajectoryCalcculator.chosen.y;
            GameObject instance = Instantiate(ball, transform.position, Quaternion.identity);
            Rigidbody rb = instance.GetComponent<Rigidbody>();
            rb.velocity = trajectory;
        //}
        shooting = false;
        hasBall = false;
    }
}
