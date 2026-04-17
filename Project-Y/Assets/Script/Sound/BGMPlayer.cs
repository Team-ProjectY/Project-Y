using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;

    void Start()
    {
        SoundManager.Instance.PlayBGM(bgmClip);
    }
}