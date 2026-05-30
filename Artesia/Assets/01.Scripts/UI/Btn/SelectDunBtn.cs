using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectDunBtn : MonoBehaviour
{
    public TextMeshProUGUI dunName;

    public void OnSelectDunBtn()
    {
        Time.timeScale = 1f;
        GameManager.instance.dungeonName = dunName.text;
        SceneLoader.Instance.LoadScene("DungeonSample");
    }
}
