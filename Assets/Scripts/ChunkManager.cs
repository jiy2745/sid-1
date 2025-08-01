using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    public Transform player;
    public float chunkHeight = 20f; // �� ����� ����
    public int chunkCount = 3; // �غ�� ��� ��
    public Tilemap[] chunks;

    private float thresholdY;

    void Start()
    {
        thresholdY = chunkHeight; // �÷��̾ 1��� �ö󰡸� üũ ����
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
        // �� �Ʒ� chunk�� ã�Ƽ� ���� ���� �ø�
        Tilemap bottomChunk = chunks.OrderBy(c => c.transform.position.y).First();
        float topY = chunks.Max(c => c.transform.position.y);
        bottomChunk.transform.position = new Vector3(bottomChunk.transform.position.x, topY + chunkHeight, 0f);
    }
}
