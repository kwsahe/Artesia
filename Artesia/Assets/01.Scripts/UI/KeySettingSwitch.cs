using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeySettingOnOff : MonoBehaviour
{
    [SerializeField] GameObject keySettingImage;
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "DungeonSample")
        {
            if (keySettingImage.activeSelf == false)
            {
                keySettingImage.SetActive(true);
            }
        }
        else
        {
            keySettingImage.SetActive(false);
        }
    }
}
