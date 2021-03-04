using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPosition : MonoBehaviour
{
    public bool northWall;
    public bool eastWall;
    public bool southWall;
    public bool westWall;

    // m_walls is an integer with the walls position
    private int m_walls;
    
    void Awake()
    {
        m_walls = 0;
        if (northWall) m_walls += 1; // first bit is north wall
        if (eastWall) m_walls += 2; // second bit is east wall
        if (southWall) m_walls += 4; // third bit is south wall
        if (westWall) m_walls += 8; // fourth bit is west wall
    }

    // GetWalls function return the integer with walls value
    public int GetWalls()
    {
        return m_walls;
    }

    // SetWalls function set the integer that define walls
    public void SetWalls(int walls)
    {
        m_walls = walls;
    }

    // RotateRight function change the value of m_walls corresponding to a rotation to the right
    // north => east, east => south, south => west, west => north
    // Shifting to the less significant bit and add the previous LSB value at MSB
    public void RotateRight()
    {
        if (m_walls % 2 != 0) m_walls = m_walls + 16;
        m_walls = m_walls >> 1;
    }

    // RotateLeft function change the value of m_walls corresponding to a rotation to the left
    // north => west, east => north, south => east, west => south
    // Shifting to the most significant bit and truncate m_walls to 4 bits, changing the LSB value if m_walls had a 5th bit value
    public void RotateLeft()
    {
        m_walls = m_walls << 1;
        if (m_walls > 15) m_walls = m_walls % 16 + 1;
    }

}
