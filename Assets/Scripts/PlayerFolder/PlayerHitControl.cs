using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitControl : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] PlayerScript player;

    private void OnMouseEnter()
    {
        player.PlayerMouseEnter();
    }

    private void OnMouseDown()
    {
        player.PlayerMouseDown();
    }

    private void OnMouseUp()
    {
        player.PlayerMouseUp();
    }

    private void OnMouseDrag()
    {
        player.PlayerMouseDrag();
    }

    private void OnMouseExit()
    {
        player.PlayerMouseExit();
    }

}
