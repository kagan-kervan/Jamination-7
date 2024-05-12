using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public WallBehaviour[] walls;
    public GameObject zombiePrefab;
    public int zombieSpawnCount;
    // Start is called before the first frame update
    void Start()
    {
        CreateWalls();
    }

    // Update is called once per frame
    void Update()
    { 
        // Check if the player clicks the mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // Check if there are available zombie spawns
            if (zombieSpawnCount > 0)
            {
                int response = CreateZombi();
                if(response == 1)
                {
                    // Decrease the zombie spawn count
                    zombieSpawnCount--;
                }
            }
        }

    }


    public int CreateZombi()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure the mouse position is at the same z-coordinate as the game objects

        // Instantiate a zombie at the mouse position
        Instantiate(zombiePrefab, mousePosition, Quaternion.identity);
        return 1;
    }

    public void CreateWalls()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].CreateWall();
        }
    }
}
