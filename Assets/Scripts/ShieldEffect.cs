using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    [SerializeField] RangedShield parent;
    [SerializeField] bool isHit;
    float ranguler = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit == true) return;

        if (collision.CompareTag("enemy"))
        {
            isHit = true;
            parent.AttackTriggerEnter(collision);
        }
    }

    private void Update()
    {
        ranguler = ranguler + 33f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, ranguler));

        if (ranguler > 360f) { 
            ranguler = ranguler / 360f;
        }

    }

}
