using UnityEngine;

public class PProfileColliderChange : MonoBehaviour
{
    [Tooltip("For using this component you should connect it to the 'Post Processing Profile Main' component.")]
    [SerializeField] private PostProcessingProfileMain main;
    [Tooltip("Set profile id from main component for this trigger.")]
    [SerializeField] private int id = 0;

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) { main.SetProfile(id); } }
}
