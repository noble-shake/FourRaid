using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Todo
    /*
     * Mouse Click
     * Mouse Click Check
     * Mouse Hold
     * Mouse Command
     * 
     * Player move
     * Player Attack
     * player Skill UI Active
     * 
     * skill list up
     * skill cooldown set
     * skill cooldown
     */
    [Header("Setup")]
    public static PlayerManager instance;

    [Header("Manager State")]
    [SerializeField] bool isPlayerCheck;
    [SerializeField] RaycastHit2D hit;

    [Header("Player Control")]
    [SerializeField] int CurrentPlayerID;


    [Header("External")]
    [SerializeField] Camera cam;


    // UNITY CYCLE

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        { 
            Destroy(gameObject);
        }


    }
    void Start()
    {

    }

    void Update()
    {
        ManagerClick();
    }

    // Main Script

    private void ManagerClick()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        // RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);
        RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);

        if (Input.GetMouseButtonDown(0)) {
            foreach (RaycastHit2D target in hit) {
                if (target.collider.gameObject.CompareTag("")) { 
                    
                }
            }
            //if (hit.collider != null)
            //{
            //    Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
            //    Debug.Log(hit.collider.name);

            //}
        }


    }

}
