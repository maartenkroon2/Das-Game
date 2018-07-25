using UnityEngine;
using UnityEngine.UI;

// The Driver class is a PlayerCharacter that contains the code for controlling the submarine.
public class Driver : PlayerCharacter
{
    private float steering, throttle, maxSteering, inputStearing, inputThrotle, speed, depthThrottle, maxSpeed;
    private float[] steeringArray = new float[10], throttleArray = new float[10];
    private int steeringArrayIndex;

    //canvas
    private Slider throttleSlider, depthSlider;
    private Image steeringwheelLeftSprite, steeringwheelRightSprite, steeringwheelPointerSprite;
    private Text speedText, depthText, directionText;
    private Vector3 steeringWheelCenter;
    public float addforce = 200000;
    public float addtorque = 800000;

    // Use this for initialization.
    protected override void Start()
    {
        base.Start();

        GetSliders();
        GetImages();
        GetTexts();
        steeringWheelCenter = steeringwheelPointerSprite.transform.position;

        maxSpeed = (addforce / rigidbody.drag - 0.01f * addforce) / rigidbody.mass;

        // Set the center of mass and intertiaTensor to the center of the object.
        rigidbody.centerOfMass = Vector3.zero;
        rigidbody.inertiaTensorRotation = Quaternion.identity;
    }

    private void GetSliders()
    {
        Slider[] sliderArray = canvas.GetComponentsInChildren<Slider>();
        foreach (Slider slider in sliderArray)
        {
            switch (slider.gameObject.name)
            {
                case "ThrottleSlider":
                    throttleSlider = slider;
                    break;

                case "DepthSlider":
                    depthSlider = slider;
                    break;

                default:
                    Debug.Log("Unkown slider component: " + slider.gameObject.name);
                    break;
            }
        }
    }

    private void GetImages()
    {
        Image[] imageArray = canvas.GetComponentsInChildren<Image>();
        foreach (Image image in imageArray)
        {
            switch (image.gameObject.name)
            {
                case "SteeringwheelLeft":
                    steeringwheelLeftSprite = image;
                    break;

                case "SteeringwheelRight":
                    steeringwheelRightSprite = image;
                    break;

                case "SteeringwheelPointer":
                    steeringwheelPointerSprite = image;
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

    private void GetTexts()
    {
        Text[] textArray = canvas.GetComponentsInChildren<Text>();
        foreach (Text text in textArray)
        {
            switch (text.gameObject.name)
            {
                case "SpeedText":
                    speedText = text;
                    break;

                case "DepthText":
                    depthText = text;
                    break;

                case "DirectionText":
                    directionText = text;
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
        GetInput();
        UpdateCanvas();
    }

    // FixedUpdate is called every fixed timestep.
    private void FixedUpdate()
    {
        rigidbody.AddTorque(rigidbody.transform.up * steering * speed * addtorque);
        rigidbody.velocity = rigidbody.transform.forward * speed;
        rigidbody.AddForce(rigidbody.transform.forward * addforce * throttle);


        if (rigidbody.transform.position.y + depthThrottle * Time.deltaTime > 0)
        {
            rigidbody.MovePosition(rigidbody.transform.position + new Vector3(0, -rigidbody.transform.position.y, 0));
        }
        else
        {
            rigidbody.MovePosition(rigidbody.transform.position + rigidbody.transform.up * depthThrottle * Time.deltaTime);
        }
    }

    private void GetInput()
    {
        //takes the average of 10 inputs of 
        steeringArrayIndex++;
        if (steeringArrayIndex >= 10) { steeringArrayIndex = 0; }
        steeringArray[steeringArrayIndex] = Input.acceleration.x;
        throttleArray[steeringArrayIndex] = Input.acceleration.y;
        float totalsteering = 0, totalthrotle = 0;
        for (int i = 0; i < 10; i++)
        {
            totalsteering += steeringArray[i];
            totalthrotle += throttleArray[i];

        }
        inputStearing = totalsteering / 10;
        inputThrotle = totalthrotle / 10;

        throttle = (Mathf.Clamp(inputThrotle, -1f, -0.4f) + 0.7f) / 0.3f;
        //throttle = throttleSlider.value / throttleSlider.maxValue;
        maxSteering = 1f - rigidbody.velocity.magnitude / maxSpeed * 0.7f; //0.7f betekent dat bij max speed nog (1-0.7)30% stuurkracht over hebt
        steering = inputStearing * 2 + Input.GetAxis("Horizontal");
        steering = Mathf.Clamp(steering, -maxSteering, maxSteering);
        depthThrottle = depthSlider.value * 10;
        speed = rigidbody.velocity.magnitude * Vector3.Dot(rigidbody.transform.forward, Vector3.Normalize(rigidbody.velocity));
    }

    private void UpdateCanvas()
    {
        steeringwheelLeftSprite.fillAmount = maxSteering;
        steeringwheelRightSprite.fillAmount = maxSteering;

        Quaternion steeringwheel_rotation = Quaternion.Euler(0f, 0f, -90 * steering);
        steeringwheelPointerSprite.transform.SetPositionAndRotation(new Vector3(Screen.width / 2,Screen.height/10 ,0) + steeringwheel_rotation * new Vector3(0, Screen.height / 2, 0), steeringwheel_rotation);

        directionText.text = Quaternion.LookRotation(transform.forward).eulerAngles.y.ToString("F0") + "°";
        speedText.text = "Speed: " + speed.ToString("F1");
        depthText.text = "Depth: " + transform.position.y.ToString("F1");

        throttleSlider.value = throttle;
    }

    private void LateUpdate()
    {
        if (camera.transform.position.y <= 0.1) { RenderSettings.fog = true; } // (camera.transform.position.y <= 0) werkt niet goed, kans op een camera half onderwater maar geen fog
        else { RenderSettings.fog = false; }
    }
}