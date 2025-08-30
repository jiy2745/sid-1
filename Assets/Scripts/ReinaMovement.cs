using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReinaMovement : MonoBehaviour
{
    [Header("랜덤 설정 범위")]
    [Tooltip("각 NPC의 이동 속도를 이 범위(X=최소, Y=최대) 안에서 랜덤으로 결정합니다.")]
    public Vector2 speedRange = new Vector2(1.5f, 2.5f);
    [Space]
    [Tooltip("각 NPC의 대기 시간 범위를 이 범위(X=최소, Y=최대) 안에서 랜덤으로 결정합니다.")]
    public Vector2 waitTimeRange = new Vector2(1.0f, 4.0f);
    [Space]
    [Tooltip("각 NPC의 이동 시간 범위를 이 범위(X=최소, Y=최대) 안에서 랜덤으로 결정합니다.")]
    public Vector2 moveTimeRange = new Vector2(1.0f, 3.0f);

    [Header("이동 제한 범위")]
    public float maxX = 10f;
    public float minX = -10f;
    public float maxY = 5f;
    public float minY = -5f;

    
    private float moveSpeed;
    private float minWaitTime;
    private float maxWaitTime;
    private float minMoveTime;
    private float maxMoveTime;

  
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movementDirection;
    private bool isMoving = false;
    private Vector2 lastBlockedDirection = Vector2.zero;
    private Coroutine movementCycleCoroutine;
    private bool isHandlingCollision = false;

    
    void Awake()
    {
        
        moveSpeed = Random.Range(speedRange.x, speedRange.y);

       
        minWaitTime = Random.Range(waitTimeRange.x, waitTimeRange.y);
        maxWaitTime = Random.Range(minWaitTime, waitTimeRange.y);

        
        minMoveTime = Random.Range(moveTimeRange.x, moveTimeRange.y);
        maxMoveTime = Random.Range(minMoveTime, moveTimeRange.y);
        
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        if (anim != null)
        {
            anim.applyRootMotion = false;
        }

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }
    
    private void OnEnable()
    {
        if (movementCycleCoroutine != null)
        {
            StopCoroutine(movementCycleCoroutine);
        }
        movementCycleCoroutine = StartCoroutine(MovementCycle());
    }
    
    private void OnDisable()
    {
        if (movementCycleCoroutine != null)
        {
            StopCoroutine(movementCycleCoroutine);
        }
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
        
        yield return new WaitForSeconds(Random.Range(0f, 2f));

        while (true)
        {
            isMoving = false;
            if (anim != null) anim.SetFloat("speed", 0f);
            
            
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            ChooseNewDirection();
            isMoving = true;
            
            
            float moveTime = Random.Range(minMoveTime, maxMoveTime);
            yield return new WaitForSeconds(moveTime);
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
        
        if (anim == null) return;
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