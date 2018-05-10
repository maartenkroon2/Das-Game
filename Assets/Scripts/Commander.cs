using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Commander : PlayerCharacter
{
    [SerializeField]
    private Heightmapmaker heightmapmaker;
    private Vector3 map_size;
    private float max_zoom;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        camera.transform.SetParent(null);
        map_size = Terrain.activeTerrain.terrainData.size;
        max_zoom = Mathf.Min(map_size.x * (float)Screen.height / (float)Screen.width * 0.5f, map_size.z /2);

        Instantiate(heightmapmaker);
    }
	
	// Update is called once per frame
	void Update () {
        Touchzoom();
        Touchscroll();
		
	}

    void Touchzoom()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


            // ... change the orthographic size based on the change in distance between the touches.
            camera.orthographicSize += deltaMagnitudeDiff;

            // Make sure the orthographic size never drops below zero.
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 250, max_zoom);

            if (camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x < 0) {camera.transform.position -= new Vector3(camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x,0,0);}
            if (camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).z < 0) {camera.transform.position -= new Vector3(0, 0 ,camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).z); }

            if (camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).x > map_size.x) { camera.transform.position -= new Vector3(camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).x - map_size.x,0 ,0); }
            if (camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).z > map_size.z) { camera.transform.position -= new Vector3(0, 0, camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).z - map_size.z); }
        }
    }

    void Touchscroll()
    {         
        if (Input.touchCount == 1)
        {
            Touch touchZero = Input.GetTouch(0);
            if (touchZero.phase == TouchPhase.Moved)
            {
                Vector3 camera_movement = new Vector3(touchZero.deltaPosition.x, 0, touchZero.deltaPosition.y) * camera.orthographicSize / -300;

                if (camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + camera_movement.x < 0) { camera_movement.x = -camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x; }
                if (camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).z + camera_movement.z < 0) { camera_movement.z = -camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).z; }

                if (camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).x + camera_movement.x > map_size.x) { camera_movement.x = map_size.x - camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).x; }
                if (camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).z + camera_movement.z > map_size.z) { camera_movement.z = map_size.z - camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0)).z; }

                camera.transform.position += camera_movement;
            }
        }
    }
}