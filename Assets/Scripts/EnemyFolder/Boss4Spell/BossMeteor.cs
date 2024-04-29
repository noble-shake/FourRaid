using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMeteor : MonoBehaviour
{
    [Header("Inspetor")]
    [SerializeField] float damage;
    [SerializeField] List<PlayerScript> Targets;
    [SerializeField] bool isHit;

    [SerializeField] GameObject MeteorObj;
    [SerializeField] Image img;
    [SerializeField] Vector2 originPos;
    [SerializeField] float dropDistance = 5f;
    [SerializeField] float currentTime;
    [SerializeField] float durationTime;
    [SerializeField] bool isSpellActive;
    [SerializeField] bool CircleActive;
    [SerializeField] GameObject ShadowSpr;
    [SerializeField] GameObject CircleSpr;
    [SerializeField] bool isSummonEnd;
    

    // Start is called before the first frame update
    void Start()
    {
        img.fillAmount = 0f;
        originPos = MeteorObj.transform.position;
        Targets = new List<PlayerScript>();
    }

    public void AttackTriggerStay(Collider2D collision)
    {
        if (collision.CompareTag("player"))
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
    }

    public void AttackTriggerEnter(Collider2D collision)
    {
        if (isHit == true) return;

        if (collision.CompareTag("enemy"))
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
    }

    public void AttackTriggerExit(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
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

    public void SpellActivate() {
        img.fillAmount += (Time.fixedDeltaTime * 80f / 60f);
        ShadowSpr.transform.localScale += new Vector3(Time.fixedDeltaTime, 0f, 0f);
        if (ShadowSpr.transform.localScale.x > 0.6f)
        {
            ShadowSpr.transform.localScale = new Vector3(0.6f, 1f, 1f);
        }

        if (img.fillAmount < 1 && !isSummonEnd)
        {
            MeteorObj.transform.position -= MeteorObj.transform.up * Time.fixedDeltaTime * 2f;
            isSummonEnd = true;
        }
        else
        {
            MeteorObj.transform.position -= MeteorObj.transform.up * Time.fixedDeltaTime * 6f;
        }


        //if (isSummonEnd && Vector2.Distance(originPos, transform.position) > 5) { 

        //}



        //1.6 secons
        if (Vector2.Distance(originPos, MeteorObj.transform.position) > 5.5f)
        {

            img.gameObject.SetActive(false);
            ShadowSpr.gameObject.SetActive(false);
            CircleSpr.transform.localScale -= new Vector3(Time.fixedDeltaTime, Time.fixedDeltaTime * 0.3f, 0f);

            if (!isHit)
            {
                isHit = true;
                int enemies = Targets.Count - 1;

                while (true)
                {
                    if (enemies < 0) break;

                    Targets[enemies--].hitHp(damage);

                }
            }

            if (CircleSpr.transform.localScale.x < 0f) {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isSpellActive && !CircleActive) {
            currentTime += Time.fixedDeltaTime;
            CircleSpr.transform.localScale += new Vector3(Time.fixedDeltaTime, Time.fixedDeltaTime * 0.3f, 0f);
            if (CircleSpr.transform.localScale.x > 1f) {
                CircleSpr.transform.localScale = new Vector3(1f, 0.3f, 0f);
            }
            if (currentTime > durationTime) {
                CircleActive = true;
            }
        }

        if (CircleActive) {
            SpellActivate();
        }


    }

    public void setOriginPos() {
        originPos = transform.position;
    }

    public void SetMeteor(float _damage, float _duration) {
        // playerID, Spell1Atk, Spell1Aggro, Spell1Duration, Spell1Targets
        damage = _damage;
        durationTime = _duration;
        isSpellActive = true;
    }
}
