using UnityEngine;

public class WeaponUpgradeManager : MonoBehaviour
{
    private void ApplyUpgradeEffects(WeaponUpgrade upgrade)
    {
        // Example of applying effects based on upgrade type
        switch (upgrade.type)
        {
            case UpgradeType.DamageBoost:
                // Increase weapon damage
                PencilArrowProjectile weapon = GetComponent<PencilArrowProjectile>();
                weapon.damage += upgrade.value;
                break;
            case UpgradeType.SpeedEnhancement:
                // Increase projectile speed
                PencilArrowProjectile pencilArrowProjectile = GetComponent<PencilArrowProjectile>();
                pencilArrowProjectile.speed += upgrade.value;
                break;
        }
    }

}

// Enum to define the type of upgrades
public enum UpgradeType
{
    DamageBoost,
    SpeedEnhancement,
    SpecialEffect
}

// Class to represent a weapon upgrade
[System.Serializable]
public class WeaponUpgrade
{
    public string name;
    public UpgradeType type;
    public int value; // Value could represent damage increase, speed increase, etc.
}
