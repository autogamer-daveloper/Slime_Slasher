using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChangeLightSourceColorByCallback : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [Tooltip("New color, which will be set up to the global light when callback.")]
    [SerializeField] private Color newColor;

    public void Callback() { globalLight.color = newColor; }
}
