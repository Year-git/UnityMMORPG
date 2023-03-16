﻿using Entities;
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

        GameObject go = (GameObject)Instantiate(obj);
        go.name = "Character_" + character.Info.Id + "_" + character.Info.Name;
        go.transform.position = GameObjectTool.LogicToWorld(character.position);
        go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
        Characters.Add(character.Info.Id, go);

        EntityController ec = go.GetComponent<EntityController>();
        if (ec != null)
        {
            ec.entity = character;
            ec.isPlayer = character.IsPlayer;
        }

        PlayerInputController pc = go.GetComponent<PlayerInputController>();
        if (pc != null)
        {
            if (character.Info.Id == Models.User.Instance.CurrentCharacter.Id)
            {
                MainPlayerCamera.Instance.player = go;
                pc.enabled = true;
                pc.character = character;
                pc.entityController = ec;
            }
            else
            {
                pc.enabled = false;
            }
        }
        //UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
    }
}
