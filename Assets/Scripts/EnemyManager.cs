using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<BallData> deadBalls = new List<BallData>();
    public List<EnemyAi> foes = new List<EnemyAi>();
    public List<BallData> taken = new List<BallData>();
    public int ticker = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       ticker++;
       if (ticker > 40){
           //screenForBalls();
           //Debug.Log("Dead Balls: " + deadBalls.Count);
       }
    }
    public void add(EnemyAi foe){
        foes.Add(foe);
    }

    public void remove(EnemyAi foe){
        foes.Remove(foe);
    }

    public void screenForBalls(){
        deadBalls.Clear();
         foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball")){
             if ((!ball.GetComponent<BallData>().alive)){//TODO: fiddle with magic number  && ball.GetComponent<BallData>().gameObject.GetComponent<Rigidbody>().velocity.magnitude < 3.0f
            deadBalls.Add(ball.GetComponent<BallData>());}
        }
    }

    public BallData requestBall(EnemyAi foe){
        screenForBalls();
        List<BallData> considering = new List<BallData>();
        foreach (BallData ballD in deadBalls)
        {
            if (!taken.Contains(ballD)){
                considering.Add(ballD);
            }
        }
        float minDistance = 999999999999999;
        BallData bestBall = null;
        float dis;
        foreach (BallData ballD in considering)
        {
           
            dis = Vector3.Distance(ballD.gameObject.transform.position,foe.gameObject.transform.position);
            if (dis < minDistance){
                minDistance = dis;
                bestBall = ballD;
            }
        }
        if (bestBall != null){
            //Debug.Log("Found Ball!");
            taken.Add(bestBall);
        }else{
            //Debug.Log("No balls!");
        }
        return bestBall;
    }

    public void freeBall(BallData b){
        taken.Remove(b);
    }
}
