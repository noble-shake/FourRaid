using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{

    [Header("Player Stat")]
    [SerializeField, Range(0, 3)] protected int playerID; // 0, 1, 2, 3 heroes
    [SerializeField] protected float playerMaxHp;
    [SerializeField] protected float playerCurHp;
    [SerializeField] protected float playerAtk;
    [SerializeField] protected float playerAtkSpeed;
    [SerializeField] protected float playerAtkAggro;
    [SerializeField] protected float playerForce;
    [SerializeField] protected float speed;
    [SerializeField] protected Vector3 MovePos;

    [Header("Player Check")]
    [SerializeField] protected bool isClicked;
    [SerializeField] protected bool isAlive;
    [SerializeField] protected GameObject IndicatorPlayer;
    [SerializeField] protected LineRenderer IndicatorLine;
    [SerializeField] protected bool isPlayerOnMouse;
    [SerializeField] protected bool isPlayerDragToMove;
    [SerializeField] protected bool isPlayerOffMouse;
    [SerializeField] protected bool isPlayerDownMouse;
    [SerializeField] protected bool isCommandedMove;
    [SerializeField] protected bool isCommandedAttack;
    [SerializeField] protected bool isMoveDone;
    [SerializeField] protected bool isAttackPlaying;
    [SerializeField] protected bool isSpellPlaying;

    [Header("UI Inspector")]
    [SerializeField] protected Image Indicator;
    [SerializeField] protected Sprite[] SpellIcon;
    [SerializeField] protected int ActivatedSpell;
    [SerializeField] protected float[] SpellAggro;
    [SerializeField] protected Slider HPBarUI;

    [Header("Target")]
    [SerializeField] protected GameObject EnemyObject;

    [Header("Player Spell")]
    protected float[] SpellCooltime = new float[4] { 0f, 0f, 0f, 0f };
    protected float[] SpellCurrentCooltime = new float[4] { 0f, 0f, 0f, 0f };
    protected float[] SpellFillAmount = new float[4] { 0f, 0f, 0f, 0f };

    protected float Spell1ChargingTime = 0f;
    protected float Spell2ChargingTime = 0f;
    protected float Spell3ChargingTime = 0f;
    protected float Spell4ChargingTime = 0f;

    [Header("External")]
    [SerializeField] protected Animator anim;
    [SerializeField] GameObject LeftBattleWall;
    [SerializeField] GameObject RightBattleWall;

    public void resetStatus() {
        isPlayerOnMouse = false;
        isPlayerDragToMove = false;
        isPlayerDownMouse = false;
        isCommandedMove = false;
        isCommandedAttack = false;
        isAttackPlaying = false;
        isMoveDone = false;
        isSpellPlaying = false;
        anim.StopPlayback();
    }

    public virtual float getSpellCurrentCooltime(int _input) {
        return SpellCurrentCooltime[_input];
    }

    public virtual void setSpellCooltime(int _input)
    {
        SpellCurrentCooltime[_input] = SpellCooltime[_input];
    }

    public void HPBarUIVisbile() {
        if (playerCurHp == playerMaxHp)
        {
            HPBarUI.gameObject.SetActive(false);
        }
        else {
            HPBarUI.gameObject.SetActive(true);
        }
    }

    public Sprite[] getSpellIcon() {
        return SpellIcon;
    }

    public void playerSelect() {
        isClicked = true;
        IndicatorPlayer.transform.GetChild(0).transform.gameObject.SetActive(true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Select"))
            anim.SetTrigger("Select");
    }

    public void playerSelectCancel() {
        isClicked = false;
        IndicatorPlayer.transform.GetChild(0).transform.gameObject.SetActive(false);
    }

    public bool getPlayerAlive() {
        return isAlive;
    }

    public virtual void commandMove(Vector2 _pos) { }

    public virtual void commandAttack() { }

    public virtual void commandSpell(int _value) { }

    public virtual void hitHp(float _value) 
    {
        playerCurHp -= _value;
        if (playerCurHp < 0) {
            // die
            playerCurHp = 0;
        }
        HPBarUIVisbile();
        if (HPBarUI.gameObject.activeSelf) {
            HPBarUI.value = playerCurHp;
        }
    }

    public void healHp(float _value) 
    {
        playerCurHp += _value;
        if (playerCurHp > playerMaxHp)
        {
            playerCurHp = playerMaxHp;
        }

        HPBarUIVisbile();
        if (HPBarUI.gameObject.activeSelf)
        {
            HPBarUI.value = playerCurHp;
        }
    }

    public int getPlayerID()
    {
        return playerID;
    }
    public float getPlayerCurHp()
    {
        return playerCurHp;
    }

    public float getPlayerMaxHp()
    {
        return playerMaxHp;
    }

    public float getPlayerAtk()
    {
        return playerAtk;
    }

    public void setPlayerID(int _value)
    {
        playerID = _value;
    }

    public void setPlayerAtk(float _value)
    {
        playerAtk = _value;
    }


    public GameObject getIndicator() {
        return IndicatorPlayer;
    }

    public LineRenderer getIndicatorLine()
    {
        return IndicatorLine;
    }


    public IEnumerator cor()
    {
        while (true)
        {
            Debug.Log("Test");
            yield return null;
        }
    }

    public virtual void ActiveSpellActivate(int _num) { 
        
    }

    public virtual void NonActiveSpellActivate(int _num)
    {

    }

    public virtual void NonTargettingSpellActivate(int _num, Vector3 _targetPos= new Vector3()) {
        
        
    }

    public virtual void TargettingSpellActivate(int _num, GameObject _targetObject= null)
    {


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

    public virtual void PlayerMouseEnter() { 
        
    }

    public virtual void PlayerMouseDown()
    {

    }

    public virtual void PlayerMouseUp()
    {

    }

    public virtual void PlayerMouseDrag()
    {

    }

    public virtual void PlayerMouseExit()
    {

    }

    public virtual float SpellCooltimeCheck(int _input)
    {
        return -1f;
    }

    public virtual SpellInfo getSpellInfo(int _val) {
        return new SpellInfo();
    }

    public Vector2 getPlayerBattlePoint(Vector2 _pos)
    {
        return Vector2.Distance(_pos, LeftBattleWall.transform.position) < Vector2.Distance(_pos, RightBattleWall.transform.position) ? LeftBattleWall.transform.position : RightBattleWall.transform.position;
    }

}
