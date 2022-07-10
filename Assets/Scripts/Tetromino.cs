using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tetromino : MonoBehaviour
{
    public float XOffset = 0f;

    [SerializeField]
    private char _Shape = 'N';

    private bool _IsOffset = false;
    public List<Tetron> Tetrons { get; private set; }

    private void Awake()
    {
        name = "NextPiece-" + _Shape;
    }
    // Start is called before the first frame update
    void Start()
    {
        Tetrons = GetComponentsInChildren<Tetron>().ToList();
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0f, 0f, 90f));
        if (_Shape != 'O')
        {
            if (_IsOffset)
            {
                transform.Translate(new Vector2(0.5f, 0f), Space.World);
            }
            else
            {
                transform.Translate(new Vector2(-0.5f, 0f), Space.World);
            }
            _IsOffset = !_IsOffset;
        }
        PreventWallClip();
    }

    private void PreventWallClip()
    {
        foreach (Tetron tetron in Tetrons)
        {
            Vector2 vector = Vector2.zero;
            if (tetron.transform.position.x < 0f)
            {
                vector.x = 1f;
            }
            else if (tetron.transform.position.x > 10f)
            {
                vector.x = -1f;
            }
            
            if(tetron.transform.position.y < 1f)
            {
                vector.y = 1f;
            }
            if(vector != Vector2.zero)
                transform.Translate(vector, Space.World);
        }
    }
}
