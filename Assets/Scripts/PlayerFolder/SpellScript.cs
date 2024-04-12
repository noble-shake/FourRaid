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
    public Image IconImage;
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

    public void SpellInit(SpellInfo _info) {
        spellSlotID = _info.spellSlotID;
        spellType = _info.spellType;
        IconImage = _info.IconImage;
        cooltime = _info.cooltime;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        IconImage = transform.GetChild(0).GetComponent<Image>();
        IconImage.fillAmount = cooltime;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        currenmt_cooltime -= Time.deltaTime;
        if (currenmt_cooltime < 0)
        {
            currenmt_cooltime = 0f;
        }

        IconImage.fillAmount = 1 - (float)(currenmt_cooltime / cooltime);
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
        Debug.Log("before click");
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        Debug.Log("after click");
        PlayerManager.instance.spellDeActivating();
    }

    IEnumerator SpellTargettingReady()
    {
        Time.timeScale = 0.5f;
        PlayerManager.instance.spellActivating();
        yield return null;
        Debug.Log("before click");
        GameObject IndicatorPlayer = privilegedPlayer.GetComponent<PlayerScript>().getIndicator();
        LineRenderer IndicatorLine = privilegedPlayer.GetComponent<PlayerScript>().getIndicatorLine();
        IndicatorPlayer.transform.GetChild(1).transform.gameObject.SetActive(true);
        IndicatorLine.positionCount = 2;
        bool isCanceled = true;
        while (true) {


            Vector3 IndicatorPos = IndicatorPlayer.transform.GetChild(1).transform.position;

            Vector3 TargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TargetPos.z = 0f;
            Vector3 ConvertedIndicator = Camera.main.WorldToScreenPoint(IndicatorPos);


            
            Vector3[] vector2s = new Vector3[2] { IndicatorPos, TargetPos };
            IndicatorLine.SetPositions(vector2s);

            if (Input.GetMouseButtonDown(0)) 
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D[] hit = Physics2D.GetRayIntersectionAll(ray);

                for (int inum = 0; inum < hit.Length; inum++) {
                    if (hit[inum].collider.CompareTag("enemy")) {
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
        IndicatorPlayer.transform.GetChild(1).transform.gameObject.SetActive(false);
        Debug.Log("after click");
        PlayerManager.instance.spellDeActivating();

        Debug.Log("spell Activated");
        if (!isCanceled) {
            currenmt_cooltime = cooltime;
        }
        
        Time.timeScale = 1f;
    }

    public void OnActive()
    {
        if (currenmt_cooltime != 0) return;

        SpellTypeSwitching(spellType);
    }

    public void SpellTypeSwitching(SpellType _enum) {
        switch (_enum) {
            case SpellType.SpellTargetting:
                // Player Spell 
                StartCoroutine(SpellTargettingReady());
                break;
            case SpellType.SpellNonTargetting:
                NonTargetting();
                break;
            case SpellType.SpellActive:
                SpellActive();
                break;
            case SpellType.SpellNonActive:
                SpellNonActive();
                break;
        }
    }

    public bool Targetting() {
        SpellTargettingReady();
        return true;
    }

    public bool NonTargetting() {

        return true;
    }

    public bool SpellActive() {

        return true;
    }

    public bool SpellNonActive() {

        return true;
    }
    
}
