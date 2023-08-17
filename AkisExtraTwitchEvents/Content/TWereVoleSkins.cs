using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content
{
	public class TWereVoleSkins
	{
		public static WereVoleSkin DEFAULT_PURPLE = new("964a77", "cb3399", "a09798");

		public static WereVoleSkin GetForPersonality(string personality)
		{
			return skinsByDupePersonality.TryGetValue(personality, out WereVoleSkin skin) ? skin : DEFAULT_PURPLE;
		}

		public static Dictionary<string, WereVoleSkin> skinsByDupePersonality = new()
		{
			// Base Game
			{ "CATALINA", new WereVoleSkin("f9eb6b", "ffffff", "fffad1") },
			{ "NISBET", new WereVoleSkin("b43324", "ffffff", "ff0000") },
			{ "ELLIE",  new("f9eb6b", "ffffff", "fffad1") },
			{ "RUBY", new WereVoleSkin("ac1200", "ffffff", "412c2e") },
			{ "LEIRA", new WereVoleSkin("9c725b", "f2b521", "ffe286") },
			{ "BUBBLES", new WereVoleSkin("454652", "00ccff", "2973a2") },
			{ "MIMA", new WereVoleSkin("d6d6d6", "f2f2f2", "9a9a9a") },
			{ "NAILS", new WereVoleSkin("6c5b5b", "22f1b9", "ba63f2") },
			{ "MAE", new WereVoleSkin("fafafa", "fff7f3", "d4bc5c") },
			{ "GOSSMANN", new WereVoleSkin("ffeed6", "ffffff", "fff0b5") },
			{ "MARIE",  new("f9eb6b", "ffffff", "fffad1") },
			{ "LINDSAY", new WereVoleSkin("3d3d3d", "513a31", "ffd735") },
			{ "DEVON", new WereVoleSkin("67544d", "5adc54", "a94037") },
			{ "REN", new WereVoleSkin("2f2f2f", "ff0000", "615e5e") },
			{ "FRANKIE", new WereVoleSkin("907b69", "f6b06f", "d4b9a0") },
			{ "BANHI", new WereVoleSkin("00eebe", "ffffff", "353535") },
			{ "ADA", new WereVoleSkin("ee004e", "ffa0bf", "353535") },
			{ "HASSAN", new WereVoleSkin("9c826c", "42e9eb", "42e9eb") },
			{ "STINKY", new WereVoleSkin("e5d399", "ff893a", "ff893a") },
			{ "JOSHUA", new WereVoleSkin("3d96e9", "ffca3a", "295680") },
			{ "LIAM", new WereVoleSkin("00a2ff", "00ffe4", "00e7be") },
			{ "ABE", new WereVoleSkin("d0d0d0", "96e5ff", "00aee7") },
			{ "BURT", new WereVoleSkin("54444d", "e4162f", "a09798") },
			{ "TRAVALDO", new WereVoleSkin("54444d", "ffd800", "d9b395") },
			{ "HAROLD", new WereVoleSkin("54444d", "15ffaf", "ac8c73") },
			{ "MAX", new WereVoleSkin("72666d", "cbff15", "68c2ec") },
			{ "ROWAN", new WereVoleSkin("4ee9b4", "cbff15", "ef9c4b") },
			{ "OTTO", new WereVoleSkin("4ee9b4", "90ff00", "efe94b") },
			{ "TURNER", new WereVoleSkin("987e69", "6975ea", "adaa9e") },
			{ "NIKOLA", new WereVoleSkin("fff77b", "ffffff", "ffffff") },
			{ "MEEP", new WereVoleSkin("c2b182", "ac606c", "86797b") },
			{ "ARI", new WereVoleSkin("ecd17b", "df5b6f", "555555") },
			{ "JEAN", new WereVoleSkin("60c5a7", "9afee0", "e9e9e9") },
			{ "CAMILLE", new WereVoleSkin("b43324", "ffffff", "ff0000") },
			{ "ASHKAN", new WereVoleSkin("46947f", "e9a949", "36333b") },
			{ "STEVE", new WereVoleSkin("346aba", "e9a949", "36333b") },
			{ "AMARI", new WereVoleSkin("bdaa1b", "e9a949", "36333b") },
			{ "PEI", new WereVoleSkin("6ecad2", "fe8686", "0099cc") },
			{ "QUINN", new WereVoleSkin("c0533e", "f7c243", "bb6931") },
			{ "JORGE", new WereVoleSkin("a98766", "f79884", "a4936a") },

			// AETE
			{ TPersonalities.HULK, new WereVoleSkin("4ccc52", "aa47be", "438345") },

			// Beached
			{ "BEACHEDMINNOW", new WereVoleSkin("f6eef1", "70e8ff", "f5accb") },
			{ "BEACHEDVAHANO", new WereVoleSkin("1c439e", "4fe3ff", "45136b") },

			// Print Cassidy
			{ "CASSIDY", new WereVoleSkin("ffcbd1", "ffaab4", "ff98a4") },

			// Print TJ
			{ "TJ", new WereVoleSkin("9b69bc", "e7ca37", "985534") },

			// Print Blug
			{ "BLUG", new WereVoleSkin("67bb45", "e7ca37", "a59e9c") },

			// NHSH
			{ "ZUNDAMON", new WereVoleSkin("2eb884", "e7f245", "8be85c") },
			{ "AKANE", new WereVoleSkin("bf323a", "ffe6aa", "ffd464") },
			{ "REIMU", new WereVoleSkin("df2630", "ffffff", "ffffff") },
			{ "PATCHOULI", new WereVoleSkin("a482c8", "e72e2b", "0f66d9") },
			{ "MARISA", new WereVoleSkin("342e30", "e8df43", "ffffff") },
			{ "PEKORA", new WereVoleSkin("c6d3ff", "ff9530", "ffffff") },
			{ "OLDSNAKE", new WereVoleSkin("a2aa9c", "e0d5c6", "575757") },
			{ "2B", new WereVoleSkin("e7ead9", "ffffff", "292929") },

			// So Many Dupes
			{ "ICHIGO", new WereVoleSkin("aa565e", "ffffff", "292929") },
			{ "FERRIS", new WereVoleSkin("ca6c55", "81fcb7", "eec153") },
			{ "MYERS", new WereVoleSkin("97857e", "ded1bd", "463a38") },
			{ "KOKOROKO", new WereVoleSkin("e2bad0", "ff65bc", "e99dc8") },
			{ "TILLY", new WereVoleSkin("39352e", "ff65bc", "e99dc8") },
			{ "LUCY", new WereVoleSkin("ffd4c3", "ffc954", "e96f62") },
			{ "PEANUT", new WereVoleSkin("b4ccab", "ffc954", "a59a9c") },
			{ "SULKA", new WereVoleSkin("464437", "d7d7d7", "6f6d64") },
			{ "TOKI", new WereVoleSkin("ffbd7b", "ff957c", "ffa089") },
			{ "CHERI", new WereVoleSkin("dd8951", "ff723b", "dd8951") },
			{ "GRIB", new WereVoleSkin("8a9074", "ff723b", "5db788") },
			{ "AINE", new WereVoleSkin("eda898", "ff723b", "e37f67") },
			{ "MERCY", new WereVoleSkin("5b5d57", "e86e54", "e1dece") },
			{ "DAIKON", new WereVoleSkin("2d2a23", "3691ff", "5c4936") },
			{ "JAY", new WereVoleSkin("47433b", "f3c044", "c65441") },
			{ "ANNIE", new WereVoleSkin("564835", "f3c044", "e7e7e7") },
			{ "PAUL", new WereVoleSkin("b482cc", "f3c044", "e7e7e7") },
			{ "JULIE", new WereVoleSkin("f9b9a0", "ffe400", "e37676") },
			{ "RUDY", new WereVoleSkin("bca8be", "9cc8fe", "c4c4c4") },
			{ "OLENA", new WereVoleSkin("dbbd90", "fff693", "b29062") },
			{ "LLOYD", new WereVoleSkin("a479aa", "fff693", "e0b65f") },
			{ "KAXON", new WereVoleSkin("4c3d2a", "ffa939", "d89d56") },
			{ "SHEPHARD", new WereVoleSkin("7b6941", "e9664c", "d89d56") },
		};

		public class WereVoleSkin
		{
			public Color skin;
			public Color accent;
			public Color drill;
			public string headAccessoryAnimFile;

			public WereVoleSkin(Color skin, Color accent, Color drill, string headAccessoryAnimFile = null)
			{
				this.skin = skin;
				this.accent = accent;
				this.drill = drill;
				this.headAccessoryAnimFile = headAccessoryAnimFile;
			}
			public WereVoleSkin(string skinHex, string accentHex, string drillHex, string headAccessoryAnimFile = null)
			{
				skin = Util.ColorFromHex(skinHex);
				accent = Util.ColorFromHex(accentHex);
				drill = Util.ColorFromHex(drillHex);
				this.headAccessoryAnimFile = headAccessoryAnimFile;
			}
		}
	}
}
