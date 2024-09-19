using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected int initialHealth;
    [SerializeField] protected Weapon weapon;
    protected ContactFilter2D hitFilter;
    protected Collider2D[] hitColliders;

    private int _health;
    protected virtual int Health
    {
        get { return _health; }
        set
        {
            _health = value;
        }
    }
    public bool IsAlive => Health > 0;

    protected virtual void Awake()
    {
        hitFilter = new ContactFilter2D();
        hitFilter.SetLayerMask(Constants.EntityLayerMask);
        hitColliders = new Collider2D[Constants.MaxEntitiesHitAtOnce];
    }

    protected virtual void Start()
    {
        Health = initialHealth;
    }

    public virtual void Attack()
    {
        weapon.Attack(transform, hitFilter, hitColliders);
    }

    public virtual void TakeDamage(int damage, Vector2 hitVector)
    {
        if(!IsAlive)
        {
            return;
        }
        Health -= damage;
        if(Health < 0) 
        {
            Die();            
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"Entity {name} died.");
        //TODO: animation
    }

}