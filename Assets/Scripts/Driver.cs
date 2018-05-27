using UnityEngine;
using UnityEngine.UI;

// The Driver class is a PlayerCharacter that contains the code for controlling the submarine.
public class Driver : PlayerCharacter
{
    private float steering, throttle, max_stearing, input_steering, speed; //depth_throttle
    private float[] stearing_arr = new float[10];
    private int stearing_arr_index;
    private Slider Slider_Throttle;
    private Text speedtext;

    // Use this for initialization.
    protected override void Start()
    {
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
        steering = input_steering * 2 + Input.GetAxis("Horizontal");
        steering = Mathf.Clamp(steering, -1 *max_stearing, max_stearing);
        //depth_throttle = Slider_depth.value;
        speed = rigidbody.velocity.magnitude * Vector3.Dot(rigidbody.transform.forward, Vector3.Normalize(rigidbody.velocity));
        speedtext.text = "Speed: " + speed.ToString("F1");
    }

    // FixedUpdate is called every fixed timestep.
    // Super floaty experimental movement, just for testing.
    private void FixedUpdate()
    {
        //rigidbody.AddTorque(rigidbody.transform.up * steering * speed * 50000);
        rigidbody.transform.Rotate(rigidbody.transform.up * steering * speed * Time.deltaTime * 0.2f);
        rigidbody.velocity = rigidbody.transform.forward * speed;
        rigidbody.AddForce(rigidbody.transform.forward * 100000 * throttle);

        if (Input.GetKey(KeyCode.LeftShift) && rigidbody.transform.position.y < 0) { rigidbody.MovePosition(rigidbody.transform.position + rigidbody.transform.up * 10 * Time.deltaTime); }
        if (Input.GetKey(KeyCode.LeftControl)) { rigidbody.MovePosition(rigidbody.transform.position + rigidbody.transform.up * -10 * Time.deltaTime); }
        if (rigidbody.transform.position.y >= 0) { rigidbody.transform.position = new Vector3(rigidbody.transform.position.x, 0, rigidbody.transform.position.z); }
        //rigidbody.MovePosition(rigidbody.transform.position + rigidbody.transform.up * depth_throttle * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (camera.transform.position.y <= 0.1) { RenderSettings.fog = true; } // (camera.transform.position.y <= 0) werkt niet goed, kans op een camera half onderwater maar geen fog
        else { RenderSettings.fog = false; }
    }
}
