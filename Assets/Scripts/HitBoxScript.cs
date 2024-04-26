using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxScript : MonoBehaviour
{


    public enum enumHitType
    {
        PlayerCheck,
        EnemyCheck,
        BossCheck,
    }

    [SerializeField] private enumHitType hitType;
    public GameObject ownedObject;

    void Start()
    {
        OwnSwithching();
    }

    public void OwnSwithching() {


    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (hitType == enumHitType.PlayerCheck)
        {
            ownedObject.GetComponent<EnemyScript>().AttackTriggerStay(hitType, collision); ;
        }
        else if (hitType == enumHitType.EnemyCheck)
        {
            ownedObject.GetComponent<PlayerScript>().AttackTriggerStay(hitType, collision); ;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitType == enumHitType.PlayerCheck)
        {
            ownedObject.GetComponent<EnemyScript>().AttackTriggerEnter(hitType, collision);
        }
        else if (hitType == enumHitType.EnemyCheck)
        {
            ownedObject.GetComponent<PlayerScript>().AttackTriggerEnter(hitType, collision); ;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (hitType == enumHitType.PlayerCheck)
        {
            ownedObject.GetComponent<EnemyScript>().AttackTriggerExit(hitType, collision); ;
        }
        else if (hitType == enumHitType.EnemyCheck)
        {
            ownedObject.GetComponent<PlayerScript>().AttackTriggerExit(hitType, collision); ;
        }
    }
}
