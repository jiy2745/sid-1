using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public List<float> playerDistances; // Distance from player for each level

    private Animator animator;
    private GameObject player; // Reference to the player object
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    //TODO: Implement movement towards player based on distance for each level, reference the minigame manager if needed
}
