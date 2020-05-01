using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public enum State{
        SEEK
    };
    private State state = State.SEEK;
    private Vector3 targetPos;
    private float followDis = 3f;
    public NavMeshAgent agent;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = transform.position;
        switch (state){
            case State.SEEK:
                if (Mathf.Abs((player.transform.position - this.gameObject.transform.position).magnitude) > followDis){
                    targetPos = player.gameObject.transform.position;
                }
            break;
        }
        agent.destination = targetPos;
    }
}
