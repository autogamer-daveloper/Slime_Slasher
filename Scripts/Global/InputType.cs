using UnityEngine;

public class InputType : MonoBehaviour
{
    private enum BuildType
    {
        Mobile,
        PC
    }

    [Header("__ Platform settings __")]
    [SerializeField] private BuildType buildType = BuildType.Mobile;
    [Space(5)]
    [Tooltip("Is need to have only mobile objects.")]
    [SerializeField] private bool isHaveMobileObjects;
    [Tooltip("Mobile objects - objects, which enabled only on mobile devices.")]
    [SerializeField] private GameObject[] mobileObjects;
    [Tooltip("Is need to have only computer objects.")]
    [SerializeField] private bool isHaveComputerObjects;
    [Tooltip("Computer objects - objects, which enabled only on computer devices.")]
    [SerializeField] private GameObject[] computerObjects;

    internal bool IsMobileInput()
    {
        if (buildType == BuildType.Mobile) { return true; }
        else { return false; }
    }

    private void Start()
    {
        switch (buildType)
        {
            case BuildType.Mobile:
                if(isHaveMobileObjects) { foreach(GameObject obj in mobileObjects) { obj.SetActive(true); }}
                if(isHaveComputerObjects) { foreach(GameObject obj in computerObjects) { obj.SetActive(false); }}
                break;
            case BuildType.PC:
                if(isHaveMobileObjects) { foreach(GameObject obj in mobileObjects) { obj.SetActive(false); }}
                if(isHaveComputerObjects) { foreach(GameObject obj in computerObjects) { obj.SetActive(true); }}
                break;
        }
    }
}
