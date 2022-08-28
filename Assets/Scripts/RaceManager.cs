using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    private static RaceManager _instance;
    public  static RaceManager Instance => _instance;


    private List<GameObject> Players = new List<GameObject>();
    private List<GameObject> PlayersTimer = new List<GameObject>();
    private float _time;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        this._time = GameSettings.Time;
    }
    
    void Update()
    {
        _time -= 1 * Time.deltaTime;
        UpdatePlayerTimer();
    }
    
    
    public void AddPlayer(GameObject player)
    {
        Players.Add(player);
        PlayersTimer.Add(GameSettings.GetChild(player, "TimeLeft"));
    }

    private void UpdatePlayerTimer()
    {
        foreach (GameObject timer in PlayersTimer)
        {
            timer.GetComponent<TextMeshProUGUI>().text = _time.ToString();
        }
    }
    
}
