using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] bool isPlayerOnMouse;
    [SerializeField] bool isPlayerDragToMove;
    [SerializeField] bool isPlayerOffMouse;

    [Header("Player Control")]
    // [SerializeField] RaycastHit2D hit;
    [SerializeField] int CurrentPlayerID;


    [Header("External")]
    [SerializeField] Camera cam;
    public float distance;
    [SerializeField] GameObject selectedPlayer;
    [SerializeField] GameObject spellUI;
    [SerializeField] List<Transform> spells;


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
        spells = new List<Transform>();
        // Transform[] rangeData = objInventory.transform.GetComponentsInChildren<Transform>();
        Transform[] rangeData = spellUI.transform.GetComponentsInChildren<Transform>(true);
        spells.AddRange(rangeData);
        spells.RemoveAt(0);
        spellDeActive();
    }

    void Update()
    {
        
        UnitSelect();
    }

    public void commandAtackk() { }

    public void commandSpell(int _value) { }

    private void UnitSelect()
    {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        // RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray);



        if (Input.GetMouseButtonDown(0))
        {

            bool isPlayerExist = false;

            if (hit.collider.gameObject.CompareTag("player"))
            {
                isPlayerExist = true;
                spellActive();
                Debug.Log("Hi");
                if (selectedPlayer != null && selectedPlayer != hit.collider.gameObject)
                {
                    selectedPlayer.GetComponent<PlayerScript>().playerSelectCancel();
                    selectedPlayer = null;
                    isPlayerCheck = false;
                }
                selectedPlayer = hit.collider.gameObject;
                selectedPlayer.GetComponent<PlayerScript>().playerSelect();
                isPlayerCheck = true;
            }
            else if (!isPlayerExist && isPlayerCheck)
            {
                if (hit.collider.gameObject.CompareTag("ground") )
                {
                    spellDeActive();
                    Debug.Log("earth");
                    if (selectedPlayer != null)
                    {
                        selectedPlayer.GetComponent<PlayerScript>().playerSelectCancel();
                        selectedPlayer = null;
                        isPlayerCheck = false;
                    }
                }
            }
        }
    }

    private void spellDeActive() 
    {
        foreach (Transform target in spells) 
        { 
            target.gameObject.SetActive(false);
        }
    }

    private void spellActive()
    {
        foreach (Transform target in spells)
        {
            target.gameObject.SetActive(true);
        }
    }
}
