using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictTrajectory : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform releasePos;
    [SerializeField][Range(10,100)] private int LinePoints = 25;
    [SerializeField][Range(0.01f,0.25f)] private float TimeBetweenPoints = 0.1f;
    
    public GameObject boxPrefab;
    public GameObject boxPredictionPrefab;
    public GameObject ballPredictionPrefab;
    public GameObject currentObject;
    public PlayerController playerController;
    public int sphereRadius = 1;
    public Vector3 lastPosition = new Vector3(0,0,0);
    
    private LayerMask throwableObjectCollisionMask;
    private GameObject boxPredictionInstance;
    private GameObject ballPredictionInstance;

    private void Awake() 
    {
        int throwableObjectLayer = boxPrefab.gameObject.layer;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(throwableObjectLayer, i))
            {
                throwableObjectCollisionMask |= 1 << i;
            }
        }
    }

    private void Start()
    {
        boxPredictionInstance = Instantiate(boxPredictionPrefab, lastPosition, transform.rotation);
        ballPredictionInstance = Instantiate(ballPredictionPrefab, lastPosition, transform.rotation);
    }

    public void Update() 
    {
        GetCurrentObject();
        IsShootTimerReady();
        if (currentObject != null && IsShootTimerReady())
        {
            DrawProjection(currentObject);
            if (currentObject.CompareTag("Box"))
            {
                boxPredictionInstance.SetActive(true);
                boxPredictionInstance.transform.position = lastPosition;
            }
            else if (currentObject.CompareTag("RedFruit"))
            {
                ballPredictionInstance.SetActive(true);
                ballPredictionInstance.transform.position = lastPosition;
            }
        }
        else 
        { 
            lineRenderer.enabled = false; 
            boxPredictionInstance.SetActive(false);
            ballPredictionInstance.SetActive(false);
        }
    }

    private bool IsShootTimerReady()
    {
        if(playerController.timer >= playerController.timeBetweenShots)
        {
            return true;
        }
        else { return false; }
    }
    public GameObject GetCurrentObject()
    {
        currentObject = playerController.currentThrowableObject;
        return currentObject;
    }

    //delete
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lastPosition, sphereRadius);
    }


    private void DrawProjection(GameObject currentObject)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt (LinePoints / TimeBetweenPoints) + 1;
        Vector3 startPosition = releasePos.position;
        Vector3 startVelocity = playerController.fruitShootingForce * transform.forward / currentObject.GetComponent<Rigidbody>().mass;
        int i = 0;
        lineRenderer.SetPosition(i, startPosition);
        for(float time = 0; time < LinePoints; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            lineRenderer.SetPosition(i, point);

            //find where we hit FIX
            lastPosition = lineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition, (point -lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude, throwableObjectCollisionMask))
            {
                lineRenderer.SetPosition(i, hit.point);
                lineRenderer.positionCount = i + 1;
               
                return;
            }
        }
    }
}