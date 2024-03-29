using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class Player : MonoBehaviour
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

}
