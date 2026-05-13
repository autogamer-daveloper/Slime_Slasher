using UnityEngine;

public class PlayAnimationClip : MonoBehaviour
{
    [SerializeField] private Animation anim;
    [SerializeField] private string name;
    [SerializeField] private bool isDefault = true;

    public void PlayAnim()
    {
        if (isDefault) { anim.Play(); }
        else { anim.Play(name); }
    }
}
