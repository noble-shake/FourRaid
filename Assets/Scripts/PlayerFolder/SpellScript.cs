using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;

[System.Serializable]
public class SpellInfo
{
    public int spellSlotID;
    public SpellType spellType;
    public Sprite IconImage;
    public float cooltime;
}

public class SpellScript: MonoBehaviour
{

    [Header("Inspetor")]
    [SerializeField, Range(0, 3)] int spellSlotID;
    [SerializeField] SpellType spellType;
    [SerializeField] GameObject privilegedPlayer;
    [SerializeField] Image IconImage;
    [SerializeField] float cooltime;
    [SerializeField] float currenmt_cooltime;
    [SerializeField] Button btnSpell;
    [SerializeField] Canvas DarkSkin;

    public void SpellInit(SpellInfo _info) {
        spellSlotID = _info.spellSlotID;
        spellType = _info.spellType;
        IconImage.sprite = _info.IconImage;
        cooltime = _info.cooltime;
    }

    // Start is called before the first frame update
    void Start()
    {
        IconImage = transform.GetChild(0).GetComponent<Image>();
        IconImage.fillAmount = cooltime;
    }

    private void FixedUpdate()
    {
        if (privilegedPlayer != null) 
        {
            IconImage.fillAmount = privilegedPlayer.GetComponent<PlayerScript>().SpellCooltimeCheck(spellSlotID);
        }

    }

    public void setIconSprite(Sprite _spell)
    {
        IconImage.sprite = _spell;
    }

    public GameObject getPlayerPrivilege()
    {
        return privilegedPlayer;
    }

    public void setPlayerPrivilage(GameObject _object)
    {
        privilegedPlayer = _object;

    }

    public void setSpellType(SpellType _enum) {
        spellType = _enum;
    }

    IEnumerator SpellCharging() {
        PlayerManager.instance.spellActivating();
        yield return null;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        PlayerManager.instance.spellDeActivating();
    }

    IEnumerator SpellActiveReady() {
        Time.timeScale = 0.5f;
        PlayerManager.instance.spellActivating();
        yield return null;
        DarkSkin.gameObject.SetActive(true);

        privilegedPlayer.GetComponent<PlayerScript>().ActiveSpellActivate(spellSlotID);
        // privilegedPlayer.GetComponent<PlayerScript>().NonTargettingSpellActivate(spellSlotID, convertedPos);

        // animator 
        yield return new WaitForSeconds(0.3f);
        privilegedPlayer.GetComponent<PlayerScript>().setSpellCooltime(spellSlotID);
        Time.timeScale = 1f;
        DarkSkin.gameObject.SetActive(false);
    }

    IEnumerator SpellNonTargettingReady() {
        Time.timeScale = 0.5f;
        PlayerManager.instance.spellActivating();
        yield return null;
        GameObject IndicatorPlayer = privilegedPlayer.GetComponent<PlayerScript>().getIndicator();
        LineRenderer IndicatorLine = privilegedPlayer.GetComponent<PlayerScript>().getIndicatorLine();
        IndicatorPlayer.transform.GetChild(1).transform.gameObject.SetActive(true);
        IndicatorLine.positionCount = 2;
        bool isCanceled = true;
        DarkSkin.gameObject.SetActive(true);
        while (true)
        {
            Vector3 IndicatorPos = IndicatorPlayer.transform.GetChild(1).transform.position;
            Vector3 TargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TargetPos.z = 0f;

            Vector3[] vector2s = new Vector3[2] { IndicatorPos, TargetPos };
            IndicatorLine.SetPositions(vector2s);

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 convertedPos = TargetPos;
                
                privilegedPlayer.GetComponent<PlayerScript>().NonTargettingSpellActivate(spellSlotID, convertedPos);
                isCanceled = false;
                break;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                isCanceled = true;
                // yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                break;
            }
            yield return null;
        }
        DarkSkin.gameObject.SetActive(false);
        IndicatorPlayer.transform.GetChild(1).transform.gameObject.SetActive(false);
        PlayerManager.instance.spellDeActivating();

        if (!isCanceled)
        {
            privilegedPlayer.GetComponent<PlayerScript>().setSpellCooltime(spellSlotID);
        }
        Time.timeScale = 1f;
    }

    IEnumerator SpellTargettingReady()
    {
        Time.timeScale = 0.5f;
        PlayerManager.instance.spellActivating();
        yield return null;
        GameObject IndicatorPlayer = privilegedPlayer.GetComponent<PlayerScript>().getIndicator();
        LineRenderer IndicatorLine = privilegedPlayer.GetComponent<PlayerScript>().getIndicatorLine();
        IndicatorPlayer.transform.GetChild(1).transform.gameObject.SetActive(true);
        IndicatorLine.positionCount = 2;
        bool isCanceled = true;
        DarkSkin.gameObject.SetActive(true);
        while (true) {
            Vector3 IndicatorPos = IndicatorPlayer.transform.GetChild(1).transform.position;
            Vector3 TargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TargetPos.z = 0f;
            
            Vector3[] vector2s = new Vector3[2] { IndicatorPos, TargetPos };
            IndicatorLine.SetPositions(vector2s);

            if (Input.GetMouseButtonDown(0)) 
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D[] hit = Physics2D.GetRayIntersectionAll(ray);

                for (int inum = 0; inum < hit.Length; inum++) {
                    if (hit[inum].collider.CompareTag("enemy")) {
                        isCanceled = false;
                        privilegedPlayer.GetComponent<PlayerScript>().TargettingSpellActivate(spellSlotID, hit[inum].collider.transform.parent.gameObject);
                    }
                    else if (hit[inum].collider.CompareTag("player") && privilegedPlayer.name == "PlayerHealer") {
                        isCanceled = false;
                        privilegedPlayer.GetComponent<PlayerScript>().TargettingSpellActivate(spellSlotID, hit[inum].collider.gameObject);
                    }
                }
                break;
            } 
            else if(Input.GetMouseButtonDown(1)) 
            {
                isCanceled = true;
                // yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                break;
            }
            yield return null;
        }
        DarkSkin.gameObject.SetActive(false);
        IndicatorPlayer.transform.GetChild(1).transform.gameObject.SetActive(false);
        PlayerManager.instance.spellDeActivating();

        if (!isCanceled) {
            privilegedPlayer.GetComponent<PlayerScript>().setSpellCooltime(spellSlotID);
        }
        Time.timeScale = 1f;
    }

    public void OnActive()
    {
        if (privilegedPlayer.GetComponent<PlayerScript>().getSpellCurrentCooltime(spellSlotID) != 0) return;

        SpellTypeSwitching(spellType);
    }

    public void SpellTypeSwitching(SpellType _enum) {
        switch (_enum) {
            case SpellType.SpellTargetting:
                // Player Spell 
                StartCoroutine(SpellTargettingReady());
                break;
            case SpellType.SpellNonTargetting:
                StartCoroutine(SpellNonTargettingReady());
                break;
            case SpellType.SpellActive:
                StartCoroutine(SpellActiveReady());
                break;
            case SpellType.SpellNonActive:
                //StartCoroutine();
                break;
        }
    }
    
}
