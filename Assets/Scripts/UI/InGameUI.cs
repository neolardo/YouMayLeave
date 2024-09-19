using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] RectTransform healthRect;
    private const float pixelPerHealthUnit = 50;

    public void UpdateHealth(int health)
    {
        healthRect.sizeDelta = new Vector2(health * pixelPerHealthUnit, healthRect.sizeDelta.y);
    }
}
