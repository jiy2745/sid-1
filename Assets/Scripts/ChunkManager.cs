using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    public Transform player;
    public float chunkHeight = 20f; // 한 블록의 높이
    public int chunkCount = 3; // 준비된 블록 수
    public Tilemap[] chunks;

    private float thresholdY;

    void Start()
    {
        thresholdY = chunkHeight; // 플레이어가 1블록 올라가면 체크 시작
    }

    void Update()
    {
        if (player.position.y > thresholdY)
        {
            MoveBottomChunkToTop();
            thresholdY += chunkHeight;
        }
    }

    void MoveBottomChunkToTop()
    {
        // 맨 아래 chunk를 찾아서 가장 위로 올림
        Tilemap bottomChunk = chunks.OrderBy(c => c.transform.position.y).First();
        float topY = chunks.Max(c => c.transform.position.y);
        bottomChunk.transform.position = new Vector3(bottomChunk.transform.position.x, topY + chunkHeight, 0f);
    }
}
