using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class CanvasActivator : MonoBehaviour
{
    private void OnEnable()
    {
        var allUI = GetComponentsInChildren<MaskableGraphic>(true);
        foreach(var uiElement in allUI)
        {
            uiElement.gameObject.SetActive(true);
        }
        
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}
