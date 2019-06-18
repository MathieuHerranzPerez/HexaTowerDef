using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;


    void Update()
    {
        //if(Input.GetButtonDown("Fire1"))
        //{
        //    RaycastHit hit;
        //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        Transform objectHit = hit.transform;
        //        if(objectHit.tag == "Turret")
        //        {
        //            objectHit.GetComponent<Turret>().InteractWithWall();
        //        }
        //    }
        //}
    }
}
