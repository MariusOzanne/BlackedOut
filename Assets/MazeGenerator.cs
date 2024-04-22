using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject mazeCellPrefab;

    [SerializeField]
    private int mazeWidth;

    [SerializeField]
    private int mazeDepth;

    private MazeCell[,] mazeGrid;

    void Start()
    {
        // create an empty array of mazeCells
        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

        // instantiate the required prefab at every position on a grid
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeDepth; z++)
            {
                GameObject cell = Instantiate(mazeCellPrefab, Vector3.zero, Quaternion.identity);
                MazeCell AzeCell = cell.GetComponent<MazeCell>();
                cell.transform.position = new Vector3(AzeCell.CellWidth * x, 0, AzeCell.CellWidth * z);
                AzeCell.x = x;
                AzeCell.z = z;
                mazeGrid[x, z] = AzeCell;

                /*Debug.Log(mazeCellPrefab.CellWidth);
                Debug.Log(mazeCellPrefab.CellWidth * x + "x coord");
                Debug.Log(mazeCellPrefab.CellWidth * z + "z coord");*/
            }
        }
        // Generate the first maze cell
        GenerateMaze(null, mazeGrid[0, 0]);

        GenerateRoomAtPointInArray(mazeGrid[mazeWidth/2, mazeDepth/2]);
    }

    private void GenerateRoomAtPointInArray(MazeCell centerCell)
    {
        int roomWidth = 5; 
        int roomDepth = 5;

        for (int x = 0;x < roomWidth; x++)
        {

        }
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;
        // Generate all maze cells
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        /*int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;*/

        if (currentCell.x + 1 < mazeWidth)
        {
            var cellToRight = mazeGrid[currentCell.x + 1, currentCell.z];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (currentCell.x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[currentCell.x - 1, currentCell.z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (currentCell.z + 1 < mazeDepth)
        {
            var cellToFront = mazeGrid[currentCell.x, currentCell.z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (currentCell.z - 1 >= 0)
        {
            var cellToBack = mazeGrid[currentCell.x, currentCell.z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    private void CreateRoom()
    {

    }
}
