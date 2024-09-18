using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "Weapon/Ranged Weapon", order = 1)]
public class RangedWeapon : Weapon
{
    public float reloadDuration;
    public float bulletSpeed;

    public override void Attack(Vector2 origin)
    {
        //TODO
    }
}
