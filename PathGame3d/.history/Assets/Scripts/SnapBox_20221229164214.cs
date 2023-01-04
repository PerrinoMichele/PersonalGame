using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapBox : MonoBehaviour
{
    [SerializeField] float sphereRadius = 0.3f;

    public bool isTouching = false;
    Vector3Int coordinates = new Vector3Int();
    public List<Vector3> neighboringTiles = new List<Vector3>();   

    void OnCollisionEnter(Collision other) 
    {
        SnapToGrid();
        AddNeighbors();
        CheckNeigbors();
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        
    }

    private void SnapToGrid()
    {
        coordinates.x = Mathf.RoundToInt(transform.position.x);
        coordinates.z = Mathf.RoundToInt(transform.position.z);
        coordinates.y = Mathf.RoundToInt(transform.position.y);

        transform.position = coordinates;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void AddNeighbors()
    {
        neighboringTiles.Add(transform.position += Vector3.up);
        neighboringTiles.Add(transform.position += Vector3.down);
        neighboringTiles.Add(transform.position += Vector3.right);
        neighboringTiles.Add(transform.position += Vector3.left);
        neighboringTiles.Add(transform.position += Vector3.forward);
        neighboringTiles.Add(transform.position += Vector3.back);        
    }

    private void CheckNeigbors()
    {
        foreach(Vector3 neighboringTile in neighboringTiles)
        {
            if(Physics.CheckSphere(neighboringTile, sphereRadius))
            {
                Gizmos.DrawSphere(neighboringTile, 1);
                //Debug.Log(neighboringTile);
                return;
            }
        }
    }

    private void MoveBoxDown()
    {
        this.transform.position += Vector3.down;
    }
}