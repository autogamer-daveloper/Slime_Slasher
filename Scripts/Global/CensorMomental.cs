using UnityEngine;
using UnityEngine.UI;
using AGamesStudio.Censor.Containers;

public class CensorMomental : MonoBehaviour
{
    [Header("__ Global language data __")]
    [Tooltip("Global settings container, which have all language's specific settings.")]
    [SerializeField] private TranslateCentralSettings globalSettings;

    [Space(5)]
    [Header("__ Censor settings in this scene __")]
    [SerializeField] private bool useResprite = false;
    [SerializeField] private Resprite[] resprite;
    [SerializeField] private bool useReimage = false;
    [SerializeField] private Reimage[] reimage;
    [SerializeField] private bool useSpriteRecolor = false;
    [SerializeField] private SpriteRecolor[] spriteRecolor;
    [SerializeField] private bool useUIRecolor = false;
    [SerializeField] private UIRecolor[] uiRecolor;
    [SerializeField] private bool useParticlesRecolor = false;
    [SerializeField] private ParticleReimage[] particleRecolor;
    [SerializeField] private bool useHidingObjects = false;
    [SerializeField] private GameObject[] objects;

    private void Awake() { CensorCheck(); }

    internal void CensorCheck()
    {
        int lang = KeyManager.Get_Bool_Key("Language");

        if (globalSettings != null)
        {
            if (globalSettings.container[lang] == null)
            {
                Debug.LogError($"[Censor.cs]: Error with settings container in global settings, it doesn't exist! Lang ID - {lang}");
                return;
            }

            if (globalSettings.container[lang].isCensored) { CensorAll(); }
        }
        else
        {
            Debug.LogError("[Censor.cs]: Error with global settings, it doesn't exist!");
            return;
        }
    }

    private void CensorAll()
    {
        if (useResprite) { CensorSprites(true); }
        if (useReimage) { CensorImages(true); }
        if (useSpriteRecolor) { RecolorSprites(true); }
        if (useUIRecolor) { RecolorImages(true); }
        if (useParticlesRecolor) { ChangeParticlesMaterial(true); }
        if (useHidingObjects) { ChangeHidingObjects(true); }
    }

    private void UncensorAll()
    {
#if UNITY_EDITOR
        if (useResprite) { CensorSprites(false); }
        if (useReimage) { CensorImages(false); }
        if (useSpriteRecolor) { RecolorSprites(false); }
        if (useUIRecolor) { RecolorImages(false); }
        if (useParticlesRecolor) { ChangeParticlesMaterial(false); }
        if (useHidingObjects) { ChangeHidingObjects(false); }
#endif

        Debug.LogWarning("[Censor.cs]: Method UncensorAll available only in Editor for debug.");
    }

    private void CensorSprites(bool isCensored)
    {
        foreach (Resprite _resprite in resprite)
        {
            _resprite.spriteRenderer.sprite = isCensored ?
            _resprite.censoredSprite :
            _resprite.normalSprite;
        }
    }

    private void CensorImages(bool isCensored)
    {
        foreach (Reimage _reimage in reimage)
        {
            _reimage.image.sprite = isCensored ?
            _reimage.censoredSprite :
            _reimage.normalSprite;
        }
    }

    private void RecolorSprites(bool isCensored)
    {
        foreach (SpriteRecolor _spriteRecolor in spriteRecolor)
        {
            _spriteRecolor.spriteRenderer.color = isCensored ?
            _spriteRecolor.censoredColor :
            _spriteRecolor.normalColor;
        }
    }

    private void RecolorImages(bool isCensored)
    {
        foreach (UIRecolor _uiRecolor in uiRecolor)
        {
            _uiRecolor.image.color = isCensored ?
            _uiRecolor.censoredColor :
            _uiRecolor.normalColor;
        }
    }

    private void ChangeParticlesMaterial(bool isCensored)
    {
        foreach (ParticleReimage _particleRecolor in particleRecolor)
        {
            _particleRecolor.particleRenderer.sharedMaterial = isCensored ?
            _particleRecolor.censoredMaterial :
            _particleRecolor.normalMaterial;
        }
    }

    private void ChangeHidingObjects(bool isCensored)
    {
        foreach (GameObject _objects in objects)
        {
            _objects.SetActive(isCensored ? false : true);
        }
    }
}