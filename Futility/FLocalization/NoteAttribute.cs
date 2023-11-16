using System;

namespace FUtility.FLocalization
{
	/// <summary>
	/// Used to add a note above a LocString, which will appear in POEdit as a comment.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class NoteAttribute : Attribute
	{
		public string message;

		public NoteAttribute(string message)
		{
			this.message = message;
		}
	}
}
