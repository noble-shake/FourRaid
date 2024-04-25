using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class PlayerArcher: PlayerScript
{
    float AttackTime = 0f;
    float ThorwAngle;

    // [SerializeField] float playerAtkAggro = 50f;
    [SerializeField] float Spell1Atk = 15f;
    [SerializeField] float Spell1Aggro = 5f;

    [SerializeField] bool isSpell2Activated;
    [SerializeField] float Spell2duration = 6f;
    [SerializeField] float Spell2current;
    [SerializeField] float Spell2AtkSpeed;
    [SerializeField] float Spell2Atk;

    [SerializeField] Vector2 Spell3Pos;

    [SerializeField] float Spell4Atk = 50f;
    [SerializeField] float Spell4Aggro = 20f;


    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D DetectCollider;
    [SerializeField] bool AttackRangedOn;

    [SerializeField] GameObject Arrow;
    [SerializeField] GameObject UltimateArrow;

    // UNITY CYCLE
    private void Awake()
    {
        DetectCollider = transform.GetChild(2).GetComponent<BoxCollider2D>();
        playerCurHp = playerMaxHp;
        HPBarUI.maxValue = playerMaxHp;
        HPBarUI.value = playerMaxHp;
        isAlive = true;
        SpellCooltime = new float[4] { 10f, 12f, 20f, 35f };

        Spell2AtkSpeed = (float)(playerAtkSpeed / 2);
        Spell2Atk = (float)(playerAtk * 1.2f);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public override void setSpellCooltime(int _input)
    {
        SpellCurrentCooltime[_input] = SpellCooltime[_input];
    }

    public override float getSpellCurrentCooltime(int _input)
    {
        return SpellCurrentCooltime[_input];
    }

    void TimeFlowing()
    {
        for (int inum = 0; inum < 4; inum++)
        {
            SpellCurrentCooltime[inum] -= Time.fixedDeltaTime;
            if (SpellCurrentCooltime[inum] < 0)
            {
                SpellCurrentCooltime[inum] = 0f;
            }
            SpellFillAmount[inum] = 1 - (float)(SpellCurrentCooltime[inum] / SpellCooltime[inum]);
        }


        AttackTime += Time.fixedDeltaTime;
        Spell1ChargingTime -= Time.fixedDeltaTime;
        Spell2ChargingTime -= Time.fixedDeltaTime;
        Spell3ChargingTime -= Time.fixedDeltaTime;
        Spell4ChargingTime -= Time.fixedDeltaTime;

        if (isSpell2Activated)
        {
            Spell2current -= Time.fixedDeltaTime;
            if (Spell2current < 0f)
            {
                isSpell2Activated = false;
            }
        }


        // except Infinity
        if (AttackTime > 10f)
        {
            AttackTime = 0f;
        }

        if (Spell1ChargingTime < 0f)
        {
            Spell1ChargingTime = 0f;
        }

        if (Spell2ChargingTime < 0f)
        {
            Spell2ChargingTime = 0f;
        }

        if (Spell3ChargingTime < 0f)
        {
            Spell3ChargingTime = 0f;
        }

        if (Spell4ChargingTime < 0f)
        {
            Spell4ChargingTime = 0f;
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

        if (EnemyObject == null) return;

        Vector2 EnemyPos = EnemyObject.transform.position;
        //float angle = Vector2.Angle(transform.position, EnemyPos) - 90.0f; don't use

        // float angle = Quaternion.FromToRotation(Vector3.up, transform.position - EnemyObject.transform.position).eulerAngles.z - 90;
        float angle = Quaternion.FromToRotation(Vector3.right, transform.position - EnemyObject.transform.position).eulerAngles.z;

        if (isSpell2Activated)
        {
            if (AttackTime < Spell2AtkSpeed) return;

            GameObject objArrow = Instantiate(Arrow, transform.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
            RangedAttack arrow = objArrow.GetComponent<RangedAttack>();

            arrow.SetPlayerRangedAttack(playerID, 8f, Spell2Atk, playerAtkAggro / 2); // ID, speed , dmg, aggro
        }
        else {
            if (AttackTime < playerAtkSpeed) return;

            GameObject objArrow = Instantiate(Arrow, transform.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
            RangedAttack arrow = objArrow.GetComponent<RangedAttack>();

            arrow.SetPlayerRangedAttack(playerID, 8f, playerAtk, playerAtkAggro); // ID, speed , dmg, aggro
        }

      


        // EnemyObject.GetComponent<EnemyScript>().hitHp(playerID, playerAtk, playerAtkAggro);
        AttackTime = 0f;

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
            commandMove(Input.mousePosition);
            isPlayerDragToMove = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D[] hit = Physics2D.GetRayIntersectionAll(ray);
            AttackRangedOn = false;

            if (hit != null) {
                foreach (RaycastHit2D target in hit) {
                    if (target.collider.CompareTag("enemy"))
                    {
                        EnemyObject = target.collider.transform.parent.gameObject;
                        Debug.Log("Enemy Attack?");
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
                    EnemyObject = collision.transform.parent.gameObject;
                    AttackRangedOn = true;
                    
                }
                break;

        }
    }

    public override void ActiveSpellActivate(int _num)
    {
        isSpellPlaying = true;
        ActivatedSpell = _num;

        if (_num == 1)
        {
            Spell2ChargingTime = 0.5f;
            Spell2current = Spell2duration;
        }
    }

    public override void NonTargettingSpellActivate(int _num, Vector3 _targetPos = new Vector3())
    {
        // Instantiate Shield Attack and move.
        isSpellPlaying = true;
        ActivatedSpell = _num;
        // float angle = Vector2.Angle(transform.position, _targetPos);
        // float angle = Quaternion.FromToRotation(Vector3.up, _targetPos - transform.position).eulerAngles.z;

        if (_num == 2)
        {
            // float angle = Quaternion.FromToRotation(Vector3.right, _targetPos - transform.position).eulerAngles.z;
            Spell3ChargingTime = 0.8f;
            Spell3Pos = _targetPos;
        }

        if (_num == 3) {
            float angle = Quaternion.FromToRotation(Vector3.right, transform.position - _targetPos).eulerAngles.z;
            Spell4ChargingTime = 1f;
            ThorwAngle = angle;

        }

        Vector3 looking = _targetPos.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        transform.GetChild(0).localScale = looking;
        transform.GetChild(1).localScale = looking;
    }

    public override void TargettingSpellActivate(int _num, GameObject _targetObject = null)
    {
        isSpellPlaying = true;
        ActivatedSpell = _num;
        EnemyObject = _targetObject;

        if (_num == 0) {
            float angle = Quaternion.FromToRotation(Vector3.right, transform.position - _targetObject.transform.position).eulerAngles.z;
            Spell1ChargingTime = 1f;
            ThorwAngle = angle;
        }

        // commandAttack(EnemyObject);


    }

    public void Spell1() {
        // Targetting Spell : Jump Attack

        // animation

        if (Spell1ChargingTime > 0f) return;
        isCommandedMove = false;

        StartCoroutine(Spell1ArrowShoot());

        // EnemyObject.GetComponent<EnemyScript>().hitHp(playerID, playerAtk, playerAtkAggro);
        AttackTime = 0f;
        ActivatedSpell = -1;
        isSpellPlaying = false;

    }

    IEnumerator Spell1ArrowShoot() {
        yield return null;
        for (int inum = 0; inum < 3; inum++) {
            yield return new WaitForSeconds(0.3f);
            GameObject objArrow = Instantiate(Arrow, transform.position, Quaternion.Euler(new Vector3(0f, 0f, ThorwAngle)));
            RangedAttack shd = objArrow.GetComponent<RangedAttack>();
            shd.SetPlayerRangedAttack(playerID, 8f, Spell1Atk, Spell1Aggro); // ID, speed , dmg, aggro
        }
        yield return null;
    }

    public override void hitHp(float _value)
    {
        playerCurHp -= _value;
        if (playerCurHp < 0)
        {
            // die
            playerCurHp = 0;
        }
        HPBarUIVisbile();
        if (HPBarUI.gameObject.activeSelf)
        {
            HPBarUI.value = playerCurHp;
        }
    }

    public void Spell2() {
        if (Spell2ChargingTime > 0f) return;

        isSpell2Activated = true;
        Spell2current = Spell2duration;
        ActivatedSpell = -1;
        isSpellPlaying = false;

    }

    public void Spell3() {
        // DASH


        // Charging Animation
        if (Spell3ChargingTime > 0f) return;
        isCommandedMove = false;

        // Jump Animation
        
        transform.position = Vector2.MoveTowards(transform.position, Spell3Pos, Time.deltaTime * 32f);

        if (Vector2.Distance(Spell3Pos, transform.position) < 0.01f)
        {
            ActivatedSpell = -1;
            isSpellPlaying = false;
            isCommandedAttack = false;
            isCommandedMove = false;
            AttackTime = 0f;
            return;
        }

    }

    public void Spell4() {
        // Targetting Spell : Jump Attack

        // animation

        if (Spell1ChargingTime > 0f) return;
        isCommandedMove = false;

        StartCoroutine(Spell4ArrowShoot());

        // EnemyObject.GetComponent<EnemyScript>().hitHp(playerID, playerAtk, playerAtkAggro);
        AttackTime = 0f;
        ActivatedSpell = -1;
        isSpellPlaying = false;
    }

    IEnumerator Spell4ArrowShoot()
    {
        yield return null;
        GameObject objArrow = Instantiate(UltimateArrow, transform.position, Quaternion.Euler(new Vector3(0f, 0f, ThorwAngle)));
        RangedArcherSpell4 shd = objArrow.GetComponent<RangedArcherSpell4>();
        shd.SetPlayerRangedAttack(playerID, 16f, Spell4Atk, Spell4Aggro); // ID, speed , dmg, aggro
        yield return null;
    }

    public override float SpellCooltimeCheck(int _input)
    {
        return SpellFillAmount[_input];
    }

    public override SpellInfo getSpellInfo(int _val)
    {
        // public int spellSlotID;
        // public SpellType spellType;
        // public Sprite IconImage;
        // public float cooltime;
        int _spellSlotID = _val;
        SpellType _spellType;
        Sprite _IconsImage;
        float _cooltime = SpellCooltime[_val];

        switch (_val)
        {
            case 0:
                _spellType = SpellType.SpellTargetting;
                _IconsImage = SpellIcon[_val];
                return new SpellInfo() { spellSlotID = _spellSlotID, spellType = _spellType, IconImage = _IconsImage, cooltime = _cooltime };
            case 1:
                _spellType = SpellType.SpellActive;
                _IconsImage = SpellIcon[_val];
                return new SpellInfo() { spellSlotID = _spellSlotID, spellType = _spellType, IconImage = _IconsImage, cooltime = _cooltime };
            case 2:
                _spellType = SpellType.SpellNonTargetting;
                _IconsImage = SpellIcon[_val];
                return new SpellInfo() { spellSlotID = _spellSlotID, spellType = _spellType, IconImage = _IconsImage, cooltime = _cooltime };
            case 3:
                _spellType = SpellType.SpellNonTargetting;
                _IconsImage = SpellIcon[_val];
                return new SpellInfo() { spellSlotID = _spellSlotID, spellType = _spellType, IconImage = _IconsImage, cooltime = _cooltime };
        }
        return new SpellInfo();
    }

}
