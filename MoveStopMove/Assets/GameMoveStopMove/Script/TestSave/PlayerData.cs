using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// save as Attribute
[System.Serializable]
public class PlayerData
{
    public int level;
    public int health;
    public float[] position;
    public PlayerData(PlayerTest playerTest)
    {
        level = playerTest.level;
        health = playerTest.health;
        position = new float[3];
        position[0] = playerTest.transform.position.x;
        position[1] = playerTest.transform.position.y;
        position[2] = playerTest.transform.position.z;
    }
}
