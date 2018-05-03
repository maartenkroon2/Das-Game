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
        if (camera.transform.position.y <= 0) { RenderSettings.fog = true; }
        else { RenderSettings.fog = false; }
    }
}
