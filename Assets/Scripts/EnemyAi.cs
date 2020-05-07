using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public enum State{
        SEEK,BALL_SEEK, FLEE
    };
    private State state = State.BALL_SEEK;
    private Vector3 targetPos;
    private float followDis = 3f;
    public NavMeshAgent agent;
    private GameObject player;
    public EnemyManager manager;
    private GameObject ballSeeking;
    private bool hasBall = false;
    private int ticker;
    // Start is called before the first frame update
    void Start()
    {
        ticker = Random.Range(1,10);
        player = GameObject.FindGameObjectWithTag("Player");
        manager.add(this);
    }

    // Update is called once per frame
    void Update()
    {
        ticker++;
        if (ticker > 15){
            ticker = 0;
            targetPos = transform.position;
            ballSeeking = null;
            if (hasBall){
                state = State.SEEK;
            }
            switch (state){
                case State.SEEK:
                    if (Mathf.Abs((player.transform.position - this.gameObject.transform.position).magnitude) > followDis){
                        targetPos = player.gameObject.transform.position;
                    }
                break;
                case State.BALL_SEEK:

                if (ballSeeking = null){
                ballSeeking = manager.requestBall(this).gameObject;
                }
                if (ballSeeking != null){
                    targetPos = ballSeeking.transform.position;
                }else{
                    state = State.FLEE;
                }
                
                break;
                case State.FLEE:
                    state = State.BALL_SEEK;
                break;
            }
            agent.destination = targetPos;
            //Debug.Log(state.ToString());
        }
    }
}
