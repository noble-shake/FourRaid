using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpellWarrior1 : SpellScript
{
    /*
     * [SerializeField, Range(0, 3)] protected int spellSlotID;
     * [SerializeField] protected int spellType;
     * [SerializeField] protected GameObject privilegedPlayer;
     */


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void SpellInit()
    {
        spellSlotID = 0;
        spellType = (int)SpellType.SpellTargetting; 
    }

    public new void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Spell Icon UP");
        Debug.Log($"Slot ID : {spellSlotID}");
    }

    public new void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Spell Icon UP");
        Debug.Log($"Slot ID : {spellSlotID}");
    }
}