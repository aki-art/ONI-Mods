using Moonlet.Utils;

namespace Moonlet.Templates
{
	public class MaterialCategoryTemplate : BaseTemplate
	{
		[Doc("Storage filters to list this category under." +
			"<ul>" +
			"<li><b>NOT_EDIBLE_SOLIDS:</b> default storage, compactor, conveyors, etc.</li>" +
			"<li><b>LIQUIDS:</b> filterable liquids</li>" +
			"<li><b>FOOD:</b> food box / fridge, Edible, Medicine</li>" +
			"<li><b>DEHYDRATED:</b> dehydrated</li>" +
			"<li><b>GASES:</b> filterable gases (Breathable / Unbreathable)</li>" +
			"<li><b>PAYLOADS:</b> Railgun Payload</li>" +
			"<li><b>BAGGABLE_CREATURES:</b> creature droppoff point</li>" +
			"<li><b>SWIMMING_CREATURES:</b> fish release</li>" +
			"<li><b>STORAGE_LOCKERS_STANDARD:</b> Standard storage filter</li>" +
			"<li><b>SPECIAL_STORAGE:</b> special storage (eggs, sublimators, clothes)</li>" +
			"<li><b>SOLID_TRANSFER_ARM_CONVEYABLE:</b> # auto sweeper can see it</li>" +
			"</ul>")]
		public string[] AddTo { get; set; }
	}
}
