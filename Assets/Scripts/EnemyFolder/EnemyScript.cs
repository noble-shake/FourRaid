using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyInfo
{
    public int enemyID;
    public float enemyMaxHp;
    public float enemyAtk;
    public float enemyAtkSpeed;
}

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
    [SerializeField] protected float originSpeed;
    [SerializeField] protected Vector3 MovePos;
    [SerializeField] protected List<GameObject> Heroes;
    [SerializeField] protected bool prevented;

    [Header("Enemy Check")]
    [SerializeField] protected bool isClicked;
    [SerializeField] protected bool isPlayerOnMouse;
    [SerializeField] protected bool AttackRangedOn;

    [Header("UI Inspector")]
    [SerializeField] protected Slider HPBarUI;

    [Header("External")]
    [SerializeField] protected float InteractTime;
    [SerializeField] GameObject LeftBattleWall;
    [SerializeField] GameObject RightBattleWall;


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

    public virtual void ObjectInit() 
    {
        
    }

    protected virtual void Attack() { 
        
    }
    protected virtual void Move() { 
    
    }

    protected virtual void AggroCalculate(int _playerID, float _value) {
        enemyAggroGauge[_playerID] += _value;
    }

    public void TargetChangeCheck() {

        AttackRangedOn = false;
        List<GameObject> HeroObjects = PlayerManager.instance.getHeroesObjects();
        int maxOrder = 0;
        float temp = -1;
        for(int i = 0; i < HeroObjects.Count; i++)
        {
            bool PlayerAliveCheck = false;
            PlayerAliveCheck = PlayerManager.instance.getHeroAlive(i);
            if (temp < enemyAggroGauge[i] && PlayerAliveCheck) {
                temp = enemyAggroGauge[i];
                maxOrder = i;
            }
        }

        enemyAggroTargetID = maxOrder;
        enemyAggroTarget = Heroes[enemyAggroTargetID];
    }

    public virtual void hitHp(int _playerID, float _value, float _aggro)
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
            
        }
        HPBarUIVisbile();
        if (HPBarUI.gameObject.activeSelf) {
            HPBarUI.value = enemyCurHp;
        }

        AggroCalculate(_playerID, _aggro);
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

    public int getEnemyID() {
        return enemyID;
    }

    public void setEnemyID(int _id) {
        enemyID = _id;
    }

    public float getEnemySpeed() {
        return speed;
    }

    public float getEnemyOriginSpeed()
    {
        return originSpeed;
    }

    public void setEnemySpeed(float _speed) {
        speed = _speed;
    }

    public void preventCollideEachOther() {
        prevented = true;
    }

    public void notpreventCollideEachOther()
    {
        prevented = false;
    }

    public Vector2 getEnemyBattlePoint(Vector2 _pos) {
        return Vector2.Distance(_pos, LeftBattleWall.transform.position) < Vector2.Distance(_pos, RightBattleWall.transform.position) ? LeftBattleWall.transform.position : RightBattleWall.transform.position;
    }

    private void OnDisable()
    {
    }
}
