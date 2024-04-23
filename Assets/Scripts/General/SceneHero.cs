using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHero : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStart() {
        anim.SetBool("GameStart", true);
    }

    public void OnReset() {
        anim.SetBool("GameStart", false);
    }
}
