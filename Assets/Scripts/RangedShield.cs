using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedShield : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;

    [SerializeField] int playerID;
    [SerializeField] float playerAtkAggro;

    public void AttackTriggerEnter(Collider2D collision)
    {
        EnemyScript enemySc = collision.GetComponent<EnemyScript>();
        enemySc.hitHp(playerID, damage, playerAtkAggro);

        Destroy(gameObject);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        //transform.position = transform.position + Vector3.up * Time.deltaTime * speed; 
        //transform.position = transform.position + transform.up * Time.deltaTime * speed;
        //transform.rotation = Quaternion.Euler(new Vector3(30f, 10f, Time.deltaTime * speed * 2));
        transform.position += transform.up * Time.deltaTime * speed;
    }

    public void SetEnemyAttack(float _speed, float _damege)
    {
        speed = _speed;
        damage = _damege;
    }

    public void SetPlayerRangedAttack(int _playerID, float _speed, float _damege, float _aggro)
    {
        playerID = _playerID;
        speed = _speed;
        damage = _damege;
        playerAtkAggro = _aggro;
    }
}
