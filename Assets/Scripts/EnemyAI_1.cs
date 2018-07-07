using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_1 : MonoBehaviour {
    
    private GameObject target;
    private int status;
    private Rigidbody rigidbody;

    // Use this for initialization
    void Start ()
    {
        status = 0;
        rigidbody = GetComponent<Rigidbody>();

        /* 
         * status 0 == idle
         * status 1 == patrolling
         * status 2 == moving to target
         * status 3 == in range of enemy
        */

    }
	
	// Update is called once per frame
	void Update ()
    {
		if (status == 0)//status 0 == idle
        { //do nothing
            rigidbody.velocity = new Vector3(0, 0, 0);
        }

        if (status == 1)//status 1 == patrolling
        { 
        }

        if (status == 2)//status 2 == moving to target
        {
            transform.LookAt(target.transform);
            rigidbody.AddForce(transform.forward * 10000);
            
        }

        if (status == 3)//status 3 == in range of enemy
        { 
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Body")
        {
            status = 0;
            //target = none;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.name == "Body")
        {
            status = 2;
            target = collider.gameObject;
        }
        
    }

    private void OnTriggerStay(Collider collider)
    {

        if (collider.gameObject.name == "Body")
        {
            /*
            if distance to target < missle range
            {
                status = 3;
            }
            else
            {
                status = 2;
            }
            */
        }

    }
}
