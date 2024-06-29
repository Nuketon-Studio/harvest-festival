using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float _distanceToPlayer;
    private Vector2 _input;

    [SerializeField] private MouseSensitivity mouseSensitivity;
    [SerializeField] private CameraAngle cameraAngle;
    [SerializeField] private CameraDistance cameraDistance;

    private CameraRotation _cameraRotation;

    private void Awake()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, target.position);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RotateTargetWithCamera()
    {
        if (_input.sqrMagnitude == 0) return;

        var camRotate = new Vector3(0, transform.eulerAngles.y, 0);
        target.transform.rotation = Quaternion.RotateTowards(target.transform.rotation, Quaternion.Euler(camRotate), 400f);
    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        _cameraRotation.Yaw += _input.x * mouseSensitivity.horizontal * BoolToInt(mouseSensitivity.invertHorizontal) * Time.deltaTime;
        _cameraRotation.Pitch += _input.y * mouseSensitivity.vertical * BoolToInt(mouseSensitivity.invertVertical) * Time.deltaTime;
        _cameraRotation.Pitch = Mathf.Clamp(_cameraRotation.Pitch, cameraAngle.min, cameraAngle.max);

        RotateTargetWithCamera();
    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(_cameraRotation.Pitch, _cameraRotation.Yaw, 0.0f);
        transform.position = target.position - transform.forward * _distanceToPlayer + new Vector3(cameraDistance.x, cameraDistance.y,0);
    }

    private static int BoolToInt(bool b) => b ? 1 : -1;
}

[Serializable]
public struct MouseSensitivity
{
    public float horizontal;
    public float vertical;
    public bool invertHorizontal;
    public bool invertVertical;
}

public struct CameraRotation
{
    public float Pitch;
    public float Yaw;
}

[Serializable]
public struct CameraAngle
{
    public float min;
    public float max;
}

[Serializable]
public struct CameraDistance {
    [Range(-10, 10)]
    public float x;
    [Range(-10, 10)]
    public float y;
}
