using UnityEngine;

public class DeathCutsceneLocation : MonoBehaviour
{
    [Header("__ Settings __")]
    [SerializeField] private GameObject[] locations;
    [SerializeField] private GameObject[] audioSources;
    private int[] gameEpisodes = { 4, 3 };

    private void Start()
    {
        int gameEpisode = KeyManager.Get_Bool_Key("gameEpisode");

        foreach(GameObject location in locations) { location.SetActive(false); }
        foreach(GameObject source in audioSources) { source.SetActive(false); }

        if (gameEpisode >= gameEpisodes[0]) { locations[2].SetActive(true); audioSources[2].SetActive(true); }
        else if (gameEpisode >= gameEpisodes[1]) { locations[1].SetActive(true); audioSources[1].SetActive(true); }
        else { locations[0].SetActive(true); audioSources[0].SetActive(true); }
    }
}
