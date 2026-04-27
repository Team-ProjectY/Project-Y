using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class LookRotation : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 3f;
    [Header("Cursor")]
    [SerializeField] private bool _requestIngameCursor = true;
    [SerializeField] private bool _requestOnEnable = true;
    [SerializeField] private bool _requestOnFocus = true;
    [SerializeField] private bool _requestOnLeftClick = true;
    [SerializeField] private int _cursorPriority = 100;

    private Vector2 _input;

    private void OnEnable()
    {
        if (_requestIngameCursor && _requestOnEnable)
            RequestIngameCursor();
    }

    private void OnDisable()
    {
        if (CursorManager.HasInstance)
            CursorManager.Instance.ReleaseState(this);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (_requestIngameCursor && _requestOnFocus && hasFocus)
            RequestIngameCursor();
    }

    private void Update()
    {
        if (!_requestIngameCursor)
            return;

        if (_requestOnLeftClick && IsLeftClickPressedThisFrame())
            RequestIngameCursor();
    }

    /// <summary>
    /// 입력 세팅 (PlayerInput에서 호출)
    /// </summary>
    public void SetInput(Vector2 input) => _input = input;

    /// <summary>
    /// 프레임 기준 회전값 반환
    /// </summary>
    public Vector2 GetLookDelta()
    {
        return _input * _sensitivity * Time.deltaTime;
    }

    private void RequestIngameCursor()
    {
        CursorManager.Instance.RequestState(this, CursorModeState.IngameLocked, _cursorPriority);
    }

    private static bool IsLeftClickPressedThisFrame()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
#else
        return Input.GetMouseButtonDown(0);
#endif
    }
}
