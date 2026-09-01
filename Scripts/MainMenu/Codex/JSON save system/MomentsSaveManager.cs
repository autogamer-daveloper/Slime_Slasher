using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SaveManager.MomentsBoard
{
    public class MomentsSaveManager : MonoBehaviour
    {
        [Tooltip("If you won't use interface to show memories, you can turn off other fields.")]
        [SerializeField] private bool usingInterface = false;

        [Space(10)]
        [Tooltip("Data for every memory card.")]
        [SerializeField] private MemoryCard[] memories;
        [Space(5)]
        [Tooltip("Uses if memory is unknown now (If player don't see moment in the game).")]
        [SerializeField] private Sprite unknownSprite;

        [Space(10)]
        [Tooltip("Enable this if you want to call methods at Start.")]
        [SerializeField] private bool callAtStart = false;
        [Space(5)]
        [Tooltip("What methods will be called at Start?")]
        [SerializeField] private UnityEvent atStart;

        private string SavePath => Path.Combine(Application.persistentDataPath, "progress.json");
        private GameProgress _progress;

        private void Awake() { Load(); GenerateMemories(); }

        private void Start() { if(callAtStart) { atStart.Invoke(); } }

        #region Load / Save JSON file

        public void Load()
        {
            if (!File.Exists(SavePath))
            {
                _progress = new GameProgress();
                return;
            }

            string json = File.ReadAllText(SavePath);
            _progress = JsonUtility.FromJson<GameProgress>(json);

            if (_progress == null) { _progress = new GameProgress(); }
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(_progress, true);
            File.WriteAllText(SavePath, json);
        }

        #endregion

        #region Work with "Memory board" data

        public bool IsMomentCompleted(int campaignId, int momentId)
        {
            CampaignProgress campaign = GetCampaign(campaignId);

            if (campaign == null) { return false; }

            return campaign.completedMoments.Contains(momentId);
        }

        public void CompleteMoment(int campaignId, int momentId)
        {
            CampaignProgress campaign = GetCampaign(campaignId);

            if (campaign == null)
            {
                campaign = new CampaignProgress { campaignId = campaignId };
                _progress.campaigns.Add(campaign);
            }

            if (!campaign.completedMoments.Contains(momentId))
            {
                campaign.completedMoments.Add(momentId);
                Save();
            }
        }

        private CampaignProgress GetCampaign(int campaignId)
        {
            return _progress.campaigns.Find(
                campaign => campaign.campaignId == campaignId
            );
        }

        #endregion

        #region Work with "Memory board" UI

        private void GenerateMemories()
        {
            if (!usingInterface) { return; }
            if (memories == null || memories.Length == 0)
            {
                Debug.LogError("|MomentsSaveManager.cs|: memories data is empty and you using interface, fix it for using UI");
                return;
            }

            for (int i = 0; i < memories.Length; i++)
            {
                int id = i;
                GenerateMemoryCard(id, memories[id].campaignId, memories[id].memoryId);
            }
        }

        private void GenerateMemoryCard(int cardId, int campaignId, int memoryId)
        {
            bool isCompletedMemory = IsMomentCompleted(campaignId, memoryId);

            if (isCompletedMemory)
            {
                if (memories[cardId].image == null || memories[cardId].completedSprite == null)
                {
                    Debug.LogError($"|MomentsSaveManager.cs|: memories[{cardId}].image or memories[{cardId}].completedSprite is null");
                    return;
                }
                memories[cardId].image.sprite = memories[cardId].completedSprite;
            }
            else
            {
                if (memories[cardId].image == null || unknownSprite == null)
                {
                    Debug.LogError($"|MomentsSaveManager.cs|: memories[{cardId}].image or unknownSprite is null");
                    return;
                }
                memories[cardId].image.sprite = unknownSprite;
            }
        }

        #endregion
    }

    #region Data bases

    [System.Serializable]
    internal class CampaignProgress
    {
        public int campaignId;
        public List<int> completedMoments = new();
    }

    [System.Serializable]
    internal class GameProgress
    {
        public List<CampaignProgress> campaigns = new();
    }

    [System.Serializable]
    internal class MemoryCard
    {
        public Image image;
        public Sprite completedSprite;
        public int campaignId = 0;
        public int memoryId = 0;
    }

    #endregion
}