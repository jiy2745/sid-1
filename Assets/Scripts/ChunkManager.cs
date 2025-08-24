using UnityEngine;
using System.Linq;

// 08/17 김지섭 - Refactored to hold reference to player animator and added isMoving flag
public class ChunkManager : MonoBehaviour
{
    public float moveSpeed = 5f;            // ûũ�� �������� �����̴� �ӵ�
    public float chunkWidth = 20f;          // ûũ�� �ʺ�
    public Transform[] chunks;              // Ÿ�ϸ� ûũ�� (Grid + Ÿ�ϸ� ���� ������Ʈ)

    [SerializeField] private bool isMoving = true; // Set to false to stop moving chunks
    [SerializeField] private Animator animator; // Reference to player animator

    void Start()
    {
        // 08/17 김지섭 - Initialize animator reference
        if (animator == null)
        {
            var player = GameObject.FindWithTag("Player");
            animator = player.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("Animator component not found.");
            }
        }
        isMoving = true; // Start moving chunks by default
    }

    void Update()
    {
        if (isMoving)
        {
            MoveChunks();
        }
    }

    private void MoveChunks()
    {
        if (animator != null)
        {
            // 08/17 김지섭 - Set player animator to move right when moving chunks
            animator.SetFloat("speed", 1);
            animator.SetFloat("moveX", 1);
        }
        foreach (Transform chunk in chunks)
        {
            chunk.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        // ���� ���ʿ� �ִ� ûũ�� ȭ�� ���� �ٱ����� �������� Ȯ��
        foreach (Transform chunk in chunks)
        {
            // ���� �� ûũ�� ������ ���� ȭ�� ���ʺ��� �� �������� �����ȴٸ�
            float rightEdge = chunk.position.x + chunkWidth / 2f;
            if (rightEdge < -chunkWidth / 2f)
            {
                // ���� �����ʿ� �ִ� ûũ ã��
                float maxX = chunks.Max(c => c.position.x);
                chunk.position = new Vector3(maxX + chunkWidth, chunk.position.y, 0f);
                //Debug.Log($"{chunk.name} ���ġ��: {chunk.position}");
            }
        }
    }

    public void StopMoving()
    {
        isMoving = false;
        if (animator != null)
        {
            animator.SetFloat("speed", 0);
            animator.SetFloat("moveX", 0);
        }
    }
}
