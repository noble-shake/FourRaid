using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss4 : EnemyScript
{
    [Header("Boss Inspector")]
    [SerializeField] string BossName = "Corrupted One.";

    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D AttackCollider;
    [SerializeField] BoxCollider2D DetectCollider;

    [SerializeField] List<GameObject> MovePoints;
    [SerializeField] List<GameObject> SpellPoints;
    [SerializeField] int targetPoint;

    // [SerializeField] float BossSpell2Cooltime;
    [SerializeField] float currentSpell2Time;

    [SerializeField] float Spell1Atk = 20f;
    [SerializeField] float Spell2Atk = 6f;
    [SerializeField] int Spell1Counter;
    [SerializeField] float BossSpell1Cooltime;
    [SerializeField] float BossSpell2Cooltime;
    [SerializeField] float currentTime1;
    [SerializeField] float currentTime2;

    [SerializeField] float SpellAttackActivateTime1;
    [SerializeField] float SpellAttackActivateTime2;

    [SerializeField] float SpellAttackDurationTime1;
    [SerializeField] float SpellAttackDurationTime2;


    [SerializeField] float SpellAttackCurrentTime;
    [SerializeField] float SpellAttackActivateTime;
    [SerializeField] float SpellAttackDurationTime;
    [SerializeField] float BossSpellAttackDamage = 4f;
    [SerializeField] bool BossAttackActivate;

    [SerializeField] float AttackInteractTime;

    [SerializeField] GameObject IceBall;
    [SerializeField] GameObject bossMeteor;
    [SerializeField] GameObject bossLightning;

    [SerializeField] List<PlayerScript> Targets;

    [Header("Boss UI Inspector")]
    [SerializeField] GameObject BossUIObject;
    [SerializeField] Slider BossHPBarUI;
    [SerializeField] TMP_Text BossHPBarUIName;

    [SerializeField] bool TargetFocus = true;


    public override void ObjectInit()
    {

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
        // TargetFocused = true;
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

        transform.position = MovePos;
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
        currentTime1 += Time.fixedDeltaTime;
        currentTime2 += Time.fixedDeltaTime;
        AttackInteractTime += Time.fixedDeltaTime;
        currentSpell2Time += Time.fixedDeltaTime;

        HPBarUIVisbile();

        if (currentTime1 > 3.5f) {
            // BossAttackActivate = true;
            Spell1();
            currentTime1 = 0f;
        }

        //if (currentTime2 > 15f)
        //{
        //    BossAttackActivate = true;
        //    Spell2();
        //}

        if (!BossAttackActivate)
        {
            Move();
            Attack();
        }


        if (InteractTime > 100f)
        {
            InteractTime = 0f;
        }
    }

    public void Spell1() {
        // Vector3 TargetPos = Spell1TargetPos;
        // TargetPos.y += 6f;
        // transform.position = new Vector2(0f, 0f);
        Spell1Counter++;

        if (Spell1Counter < 4) {
            Vector3 TargetSpread = enemyAggroTarget.transform.position;
            TargetSpread.y += 6f;

            GameObject Meteo = Instantiate(bossMeteor, TargetSpread, bossMeteor.transform.rotation);
            Meteo.GetComponent<BossMeteor>().SetMeteor(Spell1Atk, 2f);
        }
        else if (Spell1Counter == 4) {

            for (int inum = 0; inum < Heroes.Count; inum++) {
                if (Heroes[inum].activeSelf) {
                    Vector3 TargetSpread = Heroes[inum].transform.position;
                    TargetSpread.y += 6f;

                    GameObject Meteo = Instantiate(bossMeteor, TargetSpread, bossMeteor.transform.rotation);
                    Meteo.GetComponent<BossMeteor>().SetMeteor(Spell1Atk * 1.5f, 2f);
                }
            }
        }

        Vector3 looking = enemyAggroTarget.transform.position.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        transform.GetChild(0).localScale = looking;
        transform.GetChild(1).localScale = looking;
    }

    public void Spell2()
    {
        // Vector3 TargetPos = Spell1TargetPos;
        // TargetPos.y += 6f;
        transform.position = new Vector2(0f, 0f);
        SpellAttackCurrentTime += Time.fixedDeltaTime;

        if (TargetFocus) {
            TargetFocus = false;
            GameObject Lightning = Instantiate(bossLightning, transform.position, bossMeteor.transform.rotation);
            Lightning.GetComponent<BossLightning>().SetLightning(Spell2Atk, 10f);
        }


        if (SpellAttackCurrentTime > 11f) {

            InteractTime = 0f;
            currentTime2 = 0f;
            BossAttackActivate = false;
            SpellAttackCurrentTime = 0f;

            TargetFocus = true;
        }

        Vector3 looking = enemyAggroTarget.transform.position.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        transform.GetChild(0).localScale = looking;
        transform.GetChild(1).localScale = looking;

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
