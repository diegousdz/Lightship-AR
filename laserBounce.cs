using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserBounce : MonoBehaviour
{

    public int reflections;
    public float maxLength;

    private LineRenderer linerRenderer;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 direction;

    private void Awake()
    {
        linerRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        ray = new Ray(transform.position, transform.forward);

        linerRenderer.positionCount = 1;
        linerRenderer.SetPosition(0, transform.position);
        float remainingLenght = maxLength;

        for(int i = 0; i < reflections; i++)
        {
            if(Physics.Raycast(ray.origin, ray.direction, out hit, remainingLenght))
            {
                linerRenderer.positionCount += 1;
                linerRenderer.SetPosition(linerRenderer.positionCount -1, hit.point);
                remainingLenght -= Vector3.Distance(ray.origin, hit.point);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                
            }
        }
    }
   
}