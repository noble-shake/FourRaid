using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Stat")]
    [SerializeField] protected int enemyID;
    [SerializeField] protected float enemyMaxHp;
    [SerializeField] protected float enemyCurHp;
    [SerializeField] protected float enemyAtk;
    [SerializeField] protected float enemyAtkSpeed;
    [SerializeField] protected float enemyForce;
    [SerializeField] protected GameObject enemyAggroTarget;
    [SerializeField] protected int enemyAggroTargetID;
    [SerializeField] protected float[] enemyAggroGauge;
    [SerializeField] protected float speed;
    [SerializeField] protected Vector3 MovePos;
    [SerializeField] protected List<GameObject> Heroes;

    [Header("Enemy Check")]
    [SerializeField] protected bool isClicked;
    [SerializeField] protected bool isPlayerOnMouse;
    [SerializeField] protected bool AttackRangedOn;

    [Header("UI Inspector")]
    [SerializeField] protected Slider HPBarUI;

    [Header("External")]
    [SerializeField] protected float InteractTime;


    public void HPBarUIVisbile()
    {
        if (enemyCurHp == enemyMaxHp)
        {
            HPBarUI.gameObject.SetActive(false);
        }
        else
        {
            HPBarUI.gameObject.SetActive(true);
        }
    }

    protected virtual void ObjectInit() 
    {
        
    }

    protected virtual void Attack() { 
        
    }
    protected virtual void Move() { 
    
    }

    protected virtual void AggroCalculate(int _playerID, float _value) {
        enemyAggroGauge[_playerID] += _value;
    }

    protected virtual void TargetChangeCheck() {
        
        int maxOrder = 0;
        float temp = 0;
        for(int i = 0; i < enemyAggroGauge.Length; i++)
        {
            if (temp < enemyAggroGauge[i]) {
                temp = enemyAggroGauge[i];
                maxOrder = i;
            }

        }
        enemyAggroTargetID = maxOrder;
    }

    public void hitHp(int _playerID, float _value)
    {
        enemyCurHp -= _value;
        if (enemyCurHp < 0)
        {
            // die
            enemyCurHp = 0;
        }
        HPBarUIVisbile();
        if (HPBarUI.gameObject.activeSelf) {
            HPBarUI.value = enemyCurHp;
        }

        AggroCalculate(_playerID, _value);
    }

    public void healHp(float _value)
    {
        enemyCurHp += _value;
        if (enemyCurHp > enemyMaxHp)
        {
            enemyCurHp = enemyMaxHp;
        }
        HPBarUIVisbile();
        if (HPBarUI.gameObject.activeSelf)
        {
            HPBarUI.value = enemyCurHp;
        }
    }

    public virtual void AttackTriggerStay(HitBoxScript.enumHitType _hitType, Collider2D collision)
    {
    }

    public virtual void AttackTriggerEnter(HitBoxScript.enumHitType _hitType, Collider2D collision)
    {
    }

    public virtual void AttackTriggerExit(HitBoxScript.enumHitType _hitType, Collider2D collision)
    {


    }

}
