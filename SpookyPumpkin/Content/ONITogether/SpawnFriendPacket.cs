//using ONI_Together.Networking.Packets.Architecture;
//using ONI_Together_API.Networking;
//using SpookyPumpkinSO.Content.GhostPip.Spawning;
//using System;
//using System.Collections.Generic;
//using System.IO;

//namespace SpookyPumpkinSO.Content.MP
//{
//    public class SpawnFriendPacket : IPacket
//    {
//        private ulong senderId;
//        private int telepadCell;

//        public SpawnFriendPacket()
//        { 
//        }

//        public SpawnFriendPacket(int telepadCell, ulong senderId)
//        {
//            this.telepadCell = telepadCell;
//            this.senderId = senderId;
//        }

//        public void Deserialize(BinaryReader reader)
//        {
//            senderId = reader.ReadUInt64();
//            telepadCell = reader.ReadInt32();
//        }

//        public void OnDispatched()
//        {
//            Console.WriteLine($"SpawnFriendPacket with cell: {telepadCell}");
            
//            if(Grid.ObjectLayers == null)
//            {
//                Log.Warning("Attempting to send SpawnFriendPacket before Grid is initialized.");
//                return;
//            }

//            var success = false;

//            if (Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(telepadCell, out var telepad))
//            {
//                if(telepad.TryGetComponent(out GhostPipSpawner spawner))
//                {
//                    spawner.TrySpawnSusPip();
//                    success = true;
//                }
//            }

//            if (!success)
//                Log.Warning("Could not sync spawning ghost pip :(");

//            if (ONI_Together_API.SessionInfoAPI.IsHost)
//            {
//                //re-broadcast to other clients
//                HashSet<ulong> exclude = [senderId, ONI_Together_API.SessionInfoAPI.LocalUserID];
//                PacketSenderAPI.SendToAllExcluding(this, exclude);
//            }
//        }

//        public void Serialize(BinaryWriter writer)
//        {
//            writer.Write(senderId);
//            writer.Write(telepadCell);
//        }
//    }
//}
