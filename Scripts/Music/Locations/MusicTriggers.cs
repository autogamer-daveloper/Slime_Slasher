using UnityEngine;

public class MusicTriggers : MonoBehaviour
{
    [Header("__ Main Music Controller __")]
    [SerializeField] private MainMusicController controller;
    [Tooltip("Useless if is muting = true")]
    [SerializeField] private int musicId = 0;
    [SerializeField] private bool isMuting = false;

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) { Trigger(); } }

    private void Trigger()
    {
        if (isMuting) { controller.MuteMusic(); }
        else { controller.SetAudio(musicId); }
    }
}
