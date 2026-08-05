using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RenderSettings : MonoBehaviour
{
    [SerializeField] private UniversalRenderPipelineAsset SpecAsset;
    private RenderPipelineAsset defaultAsset;

    void Awake()
    {
        // Сохраняем стандартный 2D ассет
        defaultAsset = GraphicsSettings.defaultRenderPipeline;
        // Включаем 3D ассет для этой сцены
        GraphicsSettings.defaultRenderPipeline = SpecAsset;
    }

    void OnDestroy()
    {
        // Возвращаем 2D ассет при выходе из сцены
        GraphicsSettings.defaultRenderPipeline = defaultAsset;
    }
}
