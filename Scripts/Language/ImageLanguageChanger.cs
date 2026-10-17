using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageLanguageChanger : MonoBehaviour
{
    [Header("__ Settings __")]
    [Tooltip("Sprites for every language. (lang id = sprite id)")]
    [SerializeField] private Sprite[] sprites;

    private Image image;

    void Start()
    {
        int lang = KeyManager.Get_Bool_Key("Language");
        image = GetComponent<Image>();

        if (lang >= 0 && lang < sprites.Length) { image.sprite = sprites[lang]; }
        else { Debug.LogWarning($"[ImageLanguageChanger.cs] Missing sprite for lang index {lang}", this); }
    }
}
