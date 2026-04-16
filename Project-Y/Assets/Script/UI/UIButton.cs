using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButton : Button
{
    [Header("Sound")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hoverSound;

    private SoundManager soundManager;

    protected override void Start()
    {
        base.Start();
        soundManager = SoundManager.Instance;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (hoverSound != null)
            soundManager.SFXPlay("UIHover", hoverSound);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        HandleClick();
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        HandleClick();
    }

    private void HandleClick()
    {
        // 사운드
        if (clickSound != null)
            soundManager.SFXPlay("UIClick", clickSound);
    }
}