using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GenerateTerrain : MonoBehaviour
{
    public int xSize, zSize, density, scale, numBalls;
    public GameObject obstacle, player, ball;

    private int volume, remainingVol; 
    private Vector3[] verts;
    private int[] triangles;
    private Mesh mesh;


    // Start is called before the first frame update
    void Start()
    {
        volume = xSize * zSize / 8;
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        OriginalMesh();
    }

    private void Update()
    {
        //UpdateMesh();
        //if (Input.GetKeyUp(KeyCode.Space)) NewTerrain();
    }

    private void OriginalMesh()
    {
        verts = new Vector3[(xSize * density + 1) * (zSize * density + 1)];
        for (int i = 0, z = 0; z <= zSize * density; z++)
        {
            for (int x = 0; x <= xSize * density; x++, i++)
            {
                verts[i] = new Vector3(x / (float)density, 0, z / (float)density);
            }
        }

        triangles = new int[xSize * zSize * 6 * density * density];

        for (int v = 0, t = 0, z = 0; z < zSize * density; z++, v++)
        {
            for (int x = 0; x < xSize * density; x++, v++, t += 6)
            {
                triangles[t] = v;
                triangles[t + 3] = triangles[t + 1] = v + xSize * density + 1;
                triangles[t + 5] = triangles[t + 2] = v + 1;
                triangles[t + 4] = v + xSize * density + 2;
            }
        }

        AddEntities();
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = verts;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        MeshCollider mc = gameObject.AddComponent<MeshCollider>();
        mc.sharedMesh = mesh;
    }

    public void UpdateMesh(Vector3[] verts)
    {
        mesh.Clear();        

        mesh.vertices = verts;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        MeshCollider mc = gameObject.AddComponent<MeshCollider>();
        mc.sharedMesh = mesh;
    }

    private void NewTerrain()
    {        
        UpdateMesh();
        AddEntities();
    }

    private void AddEntities()
    {
        remainingVol = volume;
        for (int i = 0; i < remainingVol; i++)
        {
            Vector3 pos = RandomVert();
            GameObject instance = Instantiate(obstacle, Vector3.zero, Quaternion.identity);
            instance = SetScale(instance);
            float height = instance.transform.localScale.y / 2;
            Vector3 adjusted = instance.transform.position + pos + Vector3.up * height;
            instance.transform.position = adjusted;
            instance.transform.parent = transform;
        }
        player.transform.position = new Vector3(xSize / 2, 1.5f, -2);

        for(int i = 0; i < numBalls; i++)
        {
            Vector3 pos = RandomVert();
            GameObject instance = Instantiate(ball, Vector3.zero, Quaternion.identity);
            SphereCollider sc = instance.GetComponent<SphereCollider>();
            float height = sc.radius * 2 + .05f;
            Vector3 adjusted = instance.transform.position + pos + Vector3.up * height;
            instance.transform.position = adjusted;
        }
    }

    private GameObject SetScale(GameObject g)
    {        
        int horizontal = Mathf.Clamp(Random.Range(1, 5), 1, remainingVol);
        int y = Mathf.Clamp(Random.Range(2, 5), 1, remainingVol / horizontal);
        int x = 1, z = 1;

        if (Random.Range(0, 2) > 0) x = horizontal;
        else z = horizontal;

        remainingVol -= horizontal * y;

        g.transform.localScale = new Vector3(x, y, z);
        return g;
    }

    private Vector3 RandomVert()
    {
        int x = Random.Range(0, xSize * density + 1);
        int z = Random.Range(0, zSize * density + 1);
        Vector3 pos = verts[x * zSize * density + z];
        
        return pos;
    }

    public Vector3[] GetVertices()
    {
        return verts;
    }

    public int GetXSize()
    {
        return xSize;
    }

    public int GetZSize()
    {
        return zSize;
    }
}
