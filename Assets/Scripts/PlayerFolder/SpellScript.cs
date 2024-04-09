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

public class SpellScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IDropHandler
{

    [Header("Inspetor")]
    [SerializeField, Range(0, 3)] int spellSlotID;
    [SerializeField] string SpellType;
    [SerializeField] GameObject privilegedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getPlayerPrivilege() 
    {
        return privilegedPlayer;
    }

    public void setPlayerPrivilage(GameObject _object) 
    { 
        privilegedPlayer = _object;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Spell Icon UP");
        Debug.Log($"Slot ID : {spellSlotID}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Spell Icon UP");
        Debug.Log($"Slot ID : {spellSlotID}");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Spell Icon UP");
        Debug.Log($"Slot ID : {spellSlotID}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
