using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    public Canvas StartCanvas;
    public Canvas OptionCanvas;

    public void OnSavedGameButton()
    {
        PlayerPrefs.SetInt("SaveIndex", int.Parse(gameObject.name.Substring(gameObject.name.Length - 1)));
        SceneLoader.Instance.LoadScene("BaseCamp");
        //DataManager.instance.LoadData(PlayerPrefs.GetInt("SaveIndex"));
    }

    public void OnStartButton()
    {
        if (StartCanvas != null)
        {
            StartCanvas.gameObject.SetActive(true);
        }
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
