using Twitchery.Content;
using Twitchery.Content.Defs;

namespace Twitchery
{
    public class STRINGS
    {
        public static class DUPLICANTS
        {
            public static class MODIFIERS
            {
                public static class AKISEXTRATWITCHEVENTS_CAFFEINATEDEFFECT
                {
                    public static LocString NAME = "Caffeinated";
                    public static LocString TOOLTIP = "This dupe had a much deserved coffe break.";
                    public static LocString ADDITIONAL_EFFECTS = "+50% Work Speed";
                }

                public static class AKISEXTRATWITCHEVENTS_RADISHSTRENGTHEFFECT
                {
                    public static LocString NAME = "Rad Strength";
                    public static LocString TOOLTIP = "This Duplicant had some great radish recently.";
                }
            }
        }

        public static class MISC
        {
            public static class STATUSITEMS
            {
                public static class AKISEXTRATWITCHEVENTS_CALORIESTATUS
                {
                    public static LocString NAME = "{0}";
                    public static LocString TOOLTIP = "This is {0} of raw, delightful radish.";
                }
            }

            public static class AKISEXTRATWITCHEVENTS_PIZZABOX
            {
                public static LocString NAME = FUtility.Utils.FormatAsLink("Pile of Pizzaboxes", PizzaBoxConfig.ID);
                public static LocString DESC = "Full of delicious pizza.";
            }

            public static class AKISEXTRATWITCHEVENTS_GIANTRADISH
            {
                public static LocString NAME = FUtility.Utils.FormatAsLink("Radish of the Gods", GiantRadishConfig.ID);
                public static LocString DESC = "The singular radish.";
            }
        }

        public static class ITEMS
        {
            public static class FOOD
            {
                public static class AKISEXTRATWITCHEVENTS_RAWRADISH
                {
                    public static LocString NAME = FUtility.Utils.FormatAsLink("Chunk O' Radish", RawRadishConfig.ID);
                    public static LocString DESC = "An edible chunk of the godly Radish.";
                }

                public static class AKISEXTRATWITCHEVENTS_COOKEDRADISH
                {
                    public static LocString NAME = FUtility.Utils.FormatAsLink("Rad Dish", CookedRadishConfig.ID);
                    public static LocString DESC = "Delicious grilled radish with a radiantly green crust. It's <i>probably</i> safe.";
                }

                public static class AKISEXTRATWITCHEVENTS_PIZZA
                {
                    public static LocString NAME = FUtility.Utils.FormatAsLink("Pizza", PizzaConfig.ID);
                    public static LocString DESC = "A wonderful, filling dish.";
                }

                // JELLO is added at ELEMENTS
            }
        }

        public static class ELEMENTS
        {
            public static class RICE
            {
                public static LocString NAME = "Rice";
                public static LocString DESC = "Tasty.";
            }

            public static class TEA
            {
                public static LocString NAME = "Tea";
                public static LocString DESC = "Tasty.";
            }

            public static class FROZENTEA
            {
                public static LocString NAME = "Ice-Tea";
                public static LocString DESC = "It is cold tea.";
            }

            public static class SOAP
            {
                public static LocString NAME = FUtility.Utils.FormatAsLink("Soap", "Soap");
                public static LocString DESC = "";
            }

            public static class LIQUIDSOAP
            {
                public static LocString NAME = "Liquid Soap";
                public static LocString DESC = "";
            }

            public static class JELLO
            {
                public static LocString NAME = FUtility.Utils.FormatAsLink("Jello", Elements.Jello.ToString());
                public static LocString DESC = "A jiggly, edible liquid that behaves like Visco Gel. It's kiwi flavored.\n\n" +
                    "Contains 1840KCal per kilogram. (Must be mopped first, Duplicants will not eat a liquid directly from the floor, they do have <i>some</i> standards.";
            }

            public static class FROZENJELLO
            {
                public static LocString NAME = FUtility.Utils.FormatAsLink("Solid Jello", Elements.Jello.ToString());
                public static LocString DESC = "Solidified Jello. It's too cold and hardened to eat.";
            }
        }
    }
}
