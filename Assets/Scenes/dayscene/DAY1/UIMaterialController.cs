using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIMaterialController : MonoBehaviour
{
    private Image image;
    private Material materialInstance;

    [Range(0, 1)]
    public float cutoff = 1f;

    void Awake()
    {
        image = GetComponent<Image>();
        
        materialInstance = new Material(image.material);
        image.material = materialInstance;
    }

   
    void Update()
    {
        materialInstance.SetFloat("_Cutoff", cutoff);
    }
}