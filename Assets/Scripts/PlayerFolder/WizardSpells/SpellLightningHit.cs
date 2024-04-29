using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLightningHit : MonoBehaviour
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
        if (collision.CompareTag("enemy")) {
            Owned.GetComponent<SpellLightning>().AttackTriggerEnter(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            Owned.GetComponent<SpellLightning>().AttackTriggerStay(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            Owned.GetComponent<SpellLightning>().AttackTriggerExit(collision);
        }
    }

}
