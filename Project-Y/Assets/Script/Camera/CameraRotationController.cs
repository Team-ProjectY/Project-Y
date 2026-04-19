using UnityEngine;

public class CameraRotationController : MonoBehaviour
{
    public Vector2 _mousepos;
    private float _mouseSensitivity = 10f;
    private float _currentY;
    private float _currentX;
    private float _rotationX;
    private float _rotationY;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 고정
        Cursor.visible = false;
    }
    void Update()
    {
        _currentY = _mousepos.y * _mouseSensitivity * Time.deltaTime;
        _currentX = _mousepos.x * _mouseSensitivity * Time.deltaTime;
        _rotationX -= _currentY;
        _rotationY += _currentX;
        _rotationY = Mathf.Clamp(_rotationY, -85f, 90f);

        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, transform.rotation.z);
    }
}
