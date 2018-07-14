using UnityEngine;
using UnityEngine.Networking;

// The PlayerCharacter class is the base class for the different PlayerCharacters
public class PlayerCharacter : NetworkBehaviour {
    protected Camera camera;
    protected Canvas canvas;
    protected GameObject submarine;
    protected Rigidbody rigidbody;

    // Use this for initialization
    protected virtual void Start () {
        // Disable the default main camera.
        Camera.main.gameObject.SetActive(false);

        // Get a reference to the character specific camera and canvas
        camera = GetComponentInChildren<Camera>();
        canvas = GetComponentInChildren<Canvas>();

        // Get a reference to the submarine and it's rigidbody
        submarine = GameObject.FindGameObjectWithTag("Submarine");
        rigidbody = submarine.GetComponent<Rigidbody>();

        // Place the character in the submarine
        transform.parent = submarine.transform;
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
