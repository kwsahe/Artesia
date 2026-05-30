using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    public int HP { get; set; }
    public int EXP { get; set; }
    public int DEF { get; set; }
    public int ATK { get; set; }
    public int LV { get; set; }
}
