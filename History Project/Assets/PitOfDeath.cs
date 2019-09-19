using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitOfDeath : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;

    Canvas canvas;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }
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
        Instantiate(gameOverPanel, canvas.transform);
        Time.timeScale = 0f;
    }
}
