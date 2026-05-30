using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalCollider : MonoBehaviour
{
    bool bTriggerEnter = false;
    public GameObject SelectDunCanvas;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !bTriggerEnter)
        {
            Bounds playerBounds = other.bounds;
            Bounds areaBounds = GetComponent<Collider2D>().bounds;

            if (areaBounds.Contains(playerBounds.min) && areaBounds.Contains(playerBounds.max))
            {
                Time.timeScale = 0f;
                UIManager.instance.SetActiveUI(SelectDunCanvas, true);
                bTriggerEnter = true;
            }
        }
    }

    private void OnTriggerExit2D()
    {
        bTriggerEnter = true;
    }
}
