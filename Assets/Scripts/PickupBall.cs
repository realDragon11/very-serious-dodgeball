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

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        Catch();
    }

    private void OnTriggerEnter(Collider other)
    {
        foreignObjs.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        foreignObjs.Remove(other);
    }

    private void Catch()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {            
            if (!fumbleTime && !catching)
            {
                StartCoroutine(Catching());
                if (foreignObjs.Count > 0)
                {
                    for (int i = 0; i < foreignObjs.Count; i++)
                    {
                        if (foreignObjs[i].CompareTag("Ball"))
                        {
                            //Vector3 startPos = origin.forward * .5f;
                            Vector3 endPos = origin.forward * 3.5f;
                            //startPos.y += origin.position.y;
                            endPos.y += origin.position.y;
                            Collider[] col = Physics.OverlapCapsule(origin.position, endPos, tollerance);

                            foreach (Collider c in col)
                            {
                                if (foreignObjs[i] == c)
                                {
                                    caught++;
                                    //print("Catch: " + caught);
                                    lastCaught = foreignObjs[i];
                                    Destroy(foreignObjs[i].gameObject);
                                    foreignObjs.RemoveAt(i);
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

        if (collision.collider != lastCaught)
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
