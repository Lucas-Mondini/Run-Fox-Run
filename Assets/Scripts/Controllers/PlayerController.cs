using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
