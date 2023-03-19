using Entities;
using Models;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterHeadInfo : MonoBehaviour
{
    public Text nameText;

    public float hight = 2f;

    private Transform m_owner;
    private Character m_character;

    private void Start()
    {
    }

    private void OnEnable()
    {
    }

    private void OnDestroy()
    {
    }

    public void Init(Transform owner, Character character)
    {
        m_owner = owner;
        m_character = character;
        nameText.text = m_character.Name;
    }
    private void LateUpdate()
    {
        if (m_owner != null)
        {
            this.transform.position = m_owner.transform.position + Vector3.up * hight;
            if (MainPlayerCamera.Instance != null)
            {
                this.transform.LookAt(MainPlayerCamera.Instance.playerCamera.transform);
            }
        }
    }
}
