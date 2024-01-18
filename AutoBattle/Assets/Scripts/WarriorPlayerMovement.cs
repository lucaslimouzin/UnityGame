using UnityEngine;
using System.Collections;

public class WarriorPlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float backOffDistance = 1f;
    private GameObject enemy;
    private bool backingOff = false;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;


    private void Start()
    {
        enemy = GameObject.FindWithTag("TeamEnemy");
    }

    private void Update()
    {
        if (enemy != null && !backingOff)
        {
            MoveTowardsEnemy();
        }

        ConstrainWithinBounds();
    }
        
    private void ConstrainWithinBounds()
    {
        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
    private void MoveTowardsEnemy()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "TeamEnemy")
        {
            StartCoroutine(BackOff());
        }
    }

    private IEnumerator BackOff()
    {
        backingOff = true;

        // Reculer dans une direction aléatoire
        Vector2 backOffDirection = (transform.position - enemy.transform.position).normalized;
        float angleVariation = Random.Range(-30f, 30f);
        float angleRadians = angleVariation * Mathf.Deg2Rad;
        Vector2 randomDirection = new Vector2(
            backOffDirection.x * Mathf.Cos(angleRadians) - backOffDirection.y * Mathf.Sin(angleRadians),
            backOffDirection.x * Mathf.Sin(angleRadians) + backOffDirection.y * Mathf.Cos(angleRadians)
        );

        Vector2 backOffTarget = (Vector2)transform.position + randomDirection.normalized * backOffDistance;
        float backOffTime = 0.5f;
        float startTime = Time.time;

        while (Time.time < startTime + backOffTime)
        {
            transform.position = Vector2.MoveTowards(transform.position, backOffTarget, speed * Time.deltaTime);
            yield return null;
        }

        // Attendez un peu avant de reprendre la poursuite
        //yield return new WaitForSeconds(0.5f);

        // Déplacer vers un point aléatoire
        Vector2 randomPoint = GetRandomPoint();
        startTime = Time.time;
        while (Time.time < startTime + 2f) // Se déplace pendant 2 secondes
        {
            transform.position = Vector2.MoveTowards(transform.position, randomPoint, speed * Time.deltaTime);
            yield return null;
        }

        backingOff = false;
    }

    private Vector2 GetRandomPoint()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector2(randomX, randomY);
    }
}
