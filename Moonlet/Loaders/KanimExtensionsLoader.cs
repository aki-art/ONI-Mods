using Moonlet.TemplateLoaders;
using System.Collections.Generic;

namespace Moonlet.Loaders
{
	public class KanimExtensionsLoader(string path) : TemplatesLoader<KanimExtensionLoader>(path)
	{
		private readonly HashSet<HashedString> loopingEntries = [];

		public override void Initialize()
		{
			base.Initialize();

			ApplyToActiveLoaders(loader =>
			{
				if (loader.template.SoundEvents == null)
					return;

				foreach (var soundEvent in loader.template.SoundEvents)
				{
					if (!soundEvent.LoopingSoundEvent.IsNullOrWhiteSpace())
						loopingEntries.Add(loader.kanimName);
				}
			});
		}

		public void LoadAudioSheets(AudioSheets audioSheets)
		{
			var defaultEventsList = new List<AudioSheet.SoundInfo>();
			var loopingEventsList = new List<AudioSheet.SoundInfo>();

			ApplyToActiveLoaders(template => template.RegisterSoundEvents(ref defaultEventsList, ref loopingEventsList));

			if (defaultEventsList.Count > 0)
				audioSheets.sheets.Add(new AudioSheet()
				{
					defaultType = "SoundEvent",
					soundInfos = [.. defaultEventsList]
				});

			if (loopingEventsList.Count > 0)
				audioSheets.sheets.Add(new AudioSheet()
				{
					defaultType = "LoopingSoundEvent",
					soundInfos = [.. loopingEventsList]
				});
		}

		public bool HasLoopingSounds(HashedString anim) => loopingEntries.Contains(anim);
	}
}
