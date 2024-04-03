using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    public enum enumHitType
    {
        EnemyCheck,
        BossCheck,
    }

    [SerializeField] private enumHitType hitType;
    PlayerWarrior player;

    void Start()
    {
        player = GetComponentInParent<PlayerWarrior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.AttackTriggerEnter(hitType, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.AttackTriggerExit(hitType, collision);
    }
}
