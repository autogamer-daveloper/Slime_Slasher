using UnityEngine;

public class MusicTriggers : MonoBehaviour
{
    [Header("__ Main Music Controller __")]
    [Tooltip("Select music controller, with which will this class be connected.")]
    [SerializeField] private MainMusicController controller;
    [Tooltip("Select MusicId, which you will hear after trigger activation. Useless if is muting = true.")]
    [SerializeField] private int musicId = 0;
    [Tooltip("Is this trigger will mute music after activation?")]
    [SerializeField] private bool isMuting = false;

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) { Trigger(); } }

    private void Trigger()
    {
        if (isMuting) { controller.MuteMusic(); }
        else { controller.SetAudio(musicId); }
    }
}
