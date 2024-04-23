using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class PlayerWarrior: PlayerScript
{
    float AttackTime = 0f;

    // [SerializeField] float playerAtkAggro = 50f;
    [Header("Spell1")]
    [SerializeField] float Spell1Atk = 30f;
    [SerializeField] float Spell1Aggro = 200f;

    [Header("Spell2")]
    [SerializeField] bool isSpell2Activated;
    [SerializeField] float redueceDMG = 2f;
    [SerializeField] float Spell2Aggro = Mathf.Infinity;
    [SerializeField] float Spell2duration = 4f;
    [SerializeField] float Spell2current;

    [Header("Spell3")]
    [SerializeField] float Spell3Atk = 30f;
    [SerializeField] float Spell3Aggro = 200f;
    [SerializeField] float ThorwAngle = 0f;

    [Header("Spell4")]
    [SerializeField] bool isSpell4Activated;
    [SerializeField] float Spell4duration = 12f;
    [SerializeField] float Spell4current;
    [SerializeField] float Spell4Atk = 5f;
    [SerializeField] float Spell4Aggro = 20f;






    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D DetectCollider;
    [SerializeField] bool AttackRangedOn;

    [SerializeField] GameObject ShieldImage;
    [SerializeField] GameObject ShieldObject;

    [SerializeField] List<EnemyScript> WheelWindTargets;

    // UNITY CYCLE
    private void Awake()
    {
        DetectCollider = transform.GetChild(2).GetComponent<BoxCollider2D>();
        playerCurHp = playerMaxHp;
        HPBarUI.maxValue = playerMaxHp;
        HPBarUI.value = playerMaxHp;
        isAlive = true;
        SpellCooltime = new float[4] { 10f, 18f, 8f, 30f};
        WheelWindTargets = new List<EnemyScript>();
    }

    void Start()
    {

    }

    public override void setSpellCooltime(int _input) {
        SpellCurrentCooltime[_input] = SpellCooltime[_input];
    }

    public override float getSpellCurrentCooltime(int _input)
    {
        return SpellCurrentCooltime[_input];
    }

    void TimeFlowing() {
        for (int inum = 0; inum < 4; inum++) {
            SpellCurrentCooltime[inum] -= Time.fixedDeltaTime;
            if (SpellCurrentCooltime[inum] < 0) {
                SpellCurrentCooltime[inum] = 0f;
            }
            SpellFillAmount[inum] = 1 - (float)(SpellCurrentCooltime[inum] / SpellCooltime[inum]);
        }

        if (isSpell2Activated) {
            Spell2current -= Time.fixedDeltaTime;
            if (Spell2current < 0f) {
                isSpell2Activated = false;
            }
        }

        if (isSpell4Activated) {
            Spell4current -= Time.fixedDeltaTime;
            if (Spell4current < 0f) {
                isSpell4Activated = false;
            }
        }


        AttackTime += Time.fixedDeltaTime;
        Spell1ChargingTime -= Time.fixedDeltaTime;
        Spell2ChargingTime -= Time.fixedDeltaTime;
        Spell3ChargingTime -= Time.fixedDeltaTime;
        Spell4ChargingTime -= Time.fixedDeltaTime;



        // except Infinity
        if (AttackTime > 10f)
        {
            AttackTime = 0f;
        }

        if (Spell1ChargingTime < 0f) {
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

        bool AttackOn = false;

        Vector2 EnemyPos = EnemyObject.transform.position;
        if (Mathf.Abs(MovePos.x - transform.position.x) < 2f || Mathf.Abs(MovePos.y - transform.position.y) > 1f)
        {
            Vector2 tempLeftPos = EnemyPos;
            tempLeftPos.x = tempLeftPos.x - 3f;
            Vector2 tempRightPos = EnemyPos;
            tempRightPos.x = tempRightPos.x + 3f;
            Vector2 tempPos = Vector2.Distance(tempLeftPos, transform.position) < Vector2.Distance(tempRightPos, transform.position) ? tempLeftPos : tempRightPos;

            transform.position = Vector2.MoveTowards(transform.position, tempPos, speed * Time.deltaTime);
        }
        else {
            AttackOn = true;
        }

        if (AttackTime < playerAtkSpeed) return;

        if (AttackOn) {
            EnemyObject.GetComponent<EnemyScript>().hitHp(playerID, playerAtk, playerAtkAggro);
            AttackTime = 0f;
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
                    if (target.collider.CompareTag("enemy"))
                    {
                        Debug.Log("Enemy Attack?");
                        isCommandedAttack = true;
                        if (!AttackRangedOn)
                        {
                            commandMove(target.collider.transform.position);
                        }
                        else {
                            playerAttack();
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


    public override void AttackTriggerEnter(HitBoxScript.enumHitType _hitType, Collider2D collision)
    {
        switch (_hitType) 
        {
            case HitBoxScript.enumHitType.EnemyCheck:
                if (isCommandedAttack && collision.CompareTag("enemy"))
                {
                    EnemyObject = collision.gameObject;
                    AttackRangedOn = true;
                }

                if (collision.CompareTag("enemy")) {
                    bool existCheck = false; ;
                    for (int inum = 0; inum < WheelWindTargets.Count; inum++)
                    {
                        if (WheelWindTargets[inum].GetComponent<EnemyScript>().getEnemyID() == collision.gameObject.GetComponent<EnemyScript>().getEnemyID())
                        {
                            existCheck = true;
                            break;
                        }
                    }

                    if (!existCheck)
                    {
                        WheelWindTargets.Add(collision.gameObject.GetComponent<EnemyScript>());
                    }
                    
                }

                break;
        }
    }

    public override void AttackTriggerExit(HitBoxScript.enumHitType _hitType, Collider2D collision)
    {
        switch (_hitType)
        {
            case HitBoxScript.enumHitType.EnemyCheck:
                if (isCommandedAttack) 
                {
                    playerAttack();
                }
                
                if (collision.CompareTag("enemy"))
                {
                    for (int inum = 0; inum < WheelWindTargets.Count; inum++) {
                        if (WheelWindTargets[inum].GetComponent<EnemyScript>().getEnemyID() == collision.gameObject.GetComponent<EnemyScript>().getEnemyID()) {
                            WheelWindTargets.RemoveAt(inum);
                            break;
                        }
                    }
                }
                break;

        }

    }

    public override void ActiveSpellActivate(int _num) {
        isSpellPlaying = true;
        ActivatedSpell = _num;

        if (_num == 1) {
            Spell2ChargingTime = 0.5f;
            Spell2current = Spell2duration;
        }

        if (_num == 3) {
            Spell4ChargingTime = 0.8f;
            Spell4current = Spell4duration;
        }
    }

    public override void NonTargettingSpellActivate(int _num, Vector3 _targetPos = new Vector3())
    {
        // Instantiate Shield Attack and move.
        isSpellPlaying = true;
        ActivatedSpell = _num;
        // float angle = Vector2.Angle(transform.position, _targetPos);
        float angle = Quaternion.FromToRotation(Vector3.up, _targetPos - transform.position).eulerAngles.z;

        if (_num == 2)
        {
            Spell3ChargingTime = 0.5f;
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
            Spell1ChargingTime = 1f;
        }


        // commandAttack(EnemyObject);


    }

    public void Spell1() {
        // Targetting Spell : Jump Attack

        // animation


        // Charging Animation
        if (Spell1ChargingTime > 0f) return;
        isCommandedMove = false;

        // Jump Animation
        Vector2 tempPos = EnemyObject.transform.position;
        Vector2 tempLeftPos = tempPos;
        Vector2 tempRightPos = tempPos;
        tempLeftPos.x -= 3f;
        tempRightPos.x += 3f;

        Vector2 TargetPos = Vector2.Distance(transform.position, tempLeftPos) > Vector2.Distance(transform.position, tempRightPos) ? tempRightPos : tempLeftPos;
        MovePos = TargetPos;


        transform.position = Vector2.MoveTowards(transform.position, TargetPos, Time.deltaTime * 16f);

        if (Vector2.Distance(TargetPos, transform.position) < 0.1f)
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

    public override void hitHp(float _value)
    {
        if (isSpell2Activated) {
            _value -= redueceDMG;
            if (_value < 0f) {
                _value = 0f;
            }
        }

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
        // Buff : damage reduce & aggro max

        if (Spell2ChargingTime > 0f) return;

        isSpell2Activated = true;
        Spell2current = Spell2duration;
        ActivatedSpell = -1;
        isSpellPlaying = false;

    }

    public void Spell3() {
        // Shield Throw and Smite.

        if (Spell3ChargingTime > 0f) return;

        GameObject objArrow = Instantiate(ShieldObject, transform.position, Quaternion.Euler(new Vector3(0f, 0f, ThorwAngle)));
        RangedShield shd = objArrow.GetComponent<RangedShield>();

        shd.SetPlayerRangedAttack(playerID, 4f, playerAtk, playerAtkAggro); // ID, speed , dmg, aggro

        // EnemyObject.GetComponent<EnemyScript>().hitHp(playerID, playerAtk, playerAtkAggro);
        AttackTime = 0f;
        ActivatedSpell = -1;
        isSpellPlaying = false;

    }

    public void Spell4() {
        // Wheelwind

        if (Spell4ChargingTime > 0f) return;

        isSpell4Activated = true;
        Spell4current = Spell4duration;
        ActivatedSpell = -1;
        isSpellPlaying = false;

        StartCoroutine(WheelWind());

    }

    IEnumerator WheelWind () {
        yield return null;

        while (isSpell4Activated) {
            yield return new WaitForSeconds(0.3f);
            //foreach (EnemyScript Target in WheelWindTargets) {
            //    if (Target == null) {
            //        continue;
            //    }
            //    Target.hitHp(playerID, Spell4Atk, Spell4Aggro);
            //}
            int enemies = WheelWindTargets.Count;

            
            while (true) {
                if (enemies < 0) break;

                WheelWindTargets[enemies-1].hitHp(playerID, Spell4Atk, Spell4Aggro);
                enemies--;

                
            }
        }

        yield return null;
    }

    public override float SpellCooltimeCheck(int _input) {
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

        switch (_val) {
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
                _spellType = SpellType.SpellActive;
                _IconsImage = SpellIcon[_val];
                return new SpellInfo() { spellSlotID = _spellSlotID, spellType = _spellType, IconImage = _IconsImage, cooltime = _cooltime };
        }
        return new SpellInfo();
}

}
