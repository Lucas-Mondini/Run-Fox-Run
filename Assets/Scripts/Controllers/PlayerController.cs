using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : CarController
{
    [SerializeField] private CameraFollow cameraFollow;
    public float onBrakePositionTranslateSpeed;
    public float onBrakePotationSpeed;
    private float positionTranslateSpeed;
    private float rotationSpeed;
    
    public enum ControlMode
    {
        keyboard,
        buttons
    }

    public ControlMode control;
    
    private new void FixedUpdate()
    {
        HandleInput();
        base.FixedUpdate();

            if (Input.GetKey(KeyCode.Escape))
            {
                GameSettings.GetChild(gameObject, "QuitGame").SetActive(true);

                return;
            }
    }

    private void Start()
    {
        RaceManager.Instance.AddPlayer(this.gameObject);
        
        positionTranslateSpeed = cameraFollow.positionTranslateSpeed;
        rotationSpeed = cameraFollow.rotationSpeed;
    }

    private void Update()
    {
        if (isBraking)
        {
            cameraFollow.positionTranslateSpeed = onBrakePositionTranslateSpeed;
            cameraFollow.rotationSpeed = onBrakePotationSpeed;
        }
        else
        {
            cameraFollow.positionTranslateSpeed = positionTranslateSpeed;
            cameraFollow.rotationSpeed = rotationSpeed;
        }
    }

    public void VerticalInput(float input)
    {
        verticalInput = input;
    }
    public void HorizontalInput(float input)
    {
        horizontalInput = input;
    }
    public void Brake(bool input)
    {
        isBraking = input;
    }
    void HandleInput()
    {
        if (control == ControlMode.keyboard)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            isBraking = Input.GetKey(KeyCode.Space);
        } else if (control == ControlMode.buttons)
        {
            
        }
    }
    
    public void WinGame()
    {
        GameSettings.GetChild(gameObject, "EndGame").SetActive(true);
        Destroy(GetComponent<PlayerController>());
    }
    
    public void TimesUp()
    {
        GameSettings.GetChild(gameObject, "EndGame").SetActive(true);
        GameSettings.GetChild(gameObject, "EndGameText").GetComponent<TextMeshProUGUI>().text = "You lost\n" +
            "Time's Up!";
        Destroy(GetComponent<PlayerController>());
    }
    
    public void CarDestroyed()
    {
        GameSettings.GetChild(gameObject, "EndGame").SetActive(true);
        GameSettings.GetChild(gameObject, "EndGameText").GetComponent<TextMeshProUGUI>().text = "You lost\n" +
            "Car Destroyed!";
        Destroy(GetComponent<PlayerController>());
    }
    
    public static void PlayAgain()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("SampleLevel");
    }
    public static void MainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public static void QuitGame()
    {
        Application.Quit();        
    }
}
