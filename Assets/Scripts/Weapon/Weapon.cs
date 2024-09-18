using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public int damage;
    [Range(0.05f, 2f)] public float hitDistance;
    protected ContactFilter2D contactFilter;
    protected Collider2D[] hitColliders;
    protected GameObject self;

    private void Awake()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(Globals.EntityLayerMask);
        hitColliders = new Collider2D[Globals.MaxEntitiesHitAtOnce];
    }

    public void Initialize(GameObject self)
    {
        this.self = self;
    }

    public abstract void Attack(Vector2 origin);
}
