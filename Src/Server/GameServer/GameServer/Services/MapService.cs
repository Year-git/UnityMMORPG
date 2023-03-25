using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Data;
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
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapTeleportRequest>(this.OnMapTeleportRequest);
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

        private void OnMapTeleportRequest(NetConnection<NetSession> sender, MapTeleportRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("MapTeleportRequest: character_{0}_{1}  teleporterId:{2}", character.Id, character.Info.Name, request.teleporterId);
            TeleporterDefine teleporterDefine = null;
            TeleporterDefine linkToTeleporterDefine = null;
            if (!DataManager.Instance.Teleporters.TryGetValue(request.teleporterId, out teleporterDefine))
                return;

            if (!DataManager.Instance.Teleporters.TryGetValue(teleporterDefine.LinkTo, out linkToTeleporterDefine))
                return;
            
            if (MapManager.Instance[linkToTeleporterDefine.MapID] == null)
                return;

            MapManager.Instance[character.Info.mapId].CharacterLeave(sender, character);
            character.Position = linkToTeleporterDefine.Position;
            character.Direction = linkToTeleporterDefine.Direction;
            MapManager.Instance[linkToTeleporterDefine.MapID].CharacterEnter(sender, character);
        }
    }
}
