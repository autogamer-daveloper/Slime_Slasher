using UnityEngine;

public class DeleteEnemies : MonoBehaviour
{
    [Header("Tag to delete")]
    [SerializeField] private string targetTag = "Enemy";

    [Header("Objects that must not be deleted")]
    [SerializeField] private GameObject[] dontDelete;

    public void DeleteObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in objects)
        {
            if (obj == null)
                continue;

            bool shouldSkip = false;

            if (dontDelete != null)
            {
                foreach (GameObject protectedObj in dontDelete)
                {
                    if (protectedObj == null) continue;
                    if (protectedObj == obj)
                    {
                        shouldSkip = true;
                        break;
                    }
                }
            }

            if (!shouldSkip)
            {
                Destroy(obj);
            }
        }
    }
}