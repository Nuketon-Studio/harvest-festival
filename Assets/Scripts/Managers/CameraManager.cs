using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float _distanceToPlayer;
    private Vector2 _input;

    [SerializeField] private MouseSensitivity mouseSensitivity;
    [SerializeField] private CameraAngle cameraAngle;
    [SerializeField] private CameraDistance cameraDistance;

    private CameraRotation _cameraRotation;
    private bool _attachedCamInTarget = false;
    [SerializeField]private Transform _target;
    private bool _turnOffAttachment = false;

    public void Attachment(Transform target)
    {
        _attachedCamInTarget = true;
        _target = target;

        _distanceToPlayer = Vector3.Distance(transform.position, _target.position);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // metodo para ligar e desligar o turnoff da camera seguindo
    public void TurnToggleCamera()
    {
        _turnOffAttachment = !_turnOffAttachment;

        if(_turnOffAttachment) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
    }

    public void RotateTargetWithCamera()
    {
        if (_input.sqrMagnitude == 0 || _turnOffAttachment) return;

        var camRotate = new Vector3(0, transform.eulerAngles.y, 0);
        _target.transform.rotation = Quaternion.RotateTowards(_target.transform.rotation, Quaternion.Euler(camRotate), 400f);
    }

    private void Update()
    {
        if (!_attachedCamInTarget || _turnOffAttachment) return;

        _input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        _cameraRotation.Yaw += _input.x * mouseSensitivity.horizontal * BoolToInt(mouseSensitivity.invertHorizontal) * Time.deltaTime;
        _cameraRotation.Pitch += _input.y * mouseSensitivity.vertical * BoolToInt(mouseSensitivity.invertVertical) * Time.deltaTime;
        _cameraRotation.Pitch = Mathf.Clamp(_cameraRotation.Pitch, cameraAngle.min, cameraAngle.max);

        RotateTargetWithCamera();
    }

    private void LateUpdate()
    {
        if (!_attachedCamInTarget || _turnOffAttachment) return;

        transform.eulerAngles = new Vector3(_cameraRotation.Pitch, _cameraRotation.Yaw, 0.0f);
        transform.position = _target.position - transform.forward * _distanceToPlayer + new Vector3(cameraDistance.x, cameraDistance.y, 0);
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
public struct CameraDistance
{
    [Range(-10, 10)]
    public float x;
    [Range(-10, 10)]
    public float y;
}
