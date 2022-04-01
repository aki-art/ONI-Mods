using KSerialization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace MoreMarbleSculptures.Components
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ArtOverride : KMonoBehaviour, ISaveLoadable
    {
        [SerializeField]
        public List<Artable.Stage> extraStages;

        [SerializeField]
        public Vector3 offset;

        [SerializeField]
        public string animFileName;

        [SerializeField]
        public string[] fallbacks;

        [MyCmpReq]
        private Artable artable;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        [Serialize]
        public string overrideStage;

        private KBatchedAnimController facade;
        private KAnimLink link;
        private FieldInfo f_currentStage;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            artable.stages.AddRange(extraStages);
            f_currentStage = typeof(Artable).GetField("currentStage", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        // Switching out the actual status stored on the Artable; this way the game won't soft lock when the mod is removed
        // The counterpart is done in Artable Onspawn patch, because deserialization order does not seem to be guaranteed and Artable may override my changes later
        [OnSerializing]
        private void OnSerialize()
        {
            if(!overrideStage.IsNullOrWhiteSpace() && artable != null)
            {
                f_currentStage.SetValue(artable, fallbacks[(int)artable.CurrentStatus]);
            }
        }

        [OnSerialized]
        private void OnSerialized()
        {
            if (!Game.IsQuitting() && !overrideStage.IsNullOrWhiteSpace() && artable != null)
            {
                f_currentStage.SetValue(artable, overrideStage);
            }
        }

        private bool IsValidOverrideId(string id)
        {
            return extraStages.Any(s => s.id == id);
        }

        public void OverrideAnim(string stageId)
        {
            if (stageId == "Default" || (stageId == overrideStage && facade is object)) return;

            if (IsValidOverrideId(stageId))
            {
                ReplaceWithOverride(stageId);
                return;
            }

            RestoreOriginal();
        }

        private void ReplaceWithOverride(string stageId)
        {
            // using a facade kbac to overlay the original
            // replacing the existing kanim is suprisingly problematic
            CreateFacade(animFileName);
            LinkKanims();
            facade.Play(stageId);
            kbac.SetSymbolVisiblity("sculpt", false);
            overrideStage = stageId;
        }

        private void RestoreOriginal()
        {
            DestroyFacade();
            kbac.SetSymbolVisiblity("sculpt", true);
            overrideStage = null;
        }

        private void LinkKanims()
        {
            link = new KAnimLink(kbac, facade);
        }

        protected void DestroyFacade()
        {
            if (facade != null)
            {
                link.Unregister();
                Destroy(facade.gameObject);
            }
        }

        protected void CreateFacade(string anim)
        {
            DestroyFacade();

            var facadeGo = new GameObject(name);
            facadeGo.SetActive(false);
            facadeGo.transform.parent = transform;

            facadeGo.AddComponent<KPrefabID>().PrefabTag = new Tag(name);

            facade = facadeGo.AddComponent<KBatchedAnimController>();
            facade.AnimFiles = new KAnimFile[]
            {
                Assets.GetAnim(anim)
            };

            facade.initialAnim = "idle";
            facade.isMovable = true;
            facade.sceneLayer = Grid.SceneLayer.Building;

            facadeGo.transform.SetPosition(transform.position + offset);
            facade.gameObject.SetActive(true);
        }

        
    }
}
