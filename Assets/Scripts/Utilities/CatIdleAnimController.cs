using UnityEngine;

public class CatIdleAnimController : MonoBehaviour
{
    private Animator animator;
    private float timer;

    public float transitionTime = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        ResetTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            int choice = Random.Range(0, 3); // 0 = idle, 1 = lick, 2 = groom
            if (choice == 1)
                animator.SetTrigger("Lick");
            else if (choice == 2)
                animator.SetTrigger("Groom");

            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = transitionTime;
    }
}

