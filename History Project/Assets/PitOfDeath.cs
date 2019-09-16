using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitOfDeath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==LayerMask.NameToLayer("Player"))
        {
            Death();
        }
    }

    private void Death()
    {
        //show game over window
        Time.timeScale = 0f;
    }
}
