namespace PrintingPodRecharge
{
    public class STRINGS
    {
        public static class TWITCH
        {
            public static class PRINTING_POD_LEAK
            {
                public static LocString NAME = "Leaky Printing Pods!";
            }

            public static class USELESS_PRINTS
            {
                public static LocString NAME = "Useless Care Package";
            }
        }

        public class MISC
        {
            public class TAGS
            {
                public static LocString PPR_BIOINK = "Bio-Ink";
            }

            public static LocString NAME_PREFIXES = "Ma,Jo,Mi,Ell,Ban,Stin,Cata,Nis,Ru,Le,Bubb,Na,Ma,Goss,Lind,De,Re,Fran,A,Li,Bu,Tra,Ha,Ro,O,Tur,Ni,Mee,Je,Cam,Ash,Ste,Ama,Pe,Qui";

            public static LocString NAME_SUFFIXES = "inn,i,ri,ve,kan,mille,an,eep,la,ner,to,wan,x,rold,valdo,e,am,shua,ky,ssan,hi,kie,n,von,say,rie,mann,ils,-Ma,les,ra,by,lie,sbet,lina";
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

            public static class SHAKER_BIO_INK
            {
                public static LocString NAME = "Bio-Ink Shaker";
                public static LocString DESC = "Allows immediate recharging of a Printing Pod, and causes new printables be exclusively Duplicants. " +
                    "The Duplicants will be highly unusual, with strangely mixed traits, looks, names and attributes.\n" +
                    "\n" +
                    "Can be fed to a live Duplicant to reshuffle their attributes and traits. Their attribute and trait count will remain the same.";
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
                public static LocString DESC = "Allows immediate recharging of a Printing Pod, and causes new printables be exclusively Duplicants. " +
                    "These duplicants are likely to have much higher stats than normal, and can have Vacillator exclusive starting traits.";
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

            public static class DUPE_TIERS
            {
                public static LocString BAD_2 = "Terrible";
                public static LocString BAD_1 = "Meh";
                public static LocString OKAY = "Okay";
                public static LocString GOOD_1 = "Great";
                public static LocString GOOD_2 = "Super-Duper-Dupe";
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
