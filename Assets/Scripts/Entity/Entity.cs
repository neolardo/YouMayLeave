using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected int initialHealth;
    protected int health;
    protected bool IsAlive => health > 0;

    protected virtual void Awake()
    {
        health = initialHealth;
    }

    public virtual void TakeDamage(int damage, Vector2 hitVector)
    {
        if(!IsAlive)
        {
            return;
        }
        health -= damage;
        if(health <0) 
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