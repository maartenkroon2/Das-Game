using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TorpedoTube : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private Transform torpedoTubeTransform;
    private GameObject loadedTorpedoPrefab;
    private Image tubeIcon;
    private bool isLoaded;
    private Color unloadedColor;

    private void Start()
    {
        tubeIcon = GetComponent<Image>();
        unloadedColor = tubeIcon.color;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Load(ref TorpedoDragHandler.torpedoBeingDragged);
    }

    public void Fire()
    {
        if (isLoaded)
        {
            GameObject torpedo = Instantiate(loadedTorpedoPrefab, torpedoTubeTransform.position, torpedoTubeTransform.rotation);
            NetworkServer.Spawn(torpedo);
            Unload();
        }
    }

    private void Unload()
    {
        isLoaded = false;
        tubeIcon.color = unloadedColor;        
    }

    private void Load(ref GameObject torpedoPrefab)
    {
        loadedTorpedoPrefab = torpedoPrefab;
        tubeIcon.color = Color.black;
        isLoaded = true;
    }
}
