using Entities;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public CharacterController myCharacterController;
    public Animator myAnimator;
    public Entity entity;

    public bool isPlayer = false;

    public Vector3 position;
    public Vector3 direction;
    private Quaternion rotation;

    private Vector3 lastPosition;
    private Quaternion lastRotation;


    void Start()
    {
        if (entity != null)
            this.UpdateTransform();
    }

    void FixedUpdate()
    {
        if (this.entity == null) return;

        this.entity.OnUpdate(Time.fixedDeltaTime);

        if (!this.isPlayer)
            this.UpdateTransform();
    }

    void OnDestroy()
    {
        if (entity != null)
            Debug.LogFormat("{0} OnDestroy :ID:{1} POS:{2} DIR:{3} SPD:{4} ", this.name, entity.entityId, entity.position, entity.direction, entity.speed);

    }

    // 更新实体信息
    public void UpdateTransform()
    {
        this.position = GameObjectTool.LogicToWorld(entity.position);
        this.direction = GameObjectTool.LogicToWorld(entity.direction);
        this.myCharacterController.SimpleMove(this.position);
        this.transform.forward = this.direction;
        this.rotation = this.transform.rotation;
        this.lastPosition = this.position;
        this.lastRotation = this.rotation;
    }

    // 实体状态通知
    public void OnEntityEvent(EntityEvent entityEvent)
    {
        switch (entityEvent)
        {
            case EntityEvent.Idle:
                myAnimator.SetFloat("Speed", 0);
                // myAnimator.SetTrigger("Idle");
                break;
            case EntityEvent.MoveFwd:
                myAnimator.SetFloat("Speed", entity.speed);
                break;
            case EntityEvent.MoveBack:
                myAnimator.SetFloat("Speed", entity.speed);
                break;
        }
    }
}
