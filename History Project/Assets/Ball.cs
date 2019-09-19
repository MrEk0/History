using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float xOffset = 0.5f;
    [SerializeField] float yOffset = 1.3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.position = new Vector3(collision.transform.position.x+xOffset, 
                collision.transform.position.y+yOffset, 
                collision.transform.position.z);
            transform.parent = collision.transform;
        }
    }
}
