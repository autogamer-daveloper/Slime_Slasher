using UnityEngine;
using DG.Tweening;

public class TweenCleaner : MonoBehaviour
{
    private void Awake() { DOTween.KillAll(); }
}
