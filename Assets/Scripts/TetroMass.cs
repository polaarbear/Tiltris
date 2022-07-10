using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetroMass : MonoBehaviour
{
    public HashSet<Vector2> OccupiedCells { get; private set; } = new HashSet<Vector2>();

    public bool IsCollided(Tetron tetron)
    {
        //The cell directly below the tetron
        Vector2 dangerCell = new Vector2((int)tetron.transform.position.x + GameGrid.CENTER_OFFSET, (int)tetron.transform.position.y - 1f + GameGrid.CENTER_OFFSET);
        foreach (Vector2 occupiedCell in OccupiedCells)
        {
            if (occupiedCell == dangerCell)
            {
                if (tetron.transform.position.y - occupiedCell.y <= 1.01f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsGrounded(Tetron tetron)
    {
        if (Mathf.Abs(tetron.transform.position.y - GameGrid.CENTER_OFFSET) < 0.01f)
            return true;
        else
            return false;
    }

    public void AddTetron(Tetron tetron)
    {
        Vector2 tetronPosition = new Vector2((int)tetron.transform.position.x + GameGrid.CENTER_OFFSET, (int)tetron.transform.position.y + GameGrid.CENTER_OFFSET);
        OccupiedCells.Add(tetronPosition);
        tetron.transform.parent = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
