using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSounds : MonoBehaviour
{

    public AudioSource ballSource;
    public AudioSource catchSource;
    
    // Start is called before the first frame update
    void Start()
    {
        ballSource = GetComponent<AudioSource>();
        catchSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }

    void OnCollisionEnter(Collision collision)
    {

        if (PickupBall.isCaught == true && collision.gameObject.tag == "Player")
        {
            catchSource.Play();
        }

        else if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Person")
        {
            ballSource.Play();
            //Debug.Log("Hit");
        }


    }
}
