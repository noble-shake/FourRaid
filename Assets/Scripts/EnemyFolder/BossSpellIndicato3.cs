using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpellIndicator3 : MonoBehaviour
{
    [SerializeField] Boss3 parent;
    [SerializeField] bool isHit;
    float ranguler = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("player"))
        {
            parent.AttackTriggerEnter(HitBoxScript.enumHitType.PlayerCheck, collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            parent.AttackTriggerExit(HitBoxScript.enumHitType.PlayerCheck, collision);
        }
    }

    private void Update()
    {
    }

}
