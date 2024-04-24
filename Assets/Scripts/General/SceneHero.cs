using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHero : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] Animator anim;
    [SerializeField] bool isActive = false;
    [SerializeField] float duration = 0.8f;
    [SerializeField] float current;
    [SerializeField] Vector3 originPos;

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isActive) {
            current += Time.deltaTime;
            if (current > 0.3f) {
                Vector3 currentPos = transform.position;
                currentPos.x -= Time.deltaTime * 100f;
                transform.position = currentPos;
            }
            if (current > duration)
            {
                current = 0f;
                isActive = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void OnStart() {
        anim.SetBool("GameStart", true);
    }

    public void OnDisappear() {
        current = 0f;
        isActive = true;
    }

    public void OnReset() {
        anim.SetBool("GameStart", false);
        isActive = false;
        current = 0f;
        transform.position = originPos;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }
}
