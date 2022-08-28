using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{

    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI LapText;

    private void Start()
    {
        TimeText.text = GameSettings.Time.ToString();
        LapText.text = GameSettings.Lap.ToString();
    }

    public void AddTime()
    {
        int time = int.Parse(TimeText.text);
        if (time < 300)
        {
            time += 20;
        }

        TimeText.text = time.ToString();
        GameSettings.Time = time;
    }
    public void RemoveTime()
    {
        int time = int.Parse(TimeText.text);
        if (time > 60)
        {
            time -= 20;
        }
        TimeText.text = time.ToString();
        GameSettings.Time = time;
    }
    public void AddLap()
    {
        int lap = int.Parse(LapText.text);
        if (lap < 10)
        {
            lap += 1;
        }

        LapText.text = lap.ToString();
        GameSettings.Lap = lap;
    }
    public void RemoveLap()
    {
        int lap = int.Parse(LapText.text);
        if (lap > 1)
        {
            lap -= 1;
        }

        LapText.text = lap.ToString();
        GameSettings.Lap = lap;
    }
}
