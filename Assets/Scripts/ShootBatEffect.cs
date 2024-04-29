using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBatEffect : MonoBehaviour
{
    [SerializeField] RangedShootBat parent;
    [SerializeField] bool isHit;
    float ranguler = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit == true) return;

        if (collision.CompareTag("player"))
        {
            isHit = true;
            parent.AttackTriggerEnter(collision);
        }
    }

    private void Update()
    {
        ranguler = ranguler + 11f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, ranguler));

        if (ranguler > 360f) { 
            ranguler = ranguler / 360f;
        }

    }

}
