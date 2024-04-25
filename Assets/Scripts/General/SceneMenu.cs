using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class HeroSkillInfo { 
    public string heroName;
    public string skillSlot;
    public string descript;
}

[System.Serializable]
public class HeroInfo
{
    public string heroName;
    public string descript;
}

[System.Serializable]
public class StageInfo
{
    public int stageID;
    public string stageName;
    public string descript;
}



public class SceneMenu : MonoBehaviour
{
    public static SceneMenu instance;

    [Header("MainMenu")]
    [SerializeField] GameObject MainMenuObj;
    [SerializeField] List<GameObject> Heroes;
    [SerializeField] GameObject TextTitle;
    [SerializeField] Button BtnGameStart;
    [SerializeField] Button BtnDeveloper;
    [SerializeField] Button BtnExit;

    [Header("StageMenu")]
    [SerializeField] GameObject StageMenuObj;
    [SerializeField] Button BtnStage;
    [SerializeField] Button BtnCharacter;
    [SerializeField] Button BtnStageMenuBack;

    [Header("CharacteMenu")]
    [SerializeField] GameObject CharacterMenuObj;
    [SerializeField] List<GameObject> PlayerImages;
    [SerializeField] Button BtnPlayer1;
    [SerializeField] Button BtnPlayer2;
    [SerializeField] Button BtnPlayer3;
    [SerializeField] Button BtnPlayer4;
    [SerializeField] Button BtnCharacterMenuBack;
    [SerializeField] Vector3 StatuePos;

    [SerializeField] List<Sprite> Player1SkillIcons;
    [SerializeField] List<Sprite> Player2SkillIcons;
    [SerializeField] List<Sprite> Player3SkillIcons;
    [SerializeField] List<Sprite> Player4SkillIcons;

    // [SerializeField] List<string> SkillDescript;
    // [SerializeField] List<string> CharacterDescript;

    [SerializeField] GameObject SkillSlot;
    [SerializeField] GameObject SkillDescription;
    [SerializeField] GameObject CharacterDescription;

    [SerializeField] string TargetHero;

    [Header("StageCheckMenu")]
    [SerializeField] GameObject StageCheckObj;
    [SerializeField] List<GameObject> StageImages;
    [SerializeField] Button BtnStage1;
    [SerializeField] Button BtnStage2;
    [SerializeField] Button BtnStage3;
    [SerializeField] Button BtnStage4;
    [SerializeField] Button BtnStageCheckMenuBack;

    [SerializeField] GameObject StageBattleGrounds;

    [SerializeField] Button StageIn;
    [SerializeField] GameObject StageCommentObject;
    // [SerializeField] Image StageGround;
    // [SerializeField] TMP_Text StageText;

    [Header("External")]
    [SerializeField] TextAsset HeroSkillInfoJson;
    [SerializeField] TextAsset HeroInfoJson;
    [SerializeField] TextAsset StageInfoJson;
    [SerializeField] Dictionary<string, List<HeroSkillInfo>> HeroSkillDict;
    [SerializeField] Dictionary<string, HeroInfo> HeroDict;
    [SerializeField] Dictionary<int, StageInfo> StageDict;
    [SerializeField] string SceneName;

    [Header("Clear Check")]
    [SerializeField] bool Stage2Open;
    [SerializeField] bool Stage3Open;
    [SerializeField] bool Stage4Open;

    [Header("Developer Info")]
    [SerializeField] GameObject DeveloperObject;
    [SerializeField] Button DeveloperBack;

    public void LevelLoad()
    {
        if (PlayerPrefs.HasKey("Stage2Open")) {
            Stage2Open = true;
        }
        if (PlayerPrefs.HasKey("Stage3Open"))
        {
            Stage3Open = true;
        }
        if (PlayerPrefs.HasKey("Stage4Open"))
        {
            Stage4Open = true;
        }
    }

