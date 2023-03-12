using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using CodeMonkey.Utils;

public class MyPathfiding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private int2[] savedPath;
    private WorldGenerationHandler worldGenerationHandler;
    private void Awake()
    {
        worldGenerationHandler = GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>();
    }
    //private void Start()
    //{
    //    FunctionPeriodic.Create(() => {
    //        float startTime = Time.realtimeSinceStartup;

    //        int findPathJobCount = 5;
    //        NativeArray<JobHandle> jobHandleArray = new NativeArray<JobHandle>(findPathJobCount, Allocator.TempJob);

    //        for (int i = 0; i < findPathJobCount; i++)
    //        {
    //            FindPathJob findPathJob = new FindPathJob
    //            {
    //                startPosition = new int2(0, 0),
    //                endPosition = new int2(99, 99)
    //            };
    //            jobHandleArray[i] = findPathJob.Schedule();
    //        }

    //        JobHandle.CompleteAll(jobHandleArray);
    //        jobHandleArray.Dispose();

    //        Debug.Log("Time: " + ((Time.realtimeSinceStartup - startTime) * 1000f));
    //    }, 1f);
    //}
    public void MoveTo(Vector3 position)
    {
        int2 startPos = new int2();
        int2 endPos = new int2();

        worldGenerationHandler.blockGrid.GetXYZ(transform.position, out startPos.x, out startPos.y);
        worldGenerationHandler.blockGrid.GetXYZ(position, out endPos.x, out endPos.y);
        BlockInfo currBlockInfo = worldGenerationHandler.blockGrid.GetValue(endPos.x, endPos.y);

        //Debug.Log("Start Pos " + startPos + " end Pos " + endPos);
        //Debug.Log(currBlockInfo.walkable);
        if (currBlockInfo == null) return;
        if (!currBlockInfo.walkable) return;

        //Debug.Log("X " + x*2 + "  Y " + y*2);
        //Debug.Log("Looking for path from " + startPos*2 + " to " + endPos*2);

        bool[,] blockIsWalkable = new bool[worldGenerationHandler.blockGrid.GetLenght(0), worldGenerationHandler.blockGrid.GetLenght(1)];

        for (int i = 0; i < worldGenerationHandler.blockGrid.GetLenght(0); i++)
        {
            for (int j = 0; j < worldGenerationHandler.blockGrid.GetLenght(1); j++)
            {
                blockIsWalkable[i, j] = worldGenerationHandler.blockGrid.GetValue(i, j).walkable;
            }
        }


        //NativeArray<int2> finalPath = new NativeArray<int2>(50, Allocator.TempJob);

        float startTime = Time.realtimeSinceStartup;
        FindPathJob findPathJob = new FindPathJob
        {
            startPosition = startPos,
            endPosition = endPos,
            blockIsWalkable = blockIsWalkable,
            //blockInfoGrid = GameObject.Find("Test").GetComponent<AnotherTest>().blockGrid,
        };
        findPathJob.Execute();
        //foreach(NativeArray<bool> naraz in blockIsWalkable)
        //{
        //    naraz.Dispose();
        //}
        if (findPathJob.finalPath.Length > 0)
        {

            savedPath = new int2[findPathJob.finalPath.Length];
            findPathJob.finalPath.CopyTo(savedPath);
            //Debug.Log(savedPath.Length + " 0 " + findPathJob.finalPath[0] + " 1 " + findPathJob.finalPath[1]);
            //finalPath.Dispose();
            //Debug.Log("Time: " + ((Time.realtimeSinceStartup - startTime) * 1000f));

            //Debug.DrawLine(new Vector3(startPos.x, startPos.y), new Vector3(endPos.x, endPos.y), Color.white);
            int2 temp = new int2(-100, -100);
            int mapWidth = worldGenerationHandler.mapSettings.width;
            int mapHeight = worldGenerationHandler.mapSettings.height;
            float mapCellSize = worldGenerationHandler.mapSettings.cellSize;
            foreach (int2 node in findPathJob.finalPath)
            {
                if (temp.x != -100) Debug.DrawLine(new Vector3((temp.x * mapCellSize * 2) - mapWidth, (temp.y * mapCellSize * 2) - mapHeight), new Vector3((node.x * mapCellSize * 2) - mapWidth, (node.y * mapCellSize * 2) - mapHeight), Color.white, 100f);
                if (Vector3.Distance(transform.position, new Vector3((node.x * 2) - worldGenerationHandler.mapSettings.width, (node.y * 2) - worldGenerationHandler.mapSettings.height)) < 1f)
                {
                    //Debug.Log("ajaja");
                }
                //GetComponent<MovePositionDirect>().SetMovePosition(new Vector3((node.x * 2) - mapSettings.width, (node.y * 2) - mapSettings.height));
                temp = node;
                //Debug.Log(node);
            }
            //GetComponent<MovePositionNodes>().SetMovePosition(savedPath, endPos); //////////////////////////////////potrebne k funkcii kocu zakomentovane len docasne
            findPathJob.finalPath.Dispose();
        }
    }
    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0) && !GameObject.Find("Test").GetComponent<AnotherTest>().IsMouseOverUI()) //mouse leftcik
    //    {
    //        int x, y, z;
    //        GameObject.Find("Test").GetComponent<AnotherTest>().blockGrid.GetXYZ(transform.position, out x, out y, out z);
    //        int2 startPos = new int2(x, y);
    //        GameObject.Find("Test").GetComponent<AnotherTest>().blockGrid.GetXYZ(UtilsClass.GetMouseWorldPosition(Mathf.Abs(Camera.main.transform.position.z)), out x, out y, out z);
    //        int2 endPos = new int2(x, y);
    //        BlockInfo currBlockInfo = GameObject.Find("Test").GetComponent<AnotherTest>().blockGrid.GetValue(endPos.x, endPos.y);

    //        //Debug.Log("Start Pos " + startPos + " end Pos " + endPos);
    //        //Debug.Log(currBlockInfo.walkable);
    //        if (currBlockInfo == null) return;
    //        if (!currBlockInfo.walkable) return;

    //        //Debug.Log("X " + x*2 + "  Y " + y*2);
    //        //Debug.Log("Looking for path from " + startPos*2 + " to " + endPos*2);

    //        bool[,] blockIsWalkable = new bool[GameObject.Find("Test").GetComponent<AnotherTest>().blockGrid.GetLenght(0), GameObject.Find("Test").GetComponent<AnotherTest>().blockGrid.GetLenght(1)];

    //        for (int i = 0; i < GameObject.Find("Test").GetComponent<AnotherTest>().blockGrid.GetLenght(0); i++)
    //        {
    //            for (int j = 0; j < GameObject.Find("Test").GetComponent<AnotherTest>().blockGrid.GetLenght(1); j++)
    //            {
    //                blockIsWalkable[i,j] = GameObject.Find("Test").GetComponent<AnotherTest>().blockGrid.GetValue(i, j).walkable;
    //            }
    //        }


    //        //NativeArray<int2> finalPath = new NativeArray<int2>(50, Allocator.TempJob);

    //        float startTime = Time.realtimeSinceStartup;
    //        FindPathJob findPathJob = new FindPathJob
    //        {
    //            startPosition = startPos,
    //            endPosition = endPos,
    //            blockIsWalkable = blockIsWalkable,
    //            //blockInfoGrid = GameObject.Find("Test").GetComponent<AnotherTest>().blockGrid,
    //        };
    //        findPathJob.Execute();
    //        //foreach(NativeArray<bool> naraz in blockIsWalkable)
    //        //{
    //        //    naraz.Dispose();
    //        //}
    //        if (findPathJob.finalPath.Length > 0)
    //        {

    //            savedPath = new int2[findPathJob.finalPath.Length];
    //            findPathJob.finalPath.CopyTo(savedPath);
    //            //Debug.Log(savedPath.Length + " 0 " + findPathJob.finalPath[0] + " 1 " + findPathJob.finalPath[1]);
    //            //finalPath.Dispose();
    //            //Debug.Log("Time: " + ((Time.realtimeSinceStartup - startTime) * 1000f));

    //            //Debug.DrawLine(new Vector3(startPos.x, startPos.y), new Vector3(endPos.x, endPos.y), Color.white);
    //            int2 temp = new int2(-100, -100);
    //            int mapWidth = GameObject.Find("Test").GetComponent<AnotherTest>().mapSettings.width;
    //            int mapHeight = GameObject.Find("Test").GetComponent<AnotherTest>().mapSettings.height;
    //            float mapCellSize = GameObject.Find("Test").GetComponent<AnotherTest>().mapSettings.cellSize;
    //            foreach (int2 node in findPathJob.finalPath)
    //            {
    //                if (temp.x != -100) Debug.DrawLine(new Vector3((temp.x * mapCellSize * 2) - mapWidth, (temp.y * mapCellSize * 2) - mapHeight), new Vector3((node.x * mapCellSize * 2) - mapWidth, (node.y * mapCellSize * 2) - mapHeight), Color.white, 100f);
    //                if (Vector3.Distance(transform.position, new Vector3((node.x * 2) - mapSettings.width, (node.y * 2) - mapSettings.height)) < 1f)
    //                {
    //                    //Debug.Log("ajaja");
    //                }
    //                //GetComponent<MovePositionDirect>().SetMovePosition(new Vector3((node.x * 2) - mapSettings.width, (node.y * 2) - mapSettings.height));
    //                temp = node;
    //                //Debug.Log(node);
    //            }
    //            GetComponent<MovePositionNodes>().SetMovePosition(savedPath, endPos);
    //            findPathJob.finalPath.Dispose();
    //        }


    //        //GetComponent<MovePositionDirect>().SetMovePosition(new Vector3((endPos.x * 2) - mapSettings.width, (endPos.y * 2) - mapSettings.height));
    //    }
    //}

    //[BurstCompile]
    private struct FindPathJob : IJob
    {
        public int2 startPosition;
        public int2 endPosition;
        //public Grid<BlockInfo> blockInfoGrid;
        public bool[,] blockIsWalkable;
        public NativeArray<int2> finalPath;
        public void Execute()
        {
            int2 gridSize = new int2(blockIsWalkable.GetLength(0), blockIsWalkable.GetLength(1));

            NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    PathNode pathNode = new PathNode();
                    pathNode.x = x;
                    pathNode.y = y;
                    pathNode.index = CalculateIndex(x, y, gridSize.x);

                    pathNode.gCost = int.MaxValue;
                    pathNode.hCost = CalculateDistanceCost(new int2(x, y), endPosition);
                    pathNode.CalculateFCost();

                    pathNode.isWalkable = blockIsWalkable[x, y];
                    pathNode.cameFromNodeIndex = -1;

                    pathNodeArray[pathNode.index] = pathNode;
                }
            }

            /*
            // Place Testing Walls
            {
                PathNode walkablePathNode = pathNodeArray[CalculateIndex(1, 0, gridSize.x)];
                walkablePathNode.SetIsWalkable(false);
                pathNodeArray[CalculateIndex(1, 0, gridSize.x)] = walkablePathNode;

                walkablePathNode = pathNodeArray[CalculateIndex(1, 1, gridSize.x)];
                walkablePathNode.SetIsWalkable(false);
                pathNodeArray[CalculateIndex(1, 1, gridSize.x)] = walkablePathNode;

                walkablePathNode = pathNodeArray[CalculateIndex(1, 2, gridSize.x)];
                walkablePathNode.SetIsWalkable(false);
                pathNodeArray[CalculateIndex(1, 2, gridSize.x)] = walkablePathNode;
            }
            */

            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
            neighbourOffsetArray[0] = new int2(-1, 0); // Left
            neighbourOffsetArray[1] = new int2(+1, 0); // Right
            neighbourOffsetArray[2] = new int2(0, +1); // Up
            neighbourOffsetArray[3] = new int2(0, -1); // Down
            neighbourOffsetArray[4] = new int2(-1, -1); // Left Down
            neighbourOffsetArray[5] = new int2(-1, +1); // Left Up
            neighbourOffsetArray[6] = new int2(+1, -1); // Right Down
            neighbourOffsetArray[7] = new int2(+1, +1); // Right Up

            int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);

            PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, gridSize.x)];
            startNode.gCost = 0;
            startNode.CalculateFCost();
            pathNodeArray[startNode.index] = startNode;

            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

            openList.Add(startNode.index);

            while (openList.Length > 0)
            {
                int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
                PathNode currentNode = pathNodeArray[currentNodeIndex];

                if (currentNodeIndex == endNodeIndex)
                {
                    // Reached our destination!
                    break;
                }

                // Remove current node from Open List
                for (int i = 0; i < openList.Length; i++)
                {
                    if (openList[i] == currentNodeIndex)
                    {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                closedList.Add(currentNodeIndex);

                for (int i = 0; i < neighbourOffsetArray.Length; i++)
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                    if (!IsPositionInsideGrid(neighbourPosition, gridSize))
                    {
                        // Neighbour not valid position
                        continue;
                    }

                    int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, gridSize.x);

                    if (closedList.Contains(neighbourNodeIndex))
                    {
                        // Already searched this node
                        continue;
                    }

                    PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                    if (!neighbourNode.isWalkable)
                    {
                        // Not walkable
                        continue;
                    }

                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNodeIndex = currentNodeIndex;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.CalculateFCost();
                        pathNodeArray[neighbourNodeIndex] = neighbourNode;

                        if (!openList.Contains(neighbourNode.index))
                        {
                            openList.Add(neighbourNode.index);
                        }
                    }
                }
            }
            PathNode endNode = pathNodeArray[endNodeIndex];
            if (endNode.cameFromNodeIndex == -1)
            {
                // Didn't find a path!
                Debug.Log("Didn't find a path!");
            }
            else
            {
                // Found a path
                NativeList<int2> path = CalculatePath(pathNodeArray, endNode);
                foreach (int2 pathPosition in path)
                {
                    //Debug.Log(pathPosition*2);                    
                }
                //finalPath.ReinterpretStore(0, path);
                //finalPath = new NativeArray<int2>(path.Length, Allocator.TempJob);
                //finalPath = path;
                int i = 0;
                finalPath = new NativeArray<int2>(path.Length, Allocator.TempJob);
                foreach (int2 node in path)
                {
                    finalPath[i] = node;
                    i++;
                    //Debug.Log(node);
                }
                path.Dispose();
            }
            pathNodeArray.Dispose();
            neighbourOffsetArray.Dispose();
            openList.Dispose();
            closedList.Dispose();
            //finalPath.Dispose();
        }

        private NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode)
        {
            if (endNode.cameFromNodeIndex == -1)
            {
                // Couldn't find a path!
                return new NativeList<int2>(Allocator.Temp);
            }
            else
            {
                // Found a path
                NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                path.Add(new int2(endNode.x, endNode.y));

                PathNode currentNode = endNode;
                while (currentNode.cameFromNodeIndex != -1)
                {
                    PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
                    path.Add(new int2(cameFromNode.x, cameFromNode.y));
                    currentNode = cameFromNode;
                }

                return path;
            }
        }

        private bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize)
        {
            return
                gridPosition.x >= 0 &&
                gridPosition.y >= 0 &&
                gridPosition.x < gridSize.x &&
                gridPosition.y < gridSize.y;
        }

        private int CalculateIndex(int x, int y, int gridWidth)
        {
            return x + y * gridWidth;
        }

        private int CalculateDistanceCost(int2 aPosition, int2 bPosition)
        {
            int xDistance = math.abs(aPosition.x - bPosition.x);
            int yDistance = math.abs(aPosition.y - bPosition.y);
            int remaining = math.abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }


        private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray)
        {
            PathNode lowestCostPathNode = pathNodeArray[openList[0]];
            for (int i = 1; i < openList.Length; i++)
            {
                PathNode testPathNode = pathNodeArray[openList[i]];
                if (testPathNode.fCost < lowestCostPathNode.fCost)
                {
                    lowestCostPathNode = testPathNode;
                }
            }
            return lowestCostPathNode.index;
        }

        private struct PathNode
        {
            public int x;
            public int y;

            public int index;

            public int gCost;
            public int hCost;
            public int fCost;

            public bool isWalkable;

            public int cameFromNodeIndex;

            public void CalculateFCost()
            {
                fCost = gCost + hCost;
            }

            public void SetIsWalkable(bool isWalkable)
            {
                this.isWalkable = isWalkable;
            }
        }
    }
    private void OnDrawGizmos()
    {
        //    Gizmos.DrawCube(new Vector3(0, 0, -2), new Vector3(2, 2, 2));
        //    if (savedPath != null)
        //    {
        //        int2 temp;
        //        foreach (int2 pathPosition in savedPath)
        //        {
        //            //Debug.Log(pathPosition);
        //            temp = pathPosition;
        //            Gizmos.color = Color.yellow;
        //            Gizmos.DrawLine(new Vector3(temp.x, temp.y, -2), new Vector3(pathPosition.x, pathPosition.y, -2));

        //            Gizmos.DrawCube(new Vector3(pathPosition.x,pathPosition.y), new Vector3(2, 2, 2));
        //        }
        //        //savedPath = null;
        //    }
    }
}
