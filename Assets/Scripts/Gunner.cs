using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Gunner : PlayerCharacter
{
    [SerializeField]
    private GameObject torpedoPrefab;

    [SerializeField]
    private Transform torpedoTube1, torpedoTube2;

    private bool fireFromTube1 = true;

    //protected override void Start()
    //{
    //    base.Start();
    //}

    // Update is called once per frame.
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    private void Fire()
    {
        GameObject torpedo;
        if (fireFromTube1)
        {
            torpedo = Instantiate(torpedoPrefab, submarine.transform.position + torpedoTube1.localPosition, torpedoTube1.rotation);
        }
        else
        {
            torpedo = Instantiate(torpedoPrefab, submarine.transform.position + torpedoTube2.localPosition, torpedoTube2.rotation);
        }
        NetworkServer.Spawn(torpedo);
        fireFromTube1 = !fireFromTube1;
    }

    private void LateUpdate()
    {
        if (camera.transform.position.y <= 0.1) { RenderSettings.fog = true; } // (camera.transform.position.y <= 0) werkt niet goed, kans op een camera half onderwater maar geen fog
        else { RenderSettings.fog = false; }
    }
}
