using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EnemyType { 
    EnemyGoblin,
    EnemyBat,
    EnemyBoss,
}

[System.Serializable]
public class FlowingObject {
    public float delay;
    public EnemyType enemyType;
    public GameObject enemyInstance;
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    [Header("Enemy Instances")]
    [SerializeField] List<GameObject> EnemiesInstances;

    [SerializeField] List<GameObject> GoblinInstances;
    [SerializeField] List<GameObject> BatInstances;
    [SerializeField] List<GameObject> BossInstances;

    [SerializeField] int enemyIDCounter;
    [SerializeField, Range(0, 4)] int currentStageID;

    public void StatusReset() {
        enemyIDCounter = 0;
    }

    public void Stage1SetUp() {
        StatusReset();

        // Goblin 4 + Bat 2

    }

    public void Stage2SetUp()
    {
        StatusReset();

    }

    public void Stage3SetUp()
    {
        StatusReset();

    }

    public void Stage4SetUp()
    {
        StatusReset();

    }

    public void Stage5SetUp()
    {

    }

    public void StageSwitching(int _stage) {
        switch (_stage) {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;

        }
    }

    public EnemyScript EnemyTypeSwithcing(EnemyType _type, GameObject enemyObject) {
        switch (_type)
        {
            case EnemyType.EnemyGoblin:
                return enemyObject.GetComponent<EnemyGoblin>();
            case EnemyType.EnemyBat:
                return enemyObject.GetComponent<EnemyGoblin>(); // EnemyBat
            case EnemyType.EnemyBoss:
                return enemyObject.GetComponent<EnemyGoblin>(); // EnemyBoss
        }
        return null;
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
