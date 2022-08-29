using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

public class RoadSpline :MonoBehaviour
{
    [SerializeField]
    private SplineContainer splineContainer;

    [SerializeField] private GameObject lapCounter;

    public bool reloadMeshes = false;
    
    public Material roadMaterial;
    public int resolution = 50;
    public int roadBlockXSize = 20;
    public int roadBlockZSize = 20;
    
    public int fenceHeigth = 5;
    public int fenceX = 10;
    public int fenceZ = 10;
    [SerializeField, HideInInspector]private int lapsParts;

    private GameObject _road;
    
    
    [SerializeField] private GameObject fencePrefab;
    
    private void Awake()
    {
        RaceManager.LapParts = lapsParts;
    }

    public void OnValidate()
    {

        if (reloadMeshes)
        {
            reloadMeshes = false;
            try
            {
                GameObject[] roadsToDestroy = GameObject.FindGameObjectsWithTag("Road");
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.delayCall += () =>
                            {
                                foreach (var roadToDestroy in roadsToDestroy)
                                {
                                    DestroyImmediate(roadToDestroy);
                                }
                            };
                #endif

                CreateRoad();
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }

    private void CreateRoad()
    {
        List<GameObject> roadFloor = new List<GameObject>();
        GameObject road = new GameObject();
        road.name = "Road";
        road.tag = "Road";
        _road = road;
        
        lapsParts = 0;
        int lapCounterIndex = 1;
        for (int i = 0; i < splineContainer.Spline.Count; i++)
        {
            BezierCurve curve = splineContainer.Spline.GetCurve(i);
            List<GameObject> inPoint = new List<GameObject>();
            for (int j = 0; j < resolution; j++)
            {
                Vector3 point = GetPointInCurve(curve, (j / (float)resolution));

                GameObject roadMesh = new GameObject();
                roadMesh.name = "roadMeshFloorSegment" + j;
                roadMesh.transform.position = point + gameObject.transform.position;
                roadMesh.AddComponent<MeshGenerator>();
                roadMesh.GetComponent<MeshGenerator>().xSize = roadBlockXSize;
                roadMesh.GetComponent<MeshGenerator>().zSize = roadBlockZSize;
                roadMesh.GetComponent<MeshRenderer>().material = roadMaterial;
                roadMesh.transform.SetParent(road.transform);

                if (j == 0)
                {
                    CreateLapCounter(lapCounterIndex++, roadMesh);
                    lapsParts++;
                }

                inPoint.Add(roadMesh);
            }

            foreach (var o in inPoint)
                roadFloor.Add(o);
        }

        SetFloorListInPosition(roadFloor);
    }

    private GameObject CreateLapCounter(int index, GameObject parentGameObject)
    {
        GameObject lapCount = Instantiate(lapCounter, parentGameObject.transform, false);
        lapCount.GetComponent<LapCounter>().Index = index;

        Vector3 position = parentGameObject.transform.position;
        Vector3 scale = parentGameObject.transform.localScale;
        
        position.z += roadBlockZSize / 2;
        scale.z += roadBlockZSize;

        lapCount.transform.position = position;
        lapCount.transform.localScale = scale;

        return lapCount;
    }
    private Vector3 GetPointInCurve(BezierCurve curve, float t)
    {
        Vector3 p01 = Vector3.Lerp(curve.P0, curve.P1, t);
        Vector3 p12 = Vector3.Lerp(curve.P1, curve.P2, t);
        Vector3 p23 = Vector3.Lerp(curve.P2, curve.P3, t);
        Vector3 p012 = Vector3.Lerp(p01, p12, t);
        Vector3 p123 = Vector3.Lerp(p12, p23, t);
        return Vector3.Lerp(p012, p123, t);
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
        GameObject lFence = Instantiate(fencePrefab, road.transform, false);
        Vector3 rot = lFence.transform.rotation.eulerAngles;
        Vector3 scale =  lFence.transform.localScale;

        scale.z = fenceZ;
        scale.x = fenceX;
        scale.y = fenceHeigth;
        
        rot.y += 90;
        lFence.transform.rotation = Quaternion.Euler(rot);
        lFence.transform.localScale = scale;

        GameObject rFence = Instantiate(fencePrefab, road.transform, false);
        Vector3 pos = rFence.transform.localPosition;
        rot = lFence.transform.rotation.eulerAngles;
        pos = rFence.transform.localPosition;
        scale =  rFence.transform.localScale;

        rot.y -= 180;

        pos.z = roadBlockZSize;
        pos.x = 1;
        
        scale.z = fenceZ;
        scale.x = fenceX;
        scale.y = fenceHeigth;
        
        rFence.transform.rotation = Quaternion.Euler(rot);
        rFence.transform.localPosition = pos;
        rFence.transform.localScale = scale;
        
    }
    private void SetFloorInPosition(GameObject currentFloor, GameObject lastFloor)
    {
        var lastFloorPosition = lastFloor.transform.position;
        currentFloor.transform.LookAt(lastFloorPosition);
        Vector3 rotation = currentFloor.transform.rotation.eulerAngles;
        float y = rotation.y + 270;
        rotation.y = y;
        currentFloor.transform.rotation =  Quaternion.Euler(rotation);

        Vector3 scale = currentFloor.transform.localScale;
        scale.x = Vector3.Distance(currentFloor.transform.position, lastFloorPosition);
        currentFloor.transform.localScale = scale;
    }
}
