using UnityEngine;

[CreateAssetMenu(fileName = "BuildTypeScriptable", menuName = "Scriptable Objects/BuildTypeScriptable")]
public class BuildTypeScriptable : ScriptableObject
{
    [Header("__ Build type __")]
    [Tooltip ("Change build type there.")]
    public BuildType buildType = BuildType.Mobile;
}

public enum BuildType
{
    Mobile,
    PC
}