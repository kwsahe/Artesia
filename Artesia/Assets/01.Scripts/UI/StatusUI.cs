using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUI : MonoBehaviour
{
    public Image HPbar;
    private PlayerStat playerStat;

    public TextMeshProUGUI HPText;

    private void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        if (temp != null)
            playerStat = temp.GetComponent<PlayerStat>();
    }

    private void Update()
    {
        if (playerStat != null)
        {
            SetHPBar();
            SetHPText();
        }
        else
        {
            GameObject temp = GameObject.FindGameObjectWithTag("Player");
            if (temp != null)
                playerStat = temp.GetComponent<PlayerStat>();
        }
    }

    private void SetHPBar()
    {
        float fillAmount = (float)playerStat.NowHp / playerStat.Hp;

        HPbar.fillAmount = Mathf.Clamp01(fillAmount);
    }

    private void SetHPText()
    {
        HPText.text = $"LV : {playerStat.NowLv} HP {playerStat.NowHp}/{playerStat.Hp}";
    }
}