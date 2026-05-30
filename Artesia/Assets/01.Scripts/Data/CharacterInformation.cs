using UnityEngine;
using TMPro;
using System.Text;

public class CharacterInformation : MonoBehaviour
{
    GameObject player;
    PlayerStat playerStat;

    public TextMeshProUGUI infoText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI skill1_Text;
    public TextMeshProUGUI skill1_Name;
    public TextMeshProUGUI skill1_SP;

    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player");
        playerStat = player.GetComponent<PlayerStat>();

        UpdateCharacterInfo();
    }

    void UpdateCharacterInfo()
    {
        // 캐릭터 정보 업데이트
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("      LV  :  " + playerStat.NowLv.ToString("00") + "\t\t\t속성  :  " + playerStat.Element);
        sb.AppendLine("      HP  :  " + playerStat.NowHp.ToString("00") + " / " + playerStat.Hp.ToString("00"));
        sb.AppendLine("ATK  :  " + playerStat.Atk.ToString("00"));
        sb.AppendLine("DEF  :  " + playerStat.Def.ToString("00"));
        sb.AppendLine("EXP  :  " + playerStat.NowExp.ToString("00") + " / " + playerStat.Exp.ToString("00"));

        infoText.text = sb.ToString();

        nameText.text = player.name;
        SkillTextUpdate();
    }

    void SkillTextUpdate()
    {
        skill1_Name.text = "칼날 회오리";
        skill1_Text.text = "한 칸 범위에 있는 모든 적들에게 데미지를 준다.";
        skill1_SP.text = "SP : 10 / 속성 : 일반";
    }
}