namespace PrintingPodRecharge
{
    internal class STRINGS
    {
        public class MISC
        {
            public class TAGS
            {
                public static LocString PPR_BIOINK = "Bio-Ink";
            }
        }

        public static class ITEMS
        {
            public static class BIO_INK
            {
                public static LocString NAME = "Bio-Ink";
                public static LocString DESC = "Collected unused printing material of a Printing pod. Allows immediate recharging of a Printing Pod.";
            }

            public static class FOOD_BIO_INK
            {
                public static LocString NAME = "Nutritious Bio-Ink";
                public static LocString DESC = "Collected unused printing material of a Printing pod. Makes new printables mostly edible items.";
            }

            public static class OOZING_BIO_INK
            {
                public static LocString NAME = "Oozing Bio-Ink";
                public static LocString DESC = "Allows immediate recharging of a Printing Pod, and biases new printables towards Duplicants.";
            }

            public static class METALLIC_BIO_INK
            {
                public static LocString NAME = "Metallic Bio-Ink";
                public static LocString DESC = "Allows immediate recharging of a Printing Pod, and biases new printables towards metals and metal ores.";
            }

            public static class VACILLATING_BIO_INK
            {
                public static LocString NAME = "Vacillating Bio-Ink";
                public static LocString DESC = "Allows immediate recharging of a Printing Pod, and biases new printables to include Duplicants over resources. These duplicants are likely to have much higher stats than normal, and can have Vacillator exclusive starting traits.";
            }

            public static class GERMINATED_BIO_INK
            {
                public static LocString NAME = "Eggy Bio-Ink";
                public static LocString DESC = "Allows immediate recharging of a Printing Pod, and causes the new selection to include primarily eggs and creatures.";
            }
            public static class SEEDED_BIO_INK
            {
                public static LocString NAME = "Seeded Bio-Ink";
                public static LocString DESC = "Allows immediate recharging of a Printing Pod, and biases new printables to be mostly seeds of plants.";
            }
        }

        public static class UI
        {
            public static class SANBOXTOOLS
            {
                public static class FILTERS
                {
                    public static LocString BIO_INKS = "Bio-Inks";
                }
            }

            public static class BIOINKSIDESCREEN
            {
                public static class CONTENTS
                {
                    public static class BUTTONS
                    {
                        public static class DELIVER
                        {
                            public static LocString TEXT = "Deliver Bio-Ink";
                        }

                        public static class ACTIVATE
                        {
                            public static LocString TEXT = "Print";
                        }
                    }
                }
            }
        }
    }
}
