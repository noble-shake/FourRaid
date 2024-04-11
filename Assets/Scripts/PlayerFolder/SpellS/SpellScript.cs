using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;

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
    [SerializeField] protected Image IconImage;
    [SerializeField] protected float cooltime;
    [SerializeField] protected float currenmt_cooltime;

    // Start is called before the first frame update
    void Start()
    {
        IconImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected virtual void SpellInit() { 
        
    }

    public void setIconSprite(Sprite _spell) {
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

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Spell Icon UP");
        Debug.Log($"Slot ID : {spellSlotID}");
    }
}
