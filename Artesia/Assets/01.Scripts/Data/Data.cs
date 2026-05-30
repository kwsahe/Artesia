using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// saveData
// 플레이 캐릭터, 보조 캐릭터 - 미구현, 진행 중인 게임 스탯(nowHP, nowExp), chapter
[System.Serializable]
public class Data
{
    public string MainPlayCharacterName;
    public int nowExp;
    public int nowLv;
}