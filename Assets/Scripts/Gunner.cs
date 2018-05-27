using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Gunner : PlayerCharacter
{
    [SerializeField]
    private Torpedo torpedo;

    [SerializeField]
    private Transform torpedoTube1, torpedoTube2;

    protected override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame.
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            CmdFire();
        }
    }

    // Command the server to fire a torpedo.
    [Command]
    private void CmdFire()
    {
        Instantiate(torpedo, torpedoTube1.position, torpedoTube1.rotation);
    }

    private void LateUpdate()
    {
        if (camera.transform.position.y <= 0.1) { RenderSettings.fog = true; } // (camera.transform.position.y <= 0) werkt niet goed, kans op een camera half onderwater maar geen fog
        else { RenderSettings.fog = false; }
    }
}
