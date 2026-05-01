using UnityEngine;

/// <summary>
/// 각 UI요소에 부착되는 애니메이터다.
/// 독립적으로 실행 될 수는 없고,
/// 위에 패널이나 캔버스에 있는 UIPanel스크립트의 참조되어서 Open과 Close가 호출된다. 
/// 
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class UIAnimator : MonoBehaviour
{
    // UI가 어떤 방식으로 열리고 닫힐지 정의하는 설정값.
    [SerializeField] private UIAnimaType _uiAnimaType;

    // FadeInOut용 필드
    [SerializeField] private CanvasGroup _canvasGroup;
    public float _fadeInDuration = 0.3f;
    public float _fadeOutDuration = 0.3f;

    // ScaleInOut용 필드
    public float _scaleInDuration = 0.3f;
    public float _scaleOutDuration = 0.3f;

    // Moving용 필드
    public Vector3 _moveOffset = Vector3.zero;
    public float _moveDuration = 0.3f;



    public void Open()
    {

    }

    public void Close()
    {

    }

    private void OnDestroy()
    {
        // todo : 오브젝트가 제거될 때 남아 있는 Tween도 같이 정리한다.
    }
}