using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitControl : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] PlayerScript player 

    private void OnMouseEnter()
    {
        player.PlayerMouseEnter();
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseUp()
    {
        
    }

    private void OnMouseDrag()
    {
        
    }

    private void OnMouseExit()
    {
        
    }

}
