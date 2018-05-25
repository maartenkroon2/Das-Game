using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResetSliderToZero : MonoBehaviour, IEndDragHandler
{
private Slider slider;

    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    public void OnEndDrag(PointerEventData data)
    {
        slider.value = 0f;
    }
}

