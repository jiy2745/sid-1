using UnityEngine;
using System.Linq;

public class ChunkManager : MonoBehaviour
{
    public float moveSpeed = 5f;            // ûũ�� �������� �����̴� �ӵ�
    public float chunkWidth = 20f;          // ûũ�� �ʺ�
    public Transform[] chunks;              // Ÿ�ϸ� ûũ�� (Grid + Ÿ�ϸ� ���� ������Ʈ)

    void Update()
    {
        // ��� ûũ�� �������� �̵�
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
                Debug.Log($"{chunk.name} ���ġ��: {chunk.position}");
            }
        }
    }
}
