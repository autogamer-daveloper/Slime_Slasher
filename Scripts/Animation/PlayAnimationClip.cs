using UnityEngine;

public class PlayAnimationClip : MonoBehaviour
{
    [SerializeField] private Animation anim;
    [SerializeField] private new string name;
    [SerializeField] private bool isDefault = true;
    [SerializeField] private bool isOnEnablePlay = false;

    private void Start() { if(isOnEnablePlay == true) { PlayAnim(); }}

    public void PlayAnim()
    {
        if (isDefault) { anim.Play(anim.clip.name); }
        else { anim.Play(name); }
    }
}
