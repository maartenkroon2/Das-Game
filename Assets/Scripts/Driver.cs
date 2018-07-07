using UnityEngine;
using UnityEngine.UI;

// The Driver class is a PlayerCharacter that contains the code for controlling the submarine.
public class Driver : PlayerCharacter
{
    private float steering, throttle, maxSteering, inputStearing, speed, depthThrottle, maxSpeed;
    private float[] steeringArray = new float[10];
    private int steeringArrayIndex;

    //canvas
    private Slider throttleSlider, depthSlider;
    private Image steeringwheelLeftSprite, steeringwheelRightSprite, steeringwheelPointerSprite;
    private Text speedText, depthText, directionText;
    private Vector3 steeringWheelCenter;

    // Use this for initialization.
    protected override void Start()
    {
        base.Start();

        GetSliders();
        GetImages();
        GetTexts();
        steeringWheelCenter = steeringwheelPointerSprite.transform.position;

        /* Vraag van Maarten: waarom een berekening om de maxSpeed te bepalen? 
         * maxSpeed zou naar mijn mening de maximale velocity in Unity units per
         * x tijd moeten zijn.
         */     
        maxSpeed = (100000 / rigidbody.drag - 0.01f * 100000) / rigidbody.mass;

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
        rigidbody.AddTorque(rigidbody.transform.up * steering * speed * 100000);

        /* Vraag van Maarten: waarom gebruik je zowel AddForce en zet je handmatig de
         * velocity?
         */        
        rigidbody.velocity = rigidbody.transform.forward * speed;
        rigidbody.AddForce(rigidbody.transform.forward * 100000 * throttle);

        /* Vraag van Maarten: Ik weet niet precies wat onderstaande stukje code doet maar 
         * het zorgt er nu voor dat de submarine op en neer beweegt. Waarom doe je hier 
         * iets met Time.deltaTime?
         */
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
        /* Vraag van Maarten: hier wordt veel gebruik gemaakt van magic numbers en waarom wordt
         * er gebruikt gemaakt van een array om iets met sturen te doen?
         */
        steeringArrayIndex++;
        if (steeringArrayIndex >= 10) { steeringArrayIndex = 0; }
        steeringArray[steeringArrayIndex] = Input.acceleration.x;
        float total = 0;
        for (int i = 0; i < 10; i++) { total += steeringArray[i]; }
        inputStearing = total / 10;

        throttle = throttleSlider.value / throttleSlider.maxValue;
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
        steeringwheelPointerSprite.transform.SetPositionAndRotation(steeringWheelCenter + steeringwheel_rotation * new Vector3(0, Screen.height / 2, 0), steeringwheel_rotation);

        directionText.text = Quaternion.LookRotation(transform.forward).eulerAngles.y.ToString("F0") + "°";
        speedText.text = "Speed: " + speed.ToString("F1");
        depthText.text = "Depth: " + transform.position.y.ToString("F1");
    }

    private void LateUpdate()
    {
        if (camera.transform.position.y <= 0.1) { RenderSettings.fog = true; } // (camera.transform.position.y <= 0) werkt niet goed, kans op een camera half onderwater maar geen fog
        else { RenderSettings.fog = false; }
    }
}