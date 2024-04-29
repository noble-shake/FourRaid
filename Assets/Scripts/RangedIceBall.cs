using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedIceBall: MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float speed;

    public void AttackTriggerEnter(Collider2D collision)
    {
        PlayerScript playerSc = collision.transform.parent.gameObject.GetComponent<PlayerScript>();

        playerSc.hitHp(damage);

        Destroy(gameObject);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;
    }

    public void SetEnemyAttack(float _speed, float _damege)
    {
        speed = _speed;
        damage = _damege;
    }
}
