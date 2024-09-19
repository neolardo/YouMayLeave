using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public int damage;
    [Range(0.05f, 2f)] public float hitDistance;

    public abstract void Attack(Transform self, ContactFilter2D hitFilter, Collider2D[] hitColliders);

}
