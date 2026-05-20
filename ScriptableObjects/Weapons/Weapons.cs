using UnityEngine;

namespace Objects.Weapons
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Objects/Weapon")]
    public class Weapons : ScriptableObject
    {
        public enum WeaponType
        {
            Meele,
            Magic,
            Range
        }

        [Header("__ Weapon Type __")]
        public WeaponType Type;

        public int Damage = 1;
        public float Cooldown = 0.5f;
        public int ManaCost = 0;
        public int LifeCost = 0;
        public int WeaponId = 0;
        public int ItemId = 0;
        public AudioClip Sfx;

        [Header("__ For Meele __")]
        public bool CustomMeeleAnimation = false;
        public string MeeleAnimationName = "Default Meele Animation Name";
        public bool isHaveEffect = false;
        public bool isRotatable = false;
        public GameObject meeleEffect;

        [Header("__ For Range __")]
        public GameObject Arrow;
        public float Speed = 10f;
        public float LifeTime = 5f;

        [Header("__ For Magic __")]
        public bool IsDamageWeapon = true;
        public GameObject Effect;
        public int ManaHealing = 0;
        public int LifeHealing = 0;
    }
}