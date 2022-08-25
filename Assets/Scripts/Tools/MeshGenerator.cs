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
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        for (int i = 0, z = 0; z < zSize + 1; z++)
        for (int x = 0; x < xSize + 1; x++)
            vertices[i++] = new Vector3(x * xWidth, 0, z * zWidth);
    }

    void CreateTriangles()
    {
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }

            vert++;
        }
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
