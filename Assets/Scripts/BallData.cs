﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallData : MonoBehaviour
{
    public bool alive = false;
    public int id;

    private void Start()
    {
        id = (int)(Random.value * 100);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Person"))
            alive = false;
    }
}
