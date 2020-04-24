using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryCalcculator : MonoBehaviour
{
    public GameObject test;
    public Transform player;
    public static Vector3 highestP = Vector3.zero;
    public static float initialSpeed = 0;

    private Vector3 selectedTheta, targetPos, testRange;
    private float range;
    private int[] angles = { 15, 30, 45 };
    private float minU, maxU;
    // Start is called before the first frame update
    void Start()
    {
        targetPos = player.position;
        SelectTrajectory();
        //CorrectHeight();
    }

    // Update is called once per frame
    void Update()
    { 
        /*if(Input.GetKeyUp(KeyCode.A))
        {
            SelectTrajectory();
        }*/
    }

    private void SelectTrajectory()
    {
        /*float x = -Random.Range(0, 10.0f);
        float y = Random.Range(0, 10.0f);
        float z = Random.Range(0, 10.0f);*/

        float maxHeight = MaxHeight();
        highestP = HighestPoint(maxHeight);
        //print(highestP);
        //GameObject instance1 = Instantiate(test, transform.position, Quaternion.identity);
        GameObject instance2 = Instantiate(test, highestP, Quaternion.identity);
        CorrectHeight();
        //GameObject instance3 = Instantiate(test, testRange, Quaternion.identity);
        bool isClear = IsClearTrajectory(highestP);
        //print(isClear);
    }

    private bool IsClearTrajectory(Vector3 highestP)
    {
        bool up = Physics.CheckCapsule(transform.position, highestP, .75f), down = true;
        Collider[] cols = Physics.OverlapCapsule(highestP, targetPos, .75f);
        foreach(Collider c in cols)
        {
            if(!c.CompareTag("Player") || !c.CompareTag("Ground"))
            {
                down = false;
                break;
            }
        }

        return up && down;
    }

    private float MinimumInitialSpeed(float g)
    {
        float r = Range();
        float theta = 45;
        return Mathf.Sqrt(r * g / Mathf.Sin(2 * theta * Mathf.Deg2Rad));

    }

    private float Range()
    {
        Vector2 target = new Vector2(targetPos.x, targetPos.z);
        Vector2 origin = new Vector2(transform.position.x, transform.position.z);
        return Vector2.Distance(target, origin);
    }

    private float Angle(float g, float u)
    {
        float r = Range();
        return Mathf.Asin(r * g / (u * u)) * Mathf.Rad2Deg / 2;
    }

    private float MaxHeight(/*Vector3 trajectory*/)
    {
        //selectedTheta = CustomNormals(trajectory);
        //Vector3 targetAngle = selectedTheta + transform.position;
        float g = Physics.gravity.magnitude;
        float u = MinimumInitialSpeed(g);//selectedU;
        initialSpeed = u;
        float theta = Angle(g, u);//PointToDegree(transform.position, new Vector2(targetAngle.x, targetAngle.y));
        //print(theta);
        float yI = Mathf.Sin(theta * Mathf.Deg2Rad) * u;
        float h = yI * yI / (2 * g);
        /*range = u * u * Mathf.Sin(theta) / g;
        float x = selectedTheta.x * range + transform.position.x;
        float z = selectedTheta.z * range + transform.position.z;
        testRange = new Vector3(x, transform.position.y, z); */       

        return h;
    }

    private Vector3 HighestPoint(float y)
    {
        if (y == 0) return Vector3.zero;
        else
        {
            /*float theta = Mathf.Atan(selectedTheta.y / selectedTheta.z) * Mathf.Rad2Deg;
            float hyp = y / Mathf.Sin(theta);

            float cos = theta == 90 ? 0 : Mathf.Cos(theta);
            float adjA = hyp * cos, adjB;

            theta = Mathf.Atan(selectedTheta.y / selectedTheta.x) * Mathf.Rad2Deg;
            hyp = y / Mathf.Sin(theta);

            cos = theta == 90 ? 0 : Mathf.Cos(theta);
            adjB = hyp * cos;*/

            //return new Vector3(adjB + transform.position.x, y + transform.position.y, adjA + transform.position.z);

            Vector2 start = new Vector2(transform.position.x, transform.position.z);
            Vector2 target = new Vector2(targetPos.x, targetPos.z);
            float dist = Vector2.Distance(target, start) / 2;

            float horizontalAngle = PointToDegree(start, target);
            selectedTheta = AngleToVector3(horizontalAngle);
            //print(horizontalAngle +" "+selectedTheta);
            Vector2 adjustDir = new Vector2(target.x - selectedTheta.x, target.y - selectedTheta.z).normalized;
            Vector3 direction = new Vector3(adjustDir.x * dist, 0, adjustDir.y * dist);

            return new Vector3(transform.position.x + direction.x, y + transform.position.y, transform.position.z + direction.z);

        }
    }

    private Vector3 CustomNormals(Vector3 original)
    {
        float highMag = 0;

        highMag = Mathf.Abs(highMag) < Mathf.Abs(original.x) ? highMag = Mathf.Abs(original.x) : highMag;
        highMag = Mathf.Abs(highMag) < Mathf.Abs(original.y) ? highMag = Mathf.Abs(original.y) : highMag;
        highMag = Mathf.Abs(highMag) < Mathf.Abs(original.z) ? highMag = Mathf.Abs(original.z) : highMag;

        if (highMag == 0) return original;
        else
        {
            float x = original.x / highMag;
            float y = original.y / highMag;
            float z = original.z / highMag;

            return new Vector3(x, y, z);
        }
    }

    private float PointToDegree(Vector2 start, Vector2 end)
    {
        float x = end.x - start.x;
        float y = end.y - start.y;

        return Mathf.Abs(Mathf.Atan(y / x) * Mathf.Rad2Deg);
    }

    private Vector2 AngleToVector3(float theta)
    {
        float x, y;
        theta %= 360;
        if (theta <= 45 || (theta > 315 && theta <= 359))
        {
            int mod = theta > 315 ? 0 : 1;
            theta = theta > 315 ? 45 - (355 - theta) : theta;
            x = 1;
            y = -1 + (mod + theta / 5 * (1 / 9.0f));
        }
        else if (theta > 45 && theta <= 135)
        {
            int mod = theta > 90 ? 1 : 0;
            theta = theta > 90 ? 45 - (135 - theta) : 45 - (90 - theta);
            x = 1 - (mod + theta / 5 * (1 / 9.0f));
            y = 1;
        }
        else if (theta > 135 && theta <= 225)
        {
            int mod = theta > 180 ? 1 : 0;
            theta = theta > 180 ? 45 - (225 - theta) : 45 - (180 - theta);
            x = -1;
            y = 1 - (mod + theta / 5 * (1 / 9.0f));
        }
        else if (theta > 225 && theta <= 315)
        {
            int mod = theta > 270 ? 1 : 0;
            theta = theta > 270 ? 45 - (315 - theta) : 45 - (270 - theta);
            x = -1 + (mod + theta / 5 * (1 / 9.0f));
            y = -1;
        }
        else
        {
            x = 0;
            y = 0;
        }

        return new Vector2(x, y);
    }

    private void CorrectHeight()
    {
        Vector2 target = new Vector2(targetPos.x, targetPos.z);
        Vector2 origin = new Vector2(transform.position.x, transform.position.z);
        float r = Vector2.Distance(target, origin);
        float g = Physics.gravity.magnitude;
        float theta = 45;
        float u = Mathf.Sqrt(r * g / Mathf.Sin(2 * theta * Mathf.Deg2Rad));
        float yI = Mathf.Sin(theta * Mathf.Deg2Rad) * u;

        //print(u);
        print((yI * yI / (2 * g)) + 1.5 + " max height");
    }

    /*private void OnDrawGizmos()
    {
        if (!duh)
        {
            StartCoroutine(Duh());
            float y = MaxHeight();

            highestP = HighestPoint(y);            
        }
        Gizmos.DrawSphere(highestP, .5f);
    }

    private IEnumerator Duh()
    {
        duh = true;
        yield return new WaitForSeconds(10);
        duh = false;
    }*/
}
