using UnityEngine;

/// <summary>
/// 오브젝트 활성/비활성 시점에 CursorManager로 커서 모드를 요청/해제하는 브리지 컴포넌트입니다.
/// </summary>
public class CursorModeSetter : MonoBehaviour
{
    [SerializeField] private CursorModeState _state = CursorModeState.UIVisible;
    [SerializeField] private int _priority = 0;
    [SerializeField] private bool _applyOnEnable = true;

    /// <summary>
    /// 오브젝트 활성화 시 지정된 커서 모드를 요청합니다.
    /// </summary>
    private void OnEnable()
    {
        if (!_applyOnEnable)
            return;

        CursorManager.Instance.RequestState(this, _state, _priority);
    }

    /// <summary>
    /// 오브젝트 비활성화 시 요청했던 커서 모드를 해제합니다.
    /// </summary>
    private void OnDisable()
    {
        if (CursorManager.HasInstance)
            CursorManager.Instance.ReleaseState(this);
    }
}
