using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackBtn : MonoBehaviour
{
    public GameObject parentCanvas;
    public void OnBackButton()
    {
        if (parentCanvas != null)
        {
            parentCanvas.SetActive(false);
        }
        else
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        Time.timeScale = 1f;
    }
}
