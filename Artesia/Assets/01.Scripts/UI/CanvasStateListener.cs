using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CanvasStateListener : MonoBehaviour
{
    public UnityEvent onCanvas;

    private void OnEnable()
    {
        if (!gameObject.activeInHierarchy)
        {
            onCanvas.Invoke();
        }
    }

    private void OnDisable()
    {
        onCanvas.Invoke();
    }
}
