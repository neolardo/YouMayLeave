using UnityEngine;

[CreateAssetMenu(fileName = "New Meele Weapon", menuName = "Weapon/Meele Weapon", order = 1)]
public class MeeleWeapon : Weapon
{
    [Range(1, 3)] public float range;
    [Range(1, 5)] public float speed;

    public override void Attack(Vector2 origin)
    {
        int numEntitiesHit = Physics2D.OverlapCircle(origin, range, contactFilter, hitColliders);
        for(int i = 0; i < numEntitiesHit; i++)
        {
            if(hitColliders[i].gameObject != self)
            {
                var hitVector = (hitColliders[i].transform.position - self.transform.position).normalized * hitDistance; //TODO ?
                hitColliders[i].GetComponent<Entity>().TakeDamage(damage, hitVector);
                Debug.Log("Entity hit!");
            }
        }
    }
}
