using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class PlayerScript : MonoBehaviour
{

    [Header("Player Stat")]
    [SerializeField, Range(0, 3)] protected int playerID; // 0, 1, 2, 3 heroes
    [SerializeField] protected float playerHp;
    [SerializeField] protected float playerAtk;
    [SerializeField] protected float speed;
    [SerializeField] protected Vector3 MovePos;

    [Header("Player Check")]
    [SerializeField] protected bool isClicked;
    [SerializeField] protected GameObject IndicatorPlayer;
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

    [Header("Target")]
    [SerializeField] GameObject EnemyObject;

    public void playerSelect() {
        isClicked = true;
        IndicatorPlayer.SetActive(true);
    }

    public void playerSelectCancel() {
        isClicked = false;
        IndicatorPlayer.SetActive(false);
    }

    public virtual void commandMove(Vector2 _pos) { }

    public virtual void commandAttack() { }

    public virtual void commandSpell(int _value) { }

    
}
