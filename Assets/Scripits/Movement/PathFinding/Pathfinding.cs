using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using CodeMonkey.Utils;

public class Pathfinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private const int MOVE_NODE_MID_COST = 2;
    private const int MOVE_NODE_CLOSE_COST = 1; 
    private const int MOVE_NODE_FURTHER_COST = 2;
    private const int NODES_IN_HEXAGON = 13;
    private int3[] savedPath;
    private WorldGenerationHandler worldGenerationHandler;
    private void Awake()
    {
        worldGenerationHandler = GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>();
    }

    public void MoveTo(Vector3 goToPosition)
    {
        int3 startPos;
        int3 endPos;

        if (true)
        {
            Node tempNode;
            tempNode = worldGenerationHandler.GetNodeFromPosition(transform.position);
            if (tempNode == null) return;
            startPos = new int3(tempNode.nodesParent.positionInArray.y, tempNode.nodesParent.positionInArray.x, tempNode.positionInArray);
            tempNode = worldGenerationHandler.GetNodeFromPosition(goToPosition);
            if (tempNode == null) return;
            endPos = new int3(tempNode.nodesParent.positionInArray.y, tempNode.nodesParent.positionInArray.x, tempNode.positionInArray);
            if (!tempNode.walkable) return;
        }

        bool[,][] blockIsWalkable = new bool[worldGenerationHandler.mapNodes.GetLength(0), worldGenerationHandler.mapNodes.GetLength(1)][];

        for (int i = 0; i < worldGenerationHandler.mapNodes.GetLength(0); i++)
        {
            for (int j = 0; j < worldGenerationHandler.mapNodes.GetLength(1); j++)
            {
                //Debug.Log("Current coordinates i: " + i + " j: " + j);
                blockIsWalkable[i, j] = new bool[worldGenerationHandler.mapNodes[i, j].GetNodes().Length];
                for (int k = 0; k < worldGenerationHandler.mapNodes[i,j].GetNodes().Length; k++)
                {
                    blockIsWalkable[i, j][k] = worldGenerationHandler.mapNodes[i,j].GetNode(k).walkable;
                }
            }
        }
        
        FindPathJob findPathJob = new FindPathJob
        {
            startPosition = startPos,
            endPosition = endPos,
            blockIsWalkable = blockIsWalkable,
        };
        findPathJob.Execute();


        if (findPathJob.finalPath.Length > 0)
        {
            savedPath = new int3[findPathJob.finalPath.Length];
            findPathJob.finalPath.CopyTo(savedPath);

            //Debug.DrawLine(new Vector3(startPos.x, startPos.y), new Vector3(endPos.x, endPos.y), Color.white);
            int3 temp = new int3(-100, -100, -10);
            int mapWidth = worldGenerationHandler.mapSettings.width;
            int mapHeight = worldGenerationHandler.mapSettings.height;
            float mapCellSize = worldGenerationHandler.mapSettings.cellSize;
            foreach (int3 node in findPathJob.finalPath)
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
            GetComponent<MovePositionNodes>().SetMovePosition(savedPath, endPos);
            findPathJob.finalPath.Dispose();
        }
    }
    [BurstCompile]
    private struct FindPathJob: IJob
    {
        public int3 startPosition;
        public int3 endPosition;

        public bool[,][] blockIsWalkable;
        public NativeArray<int3> finalPath;
        public void Execute()
        {
            int2 endPositionCoefficient = GetNodeCoefficient(endPosition.z);
            int3 gridSize = new int3(blockIsWalkable.GetLength(0), blockIsWalkable.GetLength(1), blockIsWalkable[0,0].Length);
            NativeArray<PathNode>[] pathNodeArray = new NativeArray<PathNode>[gridSize.x];//= new NativeArray<PathNode>(gridSize.x * gridSize.y * gridSize.z, Allocator.Temp);
            for (int i = 0; i < pathNodeArray.Length; i++)
            {
                pathNodeArray[i] = new NativeArray<PathNode>(gridSize.y * gridSize.z, Allocator.Temp);
            }
            //Debug.Log(gridSize.x);
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    for (int z = 0; z < gridSize.z; z++)
                    {
                        PathNode pathNode = new PathNode();
                        pathNode.height = x;
                        pathNode.width = y;
                        pathNode.depth = z;

                        pathNode.index = new int2(x, CalculateIndex(y, z));

                        pathNode.gCost = int.MaxValue;
                        pathNode.hCost = CalculateDistanceCost(new int3(x, y, z), endPosition);
                        pathNode.CalculateFCost();

                        pathNode.isWalkable = blockIsWalkable[x, y][z];
                        pathNode.cameFromNodeIndex = -1;
                        //Debug.Log(pathNode.index);
                        pathNodeArray[x][pathNode.index.y] = pathNode;
                    }
                }
            }


            ///prerobit x, y a z v pathnode na width height a depth

            int2 endNodeIndex = new int2(endPosition.y, CalculateIndex(endPosition.x, endPosition.z));

            PathNode startNode = pathNodeArray[startPosition.y][CalculateIndex(startPosition.x, startPosition.z)];
            startNode.gCost = 0;
            startNode.CalculateFCost();
            pathNodeArray[startNode.index.x][startNode.index.y] = startNode;

            NativeList<int2> openList = new NativeList<int2>(Allocator.Temp);
            NativeList<int2> closedList = new NativeList<int2>(Allocator.Temp);
            //DrawNode(startNode,1);

            openList.Add(startNode.index);

            NativeArray<int3> neighbourOffsetArray = new NativeArray<int3>(0, Allocator.Temp);
            //int2 currentHexagon = new int2(-1, -1);

            //List<PathNode> localHexagonPath = new List<PathNode>();
            //int localHexNodeIndex = -1;
            
            while (openList.Length > 0)////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                int2 currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
                PathNode currentNode = pathNodeArray[currentNodeIndex.x][currentNodeIndex.y];

                //if (!closedList.Contains(currentNodeIndex))
                //{
                //    localHexNodeIndex++;
                //}//alerady searched?   

                //if (currentHexagon.x != currentNode.height && currentHexagon.y != currentNode.width)///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //{
                //    int cost = 0;
                //    //for (int i = 0; i < localHexagonPath.Length-1; i++)
                //    //{
                //    //    if (localHexagonPath[i].gCost > 0) cost += localHexagonPath[i].gCost;
                //    //    localHexagonPath[i] = new PathNode();
                //    //}
                //    if (cost > MOVE_NODE_MID_COST * 2)
                //    {
                //        Debug.Log("Lacnejsie stredom more");
                //    }
                //    Debug.Log("LocalHexNodeIndex Reseted " + localHexagonPath.Count);
                //    localHexagonPath.Clear();
                //    localHexNodeIndex = 0;
                //    currentHexagon = new int2(currentNode.height, currentNode.width);
                //}
                //Debug.Log("Current node depth " + currentNode.depth);                                                                                     ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //DrawNode(currentNode);

                if (CompareInt2(currentNodeIndex, endNodeIndex))
                {                    
                    break;
                }// Reached our destination!

                
                for (int i = 0; i < openList.Length; i++)
                {
                    if (CompareInt2(openList[i], currentNodeIndex))//if current node is equal to node in list then
                    {
                        openList.RemoveAtSwapBack(i);//swap node with last in list and make list lenght one shorter
                        break;
                    }
                }// Remove current node from Open List

                //Debug.Log("Current node index " + pathNodeArray[currentNodeIndex.x][currentNodeIndex.y].depth);
                switch (currentNode.depth)
                {
                    //if z > 11 then z-=11 && if z < 0 then z+=11
                    //parity matters for diagonal sides 5 4
                    //straight sides of hexagon             currnet == 3 x+0 y+2  currnet == 9 x+0 y-2 
                    //diagonal sides of it odd hexagon if current == 1 x+0 y+1 if current == 5 x+1 y+1 if current == 7 x+1 y-1 if current == 11 x+0 y-1
                    //diagonal sides of it even hexagon if current == 1 x-1 y+1 if current == 5 x0 y+1 if current == 7 x+0 y-1 if current == 11 x-1 y-1
                    case 0:
                    case 2:
                    case 4:
                    case 6:
                    case 8:
                    case 10:
                        {
                            neighbourOffsetArray = new NativeArray<int3>(7, Allocator.Temp);
                            int2[] sideDoubleCoefficient = new int2[2] { new int2(0, 0), new int2(0, 0) };
                            if (currentNode.depth == 0 && IsRowEven(currentNode)) sideDoubleCoefficient = new int2[2] { new int2(-1, -1), new int2(-1, 1) };
                            if (currentNode.depth == 0 && !IsRowEven(currentNode)) sideDoubleCoefficient = new int2[2] { new int2(0, -1), new int2(0, 1) };

                            if (currentNode.depth == 2 && IsRowEven(currentNode)) sideDoubleCoefficient = new int2[2] { new int2(-1, 1), new int2(0, 2) };
                            if (currentNode.depth == 2 && !IsRowEven(currentNode)) sideDoubleCoefficient = new int2[2] { new int2(0, 1), new int2(0, 2) };

                            if (currentNode.depth == 4 && IsRowEven(currentNode)) sideDoubleCoefficient = new int2[2] { new int2(0, 2), new int2(0, 1) };
                            if (currentNode.depth == 4 && !IsRowEven(currentNode)) sideDoubleCoefficient = new int2[2] { new int2(0, 2), new int2(1, 1) };

                            if (currentNode.depth == 6 && IsRowEven(currentNode)) sideDoubleCoefficient = new int2[2] { new int2(0, 1), new int2(0, -1) };
                            if (currentNode.depth == 6 && !IsRowEven(currentNode)) sideDoubleCoefficient = new int2[2] { new int2(1, 1), new int2(1, -1) };

                            if (currentNode.depth == 8 && IsRowEven(currentNode)) sideDoubleCoefficient = new int2[2] { new int2(0, -1), new int2(0, -2) };
                            if (currentNode.depth == 8 && !IsRowEven(currentNode)) sideDoubleCoefficient = new int2[2] { new int2(1, -1), new int2(0, -2) };

                            if (currentNode.depth == 10 && IsRowEven(currentNode)) sideDoubleCoefficient = new int2[2] { new int2(0, -2), new int2(-1, -1) };
                            if (currentNode.depth == 10 && !IsRowEven(currentNode)) sideDoubleCoefficient = new int2[2] { new int2(0, -2), new int2(0, -1) };

                            neighbourOffsetArray[0] = new int3(0, 0, 12);
                            neighbourOffsetArray[1] = new int3(0, 0, +1);
                            neighbourOffsetArray[2] = new int3(0, 0, -1);
                            neighbourOffsetArray[3] = new int3(sideDoubleCoefficient[0].x, sideDoubleCoefficient[0].y, +4);//5 4
                            //neighbourOffsetArray[4] = new int3(sideDoubleCoefficient[0].x, sideDoubleCoefficient[0].y, +5);//6
                            neighbourOffsetArray[5] = new int3(sideDoubleCoefficient[1].x, sideDoubleCoefficient[1].y, +7);//7
                            //neighbourOffsetArray[6] = new int3(sideDoubleCoefficient[1].x, sideDoubleCoefficient[1].y, +8);//7
                        }// nodes for flat top hexagon setup
                        break;
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 9:
                    case 11:
                        {
                            neighbourOffsetArray = new NativeArray<int3>(6, Allocator.Temp);
                            int2 sideCoefficient = new int2(0, 0);
                            if (currentNode.depth == 1 && IsRowEven(currentNode)) sideCoefficient = new int2(-1, 1);
                            if (currentNode.depth == 1 && !IsRowEven(currentNode)) sideCoefficient = new int2(0, 1);

                            if (currentNode.depth == 3) sideCoefficient = new int2(0, 2);

                            if (currentNode.depth == 5 && IsRowEven(currentNode)) sideCoefficient = new int2(0, 1);
                            if (currentNode.depth == 5 && !IsRowEven(currentNode)) sideCoefficient = new int2(1, 1);

                            if (currentNode.depth == 7 && IsRowEven(currentNode)) sideCoefficient = new int2(0, -1);
                            if (currentNode.depth == 7 && !IsRowEven(currentNode)) sideCoefficient = new int2(1, -1);

                            if (currentNode.depth == 9) sideCoefficient = new int2(0, -2);

                            if (currentNode.depth == 11 && IsRowEven(currentNode)) sideCoefficient = new int2(-1, -1);
                            if (currentNode.depth == 11 && !IsRowEven(currentNode)) sideCoefficient = new int2(0, -1);

                            neighbourOffsetArray[0] = new int3(0, 0, 12);
                            neighbourOffsetArray[1] = new int3(0, 0, +1);
                            neighbourOffsetArray[2] = new int3(0, 0, -1);
                            neighbourOffsetArray[3] = new int3(sideCoefficient.x, sideCoefficient.y, +5);//5
                            neighbourOffsetArray[4] = new int3(sideCoefficient.x, sideCoefficient.y, +6);//6
                            neighbourOffsetArray[5] = new int3(sideCoefficient.x, sideCoefficient.y, +7);//7
                        }//nodes for pointy top hexagon setup
                        break;
                    case 12:
                        neighbourOffsetArray = new NativeArray<int3>(12, Allocator.Temp);
                        for (int i = 0; i < 12; i++)
                        {
                            neighbourOffsetArray[i] = new int3(0, 0, i);
                        }//neighbours if currend node is in the middle of hexagon
                        break;
                    default:
                        neighbourOffsetArray = new NativeArray<int3>(0, Allocator.Temp);
                        break;
                }//switch for creating array of neightbours

                closedList.Add(currentNodeIndex);

                //Debug.Log("neighbourOffsetArray  lenght " + neighbourOffsetArray.Length);
                for (int i = 0; i < neighbourOffsetArray.Length; i++)
                {

                    int3 neighbourOffset = neighbourOffsetArray[i];
                    int3 neighbourPosition = new int3(currentNode.height + neighbourOffset.y, currentNode.width + neighbourOffset.x, currentNode.depth + neighbourOffset.z);
                    if (i != 0)
                    {
                        if (neighbourPosition.z < 0) neighbourPosition.z += 12;//making shure nodes in hexagon circle right
                        if (neighbourPosition.z > 11) neighbourPosition.z -= 12;
                    }//making shure nodes in hexagon circle right

                    if (!IsPositionInsideGrid(neighbourPosition, gridSize))
                    {
                        // Neighbour not valid position
                        //Debug.Log("Neighbour not valid position");
                        continue;
                    }//valid position?

                    int2 neighbourNodeIndex = new int2(neighbourPosition.x, CalculateIndex(neighbourPosition.y, neighbourPosition.z));

                    if (closedList.Contains(neighbourNodeIndex))
                    {
                        // Already searched this node
                        //Debug.Log("Already searched this node");
                        continue;
                    }//alerady searched?

                    PathNode neighbourNode = pathNodeArray[neighbourNodeIndex.x][neighbourNodeIndex.y];

                    if (!neighbourNode.isWalkable)
                    {
                        // Not walkable
                        //Debug.Log("Not walkable");
                        continue;
                    }//waklabele?

                    int3 currentNodePosition = new int3(currentNode.height, currentNode.width, currentNode.depth);

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
                    if (tentativeGCost < neighbourNode.gCost)
                    {///////////////////pridavanie nody do cesty pomocou vytvarania slucky v came from node
                        neighbourNode.cameFromNodeIndex = currentNodeIndex;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.CalculateFCost();
                        //localHexagonPath[localHexNodeIndex] = neighbourNode;///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        pathNodeArray[neighbourNodeIndex.x][neighbourNodeIndex.y] = neighbourNode;

                        if (!openList.Contains(neighbourNode.index))
                        {
                        //localHexagonPath.Add(neighbourNode);///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            openList.Add(neighbourNode.index);
                        }//!containing neihgbour node? add:else
                    }
                }//for for selecting from neighbours                                         ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //localHexagonPath.Dispose();


            //Debug.Log("Closed list lenght " + closedList.Length);
            //Debug.Log("Open list lenght " + openList.Length);
            //Debug.Log("neighbourOffsetArray  lenght " + neighbourOffsetArray.Length);
            PathNode endNode = pathNodeArray[endNodeIndex.x][endNodeIndex.y];
            //DrawNode(endNode,1);


            //Debug.Log("End node index is " + endNode.depth);

            if (endNode.cameFromNodeIndex.x == -1 || endNode.cameFromNodeIndex.y == -1)
            {
                // Didn't find a path!
                Debug.Log("Didn't find a path!");
            }
            else
            {
                // Found a path
                NativeList<int3> path = CalculatePath(pathNodeArray, endNode);

                //NativeArray<PathNode> localHexagonPath = new NativeArray<PathNode>(12, Allocator.Temp);
                //int2 currentHexagon = new int2(-1, -1);
                //int localHexNodeIndex = 0;
                //int localPositionIndex = 0;
                //for (int k = 0; k < path.Length-1; k++)///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //{
                //    PathNode currentNode = pathNodeArray[path[k].x][CalculateIndex(path[k].y, path[k].z)];
                //    if (currentHexagon.x != currentNode.height && currentHexagon.y != currentNode.width)///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //    {
                //        int cost = 0;
                //        for (int j = 0; j < localHexagonPath.Length - 1; j++)
                //        {
                //            if (localHexagonPath[j].gCost > 0) cost += localHexagonPath[j].gCost;
                //            localHexagonPath[j] = new PathNode();
                //        }
                //        if (cost > MOVE_NODE_MID_COST * 2)
                //        {
                //            path[localPositionIndex - 1] = new int3(currentHexagon.x, currentHexagon.y, localHexagonPath[localHexNodeIndex-1].depth);//-1 preto ze uz som o jedno dalej
                //            path[localPositionIndex - localHexNodeIndex + 1] = new int3(currentHexagon.x, currentHexagon.y, localHexagonPath[0].depth);
                //            path[localPositionIndex - localHexNodeIndex + 2] = new int3(currentHexagon.x, currentHexagon.y, localHexagonPath[0].depth);
                //            int currentIndex = localPositionIndex - localHexNodeIndex + 2;
                //            while (currentIndex < localPositionIndex - 1)
                //            {
                //                currentIndex++;
                //                path.RemoveAtSwapBack(currentIndex);
                //            }
                //            Debug.Log("Lacnejsie stredom more");
                //        }
                //        //Debug.Log("LocalHexNodeIndex Reseted " + localHexagonPath.Length);
                //        localHexNodeIndex = 0;
                //        currentHexagon = new int2(currentNode.height, currentNode.width);
                //    }
                //    else
                //    {
                //        bool flag = false;
                //        foreach(PathNode node in localHexagonPath)
                //        {
                //            if (node.depth == currentNode.depth)
                //            {
                //                Debug.Log("Been there done that");
                //                flag = true;
                //                continue;
                //            }
                //        }
                //        if (flag) continue;
                //        localHexagonPath[localHexNodeIndex] = currentNode;
                //        localHexNodeIndex++;
                //    }
                //    localPositionIndex++;
                //    //Debug.Log(pathPosition);                    
                //}
                //localHexagonPath.Dispose();
                //finalPath.ReinterpretStore(0, path);
                //finalPath = new NativeArray<int2>(path.Length, Allocator.TempJob);
                //finalPath = path;
                int i = 0;
                finalPath = new NativeArray<int3>(path.Length, Allocator.TempJob);
                foreach (int3 node in path)
                {
                    finalPath[i] = node;
                    i++;
                    //Debug.Log(node);
                }
                path.Dispose();
            }

            for (int i = 0; i < pathNodeArray.Length; i++)
            {
                pathNodeArray[i].Dispose();
            }

            neighbourOffsetArray.Dispose();
            openList.Dispose();
            closedList.Dispose();
        }
        private bool CompareInt2(int2 first, int2 second)
        {
            return first.x == second.x && first.y == second.y;
        }
        private NativeList<int3> CalculatePath(NativeArray<PathNode>[] pathNodeArray, PathNode endNode)
        {
            if (endNode.cameFromNodeIndex.x == -1 || endNode.cameFromNodeIndex.y == -1)
            {
                // Couldn't find a path!
                return new NativeList<int3>(Allocator.Temp);
            }

            // Check path
            PathNode currentNode = endNode;//probably always same as enter node
            //PathNode enterNode = currentNode;
            PathNode exitNode = endNode;
            int travelCost = 0;
            //Debug.Log("======================== New Path ========================");
            while (currentNode.cameFromNodeIndex.x != -1 && currentNode.cameFromNodeIndex.y != -1)
            {
                PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex.x][currentNode.cameFromNodeIndex.y];

                if (currentNode.height == cameFromNode.height && currentNode.width == cameFromNode.width)
                {
                    travelCost += MOVE_NODE_CLOSE_COST;
                }//counting travel cost while in same hexagon
                else
                {
                    travelCost += MOVE_NODE_CLOSE_COST;
                    //Debug.Log("Travelcost: " + travelCost);


                    if ((currentNode.width == endNode.width && currentNode.height == endNode.height))
                    {
                        if (endNode.depth == 12)
                        {
                            //Debug.Log("Going to mid big fella " + currentNode.depth);
                            exitNode.cameFromNodeIndex = new int2(currentNode.height, CalculateIndex(currentNode.width, currentNode.depth));//nastavenie cesty zo stredu
                            pathNodeArray[exitNode.height][CalculateIndex(exitNode.width, exitNode.depth)] = exitNode;//saving to array
                            endNode = exitNode;
                        }
                        else if (travelCost > MOVE_NODE_MID_COST * 2)
                        {
                            //Debug.Log("Going thrue mid big fella");
                            exitNode.cameFromNodeIndex.y = CalculateIndex(exitNode.width, 12);//nastavenie cesty zo stredu
                            pathNodeArray[exitNode.height][CalculateIndex(exitNode.width, exitNode.depth)] = exitNode;//saving to array
                            exitNode = pathNodeArray[exitNode.cameFromNodeIndex.x][exitNode.cameFromNodeIndex.y];//nastavenie na stred
                            exitNode.cameFromNodeIndex = new int2(currentNode.height, CalculateIndex(currentNode.width, currentNode.depth));//nastavenie cesty do stredu
                            pathNodeArray[exitNode.height][CalculateIndex(exitNode.width, exitNode.depth)] = exitNode;//saving to array
                        }
                    }//if last node is in middle and path entered last hexagon set path directly to mid (dont get confused this is first hexagon in current search)
                    else if (travelCost > MOVE_NODE_MID_COST * 2)// casual good path cost is 4 so when its bigger ten 2x mid go thrue mid
                    {
                        //Debug.Log("Going thrue mid big fella");
                        exitNode.cameFromNodeIndex.y = CalculateIndex(exitNode.width, 12);//nastavenie cesty zo stredu
                        pathNodeArray[exitNode.height][CalculateIndex(exitNode.width, exitNode.depth)] = exitNode;//saving to array
                        exitNode = pathNodeArray[exitNode.cameFromNodeIndex.x][exitNode.cameFromNodeIndex.y];//nastavenie na stred
                        exitNode.cameFromNodeIndex = new int2(currentNode.height, CalculateIndex(currentNode.width, currentNode.depth));//nastavenie cesty do stredu
                        pathNodeArray[exitNode.height][CalculateIndex(exitNode.width, exitNode.depth)] = exitNode;//saving to array
                    }//setting up path thrue middle based on cheaper cost thrue mid

                    exitNode = cameFromNode; //other one is currentNode
                    travelCost = 0; //resetting travel cost
                }//meking check for prices before entering new hexagon
                currentNode = cameFromNode;
            }//doing price check and correction compared to middle node

            // Found a path
            NativeList<int3> path = new NativeList<int3>(Allocator.Temp);
            path.Add(new int3(endNode.height, endNode.width, endNode.depth));

            /*PathNode*/ currentNode = endNode;
            while (currentNode.cameFromNodeIndex.x != -1 && currentNode.cameFromNodeIndex.y != -1)
            {
                PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex.x][currentNode.cameFromNodeIndex.y];
                path.Add(new int3(cameFromNode.height, cameFromNode.width, cameFromNode.depth));
                currentNode = cameFromNode;
            }
            return path;            
        }
        private bool IsRowEven(PathNode node)
        {
            return node.height % 2 == 0;
        }
        private bool IsPositionInsideGrid(int3 gridPosition, int3 gridSize)
        {
            return
                gridPosition.x >= 0 &&
                gridPosition.y >= 0 &&
                gridPosition.x < gridSize.x &&
                gridPosition.y < gridSize.y &&
                gridPosition.z < gridSize.z;
        }
        private int2 GetLowestCostFNodeIndex(NativeList<int2> openList, NativeArray<PathNode>[] pathNodeArray)
        {
            PathNode lowestCostPathNode = pathNodeArray[openList[0].x][openList[0].y];
            for (int i = 1; i < openList.Length; i++)
            {
                PathNode testPathNode = pathNodeArray[openList[i].x][openList[i].y];
                if (testPathNode.fCost < lowestCostPathNode.fCost)
                {
                    lowestCostPathNode = testPathNode;
                }
            }
            return lowestCostPathNode.index;
        }
        public int2 GetNodeCoefficient(int index)
        {
            int2[] listOfCoefficients = new int2[] { new int2(0, 3), new int2(1, 4), new int2(2, 5), new int2(3, 6),
                                            new int2(4, 5), new int2(5, 4), new int2(6, 3), 
                                            new int2(5, 2), new int2(4, 1), new int2(3, 0),
                                            new int2(2, 1), new int2(1, 2), new int2(3, 3)};
            if (index == 12) return new int2(0, 0);
            else return new int2(2,2);
            //if (index > listOfCoefficients.Length) return new int2(-1,-1);
            //    return listOfCoefficients[index];                
        }
        public int CalculateNodeDistanceCost(int indexA, int indexB)
        {
            if (indexA == 12 || indexB == 12) return MOVE_NODE_MID_COST; //going from or torwards middle
            if (indexA % 2 == 0 && indexB % 2 == 0) return MOVE_NODE_CLOSE_COST;//going between two hexagons from even to even
            if (indexA % 2 == 0 && indexB % 2 != 0 && (indexB - 1 == indexA || indexB + 1 == indexA)) return MOVE_NODE_CLOSE_COST;//going between two hexagons from even to odd inside hexagon
            if (indexA % 2 == 0 && indexB % 2 != 0 && !(indexB - 1 == indexA || indexB + 1 == indexA)) return MOVE_NODE_FURTHER_COST;//going between two hexagons from even to odd outside hexagon
            if (indexA % 2 != 0 && indexB % 2 != 0) return MOVE_NODE_CLOSE_COST;//going between two hexagons from odd to odd
            if (indexA % 2 != 0 && indexB % 2 == 0 && (indexB - 1 == indexA || indexB + 1 == indexA)) return MOVE_NODE_CLOSE_COST;//going between two hexagons from even to odd inside hexagon
            if (indexA % 2 != 0 && indexB % 2 == 0 && !(indexB - 1 == indexA || indexB + 1 == indexA)) return MOVE_NODE_FURTHER_COST;//going between two hexagons from even to odd outside hexagon
            return 20;
        }
        private int CalculateDistanceCost(int3 aPosition, int3 bPosition)
        {
            //int2 aNodeCoefficient = GetNodeCoefficient(aPosition.z);
            //int2 bNodeCoefficient = GetNodeCoefficient(bPosition.z);

            int xDistance = math.abs((aPosition.x - bPosition.x));// + (aNodeCoefficient.x - bNodeCoefficient.x));
            int yDistance = math.abs((aPosition.y - bPosition.y));// + (aNodeCoefficient.y - bNodeCoefficient.y));
            int remaining = math.abs(xDistance - yDistance);

            return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining + CalculateNodeDistanceCost(aPosition.z, bPosition.z);
        }
        private int CalculateIndex(int currnetWidth, int currentNode)
        {
            return currnetWidth * NODES_IN_HEXAGON + currentNode;
        }
        public struct PathNode
        {
            public int height;
            public int width;
            public int depth;

            public int2 index;

            public int gCost;
            public int hCost;
            public int fCost;

            public bool isWalkable;

            public int2 cameFromNodeIndex;

            public void CalculateFCost()
            {
                fCost = gCost + hCost;
            }

            public void SetIsWalkable(bool isWalkable)
            {
                this.isWalkable = isWalkable;
                }
        }
        public void DrawNode(PathNode node, float size = .1f)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            if (node.isWalkable) sphere.GetComponent<MeshRenderer>().material.color = Color.green;
            else sphere.GetComponent<MeshRenderer>().material.color = Color.red;
            Vector3 pos = GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>().mapNodes[node.height,node.width].GetNode(node.depth).position;
            pos.z = -5;
            sphere.transform.position = pos;
            sphere.transform.localScale = new Vector3(size, size, size);
        }

    }
}
