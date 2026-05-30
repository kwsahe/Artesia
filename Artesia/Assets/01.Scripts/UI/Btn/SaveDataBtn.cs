using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataBtn : MonoBehaviour
{
    public void OnSaveDataBtn()
    {
        DataManager.instance.SaveGameData(PlayerPrefs.GetInt("SaveIndex"));
    }
}
