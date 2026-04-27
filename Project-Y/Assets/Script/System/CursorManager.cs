using System.Collections.Generic;
using UnityEngine;

public enum CursorModeState
{
    // UI 조작용: 커서 표시 + 잠금 해제
    UIVisible,
    // 인게임 조작용: 커서 숨김 + 잠금
    IngameLocked
}

/// <summary>
/// 전역 커서 상태를 요청 기반으로 관리하는 싱글톤 매니저입니다.
/// 씬에 배치하지 않아도 최초 접근시 자동으로 생성합니다.
/// </summary>
public class CursorManager : MonoBehaviour
{
    // 각 시스템(인게임, 메뉴, 팝업 등)의 커서 상태 요청 데이터
    private struct CursorRequest
    {
        public Object Owner;
        public CursorModeState State;
        public int Priority;
    }

    private static CursorManager _instance;

    /// <summary>
    /// CursorManager 싱글톤 인스턴스를 반환합니다. 없으면 자동 생성합니다.
    /// </summary>
    public static CursorManager Instance
    {
        get
        {
            // 씬에 배치되지 않아도 최초 접근 시 자동 생성
            if (_instance == null)
            {
                GameObject go = new GameObject("[CursorManager]");
                _instance = go.AddComponent<CursorManager>();
            }

            return _instance;
        }
    }

    public static bool HasInstance => _instance != null;

    [SerializeField] private CursorModeState _defaultState = CursorModeState.UIVisible;
    // 외부에서 lockState/visible을 바꿔도 다음 프레임에 상태를 복구
    [SerializeField] private bool _enforceStateEveryFrame = true;

    private readonly List<CursorRequest> _requests = new List<CursorRequest>();
    private CursorModeState _appliedState;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        ApplyResolvedState(force: true);
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    private void Update()
    {
        CleanupDestroyedOwners();

        if (_enforceStateEveryFrame)
            ApplyResolvedState(force: false);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            return;

        ApplyResolvedState(force: true);
    }

    /// <summary>
    /// 이미 인스턴스가 존재하는지 확인하고 반환합니다.
    /// </summary>
    public static bool TryGetInstance(out CursorManager manager)
    {
        manager = _instance;
        return manager != null;
    }

    /// <summary>
    /// 요청이 없을 때 적용할 기본 커서 상태를 설정합니다.
    /// </summary>
    public void SetDefaultState(CursorModeState state)
    {
        _defaultState = state;
        ApplyResolvedState(force: true);
    }

    /// <summary>
    /// owner 기준으로 커서 상태를 요청합니다. 같은 owner는 요청을 갱신합니다.
    /// </summary>
    public void RequestState(Object owner, CursorModeState state, int priority = 0)
    {
        if (owner == null)
            return;

        // 동일 owner는 덮어쓰기, 신규 owner는 추가
        int index = FindRequestIndex(owner);
        if (index >= 0)
        {
            CursorRequest req = _requests[index];
            req.State = state;
            req.Priority = priority;
            _requests[index] = req;
        }
        else
        {
            _requests.Add(new CursorRequest
            {
                Owner = owner,
                State = state,
                Priority = priority
            });
        }

        ApplyResolvedState(force: true);
    }

    /// <summary>
    /// owner가 등록한 커서 상태 요청을 해제합니다.
    /// </summary>
    public void ReleaseState(Object owner)
    {
        if (owner == null)
            return;

        int index = FindRequestIndex(owner);
        if (index >= 0)
            _requests.RemoveAt(index);

        ApplyResolvedState(force: true);
    }

    /// <summary>
    /// 현재 요청 목록에서 최종 적용될 커서 상태를 계산합니다.
    /// </summary>
    public CursorModeState GetResolvedState()
    {
        if (_requests.Count == 0)
            return _defaultState;

        // 가장 높은 우선순위 요청을 최종 상태로 사용
        int bestIndex = 0;
        int bestPriority = _requests[0].Priority;
        for (int i = 1; i < _requests.Count; i++)
        {
            if (_requests[i].Priority >= bestPriority)
            {
                bestPriority = _requests[i].Priority;
                bestIndex = i;
            }
        }

        return _requests[bestIndex].State;
    }

    /// <summary>
    /// 계산된 최종 상태를 실제 Cursor API에 반영합니다.
    /// </summary>
    private void ApplyResolvedState(bool force)
    {
        CursorModeState resolved = GetResolvedState();
        if (!force && resolved == _appliedState)
            return;

        _appliedState = resolved;
        ApplyCursorState(resolved);
    }

    /// <summary>
    /// 전달된 상태를 기반으로 커서 잠금/표시 상태를 적용합니다.
    /// </summary>
    private static void ApplyCursorState(CursorModeState state)
    {
        if (state == CursorModeState.IngameLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// owner에 해당하는 요청 인덱스를 찾습니다.
    /// </summary>
    private int FindRequestIndex(Object owner)
    {
        for (int i = 0; i < _requests.Count; i++)
        {
            if (_requests[i].Owner == owner)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// 파괴된 owner의 요청을 목록에서 정리합니다.
    /// </summary>
    private void CleanupDestroyedOwners()
    {
        for (int i = _requests.Count - 1; i >= 0; i--)
        {
            if (_requests[i].Owner == null)
                _requests.RemoveAt(i);
        }
    }
}
