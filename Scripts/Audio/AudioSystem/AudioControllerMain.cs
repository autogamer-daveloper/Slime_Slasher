using UnityEngine;
using DG.Tweening;

public class AudioControllerMain : MonoBehaviour
{
    [Header("__ Audio Source __")]
    [SerializeField] private AudioSource source;

    internal void TurnOn() { source.DOFade(0.5f, 1f); }
    internal void TurnOff() { source.DOFade(0f, 1f); }
}
