using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionBtn : MonoBehaviour
{
    public GameObject optionCanvas;
    public GameObject escapeBtn;

    public void onMainBtn()
    {
        UIManager.instance.SetActiveUI(optionCanvas, false);
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("MainScene");
    }

    public void onBaseCampBtn()
    {
        UIManager.instance.SetActiveUI(optionCanvas, false);
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("BaseCamp");
    }

    public void onBackBtn()
    {
        optionCanvas.SetActive(false); //OptionUI
        Time.timeScale = 1f;
    }

    public void onOptionBtn()
    {
        UIManager.instance.SetActiveUI(optionCanvas, true);
        if (SceneManager.GetActiveScene().name != "BaseCamp")
            UIManager.instance.SetActiveUI(escapeBtn, true);
        Time.timeScale = 0f;
    }

    public void onEscapeBtn()
    {
        UIManager.instance.SetActiveUI(gameObject, false);
        UIManager.instance.SetActiveUI(optionCanvas, false);
        SceneLoader.Instance.LoadScene("BaseCamp");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().MovePos(Vector3.zero);
        Time.timeScale = 1f;
    }
}
