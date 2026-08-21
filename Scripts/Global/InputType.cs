using UnityEngine;

public class InputType : MonoBehaviour
{
    private enum BuildType
    {
        Mobile,
        PC
    }

    [SerializeField] private BuildType buildType = BuildType.Mobile;

    internal bool IsMobileInput()
    {
        if (buildType == BuildType.Mobile) { return true; }
        else { return false; }
    }
}
