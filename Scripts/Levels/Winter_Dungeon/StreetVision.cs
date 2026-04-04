using UnityEngine;

public class StreetVision : MonoBehaviour
{
    [Header("__ UI __")]
    [SerializeField] private GameObject streetVision;
    [SerializeField] private GameObject prisonVision;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            streetVision.SetActive(true);
            prisonVision.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            streetVision.SetActive(false);
            prisonVision.SetActive(true);
        }
    }
}
