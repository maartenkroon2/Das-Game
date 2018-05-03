using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour {
    [SerializeField]
    private float speed = 10000;

    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Fire();
    }    

    public void Fire()
    {
        rigidbody.AddForce(transform.forward * speed);
    }
}