    public void convertJson() {
        var SerialObject1 = JsonConvert.DeserializeObject<List<HeroSkillInfo>>(HeroSkillInfoJson.ToString());

        for (int inum = 0; inum < SerialObject1.Count; inum++) {
            HeroSkillInfo Target = SerialObject1[inum];

            if (!HeroSkillDict.ContainsKey(Target.heroName)) {
                HeroSkillDict[Target.heroName] = new List<HeroSkillInfo>();
            }

            HeroSkillDict[Target.heroName].Add(Target);
        }

        var SerialObject2 = JsonConvert.DeserializeObject<List<HeroInfo>>(HeroInfoJson.ToString());

        for (int inum = 0; inum < SerialObject2.Count; inum++)
        {
            HeroInfo Target = SerialObject2[inum];

            HeroDict[Target.heroName] = Target;
        }

        var SerialObject3 = JsonConvert.DeserializeObject<List<StageInfo>>(StageInfoJson.ToString());

        for (int inum = 0; inum < SerialObject3.Count; inum++)
        {
            StageInfo Target = SerialObject3[inum];

            StageDict[(int)Target.stageID] = Target;
        }
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { 
            Destroy(gameObject);
        }

        LevelLoad();
    }

    // Start is called before the first frame update
    void Start()
    {
        HeroSkillDict = new Dictionary<string, List<HeroSkillInfo>>();
        HeroDict = new Dictionary<string, HeroInfo>();
        StageDict = new Dictionary<int, StageInfo>();

        convertJson();
        MenuSceneMainMenu();
    }

    public void MenuOn(GameObject _input) {
        if (!_input.activeSelf) {
            _input.SetActive(true);
        }
    }

    public void MenuOff(GameObject _input)
    {
        if (_input.activeSelf)
        {
            _input.SetActive(false);
        }
    }

    public void MenuSceneMainMenu() { 
        TextTitle.SetActive(true);
        MenuOn(MainMenuObj);
        MenuOff(StageMenuObj);
        MenuOff(CharacterMenuObj);
        MenuOff(StageCheckObj);
        MenuOff(CharacterDescription);
        MenuOff(SkillDescription);
        MenuOff(SkillSlot);
        MenuOff(StageBattleGrounds);
        MenuOff(StageCommentObject);
        StageIn.gameObject.SetActive(false);
        MenuOff(DeveloperObject);
        DeveloperBack.gameObject.SetActive(false);
        // HeroStartReset();
    }

    public void OnGameStart()
    {
        TextTitle.SetActive(false);
        MenuOff(MainMenuObj);
        MenuOn(StageMenuObj);
        HeroStartAnim();
    }

    public void OnDeveloper()
    {
        MenuOff(MainMenuObj);
        MenuOn(DeveloperObject);
        DeveloperBack.gameObject.SetActive(true);
    }

