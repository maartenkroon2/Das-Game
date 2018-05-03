using UnityEngine;

// The Driver class is a PlayerCharacter that contains the code for controlling the submarine.
public class DriverOLD : PlayerCharacter
{
    private GameObject submarine;
    private Rigidbody rigidbody;
    private float horizontal, vertical;

    // Use this for initialization.
    protected override void Start()
    {
        // Get a reference to the submarine and it's rigidbody
        submarine = GameObject.FindGameObjectWithTag("Submarine");
        rigidbody = submarine.GetComponent<Rigidbody>();

        base.Start();
        
        // Set the center of mass and intertiaTensor to the center of the object.
        rigidbody.centerOfMass = Vector3.zero;
        rigidbody.inertiaTensorRotation = Quaternion.identity;
    }

    // Update is called once per frame.
    // Get the player input for controlling the submarine
    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    // FixedUpdate is called every fixed timestep.
    // Super floaty experimental movement, just for testing.
    private void FixedUpdate()
    {
        rigidbody.AddForce(rigidbody.transform.forward * vertical * 10000000);
        rigidbody.AddTorque(rigidbody.transform.up * horizontal * 10000000);
    }
}
