using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] GameObject door;

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            animator.SetTrigger("turnOn");
        }
    }

    public void DoorAnim()
    {
        if(door!=null)
        {
            door.GetComponent<Animator>().SetTrigger("open");
        }
    }

}
