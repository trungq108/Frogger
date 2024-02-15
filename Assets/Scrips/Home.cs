using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    public GameObject frog;

    private void OnEnable()
    {
        frog.SetActive(true);
    }

    private void OnDisable()
    {
        frog.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enabled = true;           
            FindObjectOfType<GameManager>().HomeOccupied();
        }
    }
}
