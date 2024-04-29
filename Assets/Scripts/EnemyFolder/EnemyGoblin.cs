using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyGoblin : EnemyScript
{
    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D AttackCollider;
    [SerializeField] BoxCollider2D DetectCollider;


    public override void ObjectInit()
    {
        enemyCurHp = enemyMaxHp;
        HPBarUI.maxValue = enemyMaxHp;
        HPBarUI.value = enemyMaxHp;
        enemyAggroGauge = new float[] { 0, 0, 0, 0 };
    }


    protected override void Attack()
    {
        if (!AttackRangedOn) return;

        if (enemyAggroTarget == null) return;


        Vector2 HeroPos = enemyAggroTarget.transform.position;

        

        if (InteractTime < enemyAtkSpeed) return;


        enemyAggroTarget.GetComponent <PlayerScript>().hitHp(enemyAtk);
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

        if (Vector3.Distance(BattlePoint, transform.position) < 2f)
        {
            AttackRangedOn = true;
        }

        if (TargetPos.x != transform.position.x) {
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
        InteractTime += Time.deltaTime;

        HPBarUIVisbile();
        Move();
        Attack();

        if (InteractTime > 10f) {
            InteractTime = 0f;
        }
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
                }
                break;
        }

    }

}
