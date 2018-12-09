using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipBase : MonoBehaviour {
    [SerializeField]
    protected int hp;
    [SerializeField]
    protected WeaponBase[] weapons;
    private Collider collider;

    private void Start()
    {
        //collider = GetComponent<Collider>();
    }

    public void DoDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
            
    }

    protected abstract void Die();
}
