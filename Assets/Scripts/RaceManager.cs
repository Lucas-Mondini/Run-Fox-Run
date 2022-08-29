using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    private static RaceManager _instance;
    public  static RaceManager Instance => _instance;

    public static int LapParts; 


    private List<GameObject> Players = new List<GameObject>();
    private Dictionary<GameObject, GameObject> PlayersTimer = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, GameObject> PlayersLaps = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, float> PlayersHealth = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, List<LapCounter>> PlayerAtLapPart = new Dictionary<GameObject, List<LapCounter>>();
    private Dictionary<GameObject, int> PlayerAtLap = new Dictionary<GameObject, int>();
    private float _time;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        this._time = GameSettings.Time;
    }
    
    void Update()
    {
        try
        {
            if (Time.timeScale != 0)
            {
                _time -= 1 * Time.deltaTime;
                UpdatePlayerTimer();
                HandlePlayersLoss();
            }
        }
        catch (Exception e)
        {
            //ignore
        }
    }
    
    
    public void AddPlayer(GameObject player)
    {
        Players.Add(player);
        PlayersTimer.Add(player, GameSettings.GetChild(player, "TimeLeft"));
        PlayersLaps.Add(player, GameSettings.GetChild(player, "Laps"));
        PlayersHealth.Add(player, player.GetComponent<PlayerController>().MaxHealth);
        PlayerAtLap.Add(player, 0);
        PlayerAtLapPart.Add(player, new List<LapCounter>());
        
        CarController c = player.GetComponent<PlayerController>() ?  player.GetComponent<PlayerController>() : player.GetComponent<CarController>();
        c.index = Players.Count;
    }

    public void RemovePlayer(GameObject player)
    {
        Players.Remove(player);
        PlayersTimer.Remove(player);
        PlayersLaps.Remove(player);
        PlayersHealth.Remove(player);
        PlayerAtLap.Remove(player);
    }

    public void CrossLapCounter(GameObject player, LapCounter Counter)
    {
        Debug.Log("Player: " + player + "\tCrossed Lap counter: " + Counter.Index);
        if (PlayerAtLapPart.ContainsKey(player))
        {
            if (PlayerAtLapPart[player].Count >= LapParts)
            {
                UpdatePlayerLap(player);
                PlayerAtLapPart[player] = new List<LapCounter>();
            }
            int highestIndex = 0;   
            foreach (LapCounter lapCounter in PlayerAtLapPart[player])
            {
                if (lapCounter.Index > highestIndex)
                    highestIndex = lapCounter.Index;
            }

            if (Counter.Index == highestIndex +1)
            {
                PlayerAtLapPart[player].Add(Counter);
            }
            else
            {
                if (PlayerAtLapPart[player].Contains(Counter))
                {
                    PlayerAtLapPart[player].Remove(Counter);
                }
            }
        } 
    }

    private void UpdatePlayerTimer()
    {
        foreach (var (player, timer) in PlayersTimer)
        {
            timer.GetComponent<TextMeshProUGUI>().text = _time.ToString();
        }
    }
    
    private void UpdatePlayerLap(GameObject player)
    {
        if (PlayersLaps.ContainsKey(player))
        {
            PlayerAtLap[player]++;
            PlayersLaps[player].GetComponent<TextMeshProUGUI>().text = PlayerAtLap[player].ToString();
            if (PlayerAtLap[player] >= GameSettings.Lap)
            {
                RemovePlayer(player);
                player.GetComponent<PlayerController>().WinGame();
            }
        } 
    }

    public void PlayerCrash(GameObject player)
    {
        if (PlayersHealth.ContainsKey(player))
        {
            float vel = player.transform.Find("Meshes").GetComponent<Rigidbody>().velocity.magnitude;
            float crashDamage = vel * (player.transform.Find("Meshes").GetComponent<Rigidbody>().mass / 100);
            PlayersHealth[player] -= crashDamage;
            player.GetComponent<CarController>().AddCrashDamage(crashDamage);
        }
    }

    void HandlePlayersLoss()
    {
        HandlePlayersHealth();
        HandlePlayersTime();
    }
    void HandlePlayersHealth()
    {
        foreach (var (player, health) in PlayersHealth)
        {
            if (health <= 0)
            {
                player.GetComponent<PlayerController>().CarDestroyed();
                RemovePlayer(player);
            }
        }
    }
    void HandlePlayersTime()
    {
        if (_time <= 0)
        {
            foreach (GameObject player in Players)
            {
                player.GetComponent<PlayerController>().TimesUp();
                RemovePlayer(player);
            }
        }
    }
}
