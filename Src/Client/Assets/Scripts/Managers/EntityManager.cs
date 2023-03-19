using Entities;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEntityNotify
{
    void OnEntityChanged(Entity entity);
    void OnEntityEvent(EntityEvent entityEvent);
    void OnEntityRemoved();
}

public class EntityManager : MonoSingleton<EntityManager>
{
    Dictionary<int, Entity> Entitys = new Dictionary<int, Entity>();

    Dictionary<int, IEntityNotify> EntityNotifers = new Dictionary<int, IEntityNotify>();

    public void RegisterEntityChangeNotify(int entityId, IEntityNotify entityNotify)
    {
        if (EntityNotifers.ContainsKey(entityId))
            return;

        EntityNotifers[entityId] = entityNotify;
    }

    public void AddEntity(Entity entity)
    {
        if (Entitys.ContainsKey(entity.entityId))
            return;

        Entitys[entity.entityId] = entity;
    }

    public void RemoveEntity(int entityId)
    {
        if (Entitys.ContainsKey(entityId))
            Entitys.Remove(entityId);

        if (EntityNotifers.ContainsKey(entityId))
            EntityNotifers[entityId].OnEntityRemoved();
            EntityNotifers.Remove(entityId);
    }

    public void OnEntitySync(NEntitySync entitySync)
    {
        Entity entity = null;

        if (!Entitys.TryGetValue(entitySync.Id, out entity))
            return;

        if (entitySync.Entity != null)
            entity.SetEntityData(entitySync.Entity);
            EntityNotifers[entity.entityId].OnEntityChanged(entity);
            EntityNotifers[entity.entityId].OnEntityEvent(entitySync.Event);
    }
}
