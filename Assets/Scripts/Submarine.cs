using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Submarine : ShipBase {

    protected override void Die()
    {
        SceneManager.LoadScene("World1");
    }

}
