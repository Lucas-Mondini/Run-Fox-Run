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
    private new void FixedUpdate()
    {
        HandleInput();
        base.FixedUpdate();
    }

    private void Start()
    {
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

    void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }
}
