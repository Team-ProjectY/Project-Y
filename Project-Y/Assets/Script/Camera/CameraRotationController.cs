using UnityEngine;

public class CameraRotationController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 200f;
    [SerializeField] private Transform playerBody;

    private Vector2 _lookInput;

    private float _xRotation;
    private float _yRotation;

    public void SetLookInput(Vector2 input) => _lookInput = input;

    void Awake()
    {
        // 게임 시작 시 커서를 화면 중앙에 고정 및 숨김
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 인스펙터에서 미할당 시 플레이어를 자동 참조
        if (playerBody == null)
            playerBody = transform.parent;
    }


    void Update()
    {
        // 감도와 프레임 독립성을 적용한 이번 프레임의 회전량 계산
        float mouseX = _lookInput.x * sensitivity * Time.deltaTime;
        float mouseY = _lookInput.y * sensitivity * Time.deltaTime;

        _yRotation += mouseX;           // 좌우 회전 누적
        _xRotation -= mouseY;           // 상하 회전 누적 (마우스 Y와 카메라 회전 방향이 반대라 -= 사용)
        _xRotation = Mathf.Clamp(_xRotation, -85f, 85f);   // 고개를 너무 위아래로 꺾지 못하도록 제한

        // 카메라 : 상하 회전만 담당 (로컬 기준, y·z축 고정)
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        // 플레이어 몸체 : 좌우 회전만 담당 (월드 기준, x·z축 고정)
        playerBody.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}