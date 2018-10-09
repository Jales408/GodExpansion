using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMove : MonoBehaviour {
    public float backgroundSize;
    public float parallaxSpeed;
    public bool canScroll;



    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 110;
    private int leftIndex;
    private int rightIndex;
    private float offSetY;
    private float lastCameraX;

    

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        layers = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
        }
        leftIndex = 0;
        rightIndex = layers.Length - 1;
        offSetY = layers[0].position.y;
        
    }

    private void Update()
    {
        float deltaX = cameraTransform.position.x - lastCameraX;
        lastCameraX = cameraTransform.position.x;
        transform.position += Vector3.right*(deltaX * parallaxSpeed);
        if (!canScroll)
        {
            return;
        }
        if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
        {
            ScrollLeft();
        }
        if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
        {
            ScrollRight();
        }
    }
    private void ScrollLeft()
    {
        layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize) + Vector3.up * offSetY;
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
        {
            rightIndex = layers.Length - 1;
        }
    }
    private void ScrollRight()
    {
        layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize)+Vector3.up*offSetY;
        rightIndex= leftIndex ;
        leftIndex++;
        if (leftIndex == layers.Length)
        {
            leftIndex = 0;
        }
    }
}
