using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public enum SpellType 
{
    SpellTargetting,
    SpellNonTargetting,
    SpellActive,
    SpellNonActive,
};

public class SpellScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("Inspetor")]
    [SerializeField, Range(0, 3)] protected int spellSlotID;
    [SerializeField] protected int spellType;
    [SerializeField] protected GameObject privilegedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void SpellInit() { 
        
    }

    public GameObject getPlayerPrivilege() 
    {
        return privilegedPlayer;
    }

    public void setPlayerPrivilage(GameObject _object) 
    { 
        privilegedPlayer = _object;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Spell Icon UP");
        Debug.Log($"Slot ID : {spellSlotID}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Spell Icon UP");
        Debug.Log($"Slot ID : {spellSlotID}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Spell Icon UP");
        Debug.Log($"Slot ID : {spellSlotID}");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Spell Icon UP");
        Debug.Log($"Slot ID : {spellSlotID}");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Spell Icon UP");
        Debug.Log($"Slot ID : {spellSlotID}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Spell Icon UP");
        Debug.Log($"Slot ID : {spellSlotID}");
    }
}