    public void OnDeveloperBack() {
        MenuSceneMainMenu();
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

    public void HeroStartDisappear() {
        for (int inum = 0; inum < Heroes.Count; inum++)
        {
            Heroes[inum].GetComponent<SceneHero>().OnDisappear();
        }
    }

    public void HeroStartReset()
    {
        for (int inum = 0; inum < Heroes.Count; inum++)
        {
            Heroes[inum].gameObject.SetActive(true);
            Heroes[inum].GetComponent<SceneHero>().OnReset();
        }
    }


    public void OnHeroes() {
        MenuOff(StageMenuObj);
        MenuOn(CharacterMenuObj);
        HeroStartDisappear();
    }

    public void OnStageMenuBack() {
        MenuSceneMainMenu();
        HeroStartReset();
    }

    public void HeroOn(int _target) {
        for (int inum = 0; inum < Heroes.Count; inum++) {
            if (inum == _target)
            {
                if (!Heroes[inum].activeSelf)
                {
                    Heroes[inum].SetActive(true);
                }
                Heroes[inum].GetComponent<SceneHero>().OnReset();
                Heroes[inum].transform.position = StatuePos;


            }
            else {
                if (Heroes[inum].activeSelf) {
                    Heroes[inum].SetActive(false);
                }
            }
        }
    }

    public void HeroOff() {
        for (int inum = 0; inum < Heroes.Count; inum++)
        {
            if (Heroes[inum].activeSelf)
            {
                Heroes[inum].SetActive(false);
            }
        }
    }

    public void SkillIconSwitching(int _hero) {
        Button[] Targets = SkillSlot.transform.GetComponentsInChildren<Button>();

        switch (_hero) {
            case 0:
                SkillIconChange(Targets, Player1SkillIcons);
                break;
            case 1:
                SkillIconChange(Targets, Player2SkillIcons);
                break;
            case 2:
                SkillIconChange(Targets, Player3SkillIcons);
                break;
            case 3:
                SkillIconChange(Targets, Player4SkillIcons);
                break;
        }
    }

    public void SkillIconChange(Button[] _Targets, List<Sprite> _Images) {
        for (int inum = 0; inum < _Images.Count; inum++) {
            _Targets[inum].GetComponent<Image>().sprite = _Images[inum];
        }

    }

    public void OnSkillSlot1() {
        if (!SkillDescription.activeSelf) {
            SkillDescription.SetActive(true);
        }

        SkillDescription.GetComponentInChildren<TMP_Text>().text = HeroSkillDict[TargetHero][0].descript;
    }

    public void OnSkillSlot2()
    {
        if (!SkillDescription.activeSelf)
        {
            SkillDescription.SetActive(true);
        }
        SkillDescription.GetComponentInChildren<TMP_Text>().text = HeroSkillDict[TargetHero][1].descript;
    }

    public void OnSkillSlot3()
    {
        if (!SkillDescription.activeSelf)
        {
            SkillDescription.SetActive(true);
        }
        SkillDescription.GetComponentInChildren<TMP_Text>().text = HeroSkillDict[TargetHero][2].descript;
    }

    public void OnSkillSlot4()
    {
        if (!SkillDescription.activeSelf)
        {
            SkillDescription.SetActive(true);
        }
        SkillDescription.GetComponentInChildren<TMP_Text>().text = HeroSkillDict[TargetHero][3].descript;
    }


    public void OnHero1() {
        HeroOn(0);
        if (!SkillSlot.activeSelf) {
            SkillSlot.SetActive(true);
        }
        
        SkillIconSwitching(0);
        TargetHero = "Warrior";

        MenuOff(SkillDescription);
        MenuOn(CharacterDescription);
        CharacterDescription.GetComponentInChildren<TMP_Text>().text = HeroDict[TargetHero].descript;
    }

    public void OnHero2()
    {
        HeroOn(1);
        if (!SkillSlot.activeSelf)
        {
            SkillSlot.SetActive(true);
        }

        SkillIconSwitching(1);
        TargetHero = "Archer";

        MenuOff(SkillDescription);
        MenuOn(CharacterDescription);
        CharacterDescription.GetComponentInChildren<TMP_Text>().text = HeroDict[TargetHero].descript;
    }

    public void OnHero3()
    {
        HeroOn(2);
        if (!SkillSlot.activeSelf)
        {
            SkillSlot.SetActive(true);
        }

        SkillIconSwitching(2);
        TargetHero = "Wizard";

        MenuOff(SkillDescription);
        MenuOn(CharacterDescription);
        CharacterDescription.GetComponentInChildren<TMP_Text>().text = HeroDict[TargetHero].descript;
    }

    public void OnHero4()
    {
        HeroOn(3);
        if (!SkillSlot.activeSelf)
        {
            SkillSlot.SetActive(true);
        }

        SkillIconSwitching(3);
        TargetHero = "Healer";

        MenuOff(SkillDescription);
        MenuOn(CharacterDescription);
        CharacterDescription.GetComponentInChildren<TMP_Text>().text = HeroDict[TargetHero].descript;
    }

    public void OnHeroBack() {
        MenuOff(CharacterMenuObj);
        MenuOn(StageMenuObj);
        HeroOff();

        if (CharacterDescription.activeSelf) {
            CharacterDescription.SetActive(false);
        }

        if (SkillDescription.activeSelf)
        {
            SkillDescription.SetActive(false);
        }

        MenuOff(SkillSlot);
    }


    public void OnStageSelect()
    {
        MenuOff(StageMenuObj);
        MenuOn(StageCheckObj);
        MenuOn(StageBattleGrounds);
        HeroStartDisappear();

        if (!LockCheck(1)) {
            // StageCheckObj.transform.GetChild(2).GetComponent<Image>().color = new Color(50f, 50f, 50f);
            StageCheckObj.transform.GetChild(2).GetComponent<Button>().interactable = false;
            StageBattleGrounds.transform.GetChild(1).GetComponent<Image>().color = new Color(50f/255f, 50f / 255f, 50f / 255f, 255/255f);
        }

        if (!LockCheck(2))
        {
            // StageCheckObj.transform.GetChild(3).GetComponent<Image>().color = new Color(50f, 50f, 50f);
            StageCheckObj.transform.GetChild(3).GetComponent<Button>().interactable = false;
            StageBattleGrounds.transform.GetChild(2).GetComponent<Image>().color = new Color(50f / 255f, 50f / 255f, 50f / 255f, 255 / 255f);
        }

        if (!LockCheck(3))
        {
            // StageCheckObj.transform.GetChild(4).GetComponent<Image>().color = new Color(50f, 50f, 50f);
            StageCheckObj.transform.GetChild(4).GetComponent<Button>().interactable = false;
            StageBattleGrounds.transform.GetChild(3).GetComponent<Image>().color = new Color(50f / 255f, 50f / 255f, 50f / 255f, 255 / 255f);
        }

    }

    public bool LockCheck(int _input) {
        switch (_input) {
            case 1:
                if (Stage2Open)
                {
                    return true;
                }
                else {
                    return false;
                }
                break;
            case 2:
                if (Stage3Open)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                break;
            case 3:
                if (Stage4Open)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                break;
        }

        return false;
    }

    public void OnStageOne()
    {

        //Target Stage
        MenuOn(StageCommentObject);
        StageIn.gameObject.SetActive(true);
        StageCommentObject.transform.GetChild(0).GetComponent<Image>().sprite = StageBattleGrounds.transform.GetChild(0).GetComponent<Image>().sprite;
        StageCommentObject.transform.GetChild(1).GetComponent<TMP_Text>().text = StageDict[1].stageName;
        StageCommentObject.transform.GetChild(2).GetComponent<TMP_Text>().text = StageDict[1].descript;
        MenuOff(StageBattleGrounds);
        SceneName = "Stage1Scene";
    }

    public void OnStageTwo()
    {
        if (!Stage2Open) {
            Debug.Log("Locked");
            return;
        }

        //Target Stage
        MenuOn(StageCommentObject);
        StageIn.gameObject.SetActive(true);
        StageCommentObject.transform.GetChild(0).GetComponent<Image>().sprite = StageBattleGrounds.transform.GetChild(1).GetComponent<Image>().sprite;
        StageCommentObject.transform.GetChild(1).GetComponent<TMP_Text>().text = StageDict[2].stageName;
        StageCommentObject.transform.GetChild(2).GetComponent<TMP_Text>().text = StageDict[2].descript;
        MenuOff(StageBattleGrounds);
    }

    public void OnStageThree()
    {
        if (!Stage3Open)
        {
            Debug.Log("Locked");
            return;
        }

        //Target Stage
        MenuOn(StageCommentObject);
        StageIn.gameObject.SetActive(true);
        StageCommentObject.transform.GetChild(0).GetComponent<Image>().sprite = StageBattleGrounds.transform.GetChild(2).GetComponent<Image>().sprite;
        StageCommentObject.transform.GetChild(1).GetComponent<TMP_Text>().text = StageDict[3].stageName;
        StageCommentObject.transform.GetChild(2).GetComponent<TMP_Text>().text = StageDict[3].descript;
        MenuOff(StageBattleGrounds);
    }

    public void OnStageFour()
    {
        if (!Stage4Open)
        {
            Debug.Log("Locked");
            return;
        }

        //Target Stage
        MenuOn(StageCommentObject);
        StageIn.gameObject.SetActive(true);
        StageCommentObject.transform.GetChild(0).GetComponent<Image>().sprite = StageBattleGrounds.transform.GetChild(3).GetComponent<Image>().sprite;
        StageCommentObject.transform.GetChild(1).GetComponent<TMP_Text>().text = StageDict[4].stageName;
        StageCommentObject.transform.GetChild(2).GetComponent<TMP_Text>().text = StageDict[4].descript;
        MenuOff(StageBattleGrounds);
    }

    public void OnStageBack() {
        MenuOff(StageCheckObj);
        MenuOff(StageBattleGrounds);
        MenuOn(StageMenuObj);

        MenuOff(StageCommentObject);
        StageIn.gameObject.SetActive(false);
        
    }

    public void OnStageIn() {
        // GameLoad

        SceneManager.LoadSceneAsync(SceneName);
        

    }



}
