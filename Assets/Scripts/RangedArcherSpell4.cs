using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedArcherSpell4 : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;

    [SerializeField] int playerID;
    [SerializeField] float playerAtkAggro;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy")) {
            EnemyScript enemySc = collision.GetComponent<EnemyScript>();
            enemySc.hitHp(playerID, damage, playerAtkAggro);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        // transform.position += transform.up * Time.deltaTime * speed;
        transform.position += -transform.right * Time.deltaTime * speed;
    }

    public void SetPlayerRangedAttack(int _playerID, float _speed, float _damege, float _aggro)
    {
        playerID = _playerID;
        speed = _speed;
        damage = _damege;
        playerAtkAggro = _aggro;
    }
}
