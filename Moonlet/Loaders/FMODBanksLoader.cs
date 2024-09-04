using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using System.IO;

namespace Moonlet.Loaders
{
	public class FMODBanksLoader() : ContentLoader("FMOD")
	{
		private readonly List<string> bankPaths = [];

		public void LoadContent(MoonletMod mod, string[] delimiter)
		{
			if (mod.data.LoadFMODBanks == null)
				return;

			if (!RuntimeManager.IsInitialized)
			{
				Log.Debug("Runtime Manager not yet initialized!!");
				return;
			}


			Log.Info(" == Loading FMOD Bank files ==", mod.staticID);

			foreach (var path in mod.data.LoadFMODBanks)
			{
				var parts = path.Split(delimiter, System.StringSplitOptions.RemoveEmptyEntries);
				string absolutePath = "";

				if (parts.Length > 1)
				{
					if (parts[0] == "root")
					{
						absolutePath = Path.Combine(mod.path, parts[1]);
					}
				}
				else
				{
					absolutePath = Path.Combine(mod.GetAssetPath(this.path), parts[0]);
				}

				if (!File.Exists(absolutePath))
				{
					Log.Warn($"Requested FMOD Bank to be loaded, but file not found: {absolutePath}", mod.staticID);
					continue;
				}

				LoadBankFile(absolutePath, out _);
			}
		}

		private FMOD.RESULT LoadBankFile(string path, out Bank bankFile)
		{
			var loadResult = RuntimeManager.StudioSystem.loadBankFile(path, LOAD_BANK_FLAGS.NORMAL, out bankFile);

			if (loadResult != FMOD.RESULT.OK)
			{
				Log.Warn($"Failed to load bank file {path}: {loadResult}");
				return loadResult;
			}

			var place = path.LastIndexOf(".bank");

			if (place != -1)
			{
				var stringsPath = path
					.Remove(place, 5)
					.Insert(place, ".strings.bank");

				if (File.Exists(stringsPath))
				{
					RuntimeManager.StudioSystem.loadBankFile(stringsPath, LOAD_BANK_FLAGS.NORMAL, out var stringsBank);
					bankFile.getPath(out var stringsBankPath);

					bankPaths.Add(stringsBankPath);
				}
			}

			bankFile.loadSampleData();
			bankFile.getPath(out var path2);
			Settings.Instance.Banks.Add(path2);

			CollectSoundDescriptions(bankFile);

			var countResult = bankFile.getEventCount(out int eventCount);
			bankFile.getPath(out var bankId);

			bankPaths.Add(bankId);

			if (countResult != FMOD.RESULT.OK)
				Log.Warn($"Something went wrong loading bank file: {countResult}");
			else
			{
				Log.Info($"Loaded bank file \"{bankId}\" with {eventCount} events.");
			}

			return loadResult;
		}

		/// copy paste of <see cref="KFMOD.CollectSoundDescriptions" />, but just for the one bank we want
		private static void CollectSoundDescriptions(Bank bank)
		{
			bank.getEventList(out EventDescription[] eventDescriptions);

			for (var i = 0; i < eventDescriptions.Length; ++i)
			{
				var eventDescription = eventDescriptions[i];
				eventDescription.getPath(out string path1);
				var key = (HashedString)path1;
				SoundDescription soundDescription = new()
				{
					path = path1
				};

				eventDescription.getMinMaxDistance(out float _, out float max);

				if (max == 0.0)
					max = 60f;

				soundDescription.falloffDistanceSq = max * max;
				var parameterUpdaterList = new List<OneShotSoundParameterUpdater>();
				eventDescription.getParameterDescriptionCount(out int count);
				var parameterArray = new SoundDescription.Parameter[count];

				for (var j = 0; j < count; ++j)
				{
					eventDescription.getParameterDescriptionByIndex(j, out PARAMETER_DESCRIPTION parameter);
					var name = (string)parameter.name;

					parameterArray[j] = new SoundDescription.Parameter()
					{
						name = new HashedString(name),
						id = parameter.id
					};

					if (KFMOD.parameterUpdaters.TryGetValue((HashedString)name, out OneShotSoundParameterUpdater parameterUpdater))
						parameterUpdaterList.Add(parameterUpdater);
				}

				soundDescription.parameters = parameterArray;
				soundDescription.oneShotParameterUpdaters = parameterUpdaterList.ToArray();

				KFMOD.soundDescriptions[key] = soundDescription;
			}
		}

		public void UnLoadContent()
		{
			if (bankPaths != null)
			{
				foreach (var bankPath in bankPaths)
				{
					RuntimeManager.StudioSystem.getBank(bankPath, out var bank);
					bank.unload();
				}
			}
		}
	}
}
