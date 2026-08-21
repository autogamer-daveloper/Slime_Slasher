using UnityEngine;

public class PlayerBlockInput : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerAttacking attacking;

    private void OnEnable()
    {
        if (controller != null) { controller.DialogueBlock(true); }
        if (attacking != null) { attacking.DialogueBlock(true); }
    }
    private void OnDisable()
    {
        if (controller != null) { controller.DialogueBlock(false); }
        if (attacking != null) { attacking.DialogueBlock(false); }
    }
}
