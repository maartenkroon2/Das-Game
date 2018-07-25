using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TorpedoDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private GameObject torpedoPrefab;

    public static GameObject torpedoBeingDragged;
    private Image torpedoIcon, tempIcon;
    private GridLayoutGroup panelLayoutGroup;

    private void Start()
    {
        torpedoIcon = GetComponent<Image>();
        panelLayoutGroup = GetComponentInParent<GridLayoutGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Disable the GridLayout of the panel to prevent jumping icons when instantiating the tempIcon.
        panelLayoutGroup.enabled = false;
      
        tempIcon = Instantiate(torpedoIcon, transform.parent);
        tempIcon.raycastTarget = false;
        torpedoBeingDragged = torpedoPrefab;
    }

    public void OnDrag(PointerEventData eventData)
    {
        tempIcon.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(tempIcon.gameObject);        
    }
}
