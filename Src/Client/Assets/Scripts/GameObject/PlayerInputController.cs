using Entities;
using Models;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public CharacterController myCharacterController;
    public EntityController entityController;

    public float rotateSpeed = 10.0f;

    public Character character;
    private CharacterState currentState;
    private EntityEvent currentEventState;

    private Vector2 inputMovement = Vector2.zero;
    private Vector3 playerForword = Vector3.zero;

    private Vector3 lastPos = Vector3.zero;
    private Vector3 lastDir = Vector3.zero;

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
        if (character == null || myCharacterController == null)
            return;

        inputMovement.x = Input.GetAxis("Horizontal");
        inputMovement.y = Input.GetAxis("Vertical");

        playerForword.x = inputMovement.x;
        playerForword.z = inputMovement.y;

        this.PlayerRotate();
        this.PlayerMove();
    }

    public void PlayerRotate()
    {
        if (inputMovement.Equals(Vector3.zero))
            return;

        Quaternion rotation = Quaternion.LookRotation(playerForword, Vector3.up);
        rotation = Quaternion.AngleAxis(MainPlayerCamera.Instance.playerCamera.transform.eulerAngles.y, Vector3.up) * rotation;
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
                myCharacterController.SimpleMove(Vector3.zero);
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
        myCharacterController.SimpleMove(Quaternion.Euler(0, MainPlayerCamera.Instance.playerCamera.transform.rotation.eulerAngles.y, 0) * playerForword * this.character.speed / 100f);
        this.character.SetPosition(GameObjectTool.WorldToLogic(this.transform.position));
    }

    Vector3 lastPos1;
    //float lastSync = 0;
    private void LateUpdate()
    {
        if (this.character == null) return;

        //Vector3 offset = this.transform.position - lastPos1;
        //this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        //Debug.LogFormat("LateUpdate velocity {0} : {1}", this.rb.velocity.magnitude, this.speed);
        //this.lastPos1 = this.transform.position;

        //if ((GameObjectTool.WorldToLogic(this.transform.position) - this.character.position).magnitude > 50)
        //{
        //    this.character.SetPosition(GameObjectTool.WorldToLogic(this.transform.position));
        //    this.SendEntityEvent(EntityEvent.None);
        //}
        //this.transform.position = this.transform.position;

        if (lastPos != this.transform.position || lastDir != this.transform.forward)
        {
            MapService.Instance.SendMapEntitySyncRequest(currentEventState, character.EntityData);
            lastPos = this.transform.position;
            lastDir = this.transform.forward;
        }
    }

    public void SendEntityEvent(EntityEvent entityEvent)
    {
        currentEventState = entityEvent;
        if (entityController != null)
            entityController.OnEntityEvent(entityEvent);

        MapService.Instance.SendMapEntitySyncRequest(currentEventState, character.EntityData);
    }
}
