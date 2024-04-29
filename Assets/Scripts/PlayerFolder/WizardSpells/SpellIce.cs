using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellIce : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] int playerID;
    [SerializeField] float damage;
    [SerializeField] float aggro;
    [SerializeField] float currentTime;
    [SerializeField] float durationTime;
    [SerializeField] float spreadTime;
    [SerializeField] bool SpellActive;
    // [SerializeField] float angle;


    [Header("Inspector")]
    [SerializeField] GameObject Ground;
    [SerializeField] Vector2 originPos;
    [SerializeField] Vector2 TargetPos;
    [SerializeField] List<EnemyScript> Targets;
    [SerializeField] GameObject IceEffect;
    [SerializeField] Vector3 spreadVec = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        Targets = new List<EnemyScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpellActive)
        {
            currentTime += Time.fixedDeltaTime;
            if (currentTime < spreadTime)
            {
                spreadVec = new Vector3(currentTime * 2 / spreadTime, currentTime * 2 / spreadTime, 0f);
                IceEffect.transform.localScale = spreadVec;
            }

            if(currentTime > durationTime)
            {
                resetDebuff();
                Destroy(gameObject);
            }
        }
    }

    public void hitEnemy()
    {
        int enemies = Targets.Count - 1;

        while (true)
        {
            if (enemies < 0) break;
            Targets[enemies].setEnemySpeed(0.5f);
            Targets[enemies--].hitHp(playerID, damage, aggro);
        }
    }

    public void resetDebuff() {
        int enemies = Targets.Count - 1;

        while (true)
        {
            if (enemies < 0) break;

            float originSpeed = Targets[enemies].getEnemyOriginSpeed();
            Targets[enemies--].setEnemySpeed(originSpeed);
        }
    }

    public void SetFrozen(int _playerID, float _damage, float _aggro, float _duraction, float _spread)
    {
        playerID = _playerID;
        damage = _damage;
        aggro = _aggro;
        durationTime = _duraction;
        spreadTime = _spread;


        SpellActive = true;

        InvokeRepeating("hitEnemy", 0.0f, 0.11f);
    }

    public void AttackTriggerEnter(Collider2D collision)
    {

        bool existCheck = false; ;
        for (int inum = 0; inum < Targets.Count; inum++)
        {
            if (Targets[inum].GetComponent<EnemyScript>().getEnemyID() == collision.transform.parent.gameObject.GetComponent<EnemyScript>().getEnemyID())
            {
                existCheck = true;
                break;
            }
        }

        if (!existCheck)
        {
            Targets.Add(collision.transform.parent.gameObject.GetComponent<EnemyScript>());
        }

    }
    public void AttackTriggerStay(Collider2D collision)
    {
        bool existCheck = false; ;
        for (int inum = 0; inum < Targets.Count; inum++)
        {
            if (Targets[inum].GetComponent<EnemyScript>().getEnemyID() == collision.transform.parent.gameObject.GetComponent<EnemyScript>().getEnemyID())
            {
                existCheck = true;
                break;
            }
        }

        if (!existCheck)
        {
            Targets.Add(collision.transform.parent.gameObject.GetComponent<EnemyScript>());
        }
    }
    public void AttackTriggerExit(Collider2D collision)
    {
        for (int inum = 0; inum < Targets.Count; inum++)
        {
            if (Targets[inum].GetComponent<EnemyScript>().getEnemyID() == collision.transform.parent.gameObject.GetComponent<EnemyScript>().getEnemyID())
            {
                Targets.RemoveAt(inum);
                break;
            }
        }
    }
}
