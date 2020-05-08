using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBallModel : MonoBehaviour
{
    public GameObject ballModel;

    // Update is called once per frame
    void Update()
    {
        ballModel.SetActive(ThrowBall.hasBall);
    }
}
