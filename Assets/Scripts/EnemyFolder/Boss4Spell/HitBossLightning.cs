using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBossLightning : MonoBehaviour
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
        if (collision.CompareTag("player")) {
            Owned.GetComponent<BossLightning>().AttackTriggerEnter(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            Owned.GetComponent<BossLightning>().AttackTriggerStay(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            Owned.GetComponent<BossLightning>().AttackTriggerExit(collision);
        }
    }

}
