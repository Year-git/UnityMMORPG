﻿using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterManager : MonoSingleton<CharacterManager>
{

    public Dictionary<int, Character> Characters = new Dictionary<int, Character>();


    public UnityAction<Character> OnCharacterEnter;

    public CharacterManager()
    {

    }

    public void Init()
    {

    }

    public void Clear()
    {
        this.Characters.Clear();
    }

    public void AddCharacter(SkillBridge.Message.NCharacterInfo cha)
    {
        //Debug.LogFormat("AddCharacter:{0}:{1} Map:{2} Entity:{3}", cha.Id, cha.Name, cha.mapId, cha.Entity.String());
        Character character = new Character(cha);
        this.Characters[cha.Id] = character;

        if (OnCharacterEnter != null)
        {
            OnCharacterEnter(character);
        }
    }


    public void RemoveCharacter(int characterId)
    {
        Debug.LogFormat("RemoveCharacter:{0}", characterId);
        this.Characters.Remove(characterId);
    }
}
