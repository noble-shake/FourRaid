using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBossMeteor : MonoBehaviour
{
    public GameObject ownedObject;

    void Start()
    {
        OwnSwithching();
    }

    public void OwnSwithching() {


    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            ownedObject.GetComponent<BossMeteor>().AttackTriggerStay(collision); ;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            ownedObject.GetComponent<BossMeteor>().AttackTriggerEnter(collision); ;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            ownedObject.GetComponent<BossMeteor>().AttackTriggerExit(collision); ;
        }
    }
}
