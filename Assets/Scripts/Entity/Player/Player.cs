using UnityEngine;

public class Player : Entity
{
    [SerializeField] private Weapon weapon;

    protected override void Awake()
    {
        base.Awake();
        weapon.Initialize(this.gameObject);
    }

    public void Attack()
    {
        weapon.Attack(transform.position);
    }

}
