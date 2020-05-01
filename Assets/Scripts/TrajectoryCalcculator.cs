using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryCalcculator : MonoBehaviour
{
    public GameObject test;
    public Transform player;
    public static Vector3 highestP = Vector3.zero;
    public static float initialSpeed = 0;
    public static Vector3 targetPos, lookTo;

    private Vector3 testRange, midPoint, hP;
    private float range;
    private int[] angles = { 15, 30, 45 };
    private float minU, maxU;
    public static int selectedTheta;
    private int alpha;

    // Update is called once per frame
    void Update()
    {
        targetPos = player.position + Vector3.up * .95f;
        SelectTrajectory();
    }

    private void SelectTrajectory()
    {
        float maxHeight = MaxHeight();
        highestP = HighestPoint(maxHeight);

        bool isClear = IsClearTrajectory(highestP);
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
        alpha = AdjustedAngleByElevation(transform.position, targetPos);
        int theta = 45;//Mathf.Clamp(45, alpha, 90);
        float radAlpha = alpha * Mathf.Deg2Rad;
        float numerator = r * g * Mathf.Pow(Mathf.Cos(radAlpha), 2);
        float denominator = Mathf.Sin(2 * theta * Mathf.Deg2Rad - radAlpha) - Mathf.Sin(radAlpha);

        return Mathf.Sqrt(numerator / denominator);
        //return Mathf.Sqrt(r * g / Mathf.Sin(2 * theta * Mathf.Deg2Rad));
    }

    private float Range()
    {
        return Vector3.Distance(targetPos, transform.position);
    }

    private int Angle(float g, float u)
    {
        float r = Range();
        float radAlpha = alpha * Mathf.Deg2Rad;
        float numerator = r * g * Mathf.Pow(Mathf.Cos(radAlpha), 2);
        float intoASin = Mathf.Clamp(numerator / (u * u) + Mathf.Sin(radAlpha), -1, 1);

        return (int)(((Mathf.Asin(intoASin) * Mathf.Rad2Deg + alpha) / 2));
        //return (int)(Mathf.Asin(Mathf.Clamp(r * g / (u * u), -1, 1)) * Mathf.Rad2Deg / 2);
    }

    private int AdjustedAngleByElevation(Vector3 pivot, Vector3 other)
    {
        bool xORz = Mathf.Abs(other.x - pivot.x) > Mathf.Abs(other.z - pivot.z);

        int angle;
        if (xORz) angle = PointToDegree(new Vector2(pivot.x, pivot.y), new Vector2(other.x, other.y));
        else angle = PointToDegree(new Vector2(pivot.z, pivot.y), new Vector2(other.z, other.y));
        
        if (pivot.y > other.y) angle = -angle;

        return angle;
    }

    private float MaxHeight()
    {
        float g = Physics.gravity.magnitude;
        float u = MinimumInitialSpeed(g);
        initialSpeed = u;
        int theta = Angle(g, u);
        float yI = Mathf.Sin(theta * Mathf.Deg2Rad) * u;
        float h = yI * yI / (2 * g);
        selectedTheta = theta;

        return h;
    }

    private Vector3 HighestPoint(float y)
    {
        if (y == 0) return transform.position;
        else
        {
            float dist = Vector3.Distance(targetPos, transform.position) / 2;

            Vector3 dir = (targetPos - transform.position).normalized * dist;

            return new Vector3(transform.position.x + dir.x, y + transform.position.y + dir.y + 7.5f, transform.position.z + dir.z);
        }
    }

    public static Vector3 LookToPoint(Vector3 thisPos)
    {
        Vector3 dir = (targetPos - thisPos).normalized;
        Vector2 vertical = AngleToVector2(selectedTheta);

        return new Vector3(dir.x, vertical.y, dir.z).normalized;
    }

    public static int PointToDegree(Vector2 start, Vector2 end)
    {
        float x = end.x - start.x;
        float y = end.y - start.y;

        return y != 0 ? (int)Mathf.Abs(Mathf.Atan(y / x) * Mathf.Rad2Deg) : 0;
    }
   
    public static Vector2 AngleToVector2(float theta)
    {
        float x, y;
        theta %= 360;
        if (theta <= 45 || (theta > 315 && theta <= 359))
        {
            int mod = theta > 315 ? 0 : 1;
            theta = theta > 315 ? 45 - (355 - theta) : theta;
            x = 1;
            y = -1 + (mod + theta * (1 / 45.0f));
        }
        else if (theta > 45 && theta <= 135)
        {
            int mod = theta > 90 ? 1 : 0;
            theta = theta > 90 ? 45 - (135 - theta) : 45 - (90 - theta);
            x = 1 - (mod + theta * (1 / 45.0f));
            y = 1;
        }
        else if (theta > 135 && theta <= 225)
        {
            int mod = theta > 180 ? 1 : 0;
            theta = theta > 180 ? 45 - (225 - theta) : 45 - (180 - theta);
            x = -1;
            y = 1 - (mod + theta * (1 / 45.0f));
        }
        else if (theta > 225 && theta <= 315)
        {
            int mod = theta > 270 ? 1 : 0;
            theta = theta > 270 ? 45 - (315 - theta) : 45 - (270 - theta);
            x = -1 + (mod + theta * (1 / 45.0f));
            y = -1;
        }
        else
        {
            x = 0;
            y = 0;
        }

        return new Vector2(x, y);
    }
}
