using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private Animator animator;  // Reference to animator
    [SerializeField] private Transform player;        // Reference to player
    [SerializeField] private List<float> distances;   // List of distances to maintain
    [SerializeField] private float moveSpeed = 2f;    // How fast the chaser moves
    [SerializeField] private float adjustSpeed = 1f;  // How fast it adjusts to new distance

    private int currentIndex = 0;
    private float targetDistance;
    private Coroutine adjustRoutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (distances.Count > 0)
            targetDistance = distances[currentIndex];
    }

    void Update()
    {
        if (player == null) return;

        // Direction to player
        Vector3 dir = (player.position - transform.position).normalized;

        // Desired position (player position - direction * targetDistance)
        Vector3 desiredPos = player.position - dir * targetDistance;

        // Move toward desired position
        transform.position = Vector3.MoveTowards(transform.position, desiredPos, moveSpeed * Time.deltaTime);
    }

    public void AdjustToNextDistance()
    {
        if (distances.Count == 0) return;

        currentIndex++;
        if (currentIndex >= distances.Count) return; // No more distances to adjust to
        float newDistance = distances[currentIndex];

        if (adjustRoutine != null)
            StopCoroutine(adjustRoutine);

        adjustRoutine = StartCoroutine(AdjustDistanceOverTime(newDistance));
    }

    private IEnumerator AdjustDistanceOverTime(float newDistance)
    {
        float startDistance = targetDistance;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * adjustSpeed;
            targetDistance = Mathf.Lerp(startDistance, newDistance, t);
            yield return null;
        }

        targetDistance = newDistance;
        adjustRoutine = null;
    }
    
    public void Attack()
    {
        if (animator != null)
        {
            animator.Play("monster_attack"); // Play attack animation
        }
    }
}