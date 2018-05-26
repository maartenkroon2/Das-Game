using UnityEngine;
using UnityEngine.UI;

// The Driver class is a PlayerCharacter that contains the code for controlling the submarine.
public class Driver : PlayerCharacter
{
    private GameObject submarine;
    private Rigidbody rigidbody;

    private float steering, throttle, max_stearing, input_steering, speed, depth_throttle, max_speed;
    private float[] stearing_arr = new float[10];
    private int stearing_arr_index;

    //canvas
    private Slider slider_throttle, slider_depth;
    private Image spr_steeringwheel_left, spr_steeringwheel_right, spr_steeringwheel_pointer;
    private Text text_speed, text_depth, text_direction;
    private Vector3 center_steeringwheel;


    // Use this for initialization.
    protected override void Start()
    {
        // Get a reference to the submarine and it's rigidbody
        submarine = GameObject.FindGameObjectWithTag("Submarine");
        rigidbody = submarine.GetComponent<Rigidbody>();

        Get_sliders();
        Get_images();
        Get_texts();
        center_steeringwheel = spr_steeringwheel_pointer.transform.position;
        max_speed = (100000 / rigidbody.drag - 0.01f * 100000) / rigidbody.mass; //max_speed = (addforce / drag - Time.fixedDeltaTime * addforce) / mass

        base.Start();

        // Set the center of mass and intertiaTensor to the center of the object.
        rigidbody.centerOfMass = Vector3.zero;
        rigidbody.inertiaTensorRotation = Quaternion.identity;
    }

    private void Get_sliders()
    {
        Slider[] array_sliders = transform.GetComponentsInChildren<Slider>();
        foreach (Slider slider in array_sliders)
        {
            switch (slider.gameObject.name)
            {
                case "Slider_Throttle":
                    slider_throttle = slider;
                    break;

                case "Slider_Depth":
                    slider_depth = slider;
                    break;

                default:
                    Debug.Log("Unkown slider component: " + slider.gameObject.name);
                    break;
            }
        }
    }
    private void Get_images()
    {
        Image[] array_images = transform.GetComponentsInChildren<Image>();
        foreach (Image image in array_images)
        {
            switch (image.gameObject.name)
            {
                case "Steeringwheel_left":
                    spr_steeringwheel_left = image;
                    break;

                case "Steeringwheel_right":
                    spr_steeringwheel_right = image;
                    break;

                case "Steeringwheel_pointer":
                    spr_steeringwheel_pointer = image;
                    break;

                case "Handle":
                    //do nothing, is an image of the slider
                    break;

                case "Background":
                    //do nothing, is an image of the slider
                    break;

                default:
                    Debug.Log("Unkown image component: " + image.gameObject.name);
                    break;
            }
        }
    }
    private void Get_texts()
    {
        Text[] array_texts = transform.GetComponentsInChildren<Text>();
        foreach (Text text in array_texts)
        {
            switch (text.gameObject.name)
            {
                case "Text_Speed":
                    text_speed = text;
                    break;

                case "Text_Depth":
                    text_depth = text;
                    break;

                case "Text_Direction":
                    text_direction = text;
                    break;

                default:
                    Debug.Log("Unkown text component: " + text.gameObject.name);
                    break;
            }
        }
    }

    // Update is called once per frame.
    // Get the player input for controlling the submarine
    private void Update()
    {
        Get_inputs();
        Update_canvas();
    }

    // FixedUpdate is called every fixed timestep.
    private void FixedUpdate()
    {
        rigidbody.AddTorque(rigidbody.transform.up * steering * speed * 100000);
        rigidbody.velocity = rigidbody.transform.forward * speed;
        rigidbody.AddForce(rigidbody.transform.forward * 100000 * throttle);

        if (rigidbody.transform.position.y + depth_throttle * Time.deltaTime > 0)
        {
            rigidbody.MovePosition(rigidbody.transform.position + new Vector3(0, -rigidbody.transform.position.y, 0));
        }
        else
        {
            rigidbody.MovePosition(rigidbody.transform.position + rigidbody.transform.up * depth_throttle * Time.deltaTime);
        }
    }

    private void Get_inputs()
    {
        stearing_arr_index++;
        if (stearing_arr_index >= 10) { stearing_arr_index = 0; }
        stearing_arr[stearing_arr_index] = Input.acceleration.x;
        float total = 0;
        for (int i = 0; i < 10; i++) { total += stearing_arr[i]; }
        input_steering = total / 10;

        throttle = slider_throttle.value / slider_throttle.maxValue;
        max_stearing = 1f - rigidbody.velocity.magnitude / max_speed * 0.7f; //0.7f betekent dat bij max speed nog (1-0.7)30% stuurkracht over hebt
        steering = input_steering * 2 + Input.GetAxis("Horizontal");
        steering = Mathf.Clamp(steering, -max_stearing, max_stearing);
        depth_throttle = slider_depth.value * 10;
        speed = rigidbody.velocity.magnitude * Vector3.Dot(rigidbody.transform.forward, Vector3.Normalize(rigidbody.velocity));
    }

    private void Update_canvas()
    {
        spr_steeringwheel_left.fillAmount = max_stearing;
        spr_steeringwheel_right.fillAmount = max_stearing;

        Quaternion steeringwheel_rotation = Quaternion.Euler(0f, 0f, -90 * steering);
        spr_steeringwheel_pointer.transform.SetPositionAndRotation(center_steeringwheel + steeringwheel_rotation * new Vector3(0, Screen.height / 2, 0), steeringwheel_rotation);

        text_direction.text = Quaternion.LookRotation(transform.forward).eulerAngles.y.ToString("F0") + "°";
        text_speed.text = "Speed: " + speed.ToString("F1");
        text_depth.text = "Depth: " + transform.position.y.ToString("F1");
    }

    private void LateUpdate()
    {
        if (camera.transform.position.y <= 0.1) { RenderSettings.fog = true; } // (camera.transform.position.y <= 0) werkt niet goed, kans op een camera half onderwater maar geen fog
        else { RenderSettings.fog = false; }
    }
}