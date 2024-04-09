using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoblin : EnemyScript
{
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

    }
    protected override void Move()
    {
        minNeighborhood();
        enemyAggroTarget = Heroes[enemyAggroTargetID];

        // Vector3 TargetPos = enemyAggroTarget.transform.GetChild(1).transform.position;
        Vector3 TargetPos = enemyAggroTarget.transform.position;

        if (AttackOn) return;

        if (Vector3.Distance(TargetPos, transform.position) < 0.001f)
        {
            AttackOn = true;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, TargetPos, speed * Time.deltaTime);
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

    private void Update()
    {
        HPBarUIVisbile();
        Move();
    }
}
