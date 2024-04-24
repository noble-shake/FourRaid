using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCow : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] bool isHit;

    [SerializeField] int playerID;
    [SerializeField] float playerAtkAggro;

    [SerializeField] bool StompOn;
    [SerializeField] float currentTime = 0f;
    [SerializeField] float StompTime;
    [SerializeField] float AwayTime;

    [SerializeField] List<EnemyScript> Enemies;

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        // Enemies = new List<EnemyScript>();
    }

    void Update()
    {

        if (StompOn) {
            currentTime += Time.deltaTime;
        }

        if (currentTime > StompTime) {
            if (!isHit) {
                isHit = true;
                int enemies = Enemies.Count - 1;

                while (true)
                {
                    if (enemies < 0) break;

                    Enemies[enemies--].hitHp(playerID, damage, playerAtkAggro);

                }
            }
            

        }
        if (currentTime > AwayTime) { 
            Destroy(gameObject);
        }
    }

    public void SetPlayerRangedCow(int _playerID, float _damege, float _aggro, List<EnemyScript> _enemies)
    {
        playerID = _playerID;
        damage = _damege;
        playerAtkAggro = _aggro;
        Enemies = _enemies;
        StompOn = true;
    }
}
