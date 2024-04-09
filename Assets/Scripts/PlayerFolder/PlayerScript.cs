using System.Collections;
using System.Collections.Generic;
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

    [Header("UI Inspector")]
    [SerializeField] protected Image Indicator;
    [SerializeField] protected Image[] SpellIcon;
    [SerializeField] protected float[] SpellAggro;
    [SerializeField] protected Slider HPBarUI;

    [Header("Target")]
    [SerializeField] protected GameObject EnemyObject;

    public void HPBarUIVisbile() {
        if (playerCurHp == playerMaxHp)
        {
            HPBarUI.gameObject.SetActive(false);
        }
        else {
            HPBarUI.gameObject.SetActive(true);
        }
    }

    public void playerSelect() {
        isClicked = true;
        IndicatorPlayer.transform.GetChild(0).transform.gameObject.SetActive(true);
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

    public void hitHp(float _value) 
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

    public void setPlayerAtk(int _value)
    {
        playerAtk = _value;
    }
}
