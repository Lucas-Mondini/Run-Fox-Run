using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CarController : MonoBehaviour
{
    
    protected float verticalInput;
    protected float horizontalInput;
    protected bool isBraking;
    
    public int index = 0;

    public float MaxHealth = 2000;
    [SerializeField] private float health = 2000;

    public CarController()
    {
    }

    public float Health => health;


    protected float steeringAngle;
    protected float currentBreakForce;

    [SerializeField] protected float motorForce;
    [SerializeField] protected float brakeForce;
    [SerializeField] protected float maxSteeringAngle;
    
    [SerializeField] protected float wheelMass = 80;
    
    [SerializeField] protected WheelCollider frontLeftWheelCollider;
    [SerializeField] protected WheelCollider frontRightWheelCollider;
    [SerializeField] protected WheelCollider rearLeftWheelCollider;
    [SerializeField] protected WheelCollider rearRightWheelCollider;
    
    [SerializeField] protected Transform frontLeftWheelTransform;
    [SerializeField] protected Transform frontRightWheelTransform;
    [SerializeField] protected Transform rearLeftWheelTransform;
    [SerializeField] protected Transform rearRightWheelTransform;

    public bool frontTraction;
    public bool rearTraction;
    

    private void Start()
    {
        this.health = MaxHealth;

    }

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
        if (frontTraction)
        {
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        }

        if (rearTraction)
        {
            rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
            rearRightWheelCollider.motorTorque = verticalInput * motorForce;
        }
        //WheelFrictionCurve curve = new WheelFrictionCurve();

        frontLeftWheelCollider.mass = wheelMass;
        frontRightWheelCollider.mass = wheelMass;
        rearRightWheelCollider.mass = wheelMass;
        rearLeftWheelCollider.mass = wheelMass;
        
            
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

    public void AddCrashDamage(float damage)
    {
        health -= damage;
    }
    public void Crash()
    {
        RaceManager.Instance.PlayerCrash(gameObject);
        
    }

    public virtual void WinGame()
    {
        
    }
    
    public virtual void TimesUp()
    {
        
    }
    
    public virtual void CarDestroyed()
    {
        
    }
}
