using UnityEngine;

public class Torpedo : MonoBehaviour {
    [SerializeField]
    private float startSpeed, maxSpeed, acceleration;
    private Rigidbody rigidbody;

    private void Start()
    {
        // Gives the torpedo an initial speed, maybe this should add the current speed of the submarine.
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = transform.forward * startSpeed;

        // Destroy the torpedo after 10 seconds, this is enough for them to escape view. 
        // Object pooling could be used here but is not really needed.
        Destroy(gameObject, 10);
    }

    private void FixedUpdate()
    {
        // Increases the speed of the torpedo over time until the max speed is reached.
        rigidbody.AddForce(transform.forward * acceleration);
        rigidbody.velocity = rigidbody.velocity.magnitude > maxSpeed ? transform.forward * maxSpeed : rigidbody.velocity;   
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("boom" + collision.gameObject.name);
    }
}
