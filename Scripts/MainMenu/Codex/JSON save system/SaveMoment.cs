using UnityEngine;

namespace SaveManager.MomentsBoard
{
    public class SaveMoment : MonoBehaviour
    {
        [Tooltip("Connect 'MomentsSaveManager' to this script.")]
        [SerializeField] private MomentsSaveManager momentsManager;
        [Tooltip("Set campaign and memory id, which game would save in this scene when player will play the game.")]
        [SerializeField] private DataForMemorySaving[] memoriesForSaving;

        public void SaveMemoryId(int id) { momentsManager.CompleteMoment(memoriesForSaving[id].campaignId, memoriesForSaving[id].memoryId); }
    }

    [System.Serializable]
    internal class DataForMemorySaving
    {
        public int campaignId;
        public int memoryId;
    }
}
