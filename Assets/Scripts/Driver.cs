using UnityEngine;
using UnityEngine.UI;

// The Driver class is a PlayerCharacter that contains the code for controlling the submarine.
public class Driver : PlayerCharacter
{
    private GameObject submarine;
    private Rigidbody rigidbody;

    private float steering, throttle, max_stearing, input_steering, speed; //depth_throttle
    private float[] stearing_arr = new float[10];
    private int stearing_arr_index;
    private Slider Slider_Throttle;
    private Text speedtext;

    // Use this for initialization.
    protected override void Start()
    {
        // Get a reference to the submarine and it's rigidbody
        submarine = GameObject.FindGameObjectWithTag("Submarine");
        rigidbody = submarine.GetComponent<Rigidbody>();
        Slider_Throttle = transform.GetComponentInChildren<Slider>();
        //Slider_Throttle = GameObject.FindGameObjectWithTag("Slider_Throttle").GetComponent<Slider>();
        speedtext = transform.GetComponentInChildren<Text >();


        base.Start();
        
        // Set the center of mass and intertiaTensor to the center of the object.
        rigidbody.centerOfMass = Vector3.zero;
        rigidbody.inertiaTensorRotation = Quaternion.identity;
    }

    // Update is called once per frame.
    // Get the player input for controlling the submarine
    private void Update()
    {
        //horizontal = Input.GetAxis("Horizontal");
        //vertical = Input.GetAxis("Vertical");

        stearing_arr_index++;
        if (stearing_arr_index >= 10) { stearing_arr_index = 0; }
        stearing_arr[stearing_arr_index] = Input.acceleration.x;
        float total = 0;
        for (int i = 0; i < 10; i++) { total += stearing_arr[i]; }
        input_steering = total / 10;

        throttle = Slider_Throttle.value / Slider_Throttle.maxValue;
        max_stearing = 1f;// - rigidbody.velocity.magnitude / 120;
        steering = Mathf.Min(Mathf.Max(input_steering * 2, -max_stearing), max_stearing) + Input.GetAxis("Horizontal");
        //depth_throttle = Slider_depth.value;
        speed = rigidbody.velocity.magnitude * Vector3.Dot(rigidbody.transform.forward, Vector3.Normalize(rigidbody.velocity));
        speedtext.text = "Speed: " + speed.ToString("F1");
    }

    // FixedUpdate is called every fixed timestep.
    // Super floaty experimental movement, just for testing.
    private void FixedUpdate()
    {
        rigidbody.AddTorque(rigidbody.transform.up * steering * speed * 50000);
        rigidbody.velocity = rigidbody.transform.forward * speed;
        rigidbody.AddForce(rigidbody.transform.forward * 300000 * throttle);
        //rigidbody.MovePosition(rigidbody.transform.position + rigidbody.transform.up * depth_throttle * Time.deltaTime);
    }
}
