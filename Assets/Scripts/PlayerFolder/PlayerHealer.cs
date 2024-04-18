using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class PlayerHealer: PlayerScript
{
    float AttackTime = 0f;
    float Spell1ChargingTime = 0f;
    // [SerializeField] float playerAtkAggro = 50f;
    [SerializeField] float Spell1Atk = 30f;
    [SerializeField] float Spell1Aggro = 200f;

    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D DetectCollider;
    [SerializeField] bool AttackRangedOn;

    [SerializeField] GameObject Arrow;

    [SerializeField] PlayerScript HeroObject;

    // UNITY CYCLE
    private void Awake()
    {
        DetectCollider = transform.GetChild(2).GetComponent<BoxCollider2D>();
        playerCurHp = playerMaxHp;
        HPBarUI.maxValue = playerMaxHp;
        HPBarUI.value = playerMaxHp;
        isAlive = true;
    }

    void Start()
    {

    }

    void TimeFlowing() {
        AttackTime += Time.fixedDeltaTime;
        Spell1ChargingTime -= Time.fixedDeltaTime;

        // except Infinity
        if (AttackTime > 10f)
        {
            AttackTime = 0f;
        }

        if (Spell1ChargingTime < 0f) {
            Spell1ChargingTime = 0f;
        }

    }

    void FixedUpdate()
    {
        TimeFlowing();

        HPBarUIVisbile();
        if (isSpellPlaying)
        {
            switch (ActivatedSpell) {
                case 0:
                    Spell1();
                    break;
                case 1:
                    Spell2();
                    break;
                case 2:
                    Spell3();
                    break;
                case 3:
                    Spell4();
                    break;
                case -1:
                    isSpellPlaying = false;
                    break;
            }
        }
        else {
            playerMove();
            playerAttack();
        }

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

        if (Vector2.Distance(convertedPos, transform.position) < 0.001f)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, convertedPos, speed * Time.deltaTime);

        if (convertedPos.x != transform.position.x)
        {
            Vector3 looking = convertedPos.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
            transform.GetChild(0).localScale = looking;
            transform.GetChild(1).localScale = looking;
        }


    }

    private void playerAttack() {
        if (!AttackRangedOn) return;

        if (HeroObject == null) return;

        Vector2 EnemyPos = EnemyObject.transform.position;

        if (AttackTime < playerAtkSpeed) return;

        AttackTime = 0f;

        EnemyObject.GetComponent<PlayerScript>().healHp(playerAtk);
        // Enemy Aggro Gauge up.

        Vector3 looking = EnemyPos.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        transform.GetChild(0).localScale = looking;
        transform.GetChild(1).localScale = looking;
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


    public override void PlayerMouseEnter()
    {
        isPlayerOnMouse = true;
    }

    public override void PlayerMouseDrag()
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

    public override void PlayerMouseDown()
    {
        if (isClicked)
        {
            isPlayerDownMouse = true;
            isPlayerDragToMove = true;
            IndicatorPlayer.transform.GetChild(1).transform.gameObject.SetActive(true);
        }
    }

    public override void PlayerMouseUp()
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
                    if (target.collider.CompareTag("player"))
                    {
                        HeroObject = target.collider.GetComponent<PlayerScript>();
                        isCommandedMove = false;
                        isCommandedAttack = true;
                        AttackRangedOn = true;
                        playerAttack();
                    }
                }


            }
        }
        if (IndicatorPlayer.transform.GetChild(1).transform.gameObject.activeSelf) {
            IndicatorPlayer.transform.GetChild(1).transform.gameObject.SetActive(false);
        }

        isPlayerDownMouse = false;
    }

    public override void PlayerMouseExit()
    {
        isPlayerOnMouse = false;
    }

    public override void AttackTriggerStay(HitBoxScript.enumHitType _hitType, Collider2D collision)
    {
        switch (_hitType)
        {
            case HitBoxScript.enumHitType.EnemyCheck:
                if (isCommandedAttack && collision.CompareTag("enemy")) {
                    EnemyObject = collision.gameObject;
                    AttackRangedOn = true;
                    
                }
                break;

        }
    }



    public override void NonTargettingSpellActivate(int _num, Vector3 _targetPos = new Vector3())
    {
        // Instantiate Shield Attack and move.
        isSpellPlaying = true;
        ActivatedSpell = _num;
        float angle = Vector2.Angle(transform.position, _targetPos);

    }

    public override void TargettingSpellActivate(int _num, GameObject _targetObject = null)
    {
        isSpellPlaying = true;
        ActivatedSpell = _num;
        EnemyObject = _targetObject;

        if (_num == 0) {
            Spell1ChargingTime = 1f;
        }

        // commandAttack(EnemyObject);


    }

    public void Spell1() {
        // Targetting Spell : Jump Attack

        // animation

        if (Spell1ChargingTime > 0f) return;
        isCommandedMove = false;

        Vector2 tempPos = EnemyObject.transform.position;
        Vector2 tempLeftPos = tempPos;
        Vector2 tempRightPos = tempPos;
        tempLeftPos.x -= 3f;
        tempRightPos.x += 3f;

        Vector2 TargetPos = Vector2.Distance(transform.position, tempLeftPos) > Vector2.Distance(transform.position, tempRightPos) ? tempRightPos : tempLeftPos;
        MovePos = TargetPos;
        float dist = Vector2.Distance(transform.position, TargetPos);

        // get Bazier point
        //float x = Mathf.Cos(30) * dist;
        //float y = Mathf.Sin(45) * dist;

        //Vector2 midPoint = Vector2.zero;
        //if (transform.position.x > TargetPos.x) {
        //    midPoint.x = TargetPos.x - x;
        //    midPoint.x = TargetPos.y + y;
        //}
        //else
        //{
        //    midPoint.x = TargetPos.x + x;
        //    midPoint.x = TargetPos.y + y;
        //}

        //Vector2 p4 = Vector2.Lerp(transform.position, midPoint, Time.deltaTime);
        //Vector2 p5 = Vector2.Lerp(midPoint, TargetPos, Time.deltaTime);
        //transform.position = Vector3.Lerp(p4, p5, Time.deltaTime);

        transform.position = Vector2.MoveTowards(transform.position, TargetPos, Time.deltaTime * 4f);

        if (Vector2.Distance(TargetPos, transform.position) < 0.001f)
        {
            EnemyObject.GetComponent<EnemyScript>().hitHp(playerID, Spell1Atk, Spell1Aggro);
            ActivatedSpell = -1;
            isSpellPlaying = false;
            isCommandedAttack = true;
            isCommandedMove = true;
            AttackRangedOn = true;
            playerAttack();
            return;
        }


    }

    public void Spell2() { 
        // Shield Throw and Move.


    }

    public void Spell3() { 
    
    }

    public void Spell4() { 
        
    }
}
