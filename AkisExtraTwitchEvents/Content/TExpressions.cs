using Database;

namespace Twitchery.Content
{
	public class TExpressions
	{
		public static Expression hulk;

		public static void Register(Expressions expressions)
		{
			var faces = Db.Get().Faces;
			hulk = new Expression("AkisExtraTwitchEvents_Expression_Hulk", expressions, faces.Determined)
			{
				priority = 999
			};
		}
	}
}
