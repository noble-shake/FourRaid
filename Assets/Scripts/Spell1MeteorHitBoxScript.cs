using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell1MeteorHitBoxScript : MonoBehaviour
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
        if (collision.CompareTag("enemy"))
        {
            ownedObject.GetComponent<SpellMeteor>().AttackTriggerStay(collision); ;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            ownedObject.GetComponent<SpellMeteor>().AttackTriggerEnter(collision); ;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            ownedObject.GetComponent<SpellMeteor>().AttackTriggerExit(collision); ;
        }
    }
}
