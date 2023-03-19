using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterManager : MonoSingleton<CharacterManager>
{

    public Dictionary<int, Character> Characters = new Dictionary<int, Character>();


    public UnityAction<Character> OnCharacterEnter;
    public UnityAction<int> OnCharacterLeave;

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
        if (this.Characters.ContainsKey(cha.Id))
            return;

        Debug.LogFormat("AddCharacter:{0}:{1} Map:{2} Entity:pos{3},{4},{5}", cha.Id, cha.Name, cha.mapId, cha.Entity.Position.X, cha.Entity.Position.Y, cha.Entity.Position.Z);

        Character character = new Character(cha);
        this.Characters[cha.Id] = character;
        if (OnCharacterEnter != null)
            OnCharacterEnter(character);
        EntityManager.Instance.AddEntity(character);
    }


    public void RemoveCharacter(int characterId)
    {
        if (!this.Characters.ContainsKey(characterId))
            return;
        Debug.LogFormat("RemoveCharacter:{0}", characterId);

        this.Characters.Remove(characterId);
        if (OnCharacterLeave != null)
            OnCharacterLeave(characterId);
        EntityManager.Instance.RemoveEntity(characterId);
    }
}
