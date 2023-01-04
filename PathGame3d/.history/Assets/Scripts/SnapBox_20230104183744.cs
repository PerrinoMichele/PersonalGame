using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapBox : MonoBehaviour
{
    [SerializeField] float sphereRadius = 0.3f;

    public List<Vector3> neighboringTiles = new List<Vector3>();
    public bool isPickable = false;
    public Material pickableBoxMat;

    Vector3Int coordinates = new Vector3Int();

    void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "RedFruit")
        {
            //refactor
            isPickable = true;
            gameObject.GetComponent<BoxCollider>().size = new Vector3(1,1,1);
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().mass = 0.1f;
            gameObject.GetComponent<MeshRenderer>().material = pickableBoxMat;
            this.GetComponent<SnapBox>().enabled = false;
        }
        else if (isPickable == false)
        {
            SnapToGrid();
            AddNewNeighbors();
            return;
        }
    }

    void Update()
    {
        if (gameObject.GetComponent<Rigidbody>().isKinematic == true)
        {
            if (NeighborCollides() == false)
            {           
                MoveBoxDown();
                AddNewNeighbors();
            }
        }
    }

    private void MoveBoxDown()
    {
        transform.position += Vector3.down; //add some lerp
    }

    private void OnDrawGizmos() {//delete
        Gizmos.color = Color.red;
        foreach (Vector3 neighbor in neighboringTiles)
        {
            Gizmos.DrawWireSphere(neighbor, sphereRadius);
        }
    }

    private void SnapToGrid()
    {
        coordinates.x = Mathf.RoundToInt(transform.position.x);
        coordinates.z = Mathf.RoundToInt(transform.position.z);
        coordinates.y = Mathf.RoundToInt(transform.position.y);

        transform.position = coordinates; // add some lerp
        transform.rotation = Quaternion.identity;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        //gameObject.GetComponent<Collider>().isTrigger = false;
    }

    private void AddNewNeighbors()
    {
        neighboringTiles.Clear();
        neighboringTiles.Add(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z));
        neighboringTiles.Add(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z));
        neighboringTiles.Add(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z));
        neighboringTiles.Add(new Vector3(transform.position.x - 1, transform.position.y, transform.position.z));
        neighboringTiles.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1));
        neighboringTiles.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z - 1));
    }
    //RemoveNeighbors?

    private bool NeighborCollides()
    {
        foreach(Vector3 neighboringTile in neighboringTiles)
        {
            if(Physics.CheckSphere(neighboringTile, sphereRadius))
            {
                return true;
            }
        }
    
        return false;
    }


}
