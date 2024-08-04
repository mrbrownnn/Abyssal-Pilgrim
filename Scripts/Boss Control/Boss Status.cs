using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : MonoBehaviour
{
    public bool bossAttack;
    public bool bossSkilling;
    public bool bossDaze;
    // boss have some health point, if player attack boss, boss will take double damage and daze a while
    public bool bossHeal;
    // if player dont attack boss in 5 second, boss will heal 10% health per second
    public bool bossDead;
    public bool bossInvincible;
    // boss will be invible , cant take damage form player
    public bool bossSummon;
    // in skill 3, boss will summon n zombie to attack player
    // zombie have a special skill, if player attack zombie, player will reflect 100% damage done to zombie
    public bool bossTeleport;
    public bool deadtimedeployedSkills;
    // add deadtime when deployed the next skill
    public bool bossRecall;
    // if player leave this map, boss will recall to full health
    public bool bossFlying;
    // skill invisible and summon zombie, boss will

}
