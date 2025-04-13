using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum EnemyType { 
    EnemyGoblin,
    EnemyBat,
    EnemyBoss1,
    EnemyBoss2,
    EnemyBoss3,
    EnemyBoss4,
}

[System.Serializable]
public class jsonEnemyInfo
{
    public float delay;
    public int goblins;
    public int bats;
    public float boss;
}

[System.Serializable]
public class FlowingObject {
    public int EnemyID;
    public float delay;
    public EnemyType enemyType;
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    [Header("Enemy Instances")]
    [SerializeField] GameObject GoblinePrefab;
    [SerializeField] GameObject BatPrefab;

    [SerializeField] List<GameObject> EnemiesInstances;
    [SerializeField] List<GameObject> GoblinInstances;
    [SerializeField] List<GameObject> BatInstances;
    [SerializeField] Dictionary<float, List<FlowingObject>> StageSpawner;

    [Header("Boss Instance")]
    [SerializeField] Boss1 GoblingKing;
    [SerializeField] Boss2 Kerberos;
    [SerializeField] Boss3 IceElf;
    [SerializeField] Boss4 CorruptedOne;

    [Header("Game Setup")]
    [SerializeField] int enemyIDCounter;
    [SerializeField] int clearCounter;
    [SerializeField, Range(0, 4)] int currentStageID;
    [SerializeField] GameObject BatMovePoints;
    [SerializeField] GameObject BattleGroundPoints;
    [SerializeField] GameObject BossSpawnPoint;

    [SerializeField] int totalEnemies;
    [SerializeField] int totalGoblins;
    [SerializeField] int totalBats;
    [SerializeField] int goblinCursor;
    [SerializeField] int batCursor;
    [SerializeField] int bossCursor;
    [SerializeField] bool ClearCondition;
    [SerializeField] bool poolingEnd;

    [SerializeField] float currentTime;
    [SerializeField] List<float> SpawnTimes;

    [Header("Json Files")]
    [SerializeField] TextAsset JsonStage1;
    [SerializeField] TextAsset JsonStage2;
    [SerializeField] TextAsset JsonStage3;
    [SerializeField] TextAsset JsonStage4;

    [SerializeField] GameObject GameStartUI;
    [SerializeField] GameObject GameClearUI;
    [SerializeField] GameObject GameOverUI;

    [Header("External")]
    [SerializeField] string CurrentStageName;
    [SerializeField] int StageCount;
    [SerializeField] bool StageIn;

    public void jsonLoad(TextAsset _fileName) {
        var SerialObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<jsonEnemyInfo>>(_fileName.ToString());

        bossCursor = -1;

        int Counter = 0;

        for (int inum = 0; inum < SerialObject.Count; inum++)
        {
            jsonEnemyInfo Target = SerialObject[inum];
            if (!StageSpawner.ContainsKey(Target.delay)) {
                SpawnTimes.Add((float)Target.delay);
                StageSpawner[(float)Target.delay] = new List<FlowingObject>();
            }

            totalEnemies += ((int)Target.goblins + (int)Target.bats);
            totalGoblins += (int)Target.goblins;
            totalBats += (int)Target.bats;
            
            

            for (int jnum = 0; jnum < (int)Target.goblins; jnum++) {
                StageSpawner[(float)Target.delay].Add(new FlowingObject() { EnemyID = Counter++, delay = (float)Target.delay, enemyType = EnemyType.EnemyGoblin });
            }

            for (int jnum = 0; jnum < (int)Target.bats; jnum++)
            {
                StageSpawner[(float)Target.delay].Add(new FlowingObject() { EnemyID = Counter++, delay = (float)Target.delay, enemyType = EnemyType.EnemyBat });
            }

            if ((int)Target.boss != -1) {
                bossCursor = (int)Target.boss;
                EnemyType bossType;
                switch (bossCursor) {
                    case 0:
                        bossType = EnemyType.EnemyBoss1;
                        StageSpawner[(float)Target.delay].Add(new FlowingObject() { EnemyID = Counter++, delay = (float)Target.delay, enemyType = bossType });
                        totalEnemies++;
                        break;
                    case 1:
                        bossType = EnemyType.EnemyBoss2;
                        StageSpawner[(float)Target.delay].Add(new FlowingObject() { EnemyID = Counter++, delay = (float)Target.delay, enemyType = bossType });
                        totalEnemies++;
                        break;
                    case 2:
                        bossType = EnemyType.EnemyBoss3;
                        StageSpawner[(float)Target.delay].Add(new FlowingObject() { EnemyID = Counter++, delay = (float)Target.delay, enemyType = bossType });
                        totalEnemies++;
                        break;
                    case 3:
                        bossType = EnemyType.EnemyBoss4;
                        StageSpawner[(float)Target.delay].Add(new FlowingObject() { EnemyID = Counter++, delay = (float)Target.delay, enemyType = bossType });
                        totalEnemies++;
                        break;
                }
                
            }
        }
        // ++totalEnemies; // boss count
    }

    //PlayerPrefs

    public void StageUnlock(int _stageID) {
        switch (_stageID) {
            case 0:
                PlayerPrefs.SetInt("Stage2Open", 1);
                break;
            case 1:
                PlayerPrefs.SetInt("Stage4Open", 1);
                break;
            case 3:
                PlayerPrefs.SetInt("EndOpen", 1);
                break;
        }
    }

    public void StageUnlockCheck()
    {
        if (!PlayerPrefs.HasKey("Stage2Open")) {
            PlayerPrefs.SetInt("Stage2Open", -1);
        }
        if (!PlayerPrefs.HasKey("Stage4Open"))
        {
            PlayerPrefs.SetInt("Stage4Open", -1);
        }
        if (!PlayerPrefs.HasKey("EndOpen"))
        {
            PlayerPrefs.SetInt("EndOpen", -1);
        }
    }

