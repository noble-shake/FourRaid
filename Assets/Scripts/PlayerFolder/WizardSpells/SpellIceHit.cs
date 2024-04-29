using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellIceHit : MonoBehaviour
{
    [Header("Collider Inspector")]
    [SerializeField] GameObject Owned;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            Owned.GetComponent<SpellIce>().AttackTriggerEnter(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            Owned.GetComponent<SpellIce>().AttackTriggerStay(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            Owned.GetComponent<SpellIce>().AttackTriggerExit(collision);
        }
    }

}
