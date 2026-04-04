using UnityEngine;

namespace Objects.Accessories
{
    [CreateAssetMenu(fileName = "New Accessory", menuName = "Scriptable Objects/Accessory")]
    public class Accessory : ScriptableObject
    {
        [Header("__ Life stats __")]
        public int extraLife = 0;
        public int extraRegen = 0;
        [Header("__ Mana stats __")]
        public int extraMana = 0;
        public int extraManaRegen = 0;
        [Header("__ Main __")]
        public int accessoryId = 0;
    }
}