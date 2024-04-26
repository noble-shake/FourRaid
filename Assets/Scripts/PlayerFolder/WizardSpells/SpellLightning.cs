using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLightning : MonoBehaviour
{
    [Header("Inspector")]
    [SerializeField] GameObject Ground;
    [SerializeField] Vector2 originPos;
    [SerializeField] Vector2 TargetPos;
    [SerializeField] LineRenderer LaserLine;
    [SerializeField] GameObject LaserEffect;


    // Start is called before the first frame update
    void Start()
    {
        LaserLine = GetComponent<LineRenderer>();
        originPos = transform.position;
        Debug.Log(originPos);
        
    }

    // Update is called once per frame
    void Update()
    {


        // Camera.main.scree
        // Vector3 worldConvertPos = Camera.main.ScreenToWorldPoint(originPos);
        

        // if direction right.
        
        float Angle = transform.rotation.z;

        float tanCal = Mathf.Tan(Mathf.Deg2Rad * Angle);

        float b = originPos.y  - (originPos.x * tanCal);
        if (Angle == 0) {
            b = 0;
        }

        // Screen.Pos.x
        float edgeY = tanCal * Screen.width + b;
        if (edgeY > Screen.height) { 
            float edgeX = (Screen.mainWindowPosition.y - b) / tanCal;
            TargetPos = new Vector2(edgeX, Screen.height);  
        }
        else
        {
            TargetPos = new Vector2(Screen.width, edgeY);
        }


        Ray ray = new Ray(transform.position, transform.right);
        RaycastHit hit;

        LaserLine.SetPosition(0, (Vector2)originPos);
        LaserLine.SetPosition(1, (Vector2)Camera.main.ScreenToWorldPoint(TargetPos));

        //if (Physics.Raycast(ray, out hit, 100))
        //{
        //    LaserLine.SetPosition(1, hit.point);
        //    LaserEffect.transform.localScale = new Vector3(hit.point.x, 1f, 1f);
        //}
        //else
        //{
        //    LaserLine.SetPosition(1, ray.GetPoint(100));
        //    LaserEffect.transform.localScale = new Vector3(100f, 1f, 1f);
        //}

    }
}
