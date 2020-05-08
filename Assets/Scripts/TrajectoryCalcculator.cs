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
    public static Vector2[] trajectories = new Vector2[5];
    public static Vector2 chosen = Vector2.zero;
    public int skillLevel = 0;

    private Vector3 testRange;
    private float r, g = Physics.gravity.magnitude, minU, maxU = 20;
    public static int selectedTheta;
    private int alpha;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = player.position + Vector3.up * .95f;
    }

    public void SelectTrajectory()
    {
        targetPos = AccuracyTollerance();
        LoadVariables();
        LoadAngles();
        for(int i = 0; i < trajectories.Length; i++)
        {
            float maxHeight = MaxHeight(trajectories[0]);
            highestP = HighestPoint(maxHeight);
        }
        chosen = trajectories[Random.Range(0, trajectories.Length)];
        float t = Time((int)chosen.x, chosen.y);
        lookTo = LookToPoint(t);
    }

    private Vector3 AccuracyTollerance()
    {
        int mod = Random.Range(0, 5 + skillLevel);

        switch(mod)
        {
            case 0: return targetPos;
            case 1: return targetPos + Vector3.forward; ;
            case 2: return targetPos - Vector3.forward ;
            case 3: return targetPos + Vector3.right;
            case 4: return targetPos - Vector3.right;
            default: return targetPos;
        }
    }

    private void LoadVariables()
    {
        r = Range();
        minU = MinimumInitialSpeed(g);
    }

    private float Range()
    {
        return Vector3.Distance(targetPos, transform.position);
    }

    private float MinimumInitialSpeed(float g)
    {
        float r = Range();
        alpha = AdjustedAngleByElevation(transform.position, targetPos);
        int theta = Mathf.Clamp(45 + alpha, alpha, 90);
        float radAlpha = alpha * Mathf.Deg2Rad;
        float numerator = r * g * Mathf.Pow(Mathf.Cos(radAlpha), 2);
        float denominator = Mathf.Sin(2 * (theta - alpha) * Mathf.Deg2Rad) - Mathf.Sin(radAlpha);

        return Mathf.Sqrt(numerator / denominator);
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

    public static int PointToDegree(Vector2 start, Vector2 end)
    {
        float x = end.x - start.x;
        float y = end.y - start.y;

        bool isZero = x == 0 || y == 0;

        return isZero ? (int)Mathf.Abs(Mathf.Atan(y / x) * Mathf.Rad2Deg) : 0;
    }

    private void LoadAngles()
    {
        float u = minU;
        for (int i = 0; i < trajectories.Length; i++)
        {
            trajectories[i].x = Mathf.Clamp(Angle(g, u), alpha, 90);
            trajectories[i].y = u;

            u = Random.Range(minU, maxU);
        }
    }

    private int Angle(float g, float u)
    {
        float r = Range();
        float radAlpha = alpha * Mathf.Deg2Rad;
        float numerator = r * g * Mathf.Pow(Mathf.Cos(radAlpha), 2);
        float intoASin = Mathf.Clamp(numerator / (u * u) + Mathf.Sin(radAlpha), -1, 1);

        return (int)((Mathf.Asin(intoASin) * Mathf.Rad2Deg + alpha) / 2);
    }

    private float MaxHeight(Vector2 trajectory)
    {
        int theta = (int)trajectory.x;
        float u = trajectory.y;
        float yI = Mathf.Sin(theta * Mathf.Deg2Rad) * u;
        float h = yI * yI / (2 * g);

        return h;
    }

    private Vector3 HighestPoint(float y)
    {
        if (y == 0) return transform.position;
        else
        {
            float dist = Vector3.Distance(targetPos, transform.position) / 2;

            Vector3 dir = (targetPos - transform.position).normalized * dist;

            return new Vector3(transform.position.x + dir.x, y + transform.position.y + dir.y, transform.position.z + dir.z);
        }
    }

    private float Time(int theta, float u)
    {
        float radAlpha = alpha * Mathf.Deg2Rad;
        return 2 * u * Mathf.Sin((theta - alpha) * Mathf.Deg2Rad) / (g * Mathf.Cos(radAlpha));
    }

    public Vector3 LookToPoint(float time)
    {
        Vector3 origin = transform.position;

        Vector3 distance = targetPos - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float sY = distance.y;
        float sXZ = distanceXZ.magnitude;

        float vXZ = sXZ / time;
        float vY = sY / time + .5f * g * time;

        Vector3 result = distanceXZ.normalized;
        result *= vXZ;
        result.y = vY;

        return result;
    }

    private bool IsClearTrajectory(Vector3 highestP)
    {
        bool up = Physics.CheckCapsule(transform.position, highestP, .75f), down = true;
        Collider[] cols = Physics.OverlapCapsule(highestP, targetPos, .75f);
        foreach (Collider c in cols)
        {
            if (c.CompareTag("Obstacle"))
            {
                down = false;
                break;
            }
        }

        return up && down;
    }
}
