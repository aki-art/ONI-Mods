using Moonlet.Templates;
using Moonlet.Utils;
using System.Collections.Generic;
using System.IO;

namespace Moonlet.TemplateLoaders
{
	public class KanimExtensionLoader(KanimExtensionTemplate template, string sourceMod) : TemplateLoaderBase<KanimExtensionTemplate>(template, sourceMod)
	{
		public string kanimName;

		public override void RegisterTranslations() { }

		public override void Initialize()
		{
			kanimName = Path.GetFileName(relativePath);
			if (!kanimName.EndsWith("_kanim"))
				kanimName += "_kanim";

			id = $"{sourceMod}_{kanimName}";
			template.Id = id;

			base.Initialize();


			Log.Debug($"Registering animation to {kanimName}");
		}

		public void RegisterSoundEvents(ref List<AudioSheet.SoundInfo> defaultEvents, ref List<AudioSheet.SoundInfo> loopingEvents)
		{
			if (template.SoundEvents == null)
				return;

			foreach (var soundEvent in template.SoundEvents)
			{
				if (!Assets.TryGetAnim(kanimName, out var _))
				{
					if (!template.Optional)
						Warn($"This Kanim does not exist: {kanimName}");

					continue;
				}

				if (soundEvent.LoopingSoundEvent != null)
				{
					if (IsSoundRegistered(soundEvent.LoopingSoundEvent))
					{
						loopingEvents.Add(Loop(kanimName, soundEvent.Animation, soundEvent.LoopingSoundEvent, soundEvent.LoopMinInterval.CalculateOrDefault(1000)));
					}
				}

				var ev = new AudioSheet.SoundInfo()
				{
					File = kanimName,
					Anim = soundEvent.Animation,
					RequiredDlcId = DlcManager.VANILLA_ID
				};

				if (soundEvent.Frames != null)
				{
					SortedDictionary<int, string> sortedDict = new(soundEvent.Frames);

					int index = 0;
					foreach (var frame in sortedDict)
					{
						if (index > 11)
						{
							Warn($"Maximum 12 sound effects can be registered to a single animation! {template.Id} {soundEvent.Animation} has {sortedDict.Count}.");
							break;
						}

						Log.Debug($"set event data: {index} {frame.Value} {frame.Key}");
						ev.SetEventData(index, frame.Value, frame.Key);
						index++;
					}
				}
			}
		}

		private bool IsSoundRegistered(string soundEvent)
		{
			foreach (var soundDescription in KFMOD.soundDescriptions)
			{
				var path = soundDescription.Value.path;
				if (Assets.GetSimpleSoundEventName(path) == soundEvent)
					return true;
			}

			Warn($"Sound event {soundEvent} not found. Add it through an FMOD bank file or pick one from the existing items. List all available items from the in game console with command fmoddump.");

			return false;
		}

		private static AudioSheet.SoundInfo Single(string file, string anim, string eventName, int frame)
		{
			return new AudioSheet.SoundInfo()
			{
				File = file,
				Anim = anim,
				Frame0 = frame,
				Name0 = eventName,
				RequiredDlcId = DlcManager.VANILLA_ID
			};
		}


		private static AudioSheet.SoundInfo Loop(string file, string anim, string eventName, float minInterval, int frame = 0)
		{
			return new AudioSheet.SoundInfo()
			{
				File = file,
				Anim = anim,
				MinInterval = minInterval,
				Frame0 = frame,
				Name0 = eventName,
				RequiredDlcId = DlcManager.VANILLA_ID
			};
		}
	}
}
