using FUtility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Twitchery.Content.Scripts
{
    public class PipHoverable : KMonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField]
        public KBatchedAnimController pipKbac;

        private SymbolOverrideController symbolOverrideController;

        [SerializeField]
        public HashedString symbolName = "sq_mouth";

        private KAnim.Build.Symbol normalMouth;
        private KAnim.Build.Symbol happyMouth;

        public override void OnSpawn()
        {
            base.OnSpawn();
            symbolOverrideController = pipKbac.GetComponent<SymbolOverrideController>();
            var build = pipKbac.animFiles[0].GetData().build;
            normalMouth = build.GetSymbol(symbolName);

            var symbol_to_replace = KAnimBatchManager.Instance()
                .GetBatchGroupData(pipKbac.batchGroupID)
                .GetSymbol(symbolName);

            var data = KAnimBatchManager.Instance().GetBatchGroupData(symbol_to_replace.build.batchTag);

            var expression_sfi = data.symbolFrameInstances[symbol_to_replace.firstFrameIdx + 5];
            expression_sfi.buildImageIdx = symbolOverrideController.GetAtlasIdx(symbol_to_replace.build.GetTexture(0));
            pipKbac.SetSymbolOverride(symbol_to_replace.firstFrameIdx, ref expression_sfi);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Log.Debuglog("click");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}
