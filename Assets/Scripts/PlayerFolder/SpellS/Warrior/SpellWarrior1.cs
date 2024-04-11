using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpellWarrior1 : SpellScript
{
    /*
    [SerializeField, Range(0, 3)] protected int spellSlotID;
    [SerializeField] protected int spellType;
    [SerializeField] protected GameObject privilegedPlayer;
    [SerializeField] protected Image IconImage;
    [SerializeField] protected float cooltime;
    [SerializeField] protected float currenmt_cooltime;
     */


    // Start is called before the first frame update
    void Start()
    {
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

    protected override void SpellInit()
    {
        spellSlotID = 0;
        spellType = (int)SpellType.SpellTargetting; 
    }

    public new void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("spell Activated");
        if (currenmt_cooltime != 0) {
            currenmt_cooltime = cooltime;
        }
    }

    public new void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Spell Icon UP");
        Debug.Log($"Slot ID : {spellSlotID}");
    }
}
