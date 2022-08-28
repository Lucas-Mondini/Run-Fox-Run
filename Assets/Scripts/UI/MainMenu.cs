using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SettingsMenu settings;
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleLevel");
    }

    public void Quit()
    {
        Debug.Log("Quiting");
        Application.Quit();
    }
}
