using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingProfileMain : MonoBehaviour
{
    [Tooltip("Main volume component.")]
    [SerializeField] private Volume volume;
    [Tooltip("List of volume profiles.")]
    [SerializeField] private VolumeProfile[] profiles;

    internal void SetProfile(int id)
    {
        if (profiles[id] != null) { volume.profile = profiles[id]; }
        else { Debug.LogWarning("[PostProcessingProfileMain]: wrong post process profile id"); return; }
    }
}
