using UnityEngine;

public class CameraRotationController : MonoBehaviour
{
    public Vector2 _mousepos;
    private float _mouseSensitivity = 10f;
    private float _currentY;
    private float _currentX;
    private float _rotationX;
    private float _rotationY;
    private GameObject _player;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 고정
        Cursor.visible = false;
        _player = GameObject.Find("Pascal");
        //만일 카메라가 Player의 자식계층 오브젝트가 아닐때 방지
    }
    void Update()
    {
        _currentY = _mousepos.y * _mouseSensitivity * Time.deltaTime;
        _currentX = _mousepos.x * _mouseSensitivity * Time.deltaTime;
        _rotationX -= _currentY;
        _rotationY += _currentX;

        _rotationX = Mathf.Clamp(_rotationX, -85f, 90f);

        transform.localRotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
        _player.transform.rotation = Quaternion.Euler(0f, _rotationY, 0f);
    }
}
