using UnityEngine;
using System.Linq;

public class ChunkManager : MonoBehaviour
{
    public float moveSpeed = 5f;            // 청크가 왼쪽으로 움직이는 속도
    public float chunkWidth = 20f;          // 청크의 너비
    public Transform[] chunks;              // 타일맵 청크들 (Grid + 타일맵 포함 오브젝트)

    void Update()
    {
        // 모든 청크를 왼쪽으로 이동
        foreach (Transform chunk in chunks)
        {
            chunk.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        // 가장 왼쪽에 있는 청크가 화면 왼쪽 바깥으로 나갔는지 확인
        foreach (Transform chunk in chunks)
        {
            // 만약 이 청크의 오른쪽 끝이 화면 왼쪽보다 더 왼쪽으로 가버렸다면
            float rightEdge = chunk.position.x + chunkWidth / 2f;
            if (rightEdge < -chunkWidth / 2f)
            {
                // 가장 오른쪽에 있는 청크 찾기
                float maxX = chunks.Max(c => c.position.x);
                chunk.position = new Vector3(maxX + chunkWidth, chunk.position.y, 0f);
                Debug.Log($"{chunk.name} 재배치됨: {chunk.position}");
            }
        }
    }
}
