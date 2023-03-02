using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public GameObject BarrierObject;
    public float timeToDrop;
    public Transform dropPoint;
    public float dropSpeed;
    private bool isDrop;

    private void Update()
    {
        if (isDrop)
        {
            timeToDrop -= Time.deltaTime;
            if (timeToDrop <= 0)
            {
                BarrierObject.transform.position = Vector2.MoveTowards(BarrierObject.transform.position, dropPoint.position, dropSpeed * Time.deltaTime);
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            isDrop = true;
        }
    }

}
