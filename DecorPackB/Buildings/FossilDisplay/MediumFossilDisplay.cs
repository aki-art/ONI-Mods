using FUtility;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;
using static DecorPackB.STRINGS.BUILDINGS.PREFABS;

namespace DecorPackB.Buildings.FossilDisplay
{
    public class MediumFossilDisplay : KMonoBehaviour, IExhibition
    {
        private static readonly Dictionary<string, string> descriptions = new Dictionary<string, string>()
        {
            { "Default", "Default" },
            { "Parasaur", DECORPACKB_FOSSILDISPLAY.VARIANT.PARASAUROLOPHUS.DESC },
            { "Pacu", DECORPACKB_FOSSILDISPLAY.VARIANT.PACU.DESC },
            { "Human", DECORPACKB_FOSSILDISPLAY.VARIANT.HUMAN.DESC },
            { "Trilobite", DECORPACKB_FOSSILDISPLAY.VARIANT.TRILOBITE.DESC },
            { "Spider", DECORPACKB_FOSSILDISPLAY.VARIANT.SPIDER.DESC },
            { "Volgus", DECORPACKB_FOSSILDISPLAY.VARIANT.VOLGUS.DESC },
            { "Beefalo", DECORPACKB_FOSSILDISPLAY.VARIANT.BEEFALO.DESC },
            { "HellHound", DECORPACKB_FOSSILDISPLAY.VARIANT.HELLHOUND.DESC }
        };

        [SerializeField]
        public int rangeWidth = 15;

        [SerializeField]
        public int rangeHeight = 8;

        [MyCmpGet]
        Assemblable assemblable;


        private Reactable fossilReactable;

        public void CreateInspiredReactable(string effect)
        {
            if (fossilReactable == null)
            {
                fossilReactable = new EmoteReactable(gameObject, "Inspired", Db.Get().ChoreTypes.Emote, "anim_react_starry_eyes_kanim", rangeWidth, rangeHeight, 0, 10f)
                    .AddThought(Db.Get().Thoughts.Angry)
                    .AddStep(new EmoteReactable.EmoteStep
                    {
                        anim = "react",
                        startcb = reactor => AddReactionEffect(reactor, effect)
                    })
                    .AddPrecondition(new Reactable.ReactablePrecondition(ReactorIsOnFloor));
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            assemblable.Subscribe((int)GameHashes.WorkableCompleteWork, OnAssemblyComplete);
            //fossilDisplay.Trigger((int)ModHashes.FossilStageSet);
        }

        private void OnAssemblyComplete(object obj)
        {
            //throw new NotImplementedException();
        }

        public string GetDescription()
        {
            string flavorText = descriptions.TryGetValue(GetComponent<Assemblable>().CurrentStage, out string desc) ? desc : "";
            return flavorText + "\n\n" + DECORPACKB_FOSSILDISPLAY.ASSEMBLEDBY.Replace("{duplicantName}", assemblable.assemblerName);
        }

        private void AddReactionEffect(GameObject reactor, string effect)
        {
            Effects effects = reactor.GetComponent<Effects>();

            bool hasSmall = effects.HasEffect(ModAssets.Effects.INSPIRED_LOW);
            bool hasMedium = effects.HasEffect(ModAssets.Effects.INSPIRED_GOOD);
            bool hasSuper = effects.HasEffect(ModAssets.Effects.INSPIRED_GIANT);


            switch (effect)
            {
                case ModAssets.Effects.INSPIRED_LOW:
                    if (!hasMedium && !hasSuper)
                    {
                        effects.Add(ModAssets.Effects.INSPIRED_LOW, true);
                    }

                    break;
                case ModAssets.Effects.INSPIRED_GOOD:
                    if (hasSmall)
                    {
                        effects.Remove(ModAssets.Effects.INSPIRED_LOW);
                    }

                    if (!hasSuper)
                    {
                        effects.Add(ModAssets.Effects.INSPIRED_GOOD, true);
                    }

                    break;
                case ModAssets.Effects.INSPIRED_GIANT:
                    if (hasSmall)
                    {
                        effects.Remove(ModAssets.Effects.INSPIRED_LOW);
                    }

                    if (hasMedium)
                    {
                        effects.Remove(ModAssets.Effects.INSPIRED_GOOD);
                    }

                    effects.Add(ModAssets.Effects.INSPIRED_GIANT, true);
                    break;
                default:
                    Log.Warning($"Something went wrong trying to add an Inspired Reaction effect. Effect ({effect}) is invalid.");
                    break;
            };
        }

        private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
        {
            return transition.end == NavType.Floor;
        }
    }
}
