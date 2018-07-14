using UnityEngine;

public class DetectableObject: MonoBehaviour
{
    private SpriteRenderer icon;

    private void Start()
    {
        icon = GetComponent<SpriteRenderer>();
    }

    public void SetIconActive(bool value)
    {       
        icon.enabled = value;
    }
}
