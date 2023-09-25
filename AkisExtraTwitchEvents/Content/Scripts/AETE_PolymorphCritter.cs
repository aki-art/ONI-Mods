using FUtility;
using ImGuiNET;
using KSerialization;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AETE_PolymorphCritter : KMonoBehaviour, ISim200ms, IImguiDebug
	{
		[MyCmpReq] private KSelectable kSelectable;
		[MyCmpReq] private MinionStorage minionStorage;
		[MyCmpReq] private KBatchedAnimController kbac;

		[Serialize] public float duration;
		[Serialize] public float elapsedTime;
		[Serialize] public string originalSpeciesname;
		[Serialize] public string balloonAnim;
		[Serialize] public string balloonSymbol;
		[Serialize] public string originalMinionName;
		[Serialize] public string morph;
		[Serialize] private string currentHat;

		private KBatchedAnimController balloon;
		//private KBatchedAnimTracker hatTracker;

		private float hatOffsetX, hatOffsetY, hatOffsetZ;

		public void OnImgui()
		{
			if(ImGui.Button("Force Release"))
			{
				elapsedTime = duration + 1;
			}

			if (ImGui.DragFloat("X", ref hatOffsetX)
				|| ImGui.DragFloat("Y", ref hatOffsetY)
				|| ImGui.DragFloat("Z", ref hatOffsetZ))
			{
/*				hatTracker.offset = new Vector3(hatOffsetX, hatOffsetY, hatOffsetZ);
				hatTracker.enabled = false;
				hatTracker.enabled = true;
				hatTracker.forceUpdate = true;*/
			}
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			kSelectable.AddStatusItem(TStatusItems.PolymorphStatus, this);
			NameDisplayScreen.Instance.RegisterComponent(gameObject, this);

			if (!morph.IsNullOrWhiteSpace())
			{
				var polymorph = TDb.polymorphs.TryGet(morph);
				if (polymorph == null)
					Log.Warning($"Morph {morph} does not exist ");
				else
					SetMorph(polymorph);
			}

			if (!balloonAnim.IsNullOrWhiteSpace())
				ShowBalloon();

			if (!originalMinionName.IsNullOrWhiteSpace())
				UpdateName();

			Mod.polys.Add(this);
		}

		public override void OnCleanUp()
		{
			//Util.KDestroyGameObject(hatTracker.gameObject);
			ReleaseMinions();
			Mod.polys.Remove(this);	
			base.OnCleanUp();
		}
/*
		public void CreateHat(Polymorph polymorph, string hat_id)
		{
			var hat = Db.Get().AccessorySlots.Hat;
			var accessory = hat.Lookup(hat_id);

			if (accessory == null)
			{
				Log.Warning("Missing hat: " + hat_id);
				return;
			}

			var go = new GameObject("AkisExtraTwitchEvent_Polymorph_Hat");
			go.transform.position = (transform.position + (Vector3)polymorph.hatOffset) with { z = -0.1f };
			go.transform.parent = transform;

			go.SetActive(false);

			var hatKbac = go.AddOrGet<KBatchedAnimController>();
			hatKbac.AnimFiles = new KAnimFile[] { Assets.GetAnim("barbeque_kanim") };
			hatKbac.initialAnim = "object";
*//*
			hatTracker = go.AddOrGet<KBatchedAnimTracker>();
			hatTracker.symbol = polymorph.hatTrackerSymbol;
			hatTracker.offset = polymorph.hatOffset;
			hatTracker.controller = kbac;
			hatTracker.myAnim.offset = polymorph.hatOffset;
*//*
			SymbolOverrideControllerUtil.AddToPrefab(go);

			//var controller = hatTracker.GetComponent<SymbolOverrideController>();
//			controller.AddSymbolOverride("object", accessory.symbol, 4);

			go.SetActive(true);
		}
*/
		private void ReleaseMinions()
		{
			if (minionStorage.serializedMinions == null || minionStorage.serializedMinions.Count == 0)
			{
				Log.Warning("No stored minion in AETE_PolymorphCritter");
				return;
			}

			if (minionStorage.serializedMinions.Count > 1)
				Log.Warning("There are multiple duplicants associated with this critter.");

			var minion = minionStorage.serializedMinions[0];
			var minionGo = minionStorage.DeserializeMinion(minion.id, transform.position);

			AudioUtil.PlaySound(ModAssets.Sounds.POLYMORHPH_END, ModAssets.GetSFXVolume());
			Game.Instance.SpawnFX(ModAssets.Fx.pinkPoof, transform.position, 0);

			minionGo.AddOrGet<Notifier>().Add(new Notification(
				STRINGS.AETE_EVENTS.POLYMOPRH.EVENT_END_NOTIFICATION,
				NotificationType.Neutral,
				click_focus: minionGo.transform,
				clear_on_click: true,
				show_dismiss_button: true));
		}

		public void Sim200ms(float dt)
		{
			elapsedTime += dt;

			if (elapsedTime >= duration)
				Util.KDestroyGameObject(gameObject);
		}

		public void SetMorph(MinionIdentity identity, Polymorph morph)
		{
			duration = ModTuning.POLYMOPRH_DURATION;

			originalMinionName = identity.GetProperName();

			UpdateName();
			UpdateBalloon(identity);

			minionStorage.SerializeMinion(identity.gameObject);
			currentHat = identity.GetComponent<MinionResume>().currentHat;

			SetMorph(morph);
		}

		private void SetMorph(Polymorph morph)
		{
			kbac.SwapAnims(new[] { Assets.GetAnim(morph.animFile) });
			originalSpeciesname = morph.Name;
			this.morph = morph.Id;

			if (morph.Id == TPolymorphs.MUCKROOT)
				GetComponent<Navigator>().enabled = false;

/*			if (!currentHat.IsNullOrWhiteSpace())
				CreateHat(morph, currentHat);*/
		}

		private void UpdateBalloon(MinionIdentity identity)
		{
			var equipment = identity.GetEquipment();
			if (equipment != null)
			{
				var toySlot = equipment.GetSlot(Db.Get().AssignableSlots.Toy);

				if (toySlot == null || toySlot.assignable == null)
					return;

				if (toySlot.assignable.IsPrefabID(EquippableBalloonConfig.ID))
				{
					if (toySlot.assignable.TryGetComponent(out EquippableBalloon balloon))
					{
						balloonAnim = balloon.smi.facadeAnim;
						balloonSymbol = balloon.smi.symbolID;
					}
				}
			}

			ShowBalloon();
		}

		private void ShowBalloon()
		{
			balloon = FXHelpers.CreateEffectOverride(
				new[]
				{
					"balloon_anim_kanim",
					"balloon_basic_red_kanim"
				},
				transform.position with { z = transform.position.z + 0.1f },
				transform,
				false,
				Grid.SceneLayer.Creatures);

			OverrideBalloonSymbol();
			balloon.Play("idle_default", KAnim.PlayMode.Loop);
		}

		public void OverrideBalloonSymbol()
		{
			if (!Assets.TryGetAnim(balloonAnim, out var overrideAnim))
				return;

			var symbol = overrideAnim.GetData().build.GetSymbol(balloonSymbol);

			if (symbol == null)
				return;

			balloon.SwapAnims(new[]
			{
				Assets.GetAnim("balloon_anim_kanim"),
				overrideAnim
			});

			if (balloon.TryGetComponent(out SymbolOverrideController controller))
				controller.AddSymbolOverride("body", symbol);
		}


		private void UpdateName()
		{
			kSelectable.SetName(originalMinionName);
			NameDisplayScreen.Instance.UpdateName(gameObject);
			Trigger((int)GameHashes.NameChanged, originalMinionName);
		}
	}
}
