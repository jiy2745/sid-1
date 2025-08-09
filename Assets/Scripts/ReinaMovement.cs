using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
public class ReinaMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;

    [Header("Movement Boundaries")]
    public float maxX = 10f;
    public float minX = -10f;
    public float maxY = 5f;
    public float minY = -5f;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movementDirection;
    private bool isMoving = false;
    private Vector2 lastBlockedDirection = Vector2.zero;

    private Coroutine movementCycleCoroutine;
    
    private bool isHandlingCollision = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (anim != null)
        {
            anim.applyRootMotion = false;
        }

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        
        movementCycleCoroutine = StartCoroutine(MovementCycle());
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Move();
        }
    }

    private IEnumerator MovementCycle()
    {
        while (true)
        {
            isMoving = false;
            if(anim != null) anim.SetFloat("speed", 0f);
            yield return new WaitForSeconds(2f);

            ChooseNewDirection();
            isMoving = true;
            
            yield return new WaitForSeconds(2f);
        }
    }

    private void ChooseNewDirection()
    {
        List<Vector2> possibleDirections = new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        
        if (lastBlockedDirection != Vector2.zero)
        {
            possibleDirections.Remove(lastBlockedDirection);
        }

        
        if (possibleDirections.Count == 0) 
        {
            
            lastBlockedDirection = Vector2.zero;
            possibleDirections = new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        }

        int directionIndex = Random.Range(0, possibleDirections.Count);
        movementDirection = possibleDirections[directionIndex];
        
        lastBlockedDirection = Vector2.zero;
        
        if(anim == null) return;
        anim.SetFloat("moveX", movementDirection.x);
        anim.SetFloat("moveY", movementDirection.y);
        anim.SetFloat("lastMoveX", movementDirection.x);
        anim.SetFloat("lastMoveY", movementDirection.y);
        anim.SetFloat("speed", movementDirection.sqrMagnitude);
    }

    private void Move()
    {
        Vector2 currentPos = rb.position;
        Vector2 newPos = currentPos + movementDirection * moveSpeed * Time.fixedDeltaTime;

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        rb.MovePosition(newPos);
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollision();
    }

    
    private void HandleCollision()
    {
        
        if (!isMoving || isHandlingCollision) return;
        
        
        StartCoroutine(UnstickAndRestart());
    }
    
    
    private IEnumerator UnstickAndRestart()
    {
        isHandlingCollision = true;
        isMoving = false;

        
        if (movementCycleCoroutine != null)
        {
            StopCoroutine(movementCycleCoroutine);
        }

        
        lastBlockedDirection = movementDirection;
        if(anim != null) anim.SetFloat("speed", 0f);

       
        Vector2 bounceDirection = -movementDirection.normalized;
        float bounceDistance = 0.15f; 
        Vector2 targetPosition = rb.position + bounceDirection * bounceDistance;
        rb.MovePosition(targetPosition); 
        
        
        yield return new WaitForSeconds(0.1f);

        
        movementCycleCoroutine = StartCoroutine(MovementCycle());
        
        isHandlingCollision = false;
    }
}