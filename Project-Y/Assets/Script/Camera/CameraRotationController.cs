using UnityEngine;

public class CameraRotationController : MonoBehaviour
{
    [SerializeField] public Vector2 _mousepos;
    //PlayerInput에서 Mouse/Delta(Vector2)값을 받아오기 위한 변수

    [SerializeField] private float _mouseSensitivity = 10f;//마우스 감도
    private float _currentY;
    private float _currentX;
    private float _rotationX;
    private float _rotationY;
    
    [SerializeField] private GameObject _player;
    //Player오브젝트를 받기 위한 변수

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player");
        }
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
