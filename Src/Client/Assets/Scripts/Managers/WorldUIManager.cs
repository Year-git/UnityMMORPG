using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldUIManager : MonoSingleton<WorldUIManager>
{
    private Dictionary<Transform, GameObject> characterHeadInfoDictionary = new Dictionary<Transform, GameObject>();
    public void CreateCharacterHeadInfo(Transform owner, Character character)
    {
        if (characterHeadInfoDictionary.ContainsKey(owner))
            return;

        Object obj = Resloader.LoadResources<Object>("UI/UIPlayerHeadInfo");
        GameObject go = Instantiate(obj, transform) as GameObject;
        go.GetComponent<UICharacterHeadInfo>().Init(owner, character);
        go.SetActive(true);
        characterHeadInfoDictionary[owner] = go;
    }

    public void RemoveCharacterHeadInfo(Transform owner)
    {
        if (!characterHeadInfoDictionary.ContainsKey(owner))
            return;

        Destroy(characterHeadInfoDictionary[owner]);
        characterHeadInfoDictionary.Remove(owner);
    }
}
