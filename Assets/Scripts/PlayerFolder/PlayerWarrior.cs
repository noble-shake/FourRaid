using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class PlayerWarrior: PlayerScript
{
    float InteractTime = 0f;

    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D AttackCollider;
    [SerializeField] BoxCollider2D DetectCollider;
    [SerializeField] bool AttackRangedOn;

    // UNITY CYCLE
    private void Awake()
    {
        DetectCollider = transform.GetChild(1).GetComponent<BoxCollider2D>();
        AttackCollider = transform.GetChild(2).GetComponent<BoxCollider2D>();
        playerCurHp = playerMaxHp;
        HPBarUI.maxValue = playerMaxHp;
        HPBarUI.value = playerMaxHp;
        isAlive = true;
    }

    void Start()
    {

    }
    void Update()
    {
        InteractTime += Time.fixedDeltaTime;

        HPBarUIVisbile();
        playerMove();
        playerAttack();


        // except Infinity
        if (InteractTime > 10f)
        {
            InteractTime = 0f;
        }

    }

    private void FixedUpdate()
    {

    }

    // MAIN SCRIPT

    private void playerMove() {
        if (!isCommandedMove) return;

        if (AttackRangedOn) return;

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

        if (convertedPos.x != transform.position.x)
        {
            Vector3 looking = convertedPos.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
            transform.GetChild(0).localScale = looking;
            transform.GetChild(1).localScale = looking;
        }


    }

    private void playerAttack() {
        if (!AttackRangedOn) return;

        if (EnemyObject == null) return;



        bool AttackOn = false;

        Vector3 EnemyPos = EnemyObject.transform.position;
        if (Mathf.Abs(MovePos.x - transform.position.x) < 2f || Mathf.Abs(MovePos.y - transform.position.y) > 3f)
        {
            Vector3 tempLeftPos = EnemyPos;
            tempLeftPos.x = tempLeftPos.x - 3f;
            Vector3 tempRightPos = EnemyPos;
            tempRightPos.x = tempRightPos.x + 3f;
            Vector3 tempPos = Vector3.Distance(tempLeftPos, transform.position) < Vector3.Distance(tempRightPos, transform.position) ? tempLeftPos : tempRightPos;

            transform.position = Vector3.MoveTowards(transform.position, tempPos, speed * Time.deltaTime);
        }
        else {
            AttackOn = true;
        }

        if (InteractTime < playerAtkSpeed) return;

        if (AttackOn) {
            EnemyObject.GetComponent<EnemyScript>().hitHp(playerID, playerAtk);
            InteractTime = 0f;
        }


    }

    public virtual void commandMove(Vector3 _pos) {
        isCommandedMove = true; 
        MovePos = _pos;
    }

    //public override void commandAttack(GameObject _enemy) {
    //    // isAttackPlaying = true;
    //    Debug.Log("Attack On?");
    //}

    public override void commandSpell(int _value) { }


    private void OnMouseEnter()
    {
        isPlayerOnMouse = true;
    }

    private void OnMouseDrag()
    {
        // indicator process

        if (isClicked && isPlayerDragToMove) {
            Vector3 IndicatorPos = IndicatorPlayer.transform.GetChild(1).transform.position;

            Vector3 TargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TargetPos.z = 0f;
            Vector3 ConvertedIndicator = Camera.main.WorldToScreenPoint(IndicatorPos);
           

            IndicatorLine.positionCount = 2;
            Vector3[] vector2s = new Vector3[2] { IndicatorPos, TargetPos };
            IndicatorLine.SetPositions(vector2s);
        }
    }

    private void OnMouseDown()
    {
        if (isClicked)
        {
            isPlayerDownMouse = true;
            isPlayerDragToMove = true;
            IndicatorPlayer.transform.GetChild(1).transform.gameObject.SetActive(true);
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

            RaycastHit2D[] hit = Physics2D.GetRayIntersectionAll(ray);
            AttackRangedOn = false;

            if (hit != null) {
                foreach (RaycastHit2D target in hit) {
                    if (target.collider.CompareTag("enemy"))
                    {
                        Debug.Log("Enemy Attack?");
                        isCommandedAttack = true;
                        if (!AttackRangedOn)
                        {
                            commandMove(target.collider.transform.position);
                        }
                    }
                }


            }
        }
        if (IndicatorPlayer.transform.GetChild(1).transform.gameObject.activeSelf) {
            IndicatorPlayer.transform.GetChild(1).transform.gameObject.SetActive(false);
        }

        isPlayerDownMouse = false;
    }

    private void OnMouseExit()
    {
        isPlayerOnMouse = false;
    }


    public void AttackTriggerEnter(PlayerHitBox.enumHitType _hitType, Collider2D collision)
    {
        switch (_hitType) 
        {
            case PlayerHitBox.enumHitType.EnemyCheck:
                if (isCommandedAttack && collision.CompareTag("enemy"))
                {
                    Debug.Log("Attack On!!");
                    EnemyObject = collision.gameObject;
                    AttackRangedOn = true;
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

    public override void NonTargettingSpellActivate(int _num, Vector3 _targetPos = new Vector3())
    {
        // Instantiate or rush

    }

    public override void TargettingSpellActivate(int _num, GameObject _targetObject = null)
    {
        EnemyObject = _targetObject;
        // commandAttack(EnemyObject);

    }

    public void Spell1() { 
        // Targetting Spell : Smash

        
    }

    public void Spell2() { 
    
    }

    public void Spell3() { 
    
    }

    public void Spell4() { 
        
    }
}
