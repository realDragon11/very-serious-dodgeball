using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PickupBall : MonoBehaviour
{
    public Transform origin;
    public float tollerance = .5f;
    public Image catchImg;

    private int caught = 0, hit = 0;
    private bool fumbleTime = false, catching = false;
    private List <Collider> foreignObjs = new List<Collider>();
    private Collider lastCaught = null, lastHit = null;
    private Health health;
    private CapsuleCollider cc;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        cc = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Catch();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Vector3.Distance(other.ClosestPointOnBounds(transform.position), transform.position) <= cc.radius)         
            foreignObjs.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (Vector3.Distance(other.ClosestPointOnBounds(transform.position), transform.position) > cc.radius)
            foreignObjs.Remove(other);
    }

    private void Catch()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {            
            if (!fumbleTime && !catching && !ThrowBall.hasBall)
            {
                StartCoroutine(Catching());
                if (foreignObjs.Count > 0)
                {
                    for (int i = 0; i < foreignObjs.Count; i++)
                    {
                        if (foreignObjs[i].CompareTag("Ball"))
                        {
                            Collider[] col = InSight.seen.ToArray();

                            foreach (Collider c in col)
                            {                                
                                if (foreignObjs[i] == c)
                                {
                                    caught++;
                                    //print("Catch: " + caught);
                                    lastCaught = foreignObjs[i];
                                    Destroy(foreignObjs[i].gameObject);
                                    foreignObjs.Remove(c);
                                    foreignObjs = new List<Collider>();
                                    InSight.seen.Remove(c);
                                    ThrowBall.hasBall = true;
                                    break;
                                }
                            }
                        }
                    }
                }                              
            }
        }
    }

    private IEnumerator Catching()
    {
        catching = true;
        catchImg.color = new Color(1, 0, 0, .25f);
        yield return new WaitForSeconds(1);
        catching = false;
        StartCoroutine(Unfumble());
    }

    private IEnumerator Unfumble()
    {
        catchImg.color = new Color(0, 0, 1, .25f);
        fumbleTime = true;
        yield return new WaitForSeconds(.3f);
        fumbleTime = false;
        catchImg.color = new Color(0, 0, 0, 0);
    }

    private IEnumerator CatchBuffer(Collision collision)
    {
        yield return new WaitForSeconds(.5f);

        if (collision.collider != lastCaught && collision.gameObject.GetComponent<BallData>().alive)
        {                        
            health.DeductHealth();            
            //hit++;
            //print("Hit: " + hit);
            //Destroy(collision.gameObject);
            //ThrowBall.hasBall = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && collision.collider != lastHit)
        {
            print(collision.collider);
            lastHit = collision.collider;
            StartCoroutine(CatchBuffer(collision));            
        }
    }
}
