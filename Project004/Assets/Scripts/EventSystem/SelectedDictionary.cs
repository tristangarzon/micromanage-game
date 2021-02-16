/*
* Made by: Tristan Garzon
* Website: https://tristangarzon.wordpress.com/
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDictionary : MonoBehaviour
{
    //Links a game object to an int
    public Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();

    //Adds an object to the Dictionary
    public void addSelected(GameObject go)
    {
        int id = go.GetInstanceID();

        //Checks to see if the object is already added
        //If not already added, it will add to Dictionary
        if (!(selectedTable.ContainsKey(id)))
        {
            selectedTable.Add(id, go);
            //Adds the SelectionComp to the object (Should turn the object red)
            go.AddComponent<SelectionComp>();
            Debug.Log("Added " + id + " to the selected dictionary"); 
        }
    }

    //Removes an object from the Dictionary
    public void deselect(int id)
    {
        Destroy(selectedTable[id].GetComponent<SelectionComp>());
        selectedTable.Remove(id);
    }

    //Removes all objects from the Dictionary
    public void deselectAll()
    {
        foreach(KeyValuePair<int, GameObject> pair in selectedTable)
        {
            if(pair.Value != null)
            {
                Destroy(selectedTable[pair.Key].GetComponent<SelectionComp>());
            }
        }
        selectedTable.Clear();
    }


}
