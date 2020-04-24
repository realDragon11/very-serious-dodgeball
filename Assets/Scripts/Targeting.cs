using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public Transform player;

    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //float y = PointToAngleH(transform.position, player.position);
        //float z = PointToAngleV(transform.position, player.position);
        //dir = new Vector3(0, y, -z);
        //transform.rotation = Quaternion.Euler(dir);
        transform.LookAt(TrajectoryCalcculator.highestP);
    }

    /*private float PointToAngleH(Vector3 start, Vector3 end)//Horizontal angle in degrees
    {
        float x = end.x - start.x;
        float y = end.z - start.z;

        return Mathf.Abs(Mathf.Atan(y / x) * Mathf.Rad2Deg);
    }

    private float PointToAngleV(Vector3 start, Vector3 end)//Vertical angle in degrees
    {
        float x = end.x - start.x;
        float y = end.y - start.y;

        return Mathf.Abs(Mathf.Atan(y / x) * Mathf.Rad2Deg);
    }*/
}
