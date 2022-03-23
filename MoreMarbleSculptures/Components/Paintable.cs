using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMarbleSculptures.Components
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class Paintable : KMonoBehaviour, ISaveLoadable
    {
        [Serialize]
        public string primaryColorTint;

        [Serialize]
        public string secondaryColorTint;

        [Serialize]
        public string originalColor;

        public string secondaryTintAnimFile;

        [SerializeField]
        public Vector3 offset;

        [SerializeField]
        public HashSet<string> overLays;

        [Serialize]
        public string overlayAnim;

        [MyCmpReq]
        private KBatchedAnimController primaryKbac;

        private KBatchedAnimController secondaryKbac;

        private KAnimLink link;

        protected override void OnSpawn()
        {
            gameObject.AddOrGet<CopyBuildingSettings>();

            originalColor = ((Color)primaryKbac.TintColour).ToHexString();
            base.OnSpawn();

            if(!overlayAnim.IsNullOrWhiteSpace())
            {
                //SetOverlayAnim(overlayAnim);
            }

            RestoreTints();

            Subscribe((int)ModHashes.ArtableStangeChanged, OnStageChanged);
        }

        private void OnStageChanged(object obj)
        {
            if(obj is string id && overLays.Contains(id))
            {
                //SetOverlayAnim(id);
            }
            else
            {
                HideOverlay();
            }
        }

        private void HideOverlay()
        {
            if(secondaryKbac != null)
            {
                secondaryKbac.enabled = false;
            }
        }

        private void RestoreTints()
        {
            if (!primaryColorTint.IsNullOrWhiteSpace())
            {
                primaryKbac.TintColour = Util.ColorFromHex(primaryColorTint);
            }

            if (!secondaryColorTint.IsNullOrWhiteSpace() && secondaryKbac is object)
            {
                //secondaryKbac.TintColour = Util.ColorFromHex(secondaryColorTint);
            }
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        private void OnCopySettings(object obj)
        {
            if (obj is GameObject go && go.TryGetComponent(out Paintable paintable))
            {
                if (paintable.primaryColorTint.IsNullOrWhiteSpace())
                {
                    ResetColor();
                    return;
                }

                SetColor(paintable.primaryColorTint);
            }
        }

        public void ResetColor()
        {
            SetColor(originalColor);
        }

        private void SetColor(string hex)
        {
            SetColor(Util.ColorFromHex(hex));
        }

        public void SetColor(Color color)
        {
            primaryKbac.TintColour = color;
            primaryColorTint = color.ToHexString();
        }

        public void SetOverlayAnim(string anim)
        {
            overlayAnim = anim;
            if(secondaryKbac == null)
            {
                secondaryKbac = CreateOverlay();
            }

            secondaryKbac.enabled = true;
            secondaryKbac.Play(anim);
        }

        private void SetSecondaryColor(string hex)
        {
            SetSecondaryColor(Util.ColorFromHex(hex));
        }

        public void SetSecondaryColor(Color color)
        {
            if(secondaryKbac == null)
            {
                secondaryKbac = CreateOverlay();
            }

            secondaryKbac.TintColour = color;
            secondaryColorTint = color.ToHexString();
        }

        public KBatchedAnimController CreateOverlay()
        {
            var go = new GameObject("Art Tint Overlay");

            go.transform.parent = transform;

            go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);

            var kbac = gameObject.AddComponent<KBatchedAnimController>();
            kbac.AnimFiles = new KAnimFile[]
            {
                Assets.GetAnim(secondaryTintAnimFile)
            };

            kbac.initialAnim = "slab";
            kbac.isMovable = true;
            kbac.sceneLayer = Grid.SceneLayer.BuildingFront;

            go.transform.SetPosition(transform.position + offset);

            kbac.gameObject.SetActive(true);

            link = new KAnimLink(primaryKbac, kbac);

            return kbac;
        }
    }
}
