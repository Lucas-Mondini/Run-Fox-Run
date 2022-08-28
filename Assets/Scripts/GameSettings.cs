using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{

    private static GameSettings _instance;

    public static GameSettings Instance => _instance;

    public static int Time = 120;
    public static int Lap = 2;

    public void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public static GameObject GetChild(GameObject go, string childName)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            GameObject child = go.transform.GetChild(i).gameObject;
            if (child.name == childName)
                return child;
            child = GetChild(child, childName);
            if (child != null)
                return child;
        }
        return null;
    }
}
