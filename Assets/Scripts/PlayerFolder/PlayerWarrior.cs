using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class PlayerWarrior: PlayerScript
{
    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D AttackCollider;
    [SerializeField] bool AttackOn;


    // UNITY CYCLE

    void Start()
    {
        AttackCollider = transform.GetChild(1).GetComponent<BoxCollider2D>();

    }
    void Update()
    {
        playerMove();
        playerAttack();
    }

    // MAIN SCRIPT

    private void playerMove() {
        if (!isCommandedMove) return;

        if (AttackOn) return;

        Vector3 convertedPos = MovePos;
        if (!isCommandedAttack) 
        {
            MovePos.z = Camera.main.transform.position.z;
            convertedPos = Camera.main.ScreenToWorldPoint(MovePos);
            convertedPos.z = transform.position.z;

 
        }

        if (Vector3.Distance(convertedPos, transform.position) < 0.001f)
        {
            isCommandedMove = false;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, convertedPos, speed * Time.deltaTime);
        Vector3 looking = convertedPos.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        transform.localScale = looking;
    }

    private void playerAttack() { 
        
    }

    public virtual void commandMove(Vector3 _pos) {
        isCommandedMove = true; 
        MovePos = _pos;
    }

    public override void commandAttack() {
        // isAttackPlaying = true;
        Debug.Log("Attack On?");
    }

    public override void commandSpell(int _value) { }


    private void OnMouseEnter()
    {
        isPlayerOnMouse = true;
        Debug.Log("isPlayerOnMous");
    }

    private void OnMouseDrag()
    {


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
            isCommandedAttack = false;
            Debug.Log("command on?");
            commandMove(Input.mousePosition);
            isPlayerDragToMove = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider.CompareTag("enemy")) {
                Debug.Log("Enemy Attack?");
                isCommandedAttack = true;
                if (!AttackOn) {
                    commandMove(hit.collider.transform.position);
                }
                
            }
            AttackOn = false;
        }

        isPlayerDownMouse = false;
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

    public void AttackTriggerEnter(PlayerHitBox.enumHitType _hitType, Collider2D collision)
    {
        switch (_hitType) 
        {
            case PlayerHitBox.enumHitType.EnemyCheck:
                if (isCommandedAttack)
                {
                    Debug.Log("Attack On!!");
                    AttackOn = true;
                }
                break;
        }
    }

    public void AttackTriggerExit(PlayerHitBox.enumHitType _hitType, Collider2D collision)
    {
        switch (_hitType)
        {
            case PlayerHitBox.enumHitType.EnemyCheck:
                if (isCommandedAttack) 
                {
                    commandMove(collision.transform.position);
                }
                break;
        }

    }
}
