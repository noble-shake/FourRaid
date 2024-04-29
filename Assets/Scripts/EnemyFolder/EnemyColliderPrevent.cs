using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliderPrevent : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemyPreventWall")) {
            collision.transform.parent.transform.position += new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("enemyPreventWall"))
        {
            // collision.transform.parent.GetComponent<EnemyScript>().notpreventCollideEachOther();
        }
    }
}
