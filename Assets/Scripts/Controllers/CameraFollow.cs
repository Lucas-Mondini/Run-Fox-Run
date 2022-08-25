using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform target;
    public float positionTranslateSpeed;
    public float rotationSpeed;
    private void Update()
    {
        try
        {
            HandlePosition();
            HandleRotation();
        } catch (Exception e) {}
    }
    
    void HandlePosition()
    {
        Vector3 targetPos = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPos, positionTranslateSpeed * Time.deltaTime);
    }
    void HandleRotation()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(rotation, rotation, rotationSpeed * Time.deltaTime);

    }
}
