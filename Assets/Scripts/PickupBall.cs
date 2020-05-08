using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PickupBall : MonoBehaviour
{
    public Transform origin;
    public float tollerance = .5f;
    public Image catchImg;
    public static bool isCaught;

    private int lastCaught = 0;
    private bool fumbleTime = false, catching = false, closeRangeBall = false;
    private List<Collider> foreignObjs = new List<Collider>();
    private Collider lastHit = null;
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
            closeRangeBall = false;
            if (!fumbleTime && !catching && !ThrowBall.hasBall)
            {
                StartCoroutine(Catching());
                if (foreignObjs.Count > 0)
                {
                    for (int i = 0; i < foreignObjs.Count; i++)
                    {
                        isCaught = false;                        
                        if (foreignObjs[i].CompareTag("Ball"))
                        {
                            Collider[] col = InSight.seen.ToArray();
                            foreach (Collider c in col)
                            {
                                if (foreignObjs[i] == c)
                                {
                                    closeRangeBall = true;
                                    isCaught = true;
                                    lastCaught = foreignObjs[i].GetComponent<BallData>().id;
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
                    if(!closeRangeBall)
                    {
                        if (InSight.seen.Count > 0)
                        {
                            for (int i = 0; i < InSight.seen.Count; i++)
                            {
                                if (InSight.seen[i].CompareTag("Ball"))
                                {
                                    if (!InSight.seen[i].GetComponent<BallData>().alive)
                                    {
                                        Destroy(InSight.seen[i].gameObject);
                                        InSight.seen.RemoveAt(i);
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
        if (!collision.gameObject.GetComponent<BallData>().alive && !ThrowBall.hasBall)
        {
            Destroy(collision.gameObject);
            foreignObjs.Remove(collision.collider);
            foreignObjs = new List<Collider>();
            ThrowBall.hasBall = true;
        }
        else if (collision.gameObject.GetComponent<BallData>().alive)
        {
            yield return new WaitForSeconds(.5f);
            if (collision.collider != null && collision.collider.GetComponent<BallData>().id != lastCaught)
                health.DeductHealth();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && collision.collider != lastHit)
        {
            lastHit = collision.gameObject.GetComponent<BallData>().alive ? collision.collider : lastHit;
            StartCoroutine(CatchBuffer(collision));
        }
    }
}
