using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectManager : MonoSingleton<GameObjectManager>
{
    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();
    void Start()
    {
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnter = OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave = OnCharacterLeave;
    }

    void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter = null;
    }
    public void Init()
    {

    }

    void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
    }

    void OnCharacterLeave(int characterId)
    {
        RemoveCharacterObject(characterId);
    }

    IEnumerator InitGameObjects()
    {
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }

    private void CreateCharacterObject(Character character)
    {
        if (Characters.ContainsKey(character.Info.Id))
            return;

        Object obj = Resloader.LoadResources<Object>(character.Define.Resource);
        if (obj == null)
        {
            Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.", character.Define.TID, character.Define.Resource);
            return;
        }

        GameObject go = (GameObject)Instantiate(obj, GameObjectTool.LogicToWorld(character.position), Quaternion.Euler(GameObjectTool.LogicToWorld(character.direction)));
        go.name = "Character_" + character.Info.Id + "_" + character.Info.Name;
        Characters.Add(character.Info.Id, go);
        Debug.LogFormat("CreateCharacterObject:{0}: pos {1}  dir {2}", go.name, go.transform.position, go.transform.forward);

        EntityController ec = go.GetComponent<EntityController>();
        if (ec != null)
        {
            ec.entity = character;
            ec.isPlayer = character.IsPlayer;
            EntityManager.Instance.RegisterEntityChangeNotify(character.entityId, ec);
        }

        PlayerInputController pc = go.GetComponent<PlayerInputController>();
        if (pc != null)
        {
            if (character.Info.Id == Models.User.Instance.CurrentCharacter.Id)
            {
                MainPlayerCamera.Instance.player = go;
                pc.enabled = true;
                pc.character = character;
                pc.myCharacterController = go.GetComponent<CharacterController>();
                pc.entityController = ec;
            }
            else
            {
                pc.enabled = false;
            }
        }
        WorldUIManager.Instance.CreateCharacterHeadInfo(go.transform, character);
    }

    private void RemoveCharacterObject(int characterId)
    {
        GameObject go;
        if (!Characters.TryGetValue(characterId, out go))
            return;

        WorldUIManager.Instance.RemoveCharacterHeadInfo(go.transform);

        Destroy(Characters[characterId]);
        Characters.Remove(characterId);
    }
}
