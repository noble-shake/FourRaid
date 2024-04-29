using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLightning : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] int playerID;
    [SerializeField] float damage;
    [SerializeField] float aggro;
    [SerializeField] float currentTime;
    [SerializeField] float durationTime;
    [SerializeField] bool SpellActive;
    // [SerializeField] float angle;


    [Header("Inspector")]
    [SerializeField] GameObject Ground;
    [SerializeField] Vector2 originPos;
    [SerializeField] Vector2 TargetPos;
    [SerializeField] GameObject LaserEffect;
    [SerializeField] List<EnemyScript> Targets;


    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        Targets = new List<EnemyScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpellActive) {
            currentTime += Time.fixedDeltaTime;
            if (currentTime < durationTime)
            {
                
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void hitEnemy() {
        int enemies = Targets.Count - 1;

        while (true)
        {
            if (enemies < 0) break;

            Targets[enemies--].hitHp(playerID, damage, aggro);
        } 
    }

    public void SetLightning(int _playerID, float _damage, float _aggro, float _duraction, float _angle) {
        playerID = _playerID;
        damage = _damage;
        aggro = _aggro;
        durationTime = _duraction;


        LaserEffect.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _angle));
        Vector2 distVec = new Vector2(50f, 1f);
        LaserEffect.GetComponent<SpriteRenderer>().size = distVec;
        LaserEffect.GetComponent<BoxCollider2D>().size = distVec;

        distVec.x /= 2f;
        distVec.y = 0f;
        LaserEffect.GetComponent<BoxCollider2D>().offset = distVec;
        SpellActive = true;

        InvokeRepeating("hitEnemy", 0.0f, 0.2f);
    }

    public void AttackTriggerEnter(Collider2D collision) {

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
