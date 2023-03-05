using Models;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectCharacter : MonoBehaviour
{
    public GameObject SelectCharacter;
    public GameObject CreateCharacter;
    public Button[] buttons;
    public Button baskBtn;
    public Button startGameBtn;
    public InputField characterName;

    public ScrollRect characterScroll;
    public List<GameObject> characterList;
    public GameObject characterItem;
    public Transform characterListRoot;
    public Button joinGameBtn;
    public Button cerateCharacterBtn;

    public UICharacterView uICharacterView;
    private CharacterClass characterClass;
    private int characterIdx;

    private void Start()
    {
        UserService.Instance.OnCreateCharacter = OnCreateCharacter;
        this.characterClass = CharacterClass.None;
        this.OnClickBack();
    }

    private void OnEnable()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(delegate () { this.OnClickCharacterBtn(index); });
        }

        startGameBtn.onClick.AddListener(this.OnClickStartGame);
        joinGameBtn.onClick.AddListener(this.OnClickJoinGame);
        cerateCharacterBtn.onClick.AddListener(this.OnClickCerateCharacter);
        baskBtn.onClick.AddListener(this.OnClickBack);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
        }
        startGameBtn.onClick.RemoveAllListeners();
        joinGameBtn.onClick.RemoveAllListeners();
        cerateCharacterBtn.onClick.RemoveAllListeners();
        baskBtn.onClick.RemoveAllListeners();
    }


    private void OnClickCharacterBtn(int index)
    {
        this.characterClass = (CharacterClass)(index + 1);
        uICharacterView.CurrectCharacter = index;
    }

    private void OnClickStartGame()
    {
        if (this.characterClass == CharacterClass.None)
        {
            MessageBox.Show("请选择职业");
        }

        if (string.IsNullOrEmpty(this.characterName.text))
        {
            MessageBox.Show("请输入角色名字");
        }
        UserService.Instance.SendCreateCharacter(this.characterName.text, this.characterClass);
    }

    private void OnCreateCharacter(Result result, string message)
    {
        if (result == Result.Success)
        {
            MessageBox.Show("创建角色成功", "提示", MessageBoxType.Information).OnYes = this.OnClickBack;
        }
    }

    private void OnSelectCharacter(int idx)
    {
        if (this.characterIdx == idx)
            return;

        this.characterIdx = idx;
        NCharacterInfo cha = User.Instance.Info.Player.Characters[idx];
        User.Instance.CurrentCharacter = cha;
        uICharacterView.CurrectCharacter = (int)cha.Class - 1;

        for (int i = 0; i < this.characterList.Count; i++)
        {
            UICharacterItem item = this.characterList[i].GetComponent<UICharacterItem>();
            item.Selected = i == idx;
        }
    }

    private void OnClickJoinGame()
    {
        if (this.characterIdx < 0)
        {
            MessageBox.Show("当前没有选择角色");
            return;
        }
        UserService.Instance.SendUserGameEnter(this.characterIdx);
    }

    private void OnClickCerateCharacter()
    {
        this.OnClickCharacterBtn(0);
        this.SelectCharacter.SetActive(false);
        this.CreateCharacter.SetActive(true);
    }

    private void OnClickBack()
    {
        this.characterIdx = -1;
        this.uICharacterView.CurrectCharacter = -1;

        this.SelectCharacter.SetActive(true);
        this.CreateCharacter.SetActive(false);

        this.characterList.Clear();
        for (int i = 0; i < this.characterListRoot.childCount; i++)
        {
            if (i > 0)
                Destroy(this.characterListRoot.GetChild(i).gameObject);
        }
        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
            GameObject go = Instantiate(characterItem, characterListRoot);
            UICharacterItem item = go.GetComponent<UICharacterItem>();
            item.info = User.Instance.Info.Player.Characters[i];

            Button button = go.GetComponent<Button>();
            int idx = i;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                OnSelectCharacter(idx);
            });
            item.Selected = false;
            this.characterList.Add(go);
            go.SetActive(true);
        }
    }
}
