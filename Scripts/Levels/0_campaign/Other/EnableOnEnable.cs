using UnityEngine;

//Оч забавно кнш, но этот класс понадобился в кампании 0
public class EnableOnEnable : MonoBehaviour
{
    [Tooltip("Select object, which you want to activate when this.gameObject will be activated.")]
    [SerializeField] private GameObject activate;
    [Tooltip("Do you want deactivate this object (Reverse type of this class meaning).")]
    [SerializeField] private bool isNeedDeactivateAfter = false;

    private void OnEnable() { activate.SetActive(true); }
    private void OnDisable() { if(isNeedDeactivateAfter) { activate.SetActive(false); } }
}
