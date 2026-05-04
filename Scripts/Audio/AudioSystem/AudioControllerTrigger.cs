using UnityEngine;

public class AudioControllerTrigger : MonoBehaviour
{
    [Header("__ Audio Controller __")]
    [SerializeField] private AudioControllerMain controller;

    private void OnTriggerEnter2D(Collider2D other) { if(other.CompareTag("Player")) { controller.TurnOn(); } }
    private void OnTriggerExit2D(Collider2D other) { if(other.CompareTag("Player")) { controller.TurnOff(); } }
}
