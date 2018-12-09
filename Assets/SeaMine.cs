using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMine : EnemyBase
{

    private SphereCollider collider;
    [SerializeField]
    private int explosionDamage;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {        
        Debug.Log(other.transform.parent);
        EnemyInRange(other.GetComponentInParent<ShipBase>());    //check wat er gebeurt als other geen shipbase heeft

    }

    public void EnemyInRange(ShipBase ship)
    {
        Debug.Log("boem2");
        ship.DoDamage(explosionDamage);
        Destroy(this);
    }
}
