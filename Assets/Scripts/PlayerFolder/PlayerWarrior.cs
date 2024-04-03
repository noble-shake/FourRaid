using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class PlayerWarrior: PlayerScript
{
    //[Header("Player Stat")]
    //[SerializeField, Range(0, 3)] int playerID; // 0, 1, 2, 3 heroes
    //[SerializeField] float playerHp;
    //[SerializeField] float playerAtk;
    //[SerializeField] float speed;
    

    // [Header("Player Check")]
    // [SerializeField] protected bool isClicked;
    // [SerializeField] bool isOnMouse;


    // [Header("UI Inspector")]
    // [SerializeField] Image Indicator;
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
        playerMove();
        playerAttack();
    }

    // MAIN SCRIPT

    private void playerMove() {
        if (!isCommandedMove) return;

        if (isAttackPlaying) return;
        

        Vector3 convertedPos = Camera.main.ScreenToWorldPoint(MovePos);
        // Debug.Log(MovePos);

        Vector3 looking = MovePos.x > transform.position.x ? new Vector3(-1f * transform.localScale.x, 1f, 1f) : new Vector3(1f * transform.localScale.x, 1f, 1f);
        transform.localScale = looking;

        convertedPos.z = transform.position.z;
        if (Vector3.Distance(convertedPos, transform.position) < 0.1f) {
            isCommandedMove = false;
            return;
        }

        // transform.position = (convertedPos - transform.position).normalized * Time.deltaTime;
        // transform.position = (convertedPos - transform.position).normalized * Time.deltaTime;
        // transform.position = Vector3.Lerp(transform.position, convertedPos, Time.deltaTime);
    }

    private void playerAttack() { 
        
    }

    public virtual void commandMove(Vector3 _pos) {
        isCommandedMove = true;
        MovePos = _pos;
    }

    public override void commandAttack() {
        // isAttackPlaying = true;
    }

    public override void commandSpell(int _value) { }


    private void OnMouseEnter()
    {
        isPlayerOnMouse = true;
        Debug.Log("isPlayerOnMous");
    }

    private void OnMouseDrag()
    {
        
        Debug.Log("Mouse Dragged");
        Debug.Log(Input.mousePosition);
        Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));


    }

    private void OnMouseDown()
    {
        if (isClicked) {
            isPlayerDownMouse = true;
            isPlayerDragToMove = true;
        }
    }

    private void OnMouseUp()
    {
        if (isClicked && isPlayerDragToMove && isPlayerDownMouse)
        {
            Debug.Log("command on?");
            commandMove(Input.mousePosition);
            isPlayerDragToMove = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider.CompareTag("enemy")) {
                commandAttack();
            }
        }

        isPlayerDownMouse = false;
    }

    private void OnMouseOver()
    {



    }

    private void OnMouseExit()
    {
        isPlayerOnMouse = false;
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
}
