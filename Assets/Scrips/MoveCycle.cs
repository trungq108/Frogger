using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCycle : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = Vector2.right;
    private Vector3 rightEgde;
    private Vector3 leftEgde;
    public int size;
    private void Start()
    {
      leftEgde = Camera.main.ViewportToWorldPoint(Vector3.zero);
      rightEgde= Camera.main.ViewportToWorldPoint(Vector3.right);
    }

    private void Update()
    {
        if (transform.position.x > 0 && transform.position.x - size > rightEgde.x)
        {
            transform.position = new Vector2 (leftEgde.x - size, transform.position.y);
        }
        else if (transform.position.x < 0 && transform.position.x + size < leftEgde.x)
        {
            transform .position = new Vector2 (rightEgde.x + size, transform.position.y);
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}
