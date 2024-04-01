using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class PlayerWarrior: PlayerScript
{
    /*
     * Player Stat
     * Player State
     * 
     */

    [Header("Player Stat")]
    [SerializeField, Range(0, 3)] int playerID; // 0, 1, 2, 3 heroes
    [SerializeField] float playerHp;
    [SerializeField] float playerAtk;


    [Header("Player Check")]
    [SerializeField] bool isClicked;


    [Header("UI Inspector")]
    [SerializeField] Image Indicator;
    //[SerializeField] Texture2D cursourDefault;
    //[SerializeField] Texture2D cursorAttack;

    //[Header("External")]
    //[SerializeField] Camera cam;


    // UNITY CYCLE

    void Start()
    {
        
    }
    void Update()
    {
    }

    // MAIN SCRIPT

    public virtual void commandMove() { }

    public virtual void commandAtackk() { }

    public virtual void commandSpell(int _value) { }


    public override void playerSelect() {
        isClicked = true;
    }

    public override void playerSelectCancel() {
        isClicked = false;
    }

    public int getPlayerID() {
        return playerID;
    }
    public float getPlayerHp() {
        return playerHp;
    }
    public float getPlayerAtk() {
        return playerAtk;
    }

    public void setPlayerID(int _value) {
        playerID = _value;
    }
    public void setPlayerHp(int _value)
    {
        playerHp = _value;
    }
    public void setPlayerAtk(int _value)
    {
        playerAtk = _value;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag On");
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Begin");
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag End");
    }
}
