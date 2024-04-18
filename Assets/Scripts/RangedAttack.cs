using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] bool isEnemyAttack;
    [SerializeField] bool isHit;

    [SerializeField] int playerID;
    [SerializeField] float playerAtkAggro;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit == true) return;

        if (isEnemyAttack == false && collision.CompareTag("enemy"))
        {
            isHit = true;

            EnemyScript enemySc = collision.GetComponent<EnemyScript>();
            enemySc.hitHp(playerID, damage, playerAtkAggro);

            Destroy(gameObject);
        }
        else if (isEnemyAttack == true && collision.CompareTag("player"))
        {
            isHit = true;

            PlayerScript playerSc = collision.GetComponent<PlayerScript>();
            playerSc.hitHp(damage);

            Destroy(gameObject);
        }
    }

    //렌더링 되고있다가 더이상 렌더링 되지 않게 되었을때
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        //transform.position = transform.position + Vector3.up * Time.deltaTime * speed; 
        //transform.position = transform.position + transform.up * Time.deltaTime * speed;
        transform.position += -transform.right * Time.deltaTime * speed;
    }

    public void SetEnemyAttack(float _speed, float _damege)
    {
        speed = _speed;
        damage = _damege;
        isEnemyAttack = true;
    }

    public void SetPlayerRangedAttack(int _playerID, float _speed, float _damege, float _aggro)
    {
        playerID = _playerID;
        speed = _speed;
        damage = _damege;
        playerAtkAggro = _aggro;
        isEnemyAttack = false;
    }
}
