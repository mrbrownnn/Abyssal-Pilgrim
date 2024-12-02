using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateList : MonoBehaviour
{
    public bool jumping = false;
    public bool dashing = false;
    public bool recoilingX, recoilingY;
    public bool lookingRight;
    public bool invincible;
    public bool healing;
    public bool casting;
    public bool downSpell;
    public bool upSpell;
    public bool jumpOnce;
    public int PlayerExperienceDefault = 0;
    public int PlayerLevelDeafault = 1;
    public int PlayerLevelMax = 50;
    public int PlayerCoinDefault = 0;
    // coin have been drop when player defeat boss/ enemy, useful for uplevel skill
    // need build SkillTree;
}
