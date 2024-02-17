using System;
using System.Collections.Generic;
using UnityEngine;

public enum GridSize
{
    Size3x3,
    Size4x4,
    Size5x5
}

public class Board : MonoBehaviour
{
    public static Board Instance;

    public Action ElementPlaced;

    public GridSize gridSize;

    public List<Position> positions = new List<Position>();

    private Position[,] positionGrid;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        int gridSizeValue = 0;

        switch (gridSize)
        {
            case GridSize.Size3x3:
                gridSizeValue = 3;
                break;
            case GridSize.Size4x4:
                gridSizeValue = 4;
                break;
            case GridSize.Size5x5:
                gridSizeValue = 5;
                break;
        }

        positionGrid = new Position[gridSizeValue, gridSizeValue];

        for (int i = 0; i < positions.Count; i++)
        {
            positions[i].arrayIndex = i;
        }

        foreach (Position position in positions)
        {
            int row = position.arrayIndex / gridSizeValue;
            int col = position.arrayIndex % gridSizeValue;

            position.row = row;
            position.col = col;

            positionGrid[row, col] = position;
        }

        for (int row = 0; row < gridSizeValue; row++)
        {
            for (int col = 0; col < gridSizeValue; col++)
            {
                Position position = positionGrid[row, col];
            }
        }
        Invoke("OnElementPlaced", 0.1f);
    }
    public Position[] FindNearbyPositions(Position targetPosition)
    {
        List<Position> nearbyPositions = new List<Position>();
        int targetRow = targetPosition.row;
        int targetCol = targetPosition.col;

        if (targetRow - 1 >= 0)
        {
            Position nearbyPosition = positionGrid[targetRow - 1, targetCol];
            if (nearbyPosition != null && nearbyPosition.element == null)
            {
                nearbyPositions.Add(nearbyPosition);
            }
        }

        if (targetRow + 1 < positionGrid.GetLength(0))
        {
            Position nearbyPosition = positionGrid[targetRow + 1, targetCol];
            if (nearbyPosition != null && nearbyPosition.element == null)
            {
                nearbyPositions.Add(nearbyPosition);
            }
        }

        if (targetCol - 1 >= 0)
        {
            Position nearbyPosition = positionGrid[targetRow, targetCol - 1];
            if (nearbyPosition != null && nearbyPosition.element == null)
            {
                nearbyPositions.Add(nearbyPosition);
            }
        }

        if (targetCol + 1 < positionGrid.GetLength(1))
        {
            Position nearbyPosition = positionGrid[targetRow, targetCol + 1];
            if (nearbyPosition != null && nearbyPosition.element == null)
            {
                nearbyPositions.Add(nearbyPosition);
            }
        }

        return nearbyPositions.ToArray();
    }
    public List<Position> GetPositions()
    {
        return positions;
    }
    public Position[,] GetPositionGrid()
    {
        return positionGrid;
    }
    public void OnElementPlaced()
    {
        foreach (Position position in positions)
        {
            if (position.element != null)
            {
                if (position.element.IsFinish)
                {
                    position.element.CheckPathFilled();
                }
            }
        }
        foreach (Position position in positions)
        {
            if (position.element != null)
            {
                if (position.element.IsStart)
                {
                    if (position.element.IsFilled)
                        GameState.Instance.FinishGame();
                }
            }
        }
    }
}