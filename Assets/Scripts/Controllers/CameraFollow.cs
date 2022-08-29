using System;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform target;
    [SerializeField] private Image HealthBar;
    public float positionTranslateSpeed;
    public float rotationSpeed;
    private void Update()
    {
        try
        {
            HandlePosition();
            HandleRotation();
            UpdateHealth();
        } catch (Exception e) {}
    }

    private void UpdateHealth()
    {
        HealthBar.fillAmount = target.parent.GetComponent<PlayerController>().Health / target.parent.GetComponent<PlayerController>().MaxHealth; 
    }
    void HandlePosition()
    {
        Vector3 targetPos = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPos, positionTranslateSpeed * Time.deltaTime);
    }
    void HandleRotation()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
