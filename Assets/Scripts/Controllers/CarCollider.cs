using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollider : MonoBehaviour
{
    private MeshCollider carCollider;
    private float collisionClowDown = 1f;

    private void OnValidate()
    {
        carCollider = GetComponent<MeshCollider>();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collisionClowDown < 0)
        {
            if (transform.parent.GetComponent<CarController>())
                transform.parent.GetComponent<CarController>().Crash();
            collisionClowDown = 1.0f;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        collisionClowDown -= Time.deltaTime;
    }
}
