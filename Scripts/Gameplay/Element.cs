using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class Element : MonoBehaviour
{
    public bool IsStatic;
    public bool IsStart;
    public bool IsFinish;
    public bool IsFilled;

    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _filledSprite;
    [SerializeField] private Position _position;

    [SerializeField] private List<Element> _conections = new List<Element>();

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    private bool _isMouseDown = false;
    private bool _isSetAxisCalled = false;

    private Vector2 _touchStartPos;
    private Quaternion _startRotation;

    private Vector2 _minBound;
    private Vector2 _maxBound;

    private bool _isVertical;
    private bool _isHorizontal;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _startRotation = transform.rotation;
        _position = GetComponentInParent<Position>();
        _position.element = this;

        transform.SetParent(null);

        FreezeAll();

        if (IsFinish)
        {
            CheckPathFilled();
        }
    }
    private void OnMouseDown()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;
        _isMouseDown = true;
        _touchStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _isSetAxisCalled = false;

        GetBounds();
    }

    private void OnMouseUp()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        _isMouseDown = false;
        _isSetAxisCalled = false;
        var endPosition = FindNearestPosition(gameObject.transform.position, Board.Instance.GetPositions().ToArray());
        if (endPosition.element == null)
        {
            AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.ElementPlaced, 1f);
            transform.position = endPosition.position;
        }
        else
        {
            endPosition = _position;
            transform.position = endPosition.position;
        }

        _position.element = null;
        endPosition.element = this;
        _position = endPosition;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).position = transform.position;
        }

        Board.Instance.Invoke("OnElementPlaced", 0.1f);
    }

    private void Update()
    {
        UpdateElementRotation();

        if (IsStart || IsFinish || IsStatic)
            return;

        UpdatePlayerInput();
    }
    public void ConnectWith(Element connectElement)
    {
        if (!_conections.Contains(connectElement))
            _conections.Add(connectElement);
    }
    public void DisconnectWith(Element disconnectElement)
    { 
        if(_conections.Contains(disconnectElement))
            _conections.Remove(disconnectElement);
    }
    private void UpdateElementRotation()
    {
        transform.rotation = _startRotation;
    }
    public void CheckPathFilled()
    {
        var positions = Board.Instance.GetPositions();

        foreach (var position in positions)
        {
            if (position.element != null)
            {
                position.element.UnFill();
            }
        }
        if (_conections.Count != 0)
            _conections[0].Fill(this);
    }
    private void UpdatePlayerInput()
    {
        if (_isMouseDown)
        {
            Vector2 currentTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 swipeDelta = currentTouchPos - _touchStartPos;

            if (!_isSetAxisCalled)
            {
                if (swipeDelta.magnitude >= 0.05f)
                {
                    SetAxis(currentTouchPos);
                }
            }
            else
            {

                Vector2 clampedTouchPos = new Vector2(
                Mathf.Clamp(currentTouchPos.x, _minBound.x, _maxBound.x),
                Mathf.Clamp(currentTouchPos.y, _minBound.y, _maxBound.y)
                );

                _rigidbody.MovePosition(clampedTouchPos);

                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).position = transform.position;
                }
            }
        }
    }
    public void Fill(Element element)
    {
        _spriteRenderer.sprite = _filledSprite;
        IsFilled = true;
        if (_conections.Count != 0)
        {
            for (int i = 0; i < _conections.Count; i++)
            {
                if (!_conections[i].IsFinish)
                {
                    if (element != _conections[i])
                    {
                        _conections[i].Fill(this);
                    }
                }
            }
        }
    }
    public void UnFill()
    {
        IsFilled = false;
        _spriteRenderer.sprite = _defaultSprite;
    }
    private void GetBounds()
    {
        var positions = Board.Instance.GetPositionGrid();

        _minBound = positions[positions.GetLength(0) - 1, 0].position;
        _maxBound = positions[0, positions.GetLength(1) - 1].position;
    }
    private void SetAxis(Vector2 position)
    {
        Vector2 swipeDirection = position - _touchStartPos;

        float xAbs = Mathf.Abs(swipeDirection.x);
        float yAbs = Mathf.Abs(swipeDirection.y);

        bool horizontalAvaiable = false;
        bool verticalAvaiable = false;

        foreach (Position pos in Board.Instance.FindNearbyPositions(_position))
        {
            if (pos.element == null)
            {
                if (pos.col == _position.col)
                    verticalAvaiable = true;
                if (pos.row == _position.row)
                    horizontalAvaiable = true;
            }
        }

        if (horizontalAvaiable || verticalAvaiable)
        {
            foreach (Position pos in Board.Instance.GetPositions())
            {
                if (pos.element != null && pos.element != this)
                    pos.element.FreezeAll();
            }
        }

        if (verticalAvaiable && horizontalAvaiable)
        {
            if (xAbs > yAbs)
            {
                _isVertical = false;
                _isHorizontal = true;
            }
            else
            {
                _isVertical = true;
                _isHorizontal = false;
            }
            UpdateRigidBodySettings();
        }
        else if (horizontalAvaiable)
        {
            _isVertical = false;
            _isHorizontal = true;
            UpdateRigidBodySettings();
        }
        else if (verticalAvaiable)
        {
            _isVertical = true;
            _isHorizontal = false;
            UpdateRigidBodySettings();
        }
        _isSetAxisCalled = true;
    }
    public void FreezeAll()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    private void UpdateRigidBodySettings()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.None;

        if (_isVertical)
            _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
        else
            _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
    }
    private Position FindNearestPosition(Vector3 position, Position[] positions)
    {
        Position nearestPosition = null;
        float shortestDistance = float.MaxValue;
        Vector3 currentPosition = transform.position;

        foreach (Position pos in positions)
        {
            float distance = Vector3.Distance(currentPosition, pos.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestPosition = pos;
            }
        }
        return nearestPosition;
    }
}