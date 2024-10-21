using HarmonyLib;

namespace FUtility
{
	public class FPriority
	{
		// i am sick of having to double check the values every time because "high" and "low" just make no sense to me (╯‵□′)╯︵┻━┻
		public const int
			First = Priority.First,
			VeryEarly = Priority.VeryHigh,
			Early = Priority.High,
			EarlierThanNormal = Priority.HigherThanNormal,
			Normal = Priority.Normal,
			LaterThanNormal = Priority.LowerThanNormal,
			Late = Priority.Low,
			VeryLate = Priority.VeryLow,
			Last = Priority.Last;
	}
}
