using UnityEngine;

// The PlayerCharacter class is the base class for the different PlayerCharacters
public class PlayerCharacter : MonoBehaviour {
    protected Camera camera;
    private Canvas canvas;

	// Use this for initialization
    // Enables the class specific camera and disables the default main camera.
	protected virtual void Start () {
        camera = GetComponentInChildren<Camera>();
        Camera.main.gameObject.SetActive(false);

        camera.enabled = true;

        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        canvas.worldCamera = camera;
	}
}
