using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCounter : MonoBehaviour
{
    public int Index;

    private void OnTriggerEnter(Collider other)
    {
        RaceManager.Instance.CrossLapCounter(
            other.transform.parent.gameObject, this);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
