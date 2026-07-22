using UnityEngine;

public class SetWeapon : MonoBehaviour
{
    [SerializeField] private int weaponId = 0;

    private void Start() { KeyManager.SetInt_WeaponID(weaponId); }
}
