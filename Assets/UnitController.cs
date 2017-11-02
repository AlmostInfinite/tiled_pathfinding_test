using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {


    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Dead");

        Destroy(transform.parent.gameObject);

    }


    //TODO Move to seat, destroy object? change seat material? move "pawn" from unit make unit spawn pos instead.

}
