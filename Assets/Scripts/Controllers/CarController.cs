using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    
    [SerializeField]protected float verticalInput;
    [SerializeField]protected float horizontalInput;
    [SerializeField]protected bool isBraking;
    
    
    protected float steeringAngle;
    protected float currentBreakForce;

    [SerializeField] protected float motorForce;
    [SerializeField] protected float brakeForce;
    [SerializeField] protected float maxSteeringAngle;
    
    [SerializeField] protected WheelCollider frontLeftWheelCollider;
    [SerializeField] protected WheelCollider frontRightWheelCollider;
    [SerializeField] protected WheelCollider rearLeftWheelCollider;
    [SerializeField] protected WheelCollider rearRightWheelCollider;
    
    [SerializeField] protected Transform frontLeftWheelTransform;
    [SerializeField] protected Transform frontRightWheelTransform;
    [SerializeField] protected Transform rearLeftWheelTransform;
    [SerializeField] protected Transform rearRightWheelTransform;
    
    protected void FixedUpdate()
    {
        try
        {
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }
        catch (Exception e)
        { }
    }

    private void HandleMotor() {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        currentBreakForce = isBraking ? brakeForce : 0;
        HandleBrake();
    }

    private void HandleBrake()
    {
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
    }
    private void HandleSteering()
    {
        steeringAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;
    }
    private void UpdateWheels() {
            UpdateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
            UpdateWheel(frontRightWheelCollider, frontRightWheelTransform);
            UpdateWheel(rearLeftWheelCollider, rearLeftWheelTransform);
            UpdateWheel(rearRightWheelCollider, rearRightWheelTransform);
        }
    private void UpdateWheel(WheelCollider collider, Transform transform)
    {
        Vector3 pos;
        Quaternion quat;
        collider.GetWorldPose(out pos, out quat);
        transform.rotation = quat;
        transform.position = pos;
    }
}
