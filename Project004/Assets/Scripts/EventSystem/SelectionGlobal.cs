/*
* Made by: Tristan Garzon
* Website: https://tristangarzon.wordpress.com/
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionGlobal : MonoBehaviour
{
    #region Variables
    SelectedDictionary selectedTable;
    RaycastHit hit;

    public float dragArea = 40;
    bool dragSelect;
    

    //Collider Variables
    Vector3 startPosition;  //p1
    Vector3 currentPosition;//p2

    MeshCollider selectionBox;
    Mesh selectionMesh;

    //Stores the corners of our 2D selection box
    Vector2[] corners;

    //Stores the vertices of our Mesh Collider 
    Vector3[] verts;
    Vector3[] vecs;
    

    #endregion
	
    #region Unity Methods

    void Start()
    {
        selectedTable = GetComponent<SelectedDictionary>();
        dragSelect = false;
    }

   
    void Update()
    {
        HandlesMouseInput();
    }

    void HandlesMouseInput()
    {
        //Right Mouse Button Click
        if (Input.GetMouseButtonDown(1))
        {
            startPosition = Input.mousePosition;
        }

        //Right Mouse Button Held
        if (Input.GetMouseButton(1))
        {
            if ((startPosition - Input.mousePosition).magnitude > dragArea)
            {
                dragSelect = true;
            }
        }

        //Right Mouse Button Released
        if (Input.GetMouseButtonUp(1))
        {
            //Selects a single unit
            if (dragSelect == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(startPosition);

                //I set it to an extra large number just incase
                if (Physics.Raycast(ray, out hit, 50000.0f))
                {
                    //Left shift will allow the selection of multiple units 
                    //Inclusive Select
                    if (Input.GetKey(KeyCode.LeftShift))        
                    {
                        selectedTable.addSelected(hit.transform.gameObject);
                    }
                    //Exclusive Selected
                    else
                    {
                        selectedTable.deselectAll();
                        selectedTable.addSelected(hit.transform.gameObject);
                    }
                }
                //If we didn't hit something
                else
                {
                    if(Input.GetKey(KeyCode.LeftShift))
                    {

                    }
                    else
                    {
                        selectedTable.deselectAll();
                    }  
                }
            }
            //If hit by the selection box
            else
            {
                verts = new Vector3[4];
                vecs = new Vector3[4];
                int i = 0;
                currentPosition = Input.mousePosition;
                corners = getBoundingBox(startPosition, currentPosition);
                
                //After getting the position of the boundingBox it will cast a ray from those positions 
                foreach(Vector2 corner in corners)
                {
                    Ray ray = Camera.main.ScreenPointToRay(corner);

                    //Layermask is set to ground (1 << 8, layer 8 = ground)
                    //Should be set by Unity default
                    if (Physics.Raycast(ray, out hit, 50000.0f,(1 << 8)))
                    {
                        verts[i] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        vecs[i] = ray.origin - hit.point;
                        //Draws a line in the inspector
                        Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.red, 1.0f);                    
                    }
                    i++;
                }

                //Genrates the box collider mesh
                selectionMesh = generateSelectionMesh(verts, vecs);
                //Adds a mesh collider to the componment
                selectionBox = gameObject.AddComponent<MeshCollider>();
                selectionBox.sharedMesh = selectionMesh;
                selectionBox.convex = true;
                selectionBox.isTrigger = true;

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    selectedTable.deselectAll();
                }

                Destroy(selectionBox, 0.02f);
            }
            dragSelect = false;
        }
    }

    //Draws the selection box
    private void OnGUI()
    {
        if (dragSelect == true)
        {
            var rect = Utils.GetScreenRect(startPosition, Input.mousePosition);
            //Sets the box fill color
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            //Sets the box border color
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    //Creates a bounding box from the start and end mouse position (4 corners in order)
    Vector2[] getBoundingBox(Vector2 startPosition, Vector2 currentPosition) //p1, p2
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        //If startPos is  to the LEFT of currentPos
        if (startPosition.x < currentPosition.y)
        {
            //if startPos is above currentPos
            if (startPosition.y > currentPosition.y)
            {
                newP1 = startPosition;
                newP2 = new Vector2(currentPosition.x, startPosition.y);
                newP3 = new Vector2(startPosition.x, currentPosition.y);
                newP4 = currentPosition;
            }
            //If startPos is below currentPos
            else
            {
                newP1 = new Vector2(startPosition.x, currentPosition.y);
                newP2 = currentPosition;
                newP3 = startPosition;
                newP4 = new Vector2(currentPosition.x, startPosition.y);
            }
        }
        //If startPos is to the RIGHT of currentPos
        else
        {
            //if startPos is above currentPos
            if (startPosition.y > currentPosition.y)
            {
                newP1 = new Vector2(currentPosition.x, startPosition.y);
                newP2 = startPosition;
                newP3 = currentPosition;
                newP4 = new Vector2(startPosition.x, currentPosition.y);
            }
            //If startPos is below currentPos
            else
            {
                newP1 = currentPosition;
                newP2 = new Vector2(startPosition.x, currentPosition.y);
                newP3 = new Vector2(currentPosition.x, startPosition.y);
                newP4 = startPosition;
            }
        }
        //Creates the new Vector2 
        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;
    }

    //Generates a rectangle mesh from the selected bounding box
    Mesh generateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        //Map the tris of the cube
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 };

        //Bottom Rectangle
        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }
            
        //Top Rectangle
        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        //Forms the rectangle
        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    //Detects collision between our selection box and the objects
    private void OnTriggerEnter(Collider other)
    {
        selectedTable.addSelected(other.gameObject);
    }
    #endregion
}
