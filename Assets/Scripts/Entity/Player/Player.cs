using System;

public class Player : Entity
{
    public event Action Died;
    public event Action<int> HealthChanged;

    protected override int Health
    {
        get
        {
            return base.Health;
        }
        set 
        { 
            base.Health = value;
            HealthChanged?.Invoke(base.Health); 
        }
    }

    public void Revive()
    {
        if (!IsAlive) 
        {
            Health = initialHealth;
        }
    }

    protected override void Die()
    {
        base.Die();
        Died?.Invoke();
    }

}
