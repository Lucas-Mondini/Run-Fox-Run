using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;


    public bool drawGizmos = false;
    public int xSize = 20;
    public int zSize = 20;
    public float zWidth = 1;
    public float xWidth = 1;


    private void Start()
    {
        CreateShape();
        UpdateMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        //GetComponent<MeshCollider>().convex = true;
    }

    void OnValidate()
    {
        mesh = new Mesh();
        if(!GetComponent<MeshFilter>())
            gameObject.AddComponent<MeshFilter>();
        if(!GetComponent<MeshRenderer>())
            gameObject.AddComponent<MeshRenderer>();
        if (!GetComponent<MeshCollider>())
            gameObject.AddComponent<MeshCollider>();
        
        CreateShape();
        UpdateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    

    void CreateShape()
    {
        CreateVertices();
        CreateTriangles();
    }

    void CreateVertices()
    {
        vertices = new Vector3[4];
        vertices[0] = new Vector3(0, 0, zSize * zWidth);
        vertices[1] = new Vector3(xSize * xWidth, 0, zSize * zWidth);
        vertices[2] = new Vector3(0, 0, 0);
        vertices[3] = new Vector3(zSize * xWidth, 0, 0);
    }

    void CreateTriangles()
    {
        triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 2;
        triangles[4] = 1;
        triangles[5] = 3;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if(drawGizmos)
            if (vertices != null)
            {
                for (int i = 0; i < vertices.Length; i++)
                    Gizmos.DrawSphere(vertices[i] + transform.position, 0.1f);
            }
    }
}
