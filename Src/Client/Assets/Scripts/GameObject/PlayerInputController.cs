using Entities;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoSingleton<PlayerInputController>
{
    public CharacterController myCharacterController;
    public EntityController entityController;

    public float rotateSpeed = 10.0f;

    public Character character;
    private CharacterState currentState;

    private Vector2 inputMovement = Vector2.zero;
    private Vector3 playerForword = Vector3.zero;
    void Start()
    {
        currentState = CharacterState.Idle;

        //// 测试
        //if (this.character == null)
        //{
        //    DataManager.Instance.Load();
        //    NCharacterInfo cinfo = new NCharacterInfo();
        //    cinfo.Id = 1;
        //    cinfo.Name = "Test";
        //    cinfo.Tid = 1;
        //    cinfo.Entity = new NEntity();
        //    cinfo.Entity.Position = new NVector3();
        //    cinfo.Entity.Position.X = 4100;
        //    cinfo.Entity.Position.Y = 3000;
        //    cinfo.Entity.Position.Z = 800;
        //    cinfo.Entity.Direction = new NVector3();
        //    cinfo.Entity.Direction.X = 0;
        //    cinfo.Entity.Direction.Y = 100;
        //    cinfo.Entity.Direction.Z = 0;
        //    this.character = new Character(cinfo);
        //    if (entityController != null) entityController.entity = this.character;
        //}
    }

    void FixedUpdate()
    {
        if (character == null) return;

        inputMovement.x = Input.GetAxis("Horizontal");
        inputMovement.y = Input.GetAxis("Vertical");

        this.PlayerRotate();
        this.PlayerMove();
    }

    public void PlayerRotate()
    {
        if (inputMovement.Equals(Vector3.zero)) return;
        playerForword.x = inputMovement.x;
        playerForword.z = inputMovement.y;
        Quaternion rotation = Quaternion.LookRotation(playerForword, Vector3.up);
        rotation = Quaternion.AngleAxis(MainPlayerCamera.Instance.camera.transform.eulerAngles.y, Vector3.up) * rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.fixedDeltaTime);

        character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
    }

    public void PlayerMove()
    {
        if (inputMovement.Equals(Vector3.zero))
        {
            if (currentState != SkillBridge.Message.CharacterState.Idle)
            {
                currentState = SkillBridge.Message.CharacterState.Idle;
                myCharacterController.Move(Vector3.zero);
                this.character.Stop();
                this.SendEntityEvent(EntityEvent.Idle);
            }
        }
        else
        {
            if (currentState != SkillBridge.Message.CharacterState.Move)
            {
                currentState = SkillBridge.Message.CharacterState.Move;
                this.character.MoveForward();
                this.SendEntityEvent(EntityEvent.MoveFwd);
            }
        }
        float y = MainPlayerCamera.Instance.camera.transform.rotation.eulerAngles.y;
        myCharacterController.SimpleMove(Quaternion.Euler(0, y, 0) * playerForword * this.character.speed / 100f);
    }

    public void SendEntityEvent(EntityEvent entityEvent)
    {
        if (entityController != null)
            entityController.OnEntityEvent(entityEvent);
    }
}
