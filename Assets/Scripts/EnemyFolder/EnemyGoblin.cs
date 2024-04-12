using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoblin : EnemyScript
{
    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D AttackCollider;
    [SerializeField] BoxCollider2D DetectCollider;


    protected override void ObjectInit()
    {
        enemyID = 0;
        enemyCurHp = enemyMaxHp;
        HPBarUI.maxValue = enemyMaxHp;
        HPBarUI.value = enemyMaxHp;
        enemyAggroGauge = new float[] { 0, 0, 0, 0 };
    }


    protected override void Attack()
    {
        if (!AttackRangedOn) return;

        if (enemyAggroTarget == null) return;

        bool AttackOn = false;

        Vector2 HeroPos = enemyAggroTarget.transform.position;
        if (Mathf.Abs(HeroPos.x - transform.position.x) < 2f || Mathf.Abs(HeroPos.y - transform.position.y) > 1f)
        {
            Vector2 tempLeftPos = HeroPos;
            tempLeftPos.x = tempLeftPos.x - 3f;
            Vector2 tempRightPos = HeroPos;
            tempRightPos.x = tempRightPos.x + 3f;



            Vector3 tempPos = Vector2.Distance(tempLeftPos, transform.position) < Vector2.Distance(tempRightPos, transform.position) ? tempLeftPos : tempRightPos;

            transform.position = Vector2.MoveTowards(transform.position, tempPos, speed * Time.deltaTime);
        }
        else
        {
            AttackOn = true;
        }

        if (InteractTime < enemyAtkSpeed) return;

        if (AttackOn)
        {
            enemyAggroTarget.GetComponent <PlayerScript>().hitHp(enemyAtk);
            InteractTime = 0f;
        }
    }
    protected override void Move()
    {
        minNeighborhood();
        enemyAggroTarget = Heroes[enemyAggroTargetID];

        // Vector3 TargetPos = enemyAggroTarget.transform.GetChild(1).transform.position;
        Vector3 TargetPos = enemyAggroTarget.transform.position;

        if (AttackRangedOn) return;

        if (Vector3.Distance(TargetPos, transform.position) < 0.001f)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, TargetPos, speed * Time.deltaTime);
        if (TargetPos.x != transform.position.x) {
            Vector3 looking = TargetPos.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
            transform.GetChild(0).localScale = looking;
            transform.GetChild(1).localScale = looking;
        }

    }

    private void minNeighborhood() {

        int TargetID = 0;
        float minDist = Mathf.Infinity;
        bool PlayerAliveCheck = false;

        for (int i = 0; i < Heroes.Count; i++)
        {
            PlayerAliveCheck = Heroes[i].GetComponent<PlayerScript>().getPlayerAlive();
            float dist = Vector2.Distance(Heroes[i].transform.position, transform.position);
            if (minDist < dist && PlayerAliveCheck)
            {
                minDist = dist;
                TargetID = i;
            }
        }

        enemyAggroTargetID = TargetID;


    }

    private void Awake()
    {
        ObjectInit();
        

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
                    Debug.Log("Attack On!!");
                    enemyAggroTarget = collision.gameObject;
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
                    Debug.Log("Attack On!!");
                    enemyAggroTarget = collision.gameObject;
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
