using UnityEngine;
using UnityEngine.Networking;

// The PlayerCharacter class is the base class for the different PlayerCharacters
public class PlayerCharacter : NetworkBehaviour {
    protected Camera camera;
    protected Canvas canvas;
    protected GameObject submarine;
    protected Rigidbody rigidbody;

    // Use this for initialization
    // Enables the class specific camera and disables the default main camera.
    protected virtual void Start () {
        camera = GetComponentInChildren<Camera>();
        Camera.main.gameObject.SetActive(false);

        camera.enabled = true;

        canvas = GetComponentInChildren<Canvas>();
        //canvas.worldCamera = camera;

        // Get a reference to the submarine and it's rigidbody
        submarine = GameObject.FindGameObjectWithTag("Submarine");
        rigidbody = submarine.GetComponent<Rigidbody>();

        // Place the character in the submarine
        transform.parent = submarine.transform;
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
