using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallBehaviour : MonoBehaviour
{
    public int Life;
    public float verticalOffset;
    public float horizontalOffset;
    public Vector2 startedPOS;
    public char[] spawnIndex;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CreateWall()
    {
        for(int i = 0; i < spawnIndex.Length; i++) 
        {
            switch (spawnIndex[i])
            {
                case 'N':
                    startedPOS.y = startedPOS.y + verticalOffset;
                    break;
                case 'S':
                    startedPOS.y -= verticalOffset;
                    break;
                case 'E':
                    startedPOS.x = startedPOS.x + horizontalOffset;
                    break;
                case 'W':
                    startedPOS.x -= horizontalOffset;
                    break;
            }
            Instantiate(this.gameObject,startedPOS,this.transform.rotation);
        }
    }
    public void GetHit()
    {
        Life--;
        if (CheckForAlive())
        {
            Destroy(this.gameObject);
        }
    }

    public bool CheckForAlive()
    {
        return Life <= 0;
    }
}
