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
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        }

        public void Init()
        {

        }

        void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
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
        }

         void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMapData = map;
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else
                Debug.LogErrorFormat("EnterMap: Map {0} not existed", mapId);
        }

        void LeaveMap()
        {
            currentMapId = 0;
            User.Instance.CurrentCharacter = null;
        }
    }
}
