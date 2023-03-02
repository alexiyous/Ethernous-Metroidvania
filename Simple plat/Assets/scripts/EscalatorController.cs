using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscalatorController : MonoBehaviour
{
    public GameObject EscalatorObject;
    public float timeToUp;
    public Transform UpPoint;
    public float upSpeed;
    private bool isUp;

    private void Update()
    {
        if (isUp)
        {
            timeToUp -= Time.deltaTime;
            if (timeToUp <= 0)
            {
                EscalatorObject.transform.position = Vector2.MoveTowards(EscalatorObject.transform.position, UpPoint.position, upSpeed * Time.deltaTime);
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isUp = true;
        }
    }
}
