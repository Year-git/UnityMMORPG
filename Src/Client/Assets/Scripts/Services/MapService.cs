using System;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;
using Models;
using Common.Data;

namespace Services
{
    class MapService : Singleton<MapService>, IDisposable
    {
        public int currentMapId = 0;
        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEntitySyncResponse);

            
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(this.OnMapEntitySyncResponse);
        }
        public void Init()
        {
            Debug.LogFormat("MapService:Init");
        }

        void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter:{0}", response.mapId);
            foreach (var characterInfo in response.Characters)
            {
                Debug.LogFormat("OnMapCharacterEnter:{0} [{1}]", response.mapId, characterInfo.Name);
                if (User.Instance.CurrentCharacter.Id == characterInfo.Id)
                {
                    User.Instance.CurrentCharacter = characterInfo;
                }
                CharacterManager.Instance.AddCharacter(characterInfo);
            }

            if (currentMapId != response.mapId)
            {
                this.EnterMap(User.Instance.CurrentCharacter.mapId);
                currentMapId = response.mapId;
            }
        }

        void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Debug.LogFormat("OnMapCharacterLeave:{0}", response.characterId);
            if (response.characterId == User.Instance.CurrentCharacter.Id)
            {
                this.LeaveMap();
            }
            else
            {
                CharacterManager.Instance.RemoveCharacter(response.characterId);
            }
        }

        public void SendMapEntitySyncRequest(EntityEvent entityEvent, NEntity entity)
        {
            Debug.LogFormat("MapEntitySyncRequest:  entityEvent:{0}  NEntity:{1}", entityEvent.ToString(), entity.Id);

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync();
            message.Request.mapEntitySync.entitySync.Id = entity.Id;
            message.Request.mapEntitySync.entitySync.Event = entityEvent;
            message.Request.mapEntitySync.entitySync.Entity = entity;
            UserService.Instance.SendMessage(message);
        }

        void OnMapEntitySyncResponse(object sender, MapEntitySyncResponse message)
        {
            Debug.LogFormat("OnMapEntitySyncResponse:{0}", message.entitySyncs);
            foreach (var entity in message.entitySyncs)
            {
                EntityManager.Instance.OnEntitySync(entity);
            }
        }

        void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMapData = map;
                SceneManager.Instance.LoadScene(map.Resource);
                ViewManager.Instance.CreateView("UIMain");
            }
            else
                Debug.LogErrorFormat("EnterMap: Map {0} not existed", mapId);
        }

        void LeaveMap()
        {
            currentMapId = 0;
            User.Instance.CurrentCharacter = null;
            CharacterManager.Instance.RemoveCharacter(User.Instance.CurrentCharacter.Id);
            ViewManager.Instance.RemoveView("UIMain");
            ViewManager.Instance.CreateView("UISelectCharacter");
            SceneManager.Instance.LoadScene("SelectCharacter");
        }
    }
}
