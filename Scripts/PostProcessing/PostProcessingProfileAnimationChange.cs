using UnityEngine;

public class PProfileAnimationChange : MonoBehaviour
{
    [Tooltip("For using this component you should connect it to the 'Post Processing Profile Main' component.")]
    [SerializeField] private PostProcessingProfileMain main;

    public void ProfileRequest(int id) { main.SetProfile(id); }
}
