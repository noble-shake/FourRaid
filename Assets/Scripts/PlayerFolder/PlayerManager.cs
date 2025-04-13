using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public enum SpellType
{
    SpellTargetting,
    SpellNonTargetting,
    SpellActive,
    SpellNonActive,
};

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
    [SerializeField] bool isSpellActivated;

    [Header("Player Control")]
    // [SerializeField] RaycastHit2D hit;
    [SerializeField] int CurrentPlayerID;


    [Header("External")]
    [SerializeField] Camera cam;
    public float distance;
    [SerializeField] GameObject selectedPlayer;
    [SerializeField] GameObject spellUI;
    [SerializeField] List<SpellScript> spells;
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
        spells = new List<SpellScript>();
        SpellScript[] rangeData = spellUI.transform.GetComponentsInChildren<SpellScript>(true);
        spells.AddRange(rangeData);
        //spells.RemoveAt(0);
        spellUIDeActive();
    }

    void Update()
    {
        UnitSelect();
    }

    public List<GameObject> getHeroesObjects() {
        return Heroes;
    }

    public bool getHeroAlive(int _num) {
        return Heroes[_num].GetComponent<PlayerScript>().getPlayerAlive();
    }

    public void commandAttack() { }

    public void commandSpell(int _value) { }

    private void UnitSelect()
    {
        if (!GameManager.instance.Playable()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit[] hits = Physics.RaycastAll(ray);
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray);



        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            bool isPlayerExist = false;

            foreach (RaycastHit2D target in hits)
            {
                if (target.collider.CompareTag("player"))
                {
                    isPlayerExist = true;
                    
                    if (selectedPlayer != null && selectedPlayer != target.collider.transform.parent.gameObject)
                    {
                        selectedPlayer.GetComponent<PlayerScript>().playerSelectCancel();
                        selectedPlayer = null;
                        isPlayerCheck = false;
                    }
                   
                    selectedPlayer = target.collider.transform.parent.gameObject;
                    selectedPlayer.GetComponent<PlayerScript>().playerSelect();
                    isPlayerCheck = true;
                    spellUIActive();

                    break;
                }
            }

            // Unity Select Off
            if (!isPlayerExist && isPlayerCheck && !isSpellActivated) 
            {
                foreach (RaycastHit2D target in hits)
                {
                    if (target.collider.CompareTag("ground"))
                    {
                        spellUIDeActive();
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


    private void spellUIDeActive() 
    {
        foreach (SpellScript target in spells) 
        { 
            target.gameObject.SetActive(false);
        }
    }

    private void spellUIActive()
    {
        Sprite[] Icons = selectedPlayer.GetComponent<PlayerScript>().getSpellIcon();

        for (int inum = 0; inum < spells.Count; inum++) {
            spells[inum].gameObject.SetActive(true);
            spells[inum].gameObject.GetComponent<SpellScript>().setIconSprite(Icons[inum]);
            spells[inum].gameObject.GetComponent<SpellScript>().setPlayerPrivilage(selectedPlayer);
            spells[inum].gameObject.GetComponent<SpellScript>().SpellInit(selectedPlayer.GetComponent<PlayerScript>().getSpellInfo(inum));
           

        }

    }

    public void spellActivating() {
        isSpellActivated = true;
    }

    public void spellDeActivating()
    {
        isSpellActivated = false;
    }
}
