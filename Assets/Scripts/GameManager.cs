using System;
using System.Collections;
using HpSystem;
using Unit;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance => _instance;

    public Camera Camera;


    public bool drawMode;
    public bool changeSpawnMode;
    public bool commandMode;
    public string spawnObj;
    public GhostObj ghost;

    private MilitaryBuilding MilitaryBuilding;
    private Soldier selectedSoldier;
    public Transform spawnPoint;

    private void Awake()
    {
        //singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    public Vector3 MouseWorldPos()
    {
        Vector3 mouseWorldPos = Camera.ScreenToWorldPoint((Input.mousePosition));
        mouseWorldPos.z = 0f;
        mouseWorldPos.x = (float)Math.Round(mouseWorldPos.x);
        mouseWorldPos.y = (float)Math.Round(mouseWorldPos.y);
        return mouseWorldPos;
    }

    #region UnityMethods

    // Update is called once per frame
    void Update()
    {
        var mouseWorldPos = MouseWorldPos();
        if (drawMode || changeSpawnMode)
        {
            GhostObjectMove(mouseWorldPos);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!DrawMode(mouseWorldPos))
                CheckClickObject();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (commandMode)
                CommandMode(mouseWorldPos);

            if (changeSpawnMode)
                ChangeSpawnPoint();

            CheckChangeSpawnPoint();
        }
    }

    #endregion

    #region PublicMethods

    public void CommandMode(Vector3 mouseWorldPos)
    {
        var moveModel = selectedSoldier.GetComponent<MoveModule>();
        moveModel.Move(mouseWorldPos);
        commandMode = false;
    }

    public void ChangeSpawnPoint()
    {
        if (ghost.AvailableZone)
        {
            MilitaryBuilding.ChangeSpawnPoint(ghost.transform.position);
            changeSpawnMode = false;
        }
        else
        {
            Debug.Log("There is redzone");
        }
    }

    public void CheckChangeSpawnPoint()
    {
        var hit = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            var hitObj = hit.collider.gameObject;
            if (hitObj.TryGetComponent(out MilitaryBuilding militaryBuilding))
            {
                changeSpawnMode = true;
                MilitaryBuilding = militaryBuilding;
                MilitaryBuilding.RemoveSpawnPoint();
                ghost.Set(32, 32, militaryBuilding.flag);
            }
        }
    }

    public void CheckClickObject()
    {
        var hit = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            var hitObj = hit.collider.gameObject;
            if (hitObj.TryGetComponent(out HpController hpController))
            {
                hpController.OpenInformationPopup();
            }

            if (hitObj.TryGetComponent(out Soldier soldier))
            {
                commandMode = true;
                selectedSoldier = soldier;
            }
        }
    }

    public bool DrawMode(Vector3 mouseWorldPos)
    {
        if (drawMode)
        {
            if (ghost.AvailableZone)
            {
                ghost.transform.position = mouseWorldPos;
                Spawn(spawnObj, mouseWorldPos);
                return true;
            }

            Debug.Log("There is redzone");
            return true;
        }

        return false;
    }

    public void GhostObjectMove(Vector3 mouseWorldPos)
    {
        ghost.gameObject.transform.position = mouseWorldPos;
        bool available = GridManager.Instance.CheckGrid(mouseWorldPos, ghost.height, ghost.width);
        ghost.AvailableGrid(available);
    }

    public void SpawnGhost(string spawnObj)
    {
        this.spawnObj = spawnObj;
        drawMode = true;
        var data = UnitManager.Instance.GetBuildData(spawnObj);
        ghost.Set(data.height, data.width, data.sprite);
    }

    public void Spawn(string spawnObj, Vector3 pos)
    {
        this.spawnObj = null;
        drawMode = false;
        ghost.gameObject.SetActive(false);

        UnitManager.Instance.Spawn(spawnObj, pos);
    }

    [ContextMenu("SpawnEnemy")]
    public void a()
    {
        StartCoroutine(SpawnEnemy());
    }

    public IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        var barrack = ObjectPooler.Instance.SpawnBuild("Barrack", spawnPoint.position);
        HpData hpData = UnitManager.Instance.GetBuildData("Barrack");
        barrack.SetStats(hpData, Team.Enemy);
        yield return new WaitForSeconds(0.1f);
        GridManager.Instance.UpdateGrid(spawnPoint.position, 128, 128);
    }

    #endregion
}