using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Frogger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    public Sprite leapSprite;
    public Sprite deathSprite;
    private Vector3 spawnPosition;
    private float farthesRow;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPosition = transform.position;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector3.left);
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector3.right);
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector3.down);
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector3.up);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
    void Move(Vector3 direction)
    {   
        Vector3 destination = transform.position + direction;
        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Barrier"));
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Obstacle"));
        Collider2D flatform = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Platform"));

        if (barrier != null)
        {
            return;
        }
        if (flatform != null)
        {
            transform.SetParent(flatform.transform);
        } 
        else {transform.SetParent(null);}
                   
        if (obstacle != null && flatform == null)
        {
            transform.position = destination;
            Death();
        }
        else
        {
            if(destination.y > farthesRow)
            {
                farthesRow = destination.y;
                FindObjectOfType<GameManager>().AdvancedRow();
            }
            StartCoroutine(Leap(destination));
        }

    }
    private IEnumerator Leap(Vector3 destination)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;
        float duration = 0.125f;
        spriteRenderer.sprite = leapSprite;
        
        while (elapsed < duration)
        {
            var t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, destination, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = destination;
        spriteRenderer.sprite = idleSprite;
    }
    
    public void Death()
    {
        StopAllCoroutines();
        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = deathSprite;
        enabled = false;
        FindObjectOfType<GameManager>().FrogDie();
    }
    public void Respawn()
    {
        StopAllCoroutines();
        transform.position = spawnPosition;
        transform.rotation *= Quaternion.identity;
        farthesRow = spawnPosition.y;
        spriteRenderer.sprite = idleSprite;
        gameObject.SetActive(true);
        enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( enabled && other.gameObject.layer == LayerMask.NameToLayer("Obstacle") && transform.parent == null )
        {
            Death();
        }
    }
}
