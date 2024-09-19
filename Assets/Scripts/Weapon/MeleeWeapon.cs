using UnityEngine;

[CreateAssetMenu(fileName = "New Meele Weapon", menuName = "Weapon/Meele Weapon", order = 1)]
public class MeeleWeapon : Weapon
{
    [Range(1, 3)] public float range;
    [Range(1, 5)] public float speed;

    public override void Attack(Transform self, ContactFilter2D hitFilter, Collider2D[] hitColliders)
    {
        int numEntitiesHit = Physics2D.OverlapCircle(self.position, range, hitFilter, hitColliders);
        for(int i = 0; i < numEntitiesHit; i++)
        {
            if(hitColliders[i].gameObject != self.gameObject)
            {
                var hitVector = (hitColliders[i].transform.position - self.position).normalized * hitDistance; //TODO ?
                hitColliders[i].GetComponent<Entity>().TakeDamage(damage, hitVector);
                Debug.Log("Entity hit!");
            }
        }
    }
}
