using FUtility;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GlassCase.Buildings
{
    public class GlassCaseValve : GlassCasePiece
    {
        [SerializeField]
        public float insulation = 0;

        public Dictionary<int, GlassCasePiece> elements = new Dictionary<int, GlassCasePiece>();

        public int NetworkSize => elements.Count;

        public void RemoveTile(int cell)
        {
            elements.Remove(cell);
        }

        protected override void OnSpawn()
        {
            masterID = ID;

            base.OnSpawn();
            Mod.GlassCaseValves.Add(this);
            Mod.glassCaseElements.Add(this.NaturalBuildingCell(), this);

            Refresh();
        }

        protected override void OnCleanUp()
        {
            Mod.GlassCaseValves.Remove(this);
            Mod.glassCaseElements.Remove(ID);
            base.OnCleanUp();
        }

        protected override void OnConnect(GlassCaseValve master)
        {
            if(master.ID != ID)
            {
                Log.Debuglog("Multiple masters connected");
            }
        }

        public void Refresh()
        {
            Log.Debuglog(ID, "refreshing");
            HashSet<int> connected = FindAllConnected();
            if(connected.Count <= 1)
            {
                Log.Debuglog("empty valve connections");
                return;
            }

            foreach(var element in elements)
            {
                if(!connected.Contains(element.Key))
                {
                    element.Value.Disconnect();
                }
            }

            elements.Clear();

            foreach (int pieceID in connected)
            {
                if (Mod.glassCaseElements.TryGetValue(pieceID, out GlassCasePiece piece))
                {
                    if (piece == this) continue;
                    piece.Connect(this, false);
                }
            }
        }

        protected HashSet<int> FindAllConnected()
        {
            HashSet<int> result = new HashSet<int>();
            GameUtil.FloodFillConditional(this.NaturalBuildingCell(), FloodCriteria, new HashSet<int>(), result);
            return result;
        }

        private bool FloodCriteria(int arg)
        {
            if(Mod.glassCaseElements.TryGetValue(arg, out GlassCasePiece piece)) {
                return piece.masterID == ID || piece.masterID == -1;
            }

            return false;
        }

        protected override void OnDisconnect()
        {
            foreach(var element in elements)
            {
                if(element.Value.ID != ID)
                {
                    element.Value.Disconnect();
                }
            }

            elements.Clear();
        }
    }
}
