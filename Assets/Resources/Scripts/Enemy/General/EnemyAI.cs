using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.TextCore.Text;
using UnityEngine.Assertions.Must;

public class EnemyAI : MonoBehaviour
{

    [SerializeField]
    private bool roaming = false;
    [SerializeField]
    private bool updateContinuesPath;
    private bool reachDestination = false;

    public float moveSpeed = 4f;
    public float nextWayPointDistance = 1f;
    public float repeatTimeUpdatePath = 0.5f;

    Path path;
    Seeker seeker;
    Rigidbody2D rb;

    Coroutine moveCoroutine;

    // Part 10
    public float freezeDurationTime;
    float freezeDuration;
    bool IsSeekerDone;
    private void Start()
    {
        freezeDuration = 0;

        InvokeRepeating("CalculatePath", 0f, repeatTimeUpdatePath);

        reachDestination = true;

    }

    public void Update()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        IsSeekerDone = seeker.IsDone();
    }

    Vector2 FindTarget()
    {
        Vector3 playerPos = FindAnyObjectByType<PlayerMovement>().transform.position;
        if(roaming)
        {
            return (Vector2)playerPos + (Random.Range(5f,8f) * new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f)).normalized);
        }
        else
        {
            return playerPos;
        }
    }

    void CalculatePath()
    {
        Vector2 target = FindTarget();
        if (IsSeekerDone && (reachDestination || updateContinuesPath))
            seeker.StartPath(rb.position, target, OnPathCompleted);
    }

    void OnPathCompleted(Path p)
    {
        if (!p.error)
        {
            path = p;
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveToTargetCoroutine());
    }

    public void FreezeEnemy()
    {
        freezeDuration = freezeDurationTime;
    }

    IEnumerator MoveToTargetCoroutine()
    {
        int currentWP = 0;
        reachDestination = false;
        while (currentWP < path.vectorPath.Count)
        {
            while (freezeDuration > 0)
            {
                freezeDuration -= Time.deltaTime;
                yield return null;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWP] - rb.position).normalized;
            Vector2 force = direction * moveSpeed * Time.deltaTime;
            transform.position += (Vector3)force;

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWP]);
            if (distance < nextWayPointDistance)
                currentWP++;

            yield return null;
        }
        reachDestination = true;
    }

}
