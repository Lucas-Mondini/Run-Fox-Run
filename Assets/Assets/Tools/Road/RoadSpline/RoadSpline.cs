using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using Object = UnityEngine.Object;

public class RoadSpline :MonoBehaviour
{
    [SerializeField]
    private SplineContainer splineContainer;

    public int Roadwidth = 5;
    public Material roadMaterial;
    public int resolution = 500;
    public int roadBlockXSize = 5;
    public int roadBlockZSize = 1;
    [SerializeField] private GameObject fencePrefab;

    public void Start()
    {
        List<GameObject> roadFloor = new List<GameObject>();
        GameObject road = new GameObject();
        road.name = "Road";
        for (int i = 0; i < splineContainer.Spline.Count; i++)
        {
            BezierCurve curve = splineContainer.Spline.GetCurve(i); ;
            List<GameObject> inPoint = new List<GameObject>();
            for (int j = 0; j < resolution; j++) 
            {
                Vector3 point = GetPointInCurve(curve, (j / (float) resolution));
                
                GameObject roadMesh = new GameObject();
                roadMesh.name = "roadMeshFloor";
                roadMesh.transform.position =  point + gameObject.transform.position;
                roadMesh.AddComponent<MeshGenerator>();
                roadMesh.GetComponent<MeshGenerator>().xSize = roadBlockXSize;
                roadMesh.GetComponent<MeshGenerator>().zSize = roadBlockZSize;
                roadMesh.GetComponent<MeshRenderer>().material = roadMaterial;
                roadMesh.transform.SetParent(road.transform);
                

                inPoint.Add(roadMesh);
            }
            foreach (var o in inPoint) 
                roadFloor.Add(o);
        }
        SetFloorListInPosition(roadFloor);
    }

    private Vector3 GetPointInCurve(BezierCurve curve, float t)
    {
        Vector3 P01 = Vector3.Lerp(curve.P0, curve.P1, t);
        Vector3 P12 = Vector3.Lerp(curve.P1, curve.P2, t);
        Vector3 P23 = Vector3.Lerp(curve.P2, curve.P3, t);
        Vector3 P012 = Vector3.Lerp(P01, P12, t);
        Vector3 P123 = Vector3.Lerp(P12, P23, t);
        return Vector3.Lerp(P012, P123, t);
    }

    private void SetFloorListInPosition(List<GameObject> roadFloor)
    {
        //percorre a lista de tras para frente
        for (int i = roadFloor.Count - 1, lastIndex = 0; i >= 0; i--)
        {
            SetFloorInPosition(roadFloor[i], roadFloor[lastIndex]);
            AddFencesToRoad(roadFloor[i]);
            lastIndex = i;
        }
    }

    private void AddFencesToRoad(GameObject road)
    {
        GameObject L_fence = Instantiate(fencePrefab, road.transform, false);
        Vector3 rot = L_fence.transform.rotation.eulerAngles;
        rot.y += 90;
        L_fence.transform.rotation = Quaternion.Euler(rot);

        GameObject R_fence = Instantiate(fencePrefab, road.transform, false);
        rot = L_fence.transform.rotation.eulerAngles;
        rot.y -= 180;
        R_fence.transform.rotation = Quaternion.Euler(rot);

        Vector3 pos = R_fence.transform.localPosition;
        pos.z = roadBlockZSize;
        pos.x = 1;
        R_fence.transform.localPosition = pos;
        
        Vector3 scale =  R_fence.transform.localScale;
        scale.z = -2;
        R_fence.transform.localScale = scale;


    }
    private void SetFloorInPosition(GameObject currentFloor, GameObject lastFloor)
    {
        currentFloor.transform.LookAt(lastFloor.transform.position);
        Vector3 rotation = currentFloor.transform.rotation.eulerAngles;
        float y = rotation.y + 270;
        rotation.y = y;
        currentFloor.transform.rotation =  Quaternion.Euler(rotation);

        Vector3 scale = currentFloor.transform.localScale;
        scale.x = Vector3.Distance(currentFloor.transform.position, lastFloor.transform.position);
        scale.z = Roadwidth;
        currentFloor.transform.localScale = scale;
    }
}