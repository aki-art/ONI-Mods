using FUtility;
using KSerialization;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AETE_PolymorphCritter : KMonoBehaviour, ISim200ms
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

		private KBatchedAnimController balloon;

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
		}

		public override void OnCleanUp()
		{
			ReleaseMinions();
			base.OnCleanUp();
		}

		private void ReleaseMinions()
		{
			if (minionStorage.serializedMinions == null)
			{
				Log.Warning("No stored minion in AETE_PolymorphCritter");
				return;
			}

			// there should be only one, but just in case
			for (var i = 0; i < minionStorage.serializedMinions.Count; i++)
			{
				var minion = minionStorage.serializedMinions[i];
				minionStorage.DeserializeMinion(minion.id, transform.position);
			}
		}

		public void Sim200ms(float dt)
		{
			elapsedTime += dt;

			if (elapsedTime >= duration)
				Util.KDestroyGameObject(gameObject);
		}

		public void SetMorph(MinionIdentity identity, Polymorph morph)
		{
			duration = Consts.CYCLE_LENGTH;

			originalMinionName = identity.GetProperName();

			UpdateName();
			UpdateBalloon(identity);

			minionStorage.SerializeMinion(identity.gameObject);

			SetMorph(morph);
		}

		private void SetMorph(Polymorph morph)
		{
			kbac.SwapAnims(new[] { Assets.GetAnim(morph.animFile) });
			originalSpeciesname = morph.Name;
			this.morph = morph.Id;

			if (morph.Id == TPolymorphs.MUCKROOT)
				GetComponent<Navigator>().enabled = false;
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
