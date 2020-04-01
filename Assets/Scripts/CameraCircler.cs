using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCircler : MonoBehaviour
{
    public GameObject center;

    public float rotationSpeed = 0.5f;

    public float smoothFactor = .5f;

    private Vector3 _cameraOffset;

    public void Start()
    {
        _cameraOffset = transform.position - center.transform.position;
    }

    public void Update()
    {
        Quaternion camTurnAngle = Quaternion.AngleAxis(Time.deltaTime * rotationSpeed, Vector3.up);

        _cameraOffset = camTurnAngle * _cameraOffset;

        Vector3 newPos = center.transform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);

        transform.LookAt(center.transform);

    }
}
