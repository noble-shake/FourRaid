using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] List<GameObject> Heroes;


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
        Transform[] rangeData = spellUI.transform.GetComponentsInChildren<Transform>(true);
        spells.AddRange(rangeData);
        spells.RemoveAt(0);
        spellDeActive();
    }

    void Update()
    {
        UnitSelect();
    }

    public List<GameObject> getHeroesObjects() {
        return Heroes;
    }

    public void commandAttack() { }

    public void commandSpell(int _value) { }

    private void UnitSelect()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        // RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray);

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            bool isPlayerExist = false;

            foreach (RaycastHit target in hits)
            {
                if (target.collider.CompareTag("player"))
                {
                    isPlayerExist = true;
                    spellActive();
                    if (selectedPlayer != null && selectedPlayer != target.collider.gameObject)
                    {
                        selectedPlayer.GetComponent<PlayerScript>().playerSelectCancel();
                        selectedPlayer = null;
                        isPlayerCheck = false;
                    }
                    selectedPlayer = target.collider.gameObject;
                    selectedPlayer.GetComponent<PlayerScript>().playerSelect();
                    isPlayerCheck = true;

                    break;
                }
            }

            // Unity Select Off
            if (!isPlayerExist && isPlayerCheck) 
            {
                foreach (RaycastHit target in hits)
                {
                    if (target.collider.CompareTag("ground"))
                    {
                        spellDeActive();
                        if (selectedPlayer != null)
                        {
                            selectedPlayer.GetComponent<PlayerScript>().playerSelectCancel();
                            selectedPlayer = null;
                            isPlayerCheck = false;
                            break;
                        }
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
