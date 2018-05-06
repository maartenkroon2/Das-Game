using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : PlayerCharacter
{
    [SerializeField]
    private Torpedo torpedo;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(torpedo, transform.position, transform.rotation);
        }
    }

    private void LateUpdate()
    {
        if (camera.transform.position.y <= 0.1) { RenderSettings.fog = true; } // (camera.transform.position.y <= 0) werkt niet goed, kans op een camera half onderwater maar geen fog
        else { RenderSettings.fog = false; }
    }
}
