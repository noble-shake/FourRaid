using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBat: EnemyScript
{
    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D AttackCollider;
    [SerializeField] BoxCollider2D DetectCollider;
    [SerializeField] GameObject ShootBat;
    [SerializeField] List<GameObject> MovePoints;
    [SerializeField] int targetPoint;
    [SerializeField] float AttackInteractTime;


    public override void ObjectInit()
    {
        enemyCurHp = enemyMaxHp;
        HPBarUI.maxValue = enemyMaxHp;
        HPBarUI.value = enemyMaxHp;
        enemyAggroGauge = new float[] { 0, 0, 0, 0 };
    }


    protected override void Attack()
    {

        if (enemyAggroTarget == null) return;

        if (AttackInteractTime < enemyAtkSpeed) return;

        float angle = Quaternion.FromToRotation(Vector3.down, transform.position - enemyAggroTarget.transform.position).eulerAngles.z;
        GameObject shoot = Instantiate(ShootBat, transform.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
        shoot.GetComponent<RangedShootBat>().SetEnemyAttack(10f, enemyAtk);
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

        if (InteractTime > 6f) {
            InteractTime = 0f;
            int rand = targetPoint;
            while (targetPoint == rand) {
                rand = Random.Range(0, 4);
            }
            targetPoint = rand;
            MovePos = MovePoints[targetPoint].transform.position;
            
        }

        transform.position = Vector2.MoveTowards(transform.position, MovePos, 30f * Time.deltaTime);
        // AttackRangedOn = true;
        
        // enemyAggroTarget = Heroes[enemyAggroTargetID];

        // Vector3 TargetPos = enemyAggroTarget.transform.GetChild(1).transform.position;

        if (TargetPos.x != transform.position.x) {
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
        for (int inum = 0; inum < objList.Length; inum++) {
            MovePoints.Add(objList[inum].gameObject);
        }
        MovePos = MovePoints[targetPoint].transform.position;

    }

    private void FixedUpdate()
    {
        InteractTime += Time.fixedDeltaTime;
        AttackInteractTime += Time.fixedDeltaTime;

        HPBarUIVisbile();
        Move();
        Attack();

        if (InteractTime > 10f) {
            InteractTime = 0f;
        }
    }
}
