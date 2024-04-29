using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBallEffect : MonoBehaviour
{
    [SerializeField] RangedIceBall parent;
    float ranguler = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("player"))
        {
            parent.AttackTriggerEnter(collision);
        }
    }

    private void Update()
    {


    }

}
