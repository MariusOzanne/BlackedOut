using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UIElements;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject mazeCellPrefab;

    [SerializeField]
    private GameObject SpawnerPrefab;

    [SerializeField]
    private int mazeWidth;

    [SerializeField]
    private int mazeDepth;

    [SerializeField]
    private GameObject ground;

    [SerializeField]
    private float bigRoomSize;

    [SerializeField]
    private float averageRoomSize;

    [SerializeField]
    private float smallRoomSize;

    private MazeCell[,] mazeGrid;
    private List<Vector3> spawnerPositions = new List<Vector3>();

    private float cellWidth;

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
                MazeCell tmpCell = cell.GetComponent<MazeCell>();
                cellWidth = tmpCell.CellWidth;
                cell.transform.position = new Vector3(tmpCell.CellWidth * x, 0, tmpCell.CellWidth * z);
                tmpCell.x = x;
                tmpCell.z = z;
                mazeGrid[x, z] = tmpCell;
            }
        }
        // Generate the first maze cell
        GenerateRoomFromCenterPoint(mazeGrid[mazeWidth / 2, mazeDepth / 2], bigRoomSize);
        GenerateMaze(null, mazeGrid[0, 0]);
        SpawnerPositions();
        // Generate rooms at designated points in the maze
        foreach (Vector3 position in spawnerPositions)
        {
            GenerateRoomAtPosition(position, smallRoomSize);
        }
        // Generate n spawners
        for (int i = 0; i < spawnerPositions.Count; i++)
        {
            Vector3 position = RandomSpawnerPosition();
            GenerateSpawner(position, SpawnerPrefab);
            spawnerPositions.Find(x => spawnerPositions[i] == position);
        }

        // generate a new NavMeshSurface during runtime to take walls into account
        ground.GetComponent<NavMeshSurface>().BuildNavMesh();

    }

    private void GenerateRoomFromCenterPoint(MazeCell centerCell, float roomWidth)
    {

        Vector3 roomCenter = centerCell.transform.position;

        Debug.Log(roomCenter);
        Debug.Log(mazeGrid[mazeWidth / 2, mazeDepth / 2].transform.position);

        // remove all objects in a given area
        foreach (MazeCell cell in mazeGrid)
        {
            if (Vector3.Distance(cell.transform.position, centerCell.transform.position) <= roomWidth)
            {
                cell.ClearObject();
            }
        }
    }

    private void GenerateRoomAtPosition(Vector3 position, float roomWidth)
    {
        foreach(MazeCell cell in mazeGrid)
        {
            if (Vector3.Distance(cell.transform.position, position) <= roomWidth)
            {
                cell.ClearObject();
            }
        }
    }

    private void GenerateSpawner(Vector3 position, GameObject spawnerObject)
    {
        Instantiate(spawnerObject, position, Quaternion.identity);
    }

    private void SpawnerPositions()
    {
        MazeCell templateMazeCall = new MazeCell();

        //Bot Left Portal
        spawnerPositions.Add(new Vector3(mazeDepth * cellWidth * 0.2f, 0, mazeWidth * cellWidth * 0.2f));
        //Bot Right Portal
        spawnerPositions.Add(new Vector3(mazeDepth * cellWidth * 0.2f, 0, mazeWidth * cellWidth * 0.8f));
        //Top Right Portal
        spawnerPositions.Add(new Vector3(mazeDepth * cellWidth * 0.8f, 0, mazeWidth * cellWidth * 0.8f));
        //Top Left Portal
        spawnerPositions.Add(new Vector3(mazeDepth * cellWidth * 0.8f, 0, mazeWidth * cellWidth * 0.2f));

        // Left Mid Portal
        spawnerPositions.Add(new Vector3(mazeDepth * cellWidth * 0.2f, 0, mazeWidth * cellWidth * 0.5f));
        //Right Mid portal
        spawnerPositions.Add(new Vector3(mazeDepth * cellWidth * 0.8f, 0, mazeWidth * cellWidth * 0.5f));
        //Mid Low portal 
        spawnerPositions.Add(new Vector3(mazeDepth * cellWidth * 0.5f, 0, mazeWidth * cellWidth * 0.2f));
        //Mid High portal
        spawnerPositions.Add(new Vector3(mazeDepth * cellWidth * 0.5f, 0, mazeWidth * cellWidth * 0.8f));
    }

    private Vector3 RandomSpawnerPosition()
    {
        Vector3 randomPos = spawnerPositions[UnityEngine.Random.Range(0, spawnerPositions.Count)];
        spawnerPositions.Remove(spawnerPositions[UnityEngine.Random.Range(0, spawnerPositions.Count)]);
        return randomPos;
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

        return unvisitedCells.OrderBy(_ => UnityEngine.Random.Range(1, 10)).FirstOrDefault();
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
}
