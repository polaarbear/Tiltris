using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameGrid : MonoBehaviour
{
    public static readonly float CENTER_OFFSET = 0.5f;

    [SerializeField]
    private PieceGenerator _PieceGenerator;

    private bool _CanMove = true;
    private bool _MoveLeft = false;
    private bool _LeftEdge = false;
    private bool _MoveRight = false;
    private bool _RightEdge = false;
    private float _MoveInterval = 0.5f;
    private float _TimeSinceMove = 0f;

    private bool _CanRotate = true;
    private bool _Rotate = false;
    private float _RotateInterval = 0.5f;
    private float _TimeSinceRotate = 0f;

    private bool _Locking = false;
    private float _LockInterval = 1.5f;
    private float _LockTimer = 0f;




    private Tetromino _NextDrop;
    private Tetromino _ActivePiece;

    [SerializeField]
    private TetroMass _TetroMass;

    private bool _Paused = true;
    private float _GameSpeed = 1.0f;
    private float _DropRate = -3.0f;

    private void Awake()
    {
        GenerateNextPiece();
        GenerateNextPiece();
    }
    // Start is called before the first frame update
    void Start()
    {
        _Paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_Paused)
        {            
            float deltaTime = Time.deltaTime;
            ProcessMovement(deltaTime);
            ProcessRotation(deltaTime);
            if (!_Locking)
            {
                DropActivePiece(deltaTime);
                CheckCollided();
            }
            else
            {
                ProcessPieceLock(deltaTime);
            }
        }
    }

    private void GenerateNextPiece()
    {
        if(_ActivePiece != null)
        {
            Destroy(_ActivePiece.gameObject);
        }
        if(_NextDrop != null)
        {
            _ActivePiece = _NextDrop;
            _ActivePiece.name = _ActivePiece.name.Replace("Next", "Active");
        }
        _NextDrop = _PieceGenerator.GetNextPiece(transform);
        _NextDrop.transform.Translate(new Vector2(_NextDrop.XOffset, 0f));
        _LeftEdge = _RightEdge = false;
    }

    private void DropActivePiece(float deltaTime)
    {
        float dropDistance = _GameSpeed * _DropRate * deltaTime;        
        _ActivePiece.transform.Translate(new Vector2(0f, dropDistance), Space.World);
    }

    public void ToggleLeft(InputAction.CallbackContext context)
    {
        _MoveLeft = context.ReadValueAsButton();
    }

    public void ToggleRight(InputAction.CallbackContext context)
    {
        _MoveRight = context.ReadValueAsButton();
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        _Rotate = context.ReadValueAsButton();

        //Reset rotation if the key is de-pressed
        if (!_Rotate) { 
            _CanRotate = true;
            _TimeSinceRotate = 0f;
        }
    }

    private void ProcessMovement(float deltaTime)
    {
        if (_CanMove)
        {
            if (_MoveRight && _MoveLeft)
            {
                //No movement, no penalty
            }
            else if (_MoveRight && !_RightEdge)
            {
                _ActivePiece.transform.Translate(new Vector2(1f, 0f), Space.World);
                CheckRightEdge(false);
                _CanMove = false;
            }
            else if (_MoveLeft && !_LeftEdge)
            {
                _ActivePiece.transform.Translate(new Vector2(-1f, 0f), Space.World);
                CheckLeftEdge(false);
                _CanMove = false;
            }
        }
        else
        {
            if (_MoveRight || _MoveLeft)
            {
                _TimeSinceMove += deltaTime;
                if (_TimeSinceMove > _MoveInterval)
                {
                    _TimeSinceMove = 0f;
                    _CanMove = true;
                }
            }
            else //Reset move if both keys are de-pressed
            {
                _TimeSinceMove = 0f;
                _CanMove = true;
            }
        }
    }

    private void ProcessRotation(float deltaTime)
    {
        if(_CanRotate)
        {
            if(_Rotate)
            {
                _ActivePiece.Rotate();
                CheckRightEdge(true);
                CheckLeftEdge(true);
                _CanRotate = false;
            }
        }
        else
        {
            if(_Rotate)
            {
                _TimeSinceRotate += deltaTime;
                if(_TimeSinceRotate >= _RotateInterval)
                {
                    _TimeSinceRotate = 0f;
                    _CanRotate = true; //Allow a 2nd rotation each time the interval passes while the key is held
                }
            }
        }
    }    

    private void ProcessPieceLock(float deltaTime)
    {
        _LockTimer += deltaTime;
        if(_LockTimer >= _LockInterval)
        {
            SnapToGrid();
            AddTetronsToMass();
            GenerateNextPiece();
            _Locking = false;
            _LockTimer = 0f;
        }
    }

    private void CheckRightEdge(bool checkBoth)
    {
        if(!checkBoth)
            _LeftEdge = false;
        bool rightEdge = false;
        foreach(Tetron tetron in _ActivePiece.Tetrons)
        {
            if(tetron.transform.position.x > 9f)
            {
                rightEdge = true;
                break;
            }
        }
        _RightEdge = rightEdge;
    }

    private void CheckLeftEdge(bool checkBoth)
    {
        if(!checkBoth)
            _RightEdge = false;
        bool leftEdge = false;
        foreach (Tetron tetron in _ActivePiece.Tetrons)
        {
            if (tetron.transform.position.x < 1f)
            {
                leftEdge = true;
                break;
            }
        }
        _LeftEdge = leftEdge;
    }

    private void CheckCollided()
    {
        foreach(Tetron tetron in _ActivePiece.Tetrons)
        {
            if(_TetroMass.IsCollided(tetron) || _TetroMass.IsGrounded(tetron))
            {
                _Locking = true;
                return;
            }
        }
        _Locking = false;
    }

    private void SnapToGrid()
    {
        _ActivePiece.transform.position = new Vector2(_ActivePiece.transform.position.x, Mathf.Round(_ActivePiece.transform.position.y * 2f) / 2);
    }

    private void AddTetronsToMass()
    {
        foreach(Tetron tetron in _ActivePiece.Tetrons)
        {
            _TetroMass.AddTetron(tetron);
        }
    }
}
