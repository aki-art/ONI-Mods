using FUtility;
using System.Collections.Generic;
using UnityEngine;

namespace SketchPad.History
{
    // https://www.youtube.com/watch?v=LRZ1cuXiXTI
    public class EditHistory : KMonoBehaviour
    {
        public static EditHistory Instance; // temporary, should be one per sketchbook instance

        private List<ICommand> commands;
        private int index;

        public void AddCommand(ICommand command)
        {
            if(index < commands.Count)
            {
                commands.RemoveRange(index, commands.Count - index);
            }

            commands.Add(command);
            command.Execute();
            index++;

            Log.Debuglog($"ADDING count: {commands.Count} index: {index}");
        }

        protected override void OnPrefabInit()
        {
            Instance = this;

            base.OnPrefabInit();

            commands = new List<ICommand>();
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void Undo()
        {
            if(commands.Count == 0) return;

            if(index > 0)
            {
                commands[index - 1].Undo();
                index--;
                Log.Debuglog($"{index}");
            }
        }

        public void Redo()
        {
            if (commands.Count == 0) return;

            if(index < commands.Count) 
            {
                index++;
                commands[index - 1].Execute();
                Log.Debuglog($"{index}");
            }
        }


#if DEBUG
#pragma warning disable IDE0051 // Remove unused private members
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(210, 400, 200, 500));

            GUILayout.BeginHorizontal();

            if(GUILayout.Button("Undo"))
            {
                Undo();
            }

            if(GUILayout.Button("Redo"))
            {
                Redo();
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }
#pragma warning restore IDE0051 // Remove unused private members
#endif
    }
}
