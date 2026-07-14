using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadLevel : MonoBehaviour { internal static void LoadLevelById(int id) { DOTween.KillAll(); DOTween.Clear(); SceneManager.LoadScene(id); } }