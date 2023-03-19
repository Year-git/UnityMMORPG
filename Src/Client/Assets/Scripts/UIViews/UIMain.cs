using Models;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    public Text nameText;
    private void Start()
    {
       this.nameText.text = User.Instance.CurrentCharacter.Name;
    }

    private void OnEnable()
    {
    }

    private void OnDestroy()
    {
    }
}
