using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss3 : EnemyScript
{
    [Header("Boss Inspector")]
    [SerializeField] string BossName = "Ice Elf";

    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D AttackCollider;
    [SerializeField] BoxCollider2D DetectCollider;
    [SerializeField] bool TargetFocused;

    [SerializeField] List<GameObject> MovePoints;
    [SerializeField] int targetPoint;

    [SerializeField] float BossSpell2Cooltime;
    [SerializeField] float currentSpell2Time;

    [SerializeField] float BossSpell1Cooltime;
    [SerializeField] float currentTime;
    [SerializeField] float SpellAttackCurrentTime;
    [SerializeField] float SpellAttackActivateTime;
    [SerializeField] float SpellAttackDurationTime;
    [SerializeField] float BossSpellAttackDamage = 4f;
    [SerializeField] bool BossAttackActivate;

    [SerializeField] float AttackInteractTime;

    [SerializeField] GameObject Indicator;
    [SerializeField] GameObject IndicatorEffect;
    [SerializeField] GameObject IceBall;

    [SerializeField] List<PlayerScript> Targets;

    [Header("Boss UI Inspector")]
    [SerializeField] GameObject BossUIObject;
    [SerializeField] Slider BossHPBarUI;
    [SerializeField] TMP_Text BossHPBarUIName;


    public override void ObjectInit()
    {
        //GameObject uiObj = Instantiate(BossUIObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
        ////BossHPBarUI = GameObject.Find("BossUI").GetComponent<Slider>();
        ////BossHPBarUIName = GameObject.Find("BossUI").GetComponent<Slider>();
        //BossHPBarUI = uiObj.GetComponentInChildren<Slider>();
        //BossHPBarUIName = uiObj.GetComponentInChildren<TMP_Text>();

        //BossHPBarUI.gameObject.SetActive(true);
        //BossHPBarUIName.gameObject.SetActive(true);
        //BossHPBarUIName.text = "Ice Elf";

        GameManager.instance.SetBossHP(enemyMaxHp, enemyMaxHp, BossName);

        enemyCurHp = enemyMaxHp;
        HPBarUI.maxValue = enemyMaxHp;
        HPBarUI.value = enemyMaxHp;
        //BossHPBarUI.maxValue = enemyMaxHp;
        //BossHPBarUI.value = enemyMaxHp;
        enemyAggroGauge = new float[] { 0, 0, 0, 0 };
    }

    public override void hitHp(int _playerID, float _value, float _aggro)
    {
        enemyCurHp -= _value;
        if (enemyCurHp < 0)
        {
            // die
            enemyCurHp = 0;

            if (gameObject.activeSelf)
            {
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
        TargetFocused = true;
        // ObjectInit();
    }

    private void OnDisable()
    {
        // BossHPBarUI.gameObject.SetActive(false);
        // BossHPBarUIName.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        //BossHPBarUI.gameObject.SetActive(false);
        //BossHPBarUIName.gameObject.SetActive(false);
    }


    protected override void Attack()
    {

        if (enemyAggroTarget == null) return;

        if (AttackInteractTime < enemyAtkSpeed) return;

        float angle = Quaternion.FromToRotation(Vector3.down, transform.position - enemyAggroTarget.transform.position).eulerAngles.z;
        GameObject shoot = Instantiate(IceBall, transform.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
        shoot.GetComponent<RangedIceBall>().SetEnemyAttack(20f, enemyAtk);
        // enemyAggroTarget.GetComponent <PlayerScript>().hitHp(enemyAtk);
        AttackInteractTime = 0f;

        if (enemyAggroTarget != null)
        {
            Vector3 looking = enemyAggroTarget.transform.position.x > transform.position.x ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
            transform.GetChild(0).localScale = looking;
            transform.GetChild(1).localScale = looking;
        }
    }
    protected override void Move()
    {
        TargetChangeCheck();
        Vector3 TargetPos = enemyAggroTarget.transform.position;
        MovePos = MovePoints[targetPoint].transform.position;

        if (InteractTime > 12f)
        {
            InteractTime = 0f;
            int rand = targetPoint;
            while (targetPoint == rand)
            {
                rand = Random.Range(0, 4);
            }
            targetPoint = rand;
            MovePos = MovePoints[targetPoint].transform.position;

        }

        transform.position = Vector2.MoveTowards(transform.position, MovePos, 30f * Time.deltaTime);
        // AttackRangedOn = true;

        // enemyAggroTarget = Heroes[enemyAggroTargetID];

        // Vector3 TargetPos = enemyAggroTarget.transform.GetChild(1).transform.position;

        if (TargetPos.x != transform.position.x)
        {
            Vector3 looking = TargetPos.x > transform.position.x ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
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
        targetPoint = Random.Range(0, 4);
        MovePoints = new List<GameObject>();
        Transform[] objList = GameObject.Find("MovePoints").transform.GetComponentsInChildren<Transform>();
        for (int inum = 0; inum < objList.Length; inum++)
        {
            MovePoints.Add(objList[inum].gameObject);
        }
        MovePos = MovePoints[targetPoint].transform.position;

    }

    private void FixedUpdate()
    {
        InteractTime += Time.fixedDeltaTime;
        currentTime += Time.fixedDeltaTime;
        AttackInteractTime += Time.fixedDeltaTime;
        currentSpell2Time += Time.fixedDeltaTime;

        HPBarUIVisbile();

        if (currentTime > BossSpell1Cooltime) {
            BossAttackActivate = true;
            SpellAttack1();
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

    public void SpellAttack1() {

        TargetChangeCheck();
        // 3 seconds
        SpellAttackCurrentTime += Time.fixedDeltaTime;

        Indicator.SetActive(true);
        IndicatorEffect.SetActive(false);

        // Vector2 IndicatorPos = Indicator.transform.GetChild(0).transform.position;
        Vector2 IndicatorPos = Indicator.transform.transform.position;
        Vector2 TargetPos = IndicatorPos;
        TargetPos.x += 8f;
        TargetPos.y += 2f;

        Vector3 TargetSpread = enemyAggroTarget.transform.position;
        TargetSpread.y = enemyAggroTarget.transform.position.y + 1f;


        if (TargetFocused) {
            transform.position = TargetSpread;
        }

        if (SpellAttackCurrentTime > SpellAttackActivateTime -3f) {
            Indicator.GetComponent<SpriteRenderer>().size += new Vector2(Time.fixedDeltaTime * 15f / 4f, Time.fixedDeltaTime * 15f / 10f);
            Indicator.GetComponent<CapsuleCollider2D>().size += new Vector2(Time.fixedDeltaTime * 15f / 4f, Time.fixedDeltaTime * 15f / 9f);
            TargetFocused = false;
        }
   
        if (SpellAttackCurrentTime > SpellAttackActivateTime - 2f) {
            IndicatorEffect.SetActive(true);
            IndicatorEffect.GetComponent<SpriteRenderer>().size = new Vector2(4f, 3f);
            // hit
            SpellAttackDurationTime += Time.fixedDeltaTime;

            if (SpellAttackDurationTime > 0.33f) {
                int players = Targets.Count - 1;


                while (true)
                {
                    if (players < 0) break;

                    Targets[players--].hitHp(8f);

                }
                SpellAttackDurationTime = 0f;
            }
        }

        if (SpellAttackCurrentTime > SpellAttackActivateTime)
        {
            Indicator.SetActive(false);
            InteractTime = 0f;
            currentTime = 0f;

            TargetFocused = true;

            SpellAttackCurrentTime = 0f;

            BossAttackActivate = false;
            Indicator.GetComponent<SpriteRenderer>().size = new Vector2(1f, 0.5f);
            Indicator.GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 0.5f);
            //Indicator.SetActive(false);
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
