using UnityEngine;

public class TweenVolumeManager : MonoBehaviour
{
    [Tooltip("(Optional). Made for clean management of components 'TweenVolume' for cutscenes.")]
    [SerializeField] private TweenVolume[] manageTweenVolume;

    public void SetNewVolumeValue(int id) { manageTweenVolume[id].SetNewValue(); }
    public void SetOldVolumeValue(int id) { manageTweenVolume[id].SetOldValue(); }
}
