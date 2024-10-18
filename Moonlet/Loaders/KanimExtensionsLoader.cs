using Moonlet.TemplateLoaders;
using Moonlet.Utils;
using System.Collections.Generic;

namespace Moonlet.Loaders
{
	public class KanimExtensionsLoader(string path) : TemplatesLoader<KanimExtensionLoader>(path)
	{
		private HashSet<HashedString> loopingEntries = [];

		public void LoadAudioSheets(AudioSheets audioSheets)
		{
			var defaultEventsList = new List<AudioSheet.SoundInfo>();
			var loopingEventsList = new List<AudioSheet.SoundInfo>();

			ApplyToActiveTemplates(template => template.RegisterSoundEvents(ref defaultEventsList, ref loopingEventsList));

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

			foreach (var loopy in loopingEventsList)
			{
				foreach (var sound in loopy.GetAllEvents())
					loopingEntries.Add(sound.name);
			}
		}

		public bool HasLoopingSounds(HashedString anim) => loopingEntries.Contains(anim);
	}
}
