using KSerialization;

namespace GravitasBigStorage.Content
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class FakeResearchCompleteMessage : Message
	{
		[Serialize]
		private string techName;

		public FakeResearchCompleteMessage()
		{
		}

		public FakeResearchCompleteMessage(string techName)
		{
			this.techName = techName;
		}

		public override string GetSound() => "AI_Notification_ResearchComplete";

		public override string GetMessageBody()
		{
			return string.Format(global::STRINGS.MISC.NOTIFICATIONS.RESEARCHCOMPLETE.MESSAGEBODY, techName, techName);
		}

		public override string GetTitle() => global::STRINGS.MISC.NOTIFICATIONS.RESEARCHCOMPLETE.NAME;

		public override string GetTooltip()
		{
			return string.Format(global::STRINGS.MISC.NOTIFICATIONS.RESEARCHCOMPLETE.TOOLTIP, techName);
		}

		public override bool IsValid() => !techName.IsNullOrWhiteSpace();
	}
}
