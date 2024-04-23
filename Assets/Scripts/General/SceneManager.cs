using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    [Header("MainMenu")]
    [SerializeField] List<GameObject> Heroes;
    [SerializeField] GameObject TextTitle;
    [SerializeField] Button BtnGameStart;
    [SerializeField] Button BtnDeveloper;
    [SerializeField] Button BtnExit;
    [SerializeField] List<Button> MainMenuBtns = new List<Button>();

    [Header("StageMenu")]
    [SerializeField] Button BtnStage;
    [SerializeField] Button BtnCharacter;
    [SerializeField] Button BtnStageMenuBack;

    [SerializeField] List<Button> StageMenuBtns = new List<Button>();

    [Header("CharacteMenu")]
    [SerializeField] List<GameObject> PlayerImages;
    [SerializeField] Button BtnPlayer1;
    [SerializeField] Button BtnPlayer2;
    [SerializeField] Button BtnPlayer3;
    [SerializeField] Button BtnPlayer4;
    [SerializeField] Button BtnCharacterMenuBack;
    [SerializeField] List<Button> CharacterMenuBtns = new List<Button>();

    [Header("StageCheckMenu")]
    [SerializeField] List<GameObject> StageImages;
    [SerializeField] Button BtnStage1;
    [SerializeField] Button BtnStage2;
    [SerializeField] Button BtnStage3;
    [SerializeField] Button BtnStage4;
    [SerializeField] Button BtnStageCheckMenuBack;
    [SerializeField] List<Button> StageCheckMenuBtns = new List<Button>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else { 
            Destroy(gameObject);
        }

        BtnGropping();

        // MenuOff(StageMenuBtns);
        // MenuOff(CharacterMenuBtns);
        // MenuOff(StageCheckMenuBtns);

        MenuSceneMainMenu();
    }

    private void BtnGropping() {
        MainMenuBtns.Add(BtnGameStart);
        MainMenuBtns.Add(BtnDeveloper);
        MainMenuBtns.Add(BtnExit);

        StageMenuBtns.Add(BtnStage);
        StageMenuBtns.Add(BtnCharacter);

        CharacterMenuBtns.Add(BtnPlayer1);
        CharacterMenuBtns.Add(BtnPlayer2);
        CharacterMenuBtns.Add(BtnPlayer3);
        CharacterMenuBtns.Add(BtnPlayer4);

        StageCheckMenuBtns.Add(BtnStage1);
        StageCheckMenuBtns.Add(BtnStage2);
        StageCheckMenuBtns.Add(BtnStage3);
        StageCheckMenuBtns.Add(BtnStage4);
    }

    public void MenuOff(List<Button> _input) {
        for (int inum = 0; inum < _input.Count; inum++) {
            if (_input[inum].gameObject.activeSelf) {
                _input[inum].gameObject.SetActive(false);
            }
        }
    }

    public void MenuOn(List<Button> _input)
    {
        for (int inum = 0; inum < _input.Count; inum++)
        {
            if (!_input[inum].gameObject.activeSelf)
            {
                _input[inum].gameObject.SetActive(true);
            }
        }
    }

    public void MenuSceneMainMenu() { 
        TextTitle.SetActive(true);
        MenuOn(MainMenuBtns);
    }

    public void OnGameStart()
    {
        TextTitle.SetActive(false);
        MenuOff(MainMenuBtns);
        HeroStartAnim();
    }

    public void OnDeveloper()
    {

    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void HeroStartAnim() {
        for (int inum = 0; inum < Heroes.Count; inum++) {
            Heroes[inum].GetComponent<SceneHero>().OnStart();
        }
    }

    public void HeroStartReset()
    {
        for (int inum = 0; inum < Heroes.Count; inum++)
        {
            Heroes[inum].GetComponent<SceneHero>().OnReset();
        }
    }


    public void MenuSceneStageMenu()
    {

    }

    public void MenuSceneCharacterMenu()
    {

    }

    public void MenuSceneStageOne()
    {

    }

    public void MenuSceneStageTwo()
    {

    }

    public void MenuSceneStageThree()
    {

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
