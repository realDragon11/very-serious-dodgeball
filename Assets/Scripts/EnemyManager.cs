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
           screenForBalls();
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
             if (!ball.GetComponent<BallData>().alive){
            deadBalls.Add(ball.GetComponent<BallData>());}
        }
    }

    public BallData requestBall(EnemyAi foe){
        List<BallData> considering = new List<BallData>();
        foreach (BallData ballD in deadBalls)
        {
            if (!taken.Contains(ballD)){
                considering.Add(ballD);
            }
        }
        float minDistance = 30;
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
        return bestBall;
    }
}
