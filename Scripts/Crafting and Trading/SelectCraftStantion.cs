using UnityEngine;

public class SelectCraftStantion : MonoBehaviour
{
    [SerializeField] private GameObject button;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            button.SetActive(false);
        }
    }
}
