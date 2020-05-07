using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InSight : MonoBehaviour
{
    public static List<Collider> seen = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        seen.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        seen.Remove(other);
    }
}
