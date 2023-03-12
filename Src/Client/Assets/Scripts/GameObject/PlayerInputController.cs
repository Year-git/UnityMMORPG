using Entities;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public CharacterController myCharacterController;
    public EntityController entityController;

    public float rotateSpeed = 10.0f;
    public float turnAngle = 10;
    public int speed;

    public Character myCharacter;
    private CharacterState currentState;

    void Start()
    {
        currentState = CharacterState.Idle;

        // 测试
        if (this.myCharacter == null)
        {
            DataManager.Instance.Load();
            NCharacterInfo cinfo = new NCharacterInfo();
            cinfo.Id = 1;
            cinfo.Name = "Test";
            cinfo.Tid = 1;
            cinfo.Entity = new NEntity();
            cinfo.Entity.Position = new NVector3();
            cinfo.Entity.Position.X = 4100;
            cinfo.Entity.Position.Y = 3000;
            cinfo.Entity.Position.Z = 800;
            cinfo.Entity.Direction = new NVector3();
            cinfo.Entity.Direction.X = 0;
            cinfo.Entity.Direction.Y = 100;
            cinfo.Entity.Direction.Z = 0;
            this.myCharacter = new Character(cinfo);
            if (entityController != null) entityController.entity = this.myCharacter;
        }
    }

    void FixedUpdate()
    {
        if (myCharacter == null) return;

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (v != 0 || h != 0)
        {
            if (currentState != SkillBridge.Message.CharacterState.Move)
            {
                currentState = SkillBridge.Message.CharacterState.Move;
                this.myCharacter.MoveForward();
                this.SendEntityEvent(EntityEvent.MoveFwd);
            }
            Vector3 dir = new Vector3(h, 0, v).normalized;
            myCharacterController.Move(dir * this.myCharacter.speed / 100f * Time.fixedDeltaTime);


            //Vector3 dir = GameObjectTool.LogicToWorld(myCharacter.direction);
            //Quaternion rot = new Quaternion();
            //rot.SetFromToRotation(dir, this.transform.forward);

            //if (rot.eulerAngles.y > this.turnAngle && rot.eulerAngles.y < (360 - this.turnAngle))
            //{
            //    myCharacter.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
            //    myCharacterController.transform.forward = this.transform.forward;
            //    this.SendEntityEvent(EntityEvent.None);
            //}
        }
        else
        {
            if (currentState != SkillBridge.Message.CharacterState.Idle)
            {
                currentState = SkillBridge.Message.CharacterState.Idle;
                myCharacterController.Move(Vector3.zero);
                this.myCharacter.Stop();
                this.SendEntityEvent(EntityEvent.Idle);
            }
        }

        //float v = Input.GetAxis("Vertical");
        //if (v > 0.01)
        //{
        //    if (currentState != SkillBridge.Message.CharacterState.Move)
        //    {
        //        currentState = SkillBridge.Message.CharacterState.Move;
        //        this.myCharacter.MoveForward();
        //        this.SendEntityEvent(EntityEvent.MoveFwd);
        //    }
        //    //this.myRigidbody.velocity = this.myRigidbody.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(myCharacter.direction) * (this.myCharacter.speed + 9.81f) / 100f;
        //    //myCharacterController.Move(this.myCharacterController.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(myCharacter.direction) * (this.myCharacter.speed + 9.81f) / 100f);
        //    myCharacterController.Move(this.transform.forward * Time.fixedTime);
        //}
        //else if (v < -0.01)
        //{
        //    if (currentState != SkillBridge.Message.CharacterState.Move)
        //    {
        //        currentState = SkillBridge.Message.CharacterState.Move;
        //        this.myCharacter.MoveBack();
        //        this.SendEntityEvent(EntityEvent.MoveBack);
        //    }
        //    //this.myRigidbody.velocity = this.myRigidbody.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(myCharacter.direction) * (this.myCharacter.speed + 9.81f) / 100f;
        //    //myCharacterController.Move(this.myCharacterController.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(myCharacter.direction) * (this.myCharacter.speed + 9.81f) / 100f);
        //}
        //else
        //{
        //    if (currentState != SkillBridge.Message.CharacterState.Idle)
        //    {
        //        currentState = SkillBridge.Message.CharacterState.Idle;
        //        myCharacterController.Move(Vector3.zero);
        //        this.myCharacter.Stop();
        //        this.SendEntityEvent(EntityEvent.Idle);
        //    }
        //}

        //if (Input.GetButtonDown("Jump"))
        //{
        //    this.SendEntityEvent(EntityEvent.Jump);
        //}

        //float h = Input.GetAxis("Horizontal");
        //if (h < -0.1 || h > 0.1)
        //{
        //    this.transform.Rotate(0, h * rotateSpeed, 0);
        //    Vector3 dir = GameObjectTool.LogicToWorld(myCharacter.direction);
        //    Quaternion rot = new Quaternion();
        //    rot.SetFromToRotation(dir, this.transform.forward);

        //    if (rot.eulerAngles.y > this.turnAngle && rot.eulerAngles.y < (360 - this.turnAngle))
        //    {
        //        myCharacter.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
        //        myCharacterController.transform.forward = this.transform.forward;
        //        this.SendEntityEvent(EntityEvent.None);
        //    }
        //}
    }

    public void SendEntityEvent(EntityEvent entityEvent)
    {
        if (entityController != null)
            entityController.OnEntityEvent(entityEvent);
    }
}
