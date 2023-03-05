using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour
{
    public GameObject[] characters;
    private int currentCharacter = 0;
    public int CurrectCharacter
    {
        get
        {
            return currentCharacter;
        }
        set
        {
            currentCharacter = value;
            this.UpdateCharacter();
        }
    }

    void Start()
    {
    }

    void UpdateCharacter()
    {
        if (characters.Length == 0)
        {
            GameObject pCharachterRoot = GameObject.Find("CharachterRoot");
            if (pCharachterRoot == null)
                return;
            characters = new GameObject[pCharachterRoot.transform.childCount];
            for (int i = 0; i < pCharachterRoot.transform.childCount; i++)
            {
                characters[i] = pCharachterRoot.transform.GetChild(i).gameObject;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            characters[i].SetActive(i == this.currentCharacter);
        }
    }
}
