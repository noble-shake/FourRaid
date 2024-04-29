using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDragon : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] int playerID;
    [SerializeField] float damage;
    [SerializeField] float aggro;
    [SerializeField] float currentTime;
    [SerializeField] float durationTime = 5f;
    [SerializeField] float damageTime = 2.2f;
    [SerializeField] float damageStopTime = 3.8f;
    [SerializeField] float damageTimeColltime = 0f;
    [SerializeField] bool SpellActive;
    // [SerializeField] float angle;


    [Header("Inspector")]
    [SerializeField] Animator anim;
    [SerializeField] GameObject Ground;
    [SerializeField] Vector2 originPos;
    [SerializeField] Vector2 TargetPos;
    [SerializeField] List<EnemyScript> Targets;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (SpellActive) {
            
            currentTime += Time.fixedUnscaledDeltaTime;

            if (currentTime > damageTime && currentTime < damageStopTime)
            {
                damageTimeColltime += Time.fixedUnscaledDeltaTime;
                if (damageTimeColltime > 0.5f) {
                    damageTimeColltime = 0f;
                    hitEnemy();
                }

            }
            if (currentTime > durationTime) {
                Time.timeScale = 1f;
                // gameObject.SetActive(false);
                currentTime = 0f;
                SpellActive = false;
            }
        }
    }

    public void hitEnemy()
    {
        int enemies = Targets.Count - 1;

        while (true)
        {
            if (enemies < 0) break;
            Targets[enemies--].hitHp(playerID, damage, aggro);
        }
    }

    public void SetDragonBreath(int _playerID, float _damage, float _aggro) {
        playerID = _playerID;
        damage = _damage;
        aggro = _aggro;
        SpellActive = true;
        anim.SetTrigger("Breath");
        Time.timeScale = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("enemy")) {
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


    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
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


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
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
}
