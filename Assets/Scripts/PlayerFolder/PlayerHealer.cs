using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class PlayerHealer : PlayerScript
{
    float AttackTime = 0f;

    // [SerializeField] float playerAtkAggro = 50f;
    [Header("Spell1")]
    [SerializeField] float Spell1current;
    [SerializeField] float Spell1Aggro = 200f;

    [Header("Spell2")]
    [SerializeField] bool isSpell2Activated;
    [SerializeField] float TargetOriginAtk;
    [SerializeField] PlayerScript Spell2Target;
    [SerializeField] float Spell2Aggro = Mathf.Infinity;
    [SerializeField] float Spell2duration = 4f;
    [SerializeField] float Spell2current;

    [Header("Spell3")]
    [SerializeField] float Spell3Atk = 20f;
    [SerializeField] float Spell3Aggro = 20;

    [Header("Spell4")]
    [SerializeField] bool isSpell4Activated;
    [SerializeField] float Spell4duration = 12f;
    [SerializeField] float Spell4current;
    [SerializeField] float Spell4Atk = 5f;
    [SerializeField] float Spell4Aggro = 20f;

    [Header("Hit Collider")]
    [SerializeField] BoxCollider2D DetectCollider;
    [SerializeField] BoxCollider2D AttackCollider;
    [SerializeField] bool AttackRangedOn;

    [SerializeField] GameObject Arrow;

    [SerializeField] PlayerScript HeroObject;

    [SerializeField] List<EnemyScript> HealColliderEnemies;
    [SerializeField] RangedCow minnerCow;
    [SerializeField] RangedCow blackCow;

    // UNITY CYCLE
    private void Awake()
    {
        DetectCollider = transform.GetChild(2).GetComponent<BoxCollider2D>();
        playerCurHp = playerMaxHp;
        HPBarUI.maxValue = playerMaxHp;
        HPBarUI.value = playerMaxHp;
        isAlive = true;
        HealColliderEnemies = new List<EnemyScript>();
        SpellCooltime = new float[4] { 12f, 5f, 24f, 60f };
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

        if (isSpell2Activated)
        {
            Spell2current -= Time.fixedDeltaTime;
            if (Spell2current < 0f)
            {
                isSpell2Activated = false;
                Spell2Target.GetComponent<PlayerScript>().setPlayerAtk(TargetOriginAtk);
                Spell2Target = null;
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
            switch (ActivatedSpell)
            {
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
        else
        {
            playerMove();
            playerAttack();
        }

    }


    public override void hitHp(float _value)
    {
        playerCurHp -= _value;
        if (playerCurHp < 0)
        {
            // die
            playerCurHp = 0;
            isAlive = false;
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }
        HPBarUIVisbile();
        if (HPBarUI.gameObject.activeSelf)
        {
            HPBarUI.value = playerCurHp;
        }
    }


    // MAIN SCRIPT

    private void playerMove()
    {
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

    private void playerAttack()
    {
        if (!AttackRangedOn) return;

        if (HeroObject == null) return;

        Vector2 HeroPos = HeroObject.transform.position;

        if (AttackTime < playerAtkSpeed) return;

        AttackTime = 0f;

        HeroObject.GetComponent<PlayerScript>().healHp(playerAtk);
        AttackCollider.transform.position = HeroObject.transform.position;
        // Enemy Aggro Gauge up.

        Vector3 looking = HeroPos.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        transform.GetChild(0).localScale = looking;
        transform.GetChild(1).localScale = looking;
    }

    public virtual void commandMove(Vector3 _pos)
    {
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

        if (isClicked && isPlayerDragToMove)
        {
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

            if (hit != null)
            {
                foreach (RaycastHit2D target in hit)
                {
                    if (target.collider.CompareTag("player"))
                    {
                        HeroObject = target.collider.GetComponentInParent<PlayerScript>();
                        isCommandedMove = false;
                        isCommandedAttack = true;
                        AttackRangedOn = true;
                        playerAttack();
                    }
                }

            }
        }
        if (IndicatorPlayer.transform.GetChild(1).transform.gameObject.activeSelf)
        {
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
                if (isCommandedAttack && collision.CompareTag("enemy"))
                {
                    EnemyObject = collision.transform.parent.gameObject;

                }
                break;

        }
    }

    //public override void AttackTriggerStay(HitBoxScript.enumHitType _hitType, Collider2D collision)
    //{
    //    switch (_hitType)
    //    {
    //        case HitBoxScript.enumHitType.EnemyCheck:
    //            if (isCommandedAttack && collision.CompareTag("enemy"))
    //            {
    //                EnemyObject = collision.gameObject;
    //                AttackRangedOn = true;

    //            }
    //            break;

    //    }
    //}


    public override void AttackTriggerEnter(HitBoxScript.enumHitType _hitType, Collider2D collision)
    {
        switch (_hitType)
        {
            case HitBoxScript.enumHitType.EnemyCheck:
                if (isCommandedAttack && collision.CompareTag("enemy"))
                {
                    EnemyObject = collision.transform.parent.gameObject;
                    AttackRangedOn = true;
                }

                if (collision.CompareTag("enemy"))
                {
                    bool existCheck = false; ;
                    for (int inum = 0; inum < HealColliderEnemies.Count; inum++)
                    {
                        if (HealColliderEnemies[inum].GetComponent<EnemyScript>().getEnemyID() == collision.transform.parent.gameObject.GetComponent<EnemyScript>().getEnemyID())
                        {
                            existCheck = true;
                            break;
                        }
                    }

                    if (!existCheck)
                    {
                        HealColliderEnemies.Add(collision.transform.parent.gameObject.GetComponent<EnemyScript>());
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
                    for (int inum = 0; inum < HealColliderEnemies.Count; inum++)
                    {
                        if (HealColliderEnemies[inum].GetComponent<EnemyScript>().getEnemyID() == collision.transform.parent.gameObject.GetComponent<EnemyScript>().getEnemyID())
                        {
                            HealColliderEnemies.RemoveAt(inum);
                            break;
                        }
                    }
                }
                break;

        }

    }

    public override void ActiveSpellActivate(int _num)
    {
        if (HeroObject == null) return;

            isSpellPlaying = true;
        ActivatedSpell = _num;

        if (_num == 0) {
            Spell1ChargingTime = 0.5f;
            Spell1current = Spell2duration;
        }

        if (_num == 1)
        {
            Spell2ChargingTime = 0.5f;
            Spell2current = Spell2duration;
        }

        if (_num == 2)
        {
            Spell2ChargingTime = 0.5f;
            Spell2current = Spell2duration;
        }

        if (_num == 3)
        {
            Spell4ChargingTime = 0.8f;
            Spell4current = Spell4duration;
        }

        if (HeroObject != null) {
            Vector3 looking = HeroObject.transform.position.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
            transform.GetChild(0).localScale = looking;
            transform.GetChild(1).localScale = looking;
        }

    }

    public override void NonTargettingSpellActivate(int _num, Vector3 _targetPos = new Vector3())
    {
        if (HeroObject == null) return;
        // Instantiate Shield Attack and move.
        isSpellPlaying = true;
        ActivatedSpell = _num;
        // float angle = Vector2.Angle(transform.position, _targetPos);
        float angle = Quaternion.FromToRotation(Vector3.up, _targetPos - transform.position).eulerAngles.z;

        Vector3 looking = _targetPos.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        transform.GetChild(0).localScale = looking;
        transform.GetChild(1).localScale = looking;
    }

    public override void TargettingSpellActivate(int _num, GameObject _targetObject = null)
    {
        //isSpellPlaying = true;
        //ActivatedSpell = _num;
        //EnemyObject = _targetObject;
    }

    public void Spell1()
    {
        // Great Heal, Resotre 50% MaxHP

        // animation

        if (Spell1ChargingTime > 0f) return;
        isCommandedMove = false;

        // Buff : damage reduce & aggro max

        ActivatedSpell = -1;
        isSpellPlaying = false;

        AttackCollider.transform.position = HeroObject.transform.position;
        float hpGauge = HeroObject.GetComponent<PlayerScript>().getPlayerMaxHp();
        HeroObject.GetComponent<PlayerScript>().healHp(hpGauge / 2);
        isCommandedAttack = true;

        Vector3 looking = HeroObject.transform.position.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        transform.GetChild(0).localScale = looking;
        transform.GetChild(1).localScale = looking;

    }

    public void Spell2()
    {
        // Damage Buff, Increase Atk Power for 30%

        // animation

        if (Spell2ChargingTime > 0f) return;
        isCommandedMove = false;

        isSpell2Activated = true;
        Spell2current = Spell2duration;
        ActivatedSpell = -1;
        isSpellPlaying = false;

        AttackCollider.transform.position = HeroObject.transform.position;
        TargetOriginAtk = HeroObject.GetComponent<PlayerScript>().getPlayerAtk();
        HeroObject.GetComponent<PlayerScript>().setPlayerAtk(TargetOriginAtk * 1.3f);
        isCommandedAttack = true;
        Spell2Target = HeroObject;

        Vector3 looking = HeroObject.transform.position.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        transform.GetChild(0).localScale = looking;
        transform.GetChild(1).localScale = looking;
    }

    public void Spell3()
    {
        // Summon 3 Cows
        if (Spell3ChargingTime > 0f) return;
        isCommandedMove = false;
        isSpellPlaying = false;

        StartCoroutine(SummonCow());
        isCommandedAttack = true;

        if (HeroObject != null) {
            Vector3 looking = HeroObject.transform.position.x > transform.position.x ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
            transform.GetChild(0).localScale = looking;
            transform.GetChild(1).localScale = looking;
        }


    }

    IEnumerator SummonCow()
    {
        Vector3 margin = AttackCollider.transform.position;
        margin.y -= 2f;
        margin.z = -2f;

        RangedCow objCow = Instantiate(minnerCow, margin, AttackCollider.transform.rotation);
        objCow.GetComponent<RangedCow>().SetPlayerRangedCow(playerID, Spell3Atk, Spell3Aggro * 0.2f, HealColliderEnemies);


        yield return new WaitForSeconds(0.8f);

        objCow= Instantiate(minnerCow, margin, AttackCollider.transform.rotation);
        objCow.GetComponent<RangedCow>().SetPlayerRangedCow(playerID, Spell3Atk, Spell3Aggro * 0.5f, HealColliderEnemies);

        yield return new WaitForSeconds(0.8f);

        objCow= Instantiate(blackCow, margin, AttackCollider.transform.rotation);
        objCow.GetComponent<RangedCow>().SetPlayerRangedCow(playerID, Spell3Atk * 2, Spell3Aggro, HealColliderEnemies);
    }

    public void Spell4()
    {

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
                _spellType = SpellType.SpellActive;
                _IconsImage = SpellIcon[_val];
                return new SpellInfo() { spellSlotID = _spellSlotID, spellType = _spellType, IconImage = _IconsImage, cooltime = _cooltime };
            case 1:
                _spellType = SpellType.SpellActive;
                _IconsImage = SpellIcon[_val];
                return new SpellInfo() { spellSlotID = _spellSlotID, spellType = _spellType, IconImage = _IconsImage, cooltime = _cooltime };
            case 2:
                _spellType = SpellType.SpellActive;
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
