using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss2 : EnemyScript
{
    [Header("Boss Inspector")]
    [SerializeField] string BossName = "Keroberos";

    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D AttackCollider;
    [SerializeField] BoxCollider2D DetectCollider;
    [SerializeField] bool TargetFocused;
    [SerializeField] float BossSpellCooltime;
    [SerializeField] float currentTime;
    [SerializeField] float SpellAttackCurrentTime;
    [SerializeField] float SpellAttackActivateTime;
    [SerializeField] float BossSpellAttackDamage = 50f;
    [SerializeField] bool BossAttackActivate;

    [SerializeField] GameObject Indicator1;
    [SerializeField] GameObject Indicator2;
    [SerializeField] GameObject Indicator3;
    [SerializeField] float angle1;
    [SerializeField] float angle2;
    [SerializeField] float angle3;

    [SerializeField] List<PlayerScript> Targets;

    //[Header("Boss UI Inspector")]
    //[SerializeField] GameObject BossUIObject;
    //[SerializeField] Slider BossHPBarUI;
    //[SerializeField] TMP_Text BossHPBarUIName;


    public override void ObjectInit()
    {
        //GameObject uiObj = Instantiate(BossUIObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
        ////BossHPBarUI = GameObject.Find("BossUI").GetComponent<Slider>();
        ////BossHPBarUIName = GameObject.Find("BossUI").GetComponent<Slider>();
        //BossHPBarUI = uiObj.GetComponentInChildren<Slider>();
        //BossHPBarUIName = uiObj.GetComponentInChildren<TMP_Text>();

        //BossHPBarUI.gameObject.SetActive(true);
        //BossHPBarUIName.gameObject.SetActive(true);
        //BossHPBarUIName.text = "Keroberos";

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
        TargetFocused = true;
        // ObjectInit();
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

        Indicator1.SetActive(true);
        Indicator2.SetActive(true);
        Indicator3.SetActive(true);

        // Vector2 IndicatorPos = Indicator.transform.GetChild(0).transform.position;
        Vector2 IndicatorPos = Indicator1.transform.transform.position;
        Vector2 TargetPos = IndicatorPos;
        TargetPos.x += 8f;
        TargetPos.y += 2f;

        Vector3 TargetSpread = enemyAggroTarget.transform.position;


        if (TargetFocused) { 
            angle1 = Quaternion.FromToRotation(-Vector3.right, IndicatorPos - (Vector2)TargetSpread).eulerAngles.z - 60f;
            angle2 = Quaternion.FromToRotation(-Vector3.right, IndicatorPos - (Vector2)TargetSpread).eulerAngles.z;
            angle3 = Quaternion.FromToRotation(-Vector3.right, IndicatorPos - (Vector2)TargetSpread).eulerAngles.z + 60f;

            Indicator1.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle1));
            Indicator2.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle2));
            Indicator3.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle3));

            Debug.Log(angle1);
            Debug.Log(angle2);
            Debug.Log(angle3);
            
        }

        if (SpellAttackCurrentTime > SpellAttackActivateTime -2f && TargetFocused) {

            TargetFocused = false;
        }
        
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

            Indicator1.SetActive(false);
            Indicator2.SetActive(false);
            Indicator3.SetActive(false);

            InteractTime = 0f;
            currentTime = 0f;

            TargetFocused = true;
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
