using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game UI On")]
    [SerializeField] GameObject RaidInText;
    [SerializeField] GameObject VictoryText;
    [SerializeField] GameObject GameOverText;
    [SerializeField] GameObject ContinueText;
    [SerializeField] GameObject GameOverBtn;
    [SerializeField] GameObject ContinueBtn;

    [SerializeField] GameObject DarkPanel;

    [SerializeField] GameObject BossHpUI;
    [SerializeField] Slider BossHpUISlider;
    [SerializeField] TMP_Text BossHpUIName;

    [Header("Game Managing")]
    [SerializeField] int StageCount;
    [SerializeField] bool pause;

    [SerializeField] bool eventOn;
    [SerializeField] bool ClearEventOn;
    [SerializeField] List<GameObject> Heroes;
    [SerializeField] GameObject GameStartPoint;
    [SerializeField] GameObject GameVictoryPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RaidInText.SetActive(false);
        VictoryText.SetActive(false);
        GameOverText.SetActive(false);
        ContinueText.SetActive(false);
        GameOverBtn.SetActive(false);
        ContinueBtn.SetActive(false);
        StartCoroutine(StageStartSequence());
    }

    // Update is called once per frame
    void Update()
    {
        PauseEvent();
        GameOverEvent();
        ClearEvent();
    }

    IEnumerator StageStartSequence() {
        eventOn = true;
        RaidInText.SetActive(true);
        Heroes = PlayerManager.instance.getHeroesObjects();

        while (true) {
            Vector2 MoveOrderPos;
            bool breakCheck = true;

            for (int inum = 0; inum < Heroes.Count; inum++)
            {
                MoveOrderPos = GameStartPoint.transform.position;
                MoveOrderPos.x += inum * 4f;
                Heroes[inum].transform.position = Vector2.MoveTowards(Heroes[inum].transform.position, MoveOrderPos, Time.deltaTime * 10f);
                yield return null;

                if (Vector2.Distance(Heroes[inum].transform.position, MoveOrderPos) > 0.01f)
                {
                    breakCheck = false;
                }
            }
            if (breakCheck)
            {
                break;
            }
        }
        yield return null;
        eventOn = false;
        RaidInText.SetActive(false);
        Invoke("GameOn", 3f);
    }

    public void StageGameOverSequence()
    {
        eventOn = true;
        Time.timeScale = 0f;

        GameOverText.SetActive(true);
        GameOverBtn.SetActive(true);
    }

    public void GameOverEvent() {
        
        bool aliveCheck = false;
        int aliveCount = 0;
        for (int inum = 0; inum < Heroes.Count; inum++)
        {
            if (aliveCheck = PlayerManager.instance.getHeroAlive(inum)) {
                aliveCount += 1;
            }
        }

        if (aliveCount == 0) {
            DarkPanel.gameObject.SetActive(false);
            StageGameOverSequence();
        }
    }

    public void PauseEvent() {
        if (Input.GetKeyDown(KeyCode.Escape) && !eventOn)
        {
            pause = !pause;
        }

        if (pause && !ClearEventOn)
        {
            DarkPanel.gameObject.SetActive(false);
            ContinueText.SetActive(true);
            ContinueBtn.SetActive(true);
            GameOverBtn.SetActive(true);
            Time.timeScale = 0f;
        }
        else if(!pause && ClearEventOn) {
            DarkPanel.gameObject.SetActive(false);
            ContinueText.SetActive(false);
            ContinueBtn.SetActive(false);
            GameOverBtn.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void ClearEvent() {
        if (StageManager.instance.ClearCheck()) {
            DarkPanel.gameObject.SetActive(false);
            Time.timeScale = 0f;
            ClearEventOn = true;
            VictoryText.SetActive(true);
            GameOverBtn.SetActive(true);
            // StartCoroutine(StageVictorySequence());
        }
    }

    public void OnContinue() {
        DarkPanel.gameObject.SetActive(false);
        pause = false;
        ContinueText.SetActive(false);
        ContinueBtn.SetActive(false);
        GameOverBtn.SetActive(false);
        Time.timeScale = 1f;
    }

    // Button with Game End;
    public void OnMainSceneBack() {
        DarkPanel.gameObject.SetActive(false);
        RaidInText.SetActive(false);
        VictoryText.SetActive(false);
        GameOverText.SetActive(false);
        ContinueText.SetActive(false);
        GameOverBtn.SetActive(false);
        ContinueBtn.SetActive(false);

        Time.timeScale = 1f;
        Destroy(StageManager.instance.gameObject);
        Destroy(gameObject);

        SceneManager.LoadSceneAsync("MainScene");
    }

    // Game with On Enable
    public void GameOn() {
        StageManager.instance.SpawnStart();
    }

    private void OnEnable()
    {
        // pre-load
        StageManager.instance.StageSwitching(StageCount);
    }

    public void SetBossHP(float _maxHP, float _curHP, string _name) {

        if (!BossHpUI.activeSelf)
        {
            BossHpUI.SetActive(true);
        }
        if (_curHP < 0f) {
            BossHpUI.SetActive(false);
            return;
        }
        BossHpUISlider.GetComponent<Slider>().maxValue = _maxHP;
        BossHpUISlider.GetComponent<Slider>().value = _curHP;
        BossHpUIName.GetComponent<TMP_Text>().text = _name;
    }
}
