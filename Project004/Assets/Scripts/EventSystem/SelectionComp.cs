/*
* Made by: Tristan Garzon
* Website: https://tristangarzon.wordpress.com/
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionComp : MonoBehaviour
{
    #region Variables

    #endregion
	
    #region Unity Methods

    void Start()
    {
        //Selected objects turn Red
        GetComponent<Renderer>().material.color = Color.red;
    }

   
    private void OnDestroy()
    {
        //Deselected objects turn White
        GetComponent<Renderer>().material.color = Color.white;
    }

    #endregion
}
