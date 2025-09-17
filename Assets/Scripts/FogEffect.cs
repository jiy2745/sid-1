using UnityEngine;
using UnityEngine.UI;

public class FogEffect : MonoBehaviour
{
    public RawImage fogImage;
    public float speedX = 0.02f;
    public float speedY = 0.01f;

    void Update()
    {
        if (fogImage != null)
        {
            Rect uv = fogImage.uvRect;
            uv.x += speedX * Time.deltaTime;
            uv.y += speedY * Time.deltaTime;
            fogImage.uvRect = uv;
        }
    }
}
