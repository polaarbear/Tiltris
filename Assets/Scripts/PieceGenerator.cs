using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceGenerator", menuName = "Scriptables/Piece Generator", order = 0)]
public class PieceGenerator : ScriptableObject
{
    [SerializeField]
    private List<Tetromino> _TetrominoPrefabs = new List<Tetromino>();

    public Tetromino GetNextPiece(Transform gameGrid)
    {
        return Instantiate(_TetrominoPrefabs[Random.Range(0, 6)], gameGrid);
    }
}
