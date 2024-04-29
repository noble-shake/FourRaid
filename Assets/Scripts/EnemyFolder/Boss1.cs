using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss1 : EnemyScript
{
    [Header("Boss Inspector")]
    [SerializeField] string BossName = "Gobline Warrior";
    
    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D AttackCollider;
    [SerializeField] BoxCollider2D DetectCollider;
    [SerializeField] float BossSpellCooltime;
    [SerializeField] float currentTime;
    [SerializeField] float SpellAttackCurrentTime;
    [SerializeField] float SpellAttackActivateTime;
    [SerializeField] float BossSpellAttackDamage = 50f;
    [SerializeField] bool BossAttackActivate;

    [SerializeField] GameObject Indicator;
    [SerializeField] List<PlayerScript> Targets;

    //[Header("Boss UI Inspector")]
    //[SerializeField] GameObject BossUIObject;
    //[SerializeField] Slider BossHPBarUI;
    //[SerializeField] TMP_Text BossHPBarUIName;


    public override void ObjectInit()
    {
        // GameObject uiObj = Instantiate(BossUIObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
        //BossHPBarUI = GameObject.Find("BossUI").GetComponent<Slider>();
        //BossHPBarUIName = GameObject.Find("BossUI").GetComponent<Slider>();
        // BossHPBarUI = uiObj.GetComponentInChildren<Slider>();
        // BossHPBarUIName = uiObj.GetComponentInChildren<TMP_Text>();

        // BossHPBarUI.gameObject.SetActive(true);
        // BossHPBarUIName.gameObject.SetActive(true);
        //BossHPBarUIName.text = "Goblin Warrior";
        //BossHPBarUI.maxValue = enemyMaxHp;
        //BossHPBarUI.value = enemyMaxHp;

        GameManager.instance.SetBossHP(enemyMaxHp, enemyMaxHp, BossName);

        enemyCurHp = enemyMaxHp;
        HPBarUI.maxValue = enemyMaxHp;
        HPBarUI.value = enemyMaxHp;

        enemyAggroGauge = new float[] { 0, 0, 0, 0 };
    }

    public override void hitHp(int _playerID, float _value, float _aggro)
    {
        enemyCurHp -= _value;
        if (enemyCurHp < 0)
        {
            // die
            enemyCurHp = 0;

            if (gameObject.activeSelf) {
                StageManager.instance.setClearCounter();
            }
            
            gameObject.SetActive(false);
            // StageManager.instance.setClearCounter();
        }
        HPBarUIVisbile();
        if (HPBarUI.gameObject.activeSelf)
        {
            HPBarUI.value = enemyCurHp;
        }

        GameManager.instance.SetBossHP(enemyMaxHp, enemyCurHp, BossName);
        //if (BossHPBarUI.gameObject.activeSelf) {
        //    BossHPBarUI.value = enemyCurHp;
        //}

        AggroCalculate(_playerID, _aggro);
    }

    private void OnEnable()
    {
        // ObjectInit();
    }

    private void OnDisable()
    {
        
    }

    private void OnDestroy()
    {
    }


    protected override void Attack()
    {
        if (!AttackRangedOn) return;

        if (enemyAggroTarget == null) return;


        Vector2 HeroPos = enemyAggroTarget.transform.position;

        if (InteractTime < enemyAtkSpeed) return;


        enemyAggroTarget.GetComponent<PlayerScript>().hitHp(enemyAtk);
        InteractTime = 0f;

        Vector3 looking = HeroPos.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        transform.GetChild(0).localScale = looking;
        transform.GetChild(1).localScale = looking;
    }
    protected override void Move()
    {
        TargetChangeCheck();
        // enemyAggroTarget = Heroes[enemyAggroTargetID];

        Vector3 TargetPos = enemyAggroTarget.transform.position;
        Vector2 BattlePoint = enemyAggroTarget.GetComponent<PlayerScript>().getPlayerBattlePoint(transform.position);
        transform.position = Vector2.MoveTowards(transform.position, BattlePoint, speed * Time.deltaTime);

        if (Vector3.Distance(BattlePoint, transform.position) < 6f)
        {
            AttackRangedOn = true;
        }

        if (TargetPos.x != transform.position.x)
        {
            Vector3 looking = TargetPos.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
            transform.GetChild(0).localScale = looking;
            transform.GetChild(1).localScale = looking;
        }
    }

    private void Awake()
    {
    }

    private void Start()
    {
        Heroes = PlayerManager.instance.getHeroesObjects();
    }

    private void FixedUpdate()
    {
        InteractTime += Time.fixedDeltaTime;
        currentTime += Time.fixedDeltaTime;

        HPBarUIVisbile();

        if (currentTime > BossSpellCooltime) {
            BossAttackActivate = true;
            SpellAttack();
        }

        if (!BossAttackActivate)
        {
            Move();
            Attack();
        }


        if (InteractTime > 10f)
        {
            InteractTime = 0f;
        }
    }

    public void SpellAttack() {

        TargetChangeCheck();
        // 3 seconds
        SpellAttackCurrentTime += Time.fixedDeltaTime;

        Indicator.SetActive(true);
        // Vector2 IndicatorPos = Indicator.transform.GetChild(0).transform.position;
        Vector2 IndicatorPos = Indicator.transform.transform.position;
        Vector2 TargetPos = IndicatorPos;
        TargetPos.x += 8f;
        TargetPos.y += 3f;

        float angle = Quaternion.FromToRotation(-Vector3.right, IndicatorPos - (Vector2)enemyAggroTarget.transform.position).eulerAngles.z;

        Indicator.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        if (SpellAttackCurrentTime > SpellAttackActivateTime)
        {
            // hit
            int players = Targets.Count - 1;


            while (true)
            {
                if (players < 0) break;

                Targets[players--].hitHp(BossSpellAttackDamage);

            }

            SpellAttackCurrentTime = 0f;
            BossAttackActivate = false;
            Indicator.GetComponent<SpriteRenderer>().size = new Vector2(0.02f, 0.02f);
            Indicator.GetComponent<BoxCollider2D>().size = new Vector2(0.02f, 0.02f);
            Indicator.GetComponent<BoxCollider2D>().offset = new Vector2(0.01f, 0f);
            Indicator.SetActive(false);
            InteractTime = 0f;
            currentTime = 0f;
        }
        else {
            Indicator.GetComponent<SpriteRenderer>().size += new Vector2(Time.fixedDeltaTime * 10f / 4f, Time.fixedDeltaTime * 10f / 8f);
            Indicator.GetComponent<BoxCollider2D>().size += new Vector2(Time.fixedDeltaTime * 10f / 4f, Time.fixedDeltaTime * 10f / 8f);
            Indicator.GetComponent<BoxCollider2D>().offset += new Vector2(Time.fixedDeltaTime * 10f / 8f, 0f);
        }

        Vector3 looking = enemyAggroTarget.transform.position.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        transform.GetChild(0).localScale = looking;
        transform.GetChild(1).localScale = looking;



        // BossAttackActivate = false;
    }

    public override void AttackTriggerStay(HitBoxScript.enumHitType _hitType, Collider2D collision)
    {
        switch (_hitType)
        {
            case HitBoxScript.enumHitType.PlayerCheck:
                if (collision.CompareTag("player"))
                {
                    TargetChangeCheck();
                    AttackRangedOn = true;

                    bool existCheck = false; ;
                    for (int inum = 0; inum < Targets.Count; inum++)
                    {
                        if (Targets[inum].GetComponent<PlayerScript>().getPlayerID() == collision.transform.parent.gameObject.GetComponent<PlayerScript>().getPlayerID())
                        {
                            existCheck = true;
                            break;
                        }
                    }

                    if (!existCheck)
                    {
                        Targets.Add(collision.transform.parent.gameObject.GetComponent<PlayerScript>());
                    }
                }
                break;
        }
    }

    public override void AttackTriggerEnter(HitBoxScript.enumHitType _hitType, Collider2D collision)
    {
        switch (_hitType)
        {
            case HitBoxScript.enumHitType.PlayerCheck:
                if (collision.CompareTag("player"))
                {
                    TargetChangeCheck();
                    AttackRangedOn = true;

                    bool existCheck = false; ;
                    for (int inum = 0; inum < Targets.Count; inum++)
                    {
                        if (Targets[inum].GetComponent<PlayerScript>().getPlayerID() == collision.transform.parent.gameObject.GetComponent<PlayerScript>().getPlayerID())
                        {
                            existCheck = true;
                            break;
                        }
                    }

                    if (!existCheck)
                    {
                        Targets.Add(collision.transform.parent.gameObject.GetComponent<PlayerScript>());
                    }

                }
                break;
        }
    }

    public override void AttackTriggerExit(HitBoxScript.enumHitType _hitType, Collider2D collision)
    {
        switch (_hitType)
        {
            case HitBoxScript.enumHitType.PlayerCheck:
                if (collision.CompareTag("player"))
                {
                    AttackRangedOn = false;

                    for (int inum = 0; inum < Targets.Count; inum++)
                    {
                        if (Targets[inum].GetComponent<PlayerScript>().getPlayerID() == collision.transform.parent.gameObject.GetComponent<PlayerScript>().getPlayerID())
                        {
                            Targets.RemoveAt(inum);
                            break;
                        }
                    }
                }
                break;
        }

    }
}
