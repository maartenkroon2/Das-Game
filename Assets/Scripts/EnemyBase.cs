using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : ShipBase, IBehaviour

{
    public virtual void Idle()
    {

    }
    public virtual void EnemyInSight()
    {

    }
    public virtual void EnemyInRange()
    {

    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

}
