namespace Moonlet.Entities
{
	public class PoiData : EntityData
	{
		public bool Pickupable { get; set; }

		public ObjectLayer[] Layers { get; set; }
	}
}
