using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
namespace Boss_Control
{
    
    public class BossSkill : BossController
    {
        [Header("Boss Skill1")] private float damageSkill1;
        private float skill1Cooldown;
        private float skill1Duration;

        private float skill1Range;

        // Weakness decrease 50% damage form player to boss
        private float weaknessDuration;

        // if player take dame form skill1, player will be slow 50% damage and speed in alpha second
        [Space(7)]

        //setting variable skill2


        [Header("Boss Skill2")]
        [SerializeField]
        private float damageSkill2;

        [SerializeField] private float skill2Cooldown;
        [SerializeField] private float skill2Duration;
        [SerializeField] private float skill2Range;

        [SerializeField] private float timeFreeze;

        // summon zombie
        // summon zombie
        [Space(7)]



        // setting variable skill3
        [Header("Boss Skill3")]
        [SerializeField]
        private float timeinvincible;

        [SerializeField] private float skill3Cooldown;
        [SerializeField] private float skill3Duration;
        [SerializeField] private int quantitySummon;
        [SerializeField] private Transform posisionSummon;

        [Space(7)]
        // summon zombie


        [Header("Boss Position")]
        [SerializeField]
        private Transform BossSideAttackTransform; //the middle of the side attack area

        [SerializeField] private Vector2 BossSideAttackArea; //how large the area of side attack is

        [SerializeField] private Transform BossUpAttackTransform; //the middle of the up attack area
        [SerializeField] private Vector2 BossUpAttackArea; //how large the area of side attack is
        [SerializeField] private Transform BossDownAttackTransform; //the middle of the down attack area
        [SerializeField] private Vector2 BossDownAttackArea; //how large the area of down attack is

        [Space(7)] [Header("Boss Awake")] [SerializeField]
        private Transform BossAwakeside; // the middle of the side awake area

        [SerializeField] private Vector2 BossAwakeSide; // how large the area of side awake is

        [SerializeField] private Transform BossAwakeup; // the middle of the up awake area
        [SerializeField] private Vector2 BossAwakeUp; // how large the area of up awake is
        [SerializeField] private Transform BossAwakedown; // the middle of the down awake area

        [SerializeField] private Vector2 BossAwakeDown; // how large the area of down awake is

        // Update is called once per frame
        [Header("Shockwave settings")] 
        [SerializeField]  private float shockwaveDamage;

        [SerializeField] private float shockwaveForce;
        [SerializeField] private float shockwaveDuration;
        [SerializeField] private float shockwaveSpeed;
        private Transform shockwaveRadius;
        public BossStatus bStatus;

        void Update()
        {

        }

        private void OnBecameInvisible()
        {
            // take the boss will be invisible
            // dont take damage form player
            // lets the boss summon zombie
        }
        private void Skill1(){
            // this skill will be decrease 50% damage form player to boss
            // if player take dame form skill1, player will be slow 50% damage and speed in alpha second
        }
    }
}