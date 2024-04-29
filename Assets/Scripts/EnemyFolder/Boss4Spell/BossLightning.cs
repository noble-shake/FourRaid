using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLightning : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] float damage;
    [SerializeField] float currentTime;
    [SerializeField] float durationTime;
    [SerializeField] bool SpellActive;
    [SerializeField] float ranguler;
    // [SerializeField] float angle;


    [Header("Inspector")]
    [SerializeField] GameObject Ground;
    [SerializeField] Vector2 originPos;
    [SerializeField] Vector2 TargetPos;
    [SerializeField] GameObject LaserEffect1;
    [SerializeField] GameObject LaserEffect2;
    [SerializeField] List<PlayerScript> Targets;


    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        Targets = new List<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        ranguler = ranguler + 6f;

        if (ranguler > 360f)
        {
            ranguler = ranguler / 360f;
        }

        if (SpellActive) {
            currentTime += Time.fixedDeltaTime;
            if (currentTime > durationTime)
            {
                LaserEffect1.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, ranguler));
                LaserEffect2.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, ranguler + 90f));
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void hitPlayer() {
        int enemies = Targets.Count - 1;

        while (true)
        {
            if (enemies < 0) break;

            Targets[enemies--].hitHp(damage);
        } 
    }

    public void SetLightning(float _damage, float _duraction) {
        damage = _damage;
        durationTime = _duraction;


        // LaserEffect1.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _angle));
        Vector2 distVec = new Vector2(55f, 1f);
        LaserEffect1.GetComponent<SpriteRenderer>().size = distVec;
        LaserEffect1.GetComponent<BoxCollider2D>().size = distVec;
        LaserEffect1.GetComponent<BoxCollider2D>().offset = distVec;

        LaserEffect2.GetComponent<SpriteRenderer>().size = distVec;
        LaserEffect2.GetComponent<BoxCollider2D>().size = distVec;
        LaserEffect2.GetComponent<BoxCollider2D>().offset = distVec;
        SpellActive = true;

        InvokeRepeating("hitPlayer", 0.0f, 0.33f);
    }

    public void AttackTriggerEnter(Collider2D collision) {

        bool existCheck = false; ;
        for (int inum = 0; inum < Targets.Count; inum++)
        {
            if (Targets[inum].GetComponent<PlayerScript>().getPlayerID() == collision.transform.parent.gameObject.GetComponent<PlayerScript>().getPlayerID())
            {
                existCheck = true;
                break;
            }
        }

        if (!existCheck)
        {
            Targets.Add(collision.transform.parent.gameObject.GetComponent<PlayerScript>());
        }

    }
    public void AttackTriggerStay(Collider2D collision)
    {
        bool existCheck = false; ;
        for (int inum = 0; inum < Targets.Count; inum++)
        {
            if (Targets[inum].GetComponent<PlayerScript>().getPlayerID() == collision.transform.parent.gameObject.GetComponent<PlayerScript>().getPlayerID())
            {
                existCheck = true;
                break;
            }
        }

        if (!existCheck)
        {
            Targets.Add(collision.transform.parent.gameObject.GetComponent<PlayerScript>());
        }
    }
    public void AttackTriggerExit(Collider2D collision)
    {
        for (int inum = 0; inum < Targets.Count; inum++)
        {
            if (Targets[inum].GetComponent<PlayerScript>().getPlayerID() == collision.transform.parent.gameObject.GetComponent<PlayerScript>().getPlayerID())
            {
                Targets.RemoveAt(inum);
                break;
            }
        }
    }


}