    public void StatusReset() {
        totalEnemies = 0;
        totalGoblins = 0;
        totalBats = 0;
        enemyIDCounter = 0;
        goblinCursor = 0;
        batCursor = 0;

        StageSpawner = new Dictionary<float, List<FlowingObject>>();
        EnemiesInstances = new List<GameObject>();
        GoblinInstances = new List<GameObject>();
        BatInstances = new List<GameObject>();
        SpawnTimes = new List<float>();
    }

    public void BossInstantiate(int _num, FlowingObject _info) {
        switch (_num) {
            case 0:
                Boss1 bossObject1 = Instantiate(GoblingKing, BossSpawnPoint.transform.position, Quaternion.identity);
                bossObject1.setEnemyID((int)_info.EnemyID);
                bossObject1.ObjectInit();
                break;
            case 1:
                Boss2 bossObject2 = Instantiate(Kerberos, BossSpawnPoint.transform.position, Quaternion.identity);
                bossObject2.setEnemyID((int)_info.EnemyID);
                bossObject2.ObjectInit();
                break;
            case 2:
                Boss3 bossObject3 = Instantiate(IceElf, BossSpawnPoint.transform.position, Quaternion.identity);
                bossObject3.setEnemyID((int)_info.EnemyID);
                bossObject3.ObjectInit();
                break;
            case 3:
                Boss4 bossObject4 = Instantiate(CorruptedOne, BossSpawnPoint.transform.position, Quaternion.identity);
                bossObject4.setEnemyID((int)_info.EnemyID);
                bossObject4.ObjectInit();
                break;
        }
    }


    IEnumerator EnemySpawnCoroutine() {
        // EnemySpawnTimeCheck();
        // SpawnTimes.Count = 0;

        yield return null;

        int StopCounter = 0;
        while (true) {
            if (StopCounter == SpawnTimes.Count) break;

            float delay = SpawnTimes[StopCounter];
            yield return new WaitForSecondsRealtime(delay);
            EnemySpawn((int)delay);

            StopCounter++;
        }
        yield return null;
    }

    public void EnemySpawn(int _input) {
        List<FlowingObject> flows = StageSpawner[_input];
        for (int inum = 0; inum < flows.Count; inum++) {
            FlowingObject flow = flows[inum];
            switch (flow.enemyType) {
                case EnemyType.EnemyGoblin:
                    GameObject goblinObject = GoblinInstances[goblinCursor++];
                    goblinObject.GetComponent<EnemyGoblin>().setEnemyID((int)flow.EnemyID);
                    goblinObject.GetComponent<EnemyGoblin>().ObjectInit();
                    goblinObject.transform.position = BattleGroundPoints.transform.GetChild(Random.Range(0, 8)).transform.position;
                    goblinObject.SetActive(true);

                    break;
                case EnemyType.EnemyBat:
                    GameObject batObject = BatInstances[batCursor++];
                    batObject.GetComponent<EnemyBat>().ObjectInit();
                    batObject.GetComponent<EnemyBat>().setEnemyID((int)flow.EnemyID);
                    batObject.transform.position = BattleGroundPoints.transform.GetChild(Random.Range(0, 8)).transform.position;
                    batObject.SetActive(true);

                    break;
                case EnemyType.EnemyBoss1:
                    BossInstantiate(0, flow);
                    break;
                case EnemyType.EnemyBoss2:
                    BossInstantiate(1, flow);
                    break;
                case EnemyType.EnemyBoss3:
                    BossInstantiate(2, flow);
                    break;
                case EnemyType.EnemyBoss4:
                    BossInstantiate(3, flow);
                    break;
            }
        }
    }

    public void Stage1SetUp() {
        jsonLoad(JsonStage1);
    }

    public void Stage2SetUp()
    {
        jsonLoad(JsonStage2);
    }

    public void Stage3SetUp()
    {
        jsonLoad(JsonStage3);
    }

    public void Stage4SetUp()
    {
        jsonLoad(JsonStage4);
    }

    public void StageSwitching(int _stage) {
        switch (_stage) {
            case 0:
                Stage1SetUp();
                break;
            case 1:
                Stage2SetUp();
                break;
            case 2:
                Stage3SetUp();
                break;
            case 3:
                Stage4SetUp();
                break;
        }
        currentStageID = _stage;
        for (int inum = 0; inum < totalGoblins; inum++)
        {
            GameObject GoblingSC = Instantiate(GoblinePrefab, transform.position, Quaternion.identity);
            GoblingSC.SetActive(false);
            GoblinInstances.Add(GoblingSC);
        }

        for (int inum = 0; inum < totalBats; inum++)
        {
            GameObject BatSC = Instantiate(BatPrefab, transform.position, Quaternion.identity);
            BatSC.SetActive(false);
            BatInstances.Add(BatSC);
        }

        
    }

    public void SpawnStart() {
        StartCoroutine(EnemySpawnCoroutine());
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StageUnlockCheck();
        StatusReset();
        
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

    }

    public void setClearCounter() {
        clearCounter++;
        if (clearCounter >= totalEnemies)
        {
            ClearCondition = true;
            StageUnlock(currentStageID);
            // StatusReset();
            // Clear;
            // SceneManager.LoadSceneAsync("MainScene");
            // Destroy(gameObject);
        }
    }

    public bool ClearCheck() {
        if (ClearCondition) {
            return true;
        }

        return false;
    }

    public void proxyload(int stageincount)
    {
        StageSwitching(stageincount);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }
}
