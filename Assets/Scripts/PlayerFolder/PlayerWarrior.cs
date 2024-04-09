using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class PlayerWarrior: PlayerScript
{
    float InteractTime = 0f;

    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D AttackCollider;
    [SerializeField] bool AttackOn;

    // UNITY CYCLE
    private void Awake()
    {
        AttackCollider = transform.GetChild(1).GetComponent<BoxCollider2D>();
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
        HPBarUIVisbile();
        playerMove();

    }

    private void FixedUpdate()
    {
        InteractTime += Time.fixedDeltaTime;
        playerAttack();


        // except Infinity
        if (InteractTime > 10f) {
            InteractTime = 0f;
        }
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

        if (convertedPos.x != transform.position.x)
        {
            Vector3 looking = convertedPos.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
            transform.GetChild(0).localScale = looking;
            transform.GetChild(1).localScale = looking;
        }


    }

    private void playerAttack() {
        if (!AttackOn) return;

        if (EnemyObject == null) return;

        if (InteractTime < playerAtkSpeed) return;

        EnemyObject.GetComponent<EnemyScript>().hitHp(playerID, playerAtk);
        InteractTime = 0f;
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
        // indicator process

        if (isClicked && isPlayerDragToMove) {
            float angle;

            Vector3 IndicatorPos = IndicatorPlayer.transform.GetChild(1).transform.position;
            
            Vector2 TargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 ConvertedIndicator = Camera.main.WorldToScreenPoint(IndicatorPos);
            Vector2 ConvertedTargetPos = new Vector2(Camera.main.WorldToScreenPoint(TargetPos).x, Camera.main.WorldToScreenPoint(TargetPos).y);

            float ScaleX = Vector2.Distance(ConvertedIndicator, ConvertedTargetPos);
            Vector3 IndicatorChange = new Vector3(ScaleX, 20, 0);

            IndicatorPlayer.transform.GetChild(1).transform.localScale = IndicatorChange;
            angle = Mathf.Atan2(TargetPos.y - IndicatorPos.y, TargetPos.x - IndicatorPos.x) * Mathf.Rad2Deg;
            IndicatorPlayer.transform.GetChild(1).transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnMouseDown()
    {
        if (isClicked) {
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
            AttackOn = false;

            if (hit != null) {
                foreach (RaycastHit2D target in hit) {
                    if (target.collider.CompareTag("enemy"))
                    {
                        Debug.Log("Enemy Attack?");
                        isCommandedAttack = true;
                        if (!AttackOn)
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

        // indicator process
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
