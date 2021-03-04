using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLocation : MonoBehaviour
{
    public float gridSize = 0.5f;
    public int minWidth = -5;
    public int maxWidth = 5;
    public int minHeight = -5;
    public int maxHeight = 5;
    public GameObject buildingsHandler;
    public GameObject darkMatterHandler;
    public Material ghost_mat;
    public Material ghostUnable_mat;
    public GameObject startBuilding;
    public Vector2Int startPoint;
    public GameObject endBuilding;
    public Vector2Int endPoint;
    public GameObject darkMatterBlock;
    public GameObject indestructibleBlock;
    public Vector2Int[] indestrictiblePositions;
    public GameObject drone;

    private Camera mainCam;
    private Vector3 initDronePos;
    private Grid grid;
    private Collider plane;
    private float buildingSize;

    private List<Vector3Int> cellsList;
    private List<GameObject> buildingsList;
    private List<Vector3Int> matterCellsList;
    private List<GameObject> matterBlocksList;

    private bool stopBuilding;
    internal bool buildingEnds;
    private bool droneIsMoving;

    private AStarPathFinding aStar;

    private Transform foWQuad;
    private Texture2D foWTexture;
    private Color alpha0 = new Color(1,1,1,0);

    private GameObject endBuildingInstance;
    public RectTransform arrowImage;

    private int uiButtonSlot = 0;
    private UIManager uiManager;

    void Start()
    {
        mainCam = FindObjectOfType<Camera>();
        initDronePos = drone.transform.position;
        grid = GetComponent<Grid>();
        plane = GetComponent<MeshCollider>();
        cellsList = new List<Vector3Int>();
        buildingsList = new List<GameObject>();
        matterCellsList = new List<Vector3Int>();
        matterBlocksList = new List<GameObject>();

        // check width and height inputs
        if (minWidth > 0) minWidth = -minWidth;
        if (minWidth == 0) minWidth = -1;
        if (maxWidth < 0) maxWidth = -maxWidth;
        if (maxWidth == 0) maxWidth = 1;
        if (minHeight > 0) minHeight = -minHeight;
        if (minHeight == 0) minHeight = -1;
        if (maxHeight < 0) maxHeight = -maxHeight;
        if (maxHeight == 0) maxHeight = 1;

        // define astar as a new component
        aStar = this.gameObject.AddComponent<AStarPathFinding>();

        // Init grid, cube size and FogOfWar size
        grid.cellSize = new Vector3(gridSize, gridSize, 1f);
        buildingSize = transform.localScale.x * gridSize;

        // Init Fog of War
        foWQuad = transform.Find("FogOfWar").transform;
        int _fowWidth = ((maxWidth - minWidth) % 2 == 0) ? (5 + maxWidth - minWidth) : (6 + maxWidth - minWidth); // Update for even width
        int _fowHeight = ((maxHeight - minHeight) % 2 == 0) ? (5 + maxHeight - minHeight) : (6 + maxHeight - minHeight); // Update for even height
        foWQuad.localScale = new Vector3(_fowWidth * gridSize, _fowHeight * gridSize, 0f);
        foWTexture = new Texture2D(_fowWidth, _fowHeight);
        foWQuad.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", foWTexture);
        for (int i = 0; i < _fowWidth; i++)
        {
            for (int j = 0; j < _fowHeight; j++)
            {
                foWTexture.SetPixel(i, j, Color.black);
            }
        }
        foWTexture.Apply();

        // Init cave with darkMatter blocks
        for (int x = minWidth; x <= maxWidth; x++)
        {
            for (int y = minHeight; y <= maxHeight; y++)
            {
                Vector3Int c = new Vector3Int(x, y, 0);
                GameObject block = Instantiate(darkMatterBlock, grid.CellToWorld(c), Quaternion.identity);
                block.transform.localScale = new Vector3(buildingSize, 90*buildingSize/100, buildingSize);  // darkMatter is slightly smaller than building blocks to see the building ghost
                block.transform.Translate(new Vector3(0f, (90 * buildingSize / 100) / 2, 0f));
                block.transform.SetParent(darkMatterHandler.transform);
                matterCellsList.Add(c);
                matterBlocksList.Add(block);
            }
        }

        // Init cave borders with indestructible blocks
        for (int x = minWidth-1; x <= maxWidth+1; x++)
        {
            Vector3Int c = new Vector3Int(x, minHeight-1, 0);
            GameObject border = Instantiate(indestructibleBlock, grid.CellToWorld(c), Quaternion.identity);
            border.transform.localScale = new Vector3(buildingSize, buildingSize, buildingSize);
            border.transform.Translate(new Vector3(0f, buildingSize / 2, 0f));
            border.transform.SetParent(buildingsHandler.transform);
            cellsList.Add(c);
            buildingsList.Add(border);
            c = new Vector3Int(x, maxHeight + 1, 0);
            border = Instantiate(indestructibleBlock, grid.CellToWorld(c), Quaternion.identity);
            border.transform.localScale = new Vector3(buildingSize, buildingSize, buildingSize);
            border.transform.Translate(new Vector3(0f, buildingSize / 2, 0f));
            border.transform.SetParent(buildingsHandler.transform);
            cellsList.Add(c);
            buildingsList.Add(border);
        }
        for (int y = minHeight; y <= maxHeight; y++)
        {
            Vector3Int c = new Vector3Int(minWidth-1, y, 0);
            GameObject border = Instantiate(indestructibleBlock, grid.CellToWorld(c), Quaternion.identity);
            border.transform.localScale = new Vector3(buildingSize, buildingSize, buildingSize);
            border.transform.Translate(new Vector3(0f, buildingSize / 2, 0f));
            border.transform.SetParent(buildingsHandler.transform);
            cellsList.Add(c);
            buildingsList.Add(border);
            c = new Vector3Int(maxWidth+1, y, 0);
            border = Instantiate(indestructibleBlock, grid.CellToWorld(c), Quaternion.identity);
            border.transform.localScale = new Vector3(buildingSize, buildingSize, buildingSize);
            border.transform.Translate(new Vector3(0f, buildingSize / 2, 0f));
            border.transform.SetParent(buildingsHandler.transform);
            cellsList.Add(c);
            buildingsList.Add(border);
        }

        // Place startBuilding @ startPoint
        if (startPoint.x < minWidth) startPoint.x = minWidth;
        if (startPoint.x > maxWidth) startPoint.x = maxWidth;
        if (startPoint.y < minHeight) startPoint.y = minHeight;
        if (startPoint.y > maxHeight) startPoint.y = maxHeight;
        Vector3Int cell = new Vector3Int(startPoint.x,startPoint.y,0);
        deleteDarkMatter(cell);
        GameObject instance = Instantiate(startBuilding, grid.CellToWorld(cell), Quaternion.identity);
        instance.transform.localScale = new Vector3(buildingSize, buildingSize, buildingSize);
        instance.transform.Translate(new Vector3(0f, buildingSize / 2, 0f));
        instance.transform.SetParent(buildingsHandler.transform);
        cellsList.Add(cell);
        buildingsList.Add(instance);

        // Move drone @ startPoint
        drone.transform.Translate(grid.CellToWorld(cell));
        // Move Camera over drone postion
        mainCam.transform.Translate(mainCam.transform.InverseTransformVector(drone.transform.position-initDronePos));
        if(mainCam.GetComponent<CameraManager>() != null) mainCam.GetComponent<CameraManager>().SetInitPosition(mainCam.transform.position);
        // Set visible FogOfWar at drone position
        UpdateFoW(cell);

        // Place endBuilding @ endPoint
        if (endPoint.x < minWidth) endPoint.x = minWidth;
        if (endPoint.x > maxWidth) endPoint.x = maxWidth;
        if (endPoint.y < minHeight) endPoint.y = minHeight;
        if (endPoint.y > maxHeight) endPoint.y = maxHeight;
        cell = new Vector3Int(endPoint.x, endPoint.y, 0);
        deleteDarkMatter(cell);
        endBuildingInstance = Instantiate(endBuilding, grid.CellToWorld(cell), Quaternion.identity);
        endBuildingInstance.transform.localScale = new Vector3(buildingSize, buildingSize, buildingSize);
        endBuildingInstance.transform.Translate(new Vector3(0f, buildingSize / 2, 0f));
        endBuildingInstance.transform.SetParent(buildingsHandler.transform);
        cellsList.Add(cell);
        buildingsList.Add(endBuildingInstance);

        // Place indestructible blocks at positions if they exist
        if (indestrictiblePositions.Length > 0)
        {
            for(int i = 0; i < indestrictiblePositions.Length; i++)
            {
                if (indestrictiblePositions[i].x < minWidth) indestrictiblePositions[i].x = minWidth;
                if (indestrictiblePositions[i].x > maxWidth) indestrictiblePositions[i].x = maxWidth;
                if (indestrictiblePositions[i].y < minHeight) indestrictiblePositions[i].y = minHeight;
                if (indestrictiblePositions[i].y > maxHeight) indestrictiblePositions[i].y = maxHeight;
                cell = new Vector3Int(indestrictiblePositions[i].x, indestrictiblePositions[i].y, 0);
                if (!cellsList.Contains(cell))
                {
                    deleteDarkMatter(cell);
                    instance = Instantiate(indestructibleBlock, grid.CellToWorld(cell), Quaternion.identity);
                    instance.transform.localScale = new Vector3(buildingSize, buildingSize, buildingSize);
                    instance.transform.Translate(new Vector3(0f, buildingSize / 2, 0f));
                    instance.transform.SetParent(buildingsHandler.transform);
                    cellsList.Add(cell);
                    buildingsList.Add(instance);
                }
            }
        }

        // Init booleans for coroutines
        buildingEnds = true;
        stopBuilding = false;
        droneIsMoving = false;

        // Init UI Manager (used for both mode1 and mode2)
        uiManager = FindObjectOfType<UIManager>();
    }

    void deleteDarkMatter(Vector3Int c)
    {
        matterBlocksList[matterCellsList.FindIndex(x => x == c)].SetActive(false);
    }

    public void StartRadar()
    {
        StartCoroutine(drone.GetComponent<DroneController>().Radar());
        // Rotation of the radar arrow (2D sprite on UI canvas)
        Vector3 arrowDirection = endBuildingInstance.transform.position - drone.transform.position;
        arrowImage.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(arrowDirection.x, arrowDirection.z, 0f));
    }

    public void StartRadar(int i) // another way to StartRadar to add an action on UI slot at the end of the build
    {
        StartCoroutine(drone.GetComponent<DroneController>().Radar());
        // Rotation of the radar arrow (2D sprite on UI canvas)
        Vector3 arrowDirection = endBuildingInstance.transform.position - drone.transform.position;
        arrowImage.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(arrowDirection.x, arrowDirection.z, 0f));
        // Change UI slot
        if (i > 0) uiManager.ChangeButton(i);
    }

    public void PlaceBuldings(GameObject building)
    {
        StartCoroutine(WaitForBuilding(building,0));
    }

    public void PlaceBuldings(GameObject building, int slot) // another way to PlaceBuilding to add an action on UI slot at the end of the build
    {
        StartCoroutine(WaitForBuilding(building, slot));
    }

    IEnumerator WaitForBuilding(GameObject building, int slot)
    {
        // Adding a way to replace a coroutine by another when the player clicks on another tile
        if (!buildingEnds)
        {
            stopBuilding = true;

            // If previous coroutine has not ended, wait for it to end
            while (!buildingEnds || droneIsMoving)
            {
                yield return null;
            }

            //yield return new WaitWhile(() => (buildingEnds && !droneIsMoving)); // Not working as I want, need to better understand WaitWhile
        }
        uiButtonSlot = slot;
        StartCoroutine(Build(building));
    }

    IEnumerator Build(GameObject building)
    {
        GameObject ghostInstance = Instantiate(building);
        ghostInstance.transform.localScale = new Vector3(buildingSize, buildingSize, buildingSize);
        ghostInstance.GetComponent<MeshRenderer>().enabled = true;

        stopBuilding = false;
        buildingEnds = false;
        int iBuild = 0;
        Vector3Int lastCell = new Vector3Int();

        while (!stopBuilding)
        {
            Vector3Int cell = new Vector3Int();
            Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

            // Define the cell under mouse position, move the ghost to it and check if the instance can be built
            RaycastHit hit = new RaycastHit();
            if (plane.Raycast(camRay, out hit, Mathf.Infinity))
            {
                cell = grid.WorldToCell(hit.point);
                ghostInstance.transform.position = grid.CellToWorld(cell) + new Vector3(0f, buildingSize / 2, 0f);

                if (!cell.Equals(lastCell))
                {
                    iBuild = CanBuild(ghostInstance, cell);
                }
                lastCell = cell;
            }

            // Turn the ghost instance with the mouse wheel, change its "wall" integer and check if the instance can be built
            if (Input.mouseScrollDelta.y > 0)
            {
                ghostInstance.transform.Rotate(new Vector3(0, 90, 0));
                ghostInstance.GetComponent<WallPosition>().RotateLeft();
                iBuild = CanBuild(ghostInstance, cell);
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                ghostInstance.transform.Rotate(new Vector3(0, -90, 0));
                ghostInstance.GetComponent<WallPosition>().RotateRight();
                iBuild = CanBuild(ghostInstance, cell);
            }

            // Build if LeftClick, cell is empty and building can be placed (wall alignment) := iBuild != 0
            if ((iBuild > 0) && Input.GetMouseButtonDown(0))
            {
                if (!cellsList.Contains(cell))
                {
                    // Find the path to the cell and wait until the end of the drone movement
                    droneIsMoving = true;
                    StartCoroutine(GetPathAndMove(cell, false));
                    while (droneIsMoving)
                    {
                        yield return null;
                    }

                    // The instance must be at ghost position, with ghost rotation and with the ghost "wall" integer
                    deleteDarkMatter(cell);
                    GameObject instance = Instantiate(building, grid.CellToWorld(cell), ghostInstance.transform.rotation);
                    instance.transform.localScale = new Vector3(buildingSize, buildingSize, buildingSize);
                    instance.transform.Translate(new Vector3(0f, buildingSize / 2, 0f));
                    instance.transform.SetParent(buildingsHandler.transform);
                    instance.GetComponent<WallPosition>().SetWalls(ghostInstance.GetComponent<WallPosition>().GetWalls());
                    if(iBuild>1) instance.GetComponent<WallLightManager>().ActivateLight(); // Activate light if iBuild == 2
                    cellsList.Add(cell);
                    buildingsList.Add(instance);
                    instance.GetComponent<EnableTrashWhenBuilding>().EnableTrash(); // activate trash gameobjects to see the blocks fall and disapear
                    UpdateFoW(cell);
                    stopBuilding = true;
                    // Change UI slot
                    if (uiButtonSlot > 0) uiManager.ChangeButton(uiButtonSlot);

                    // Test if the endPoint is accessible and move to it if possible
                    StartCoroutine(GetPathAndMove(new Vector3Int(endPoint.x,endPoint.y,0), true));
                }
            }

            // Escape or RightClick to stop building
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                stopBuilding = true;
            }

            yield return null;
        }

        Destroy(ghostInstance);
        uiButtonSlot = 0; // Reset of the slot from which the button was clicked
        buildingEnds = true; // Added to ensure the coroutine has ended (instance destroyed) and a new one can be launched
        
    }

    private int CanBuild(GameObject ghostInstance, Vector3Int cell)
    {
        // Search if the building can be placed by checking its ways/walls and the neighbours ways/walls alignment
        bool canBuild = true;
        int nbNeighbours = 0;
        bool neighbourLight = false;
        bool neigbourIsEnd = false;
        ghostInstance.GetComponent<MeshRenderer>().material = ghost_mat;
        // Check cell at x-1 (west)
        if (cellsList.Contains(cell + new Vector3Int(-1, 0, 0)))
        {
            // Get tile if the cell is in the cell list
            GameObject westNeighbour = buildingsList[cellsList.FindIndex(x => x == cell + new Vector3Int(-1, 0, 0))];
            // If the neighbour tile has a passage, count it as a neighbour
            if ((westNeighbour.GetComponent<WallPosition>().GetWalls() >> 1 & 1) == 0)
            {
                nbNeighbours++;
                // Check if the neigbour is the end point
                neigbourIsEnd = neigbourIsEnd || new Vector3Int(endPoint.x, endPoint.y, 0).Equals(cell + new Vector3Int(-1, 0, 0));
            }
            // If the neighbour tile and the current tile don't have the same connection (wall or passage), set canBuild to false
            if ((westNeighbour.GetComponent<WallPosition>().GetWalls() >> 1 & 1) != (ghostInstance.GetComponent<WallPosition>().GetWalls() >> 3 & 1))
            {
                canBuild = false;
                ghostInstance.GetComponent<MeshRenderer>().material = ghostUnable_mat;
            }
            else // Check if a light is needed and if the neigbour is the end point
            {
                neighbourLight = neighbourLight || westNeighbour.GetComponent<WallLightManager>().GetLightStatus();
            }
            
        }
        // Check cell at x+1 (east)
        if (cellsList.Contains(cell + new Vector3Int(1, 0, 0)))
        {
            GameObject eastNeighbour = buildingsList[cellsList.FindIndex(x => x == cell + new Vector3Int(1, 0, 0))];
            if ((eastNeighbour.GetComponent<WallPosition>().GetWalls() >> 3 & 1) == 0)
            {
                nbNeighbours++;
                // Check if the neigbour is the end point
                neigbourIsEnd = neigbourIsEnd || new Vector3Int(endPoint.x, endPoint.y, 0).Equals(cell + new Vector3Int(1, 0, 0));
            }
            if ((eastNeighbour.GetComponent<WallPosition>().GetWalls() >> 3 & 1) != (ghostInstance.GetComponent<WallPosition>().GetWalls() >> 1 & 1))
            {
                canBuild = false;
                ghostInstance.GetComponent<MeshRenderer>().material = ghostUnable_mat;
            }
            else // Check if a light is needed and if the neigbour is the end point
            {
                neighbourLight = neighbourLight || eastNeighbour.GetComponent<WallLightManager>().GetLightStatus();
            }
            
        }
        // Check cell at z+1 (north)
        if (cellsList.Contains(cell + new Vector3Int(0, 1, 0)))
        {
            GameObject northNeighbour = buildingsList[cellsList.FindIndex(x => x == cell + new Vector3Int(0, 1, 0))];
            if ((northNeighbour.GetComponent<WallPosition>().GetWalls() >> 2 & 1) == 0)
            {
                nbNeighbours++;
                // Check if the neigbour is the end point
                neigbourIsEnd = neigbourIsEnd || new Vector3Int(endPoint.x, endPoint.y, 0).Equals(cell + new Vector3Int(0, 1, 0));
            }
            if ((northNeighbour.GetComponent<WallPosition>().GetWalls() >> 2 & 1) != (ghostInstance.GetComponent<WallPosition>().GetWalls() & 1))
            {
                canBuild = false;
                ghostInstance.GetComponent<MeshRenderer>().material = ghostUnable_mat;
            }
            else // Check if a light is needed and if the neigbour is the end point
            {
                neighbourLight = neighbourLight || northNeighbour.GetComponent<WallLightManager>().GetLightStatus();
            }
            
        }
        // Check cell at z-1 (south)
        if (cellsList.Contains(cell + new Vector3Int(0, -1, 0)))
        {
            GameObject southNeighbour = buildingsList[cellsList.FindIndex(x => x == cell + new Vector3Int(0, -1, 0))];
            if ((southNeighbour.GetComponent<WallPosition>().GetWalls() & 1) == 0)
            {
                nbNeighbours++;
                // Check if the neigbour is the end point
                neigbourIsEnd = neigbourIsEnd || new Vector3Int(endPoint.x, endPoint.y, 0).Equals(cell + new Vector3Int(0, -1, 0));
            }
            if ((southNeighbour.GetComponent<WallPosition>().GetWalls() & 1) != (ghostInstance.GetComponent<WallPosition>().GetWalls() >> 2 & 1))
            {
                canBuild = false;
                ghostInstance.GetComponent<MeshRenderer>().material = ghostUnable_mat;
            }
            else // Check if a light is needed
            {
                neighbourLight = neighbourLight || southNeighbour.GetComponent<WallLightManager>().GetLightStatus();
            }
            
        }

        // Cannot build if there is no neighbours (no access for player)
        if (nbNeighbours == 0)
        {
            ghostInstance.GetComponent<MeshRenderer>().material = ghostUnable_mat;
            canBuild = false;
        }

        // Cannot build if the only neighbour is the end point (no access for player)
        if(nbNeighbours ==1 && neigbourIsEnd)
        {
            ghostInstance.GetComponent<MeshRenderer>().material = ghostUnable_mat;
            canBuild = false;
        }

        // Cannot build on a building
        if (cellsList.Contains(cell))
        {
            canBuild = false;
            ghostInstance.GetComponent<MeshRenderer>().material = ghostUnable_mat;
        }

            // Return the integer: 0 if not buildable, 1 if buildable with no light and 2 if buildable with light
            if (!canBuild)
        {
            return 0;
        }
        else
        {
            if (neighbourLight)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
    }

    private IEnumerator GetPathAndMove(Vector3Int cell, bool checkEndPoint)
    {
        // Using droneCellPos with Mathf.Round because grid.WorldToCell sometimes return a false position
        Vector3Int droneCellPos = new Vector3Int(Mathf.RoundToInt(drone.transform.position.x/ (10f * gridSize)), Mathf.RoundToInt(drone.transform.position.z/ (10f * gridSize)), 0);
        // Get the A* path
        List<Vector3Int> path = aStar.PathFinding(cellsList, droneCellPos, cell);
        if (path != null)
        {
            // if destination is the endPoint, wait for the end of trash deletion
            if (checkEndPoint) yield return new WaitForSeconds(0.9f);

            // Move the drone to the position
            for (int i = path.Count - 1; i >= 0; i--)
            {
                // Using droneCellPos with Mathf.Round because grid.WorldToCell sometimes return a false position
                droneCellPos = new Vector3Int(Mathf.RoundToInt(drone.transform.position.x / (10f * gridSize)), Mathf.RoundToInt(drone.transform.position.z / (10f * gridSize)), 0);
                //if (droneCellPos != grid.WorldToCell(drone.transform.position)) Debug.LogError("WorldToCell diff! : " + droneCellPos + " | " + grid.WorldToCell(drone.transform.position)); //For Debug
                Vector3 diff = path[i] - droneCellPos;
                Vector3 dir = Quaternion.Euler(-90, 0, 0) * drone.transform.TransformDirection(new Vector3Int(0, 0, 1));
                Vector3 vectProd = Vector3.Cross(diff, dir);

                // If diff and dir normal is negative, turn left
                if (vectProd == -Vector3.forward)
                {
                    StartCoroutine(drone.GetComponent<DroneController>().TurnLeft());
                    yield return new WaitForSeconds(Time.deltaTime);
                    while (drone.GetComponent<DroneController>().isMoving == true)
                    {
                        yield return null;
                    }
                }
                // If diff and dir normal is positive, turn right
                else if (vectProd == Vector3.forward)
                {
                    StartCoroutine(drone.GetComponent<DroneController>().TurnRight());
                    yield return new WaitForSeconds(Time.deltaTime);
                    while (drone.GetComponent<DroneController>().isMoving == true)
                    {
                        yield return null;
                    }
                }
                // If diff and dir normal is null but diff and dir are not equals, make a U-turn
                else if ((dir != diff) && (vectProd == Vector3.zero))
                {
                    // random to choose to turn left or right
                    float rand = Random.Range(0f, 1f);
                    if (rand > 0.5)
                    {
                        StartCoroutine(drone.GetComponent<DroneController>().UTurnLeft());
                    }
                    else
                    {
                        StartCoroutine(drone.GetComponent<DroneController>().UTurnRight());
                    }
                    yield return new WaitForSeconds(Time.deltaTime);
                    while (drone.GetComponent<DroneController>().isMoving == true)
                    {
                        yield return null;
                    }
                }
                // After turning, drone is in line: move forward or scan if last cell
                if (i == 0)
                {
                    if (!checkEndPoint) // not endPoint: building => do the scan
                    {
                        StartCoroutine(drone.GetComponent<DroneController>().Scan());
                        yield return new WaitForSeconds(Time.deltaTime);
                        while (drone.GetComponent<DroneController>().isMoving == true)
                        {
                            yield return null;
                        }
                    }
                    else //endPoint: move to endPoint
                    {
                        StartCoroutine(drone.GetComponent<DroneController>().MoveForward());
                        yield return new WaitForSeconds(Time.deltaTime);
                        while (drone.GetComponent<DroneController>().isMoving == true)
                        {
                            yield return null;
                        }
                        StartCoroutine(drone.GetComponent<DroneController>().WinAnimation());
                        yield return new WaitForSeconds(2f);
                        FindObjectOfType<GameController>().ActivateWinMenu();
                    }
                }
                else
                {
                    StartCoroutine(drone.GetComponent<DroneController>().MoveForward());
                    yield return new WaitForSeconds(Time.deltaTime);
                    while (drone.GetComponent<DroneController>().isMoving == true)
                    {
                        yield return null;
                    }
                }
            }
        }
        droneIsMoving = false;
    }

    public bool hasNorthWall(Vector3Int cell)
    {
        if (cellsList.Contains(cell))
        {
            GameObject b = buildingsList[cellsList.FindIndex(x => x == cell)];
            return (b.GetComponent<WallPosition>().GetWalls() & 1) == 1;
        }
        // Default value if the cell is not built: cell has a wall
        return true;
    }
    public bool hasWestWall(Vector3Int cell)
    {
        if (cellsList.Contains(cell))
        {
            GameObject b = buildingsList[cellsList.FindIndex(x => x == cell)];
            return (b.GetComponent<WallPosition>().GetWalls() >> 1 & 1) == 1;
        }
        // Default value if the cell is not built: cell has a wall
        return true;
    }
    public bool hasSouthWall(Vector3Int cell)
    {
        if (cellsList.Contains(cell))
        {
            GameObject b = buildingsList[cellsList.FindIndex(x => x == cell)];
            return (b.GetComponent<WallPosition>().GetWalls() >> 2 & 1) == 1;
        }
        // Default value if the cell is not built: cell has a wall
        return true;
    }
    public bool hasEastWall(Vector3Int cell)
    {
        if (cellsList.Contains(cell))
        {
            GameObject b = buildingsList[cellsList.FindIndex(x => x == cell)];
            return (b.GetComponent<WallPosition>().GetWalls() >> 3 & 1) == 1;
        }
        // Default value if the cell is not built: cell has a wall
        return true;
    }


    private void UpdateFoW(Vector3Int cell)
    {

        int _addWidth = ((maxWidth - minWidth) % 2 == 0) ? 2 : 3; // Update for even width
        int _addHeight = ((maxHeight - minHeight) % 2 == 0) ? 2 : 3; // Update for even height

        for (int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                foWTexture.SetPixel(cell.x - minWidth + _addWidth + i, cell.y - minHeight + _addHeight + j, alpha0);
            }
        }
        foWTexture.Apply();
    }



}
