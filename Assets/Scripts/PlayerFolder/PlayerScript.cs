using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class PlayerScript : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public virtual void playerSelect() { }

    public virtual void playerSelectCancel() { }

    public virtual void commandMove() { }

    public virtual void commandAtackk() { }

    public virtual void commandSpell(int _value) { }

    public virtual void OnDrag(PointerEventData eventData)
    {
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
    }
}
