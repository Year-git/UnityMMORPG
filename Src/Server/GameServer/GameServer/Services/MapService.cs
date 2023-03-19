using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class MapService : Singleton<MapService>
    {
        public MapService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnMapEntitySyncRequest);
        }

        public void Init()
        {
            MapManager.Instance.Init();
        }

        public void Start()
        {
        }

        private void OnMapEntitySyncRequest(NetConnection<NetSession> sender, MapEntitySyncRequest request)
        {
            Character character = sender.Session.Character;

            Log.InfoFormat("MapEntitySyncRequest: character_{0}_{1}  EntityId:{2}  Event{3}  Entity{4}", character.Id, character.Info.Name, request.entitySync.Id, request.entitySync.Event, request.entitySync.Entity.String());

            MapManager.Instance[character.Info.mapId].UpdateEntity(request.entitySync);
        }

        public void SendMapEntitySyncResponse(NetConnection<NetSession> conn, NetMessage netMessage)
        {
            byte[] data = PackageHandler.PackMessage(netMessage);
            conn.SendData(data, 0, data.Length);
        }
    }
}
