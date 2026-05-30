using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageButton : MonoBehaviour
{
    public void onClickYesButton()
    {
        if (!UIManager.instance.isFade)
        {
            UIManager.instance.SetActiveUI("NextStage", false);
            GameManager.instance.NextStage();
            Time.timeScale = 1f;
        }
    }
    public void onClickNoButton()
    {
        UIManager.instance.SetActiveUI("NextStage", false);
        Time.timeScale = 1f;
    }
}
