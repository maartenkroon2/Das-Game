using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Commander : PlayerCharacter
{
    [SerializeField]
    private Map map;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        //camera.transform.SetParent(null);

        map = Instantiate(map);
        map.SetCameraPosition(submarine.transform.position);
    }	
}