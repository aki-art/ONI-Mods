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
                public static LocString DESC = "Collected unused printing material of a Printing pod. Makes new printables only edible items.";
            }

            public static class SHAKER_BIO_INK
            {
                public static LocString NAME = "Suspicious Bio-Ink";
                public static LocString DESC = "Collected unused printing material of a Printing pod, mixed with a questionable selection of other ingredients. Allows immediate recharging of a Printing Pod, and causes new printables be exclusively Duplicants. \n\n" +
                    "<i>But something appears not quite right...</i>";
            }

            public static class METALLIC_BIO_INK
            {
                public static LocString NAME = "Metallic Bio-Ink";
                public static LocString DESC = "Collected unused printing material of a Printing pod. Allows immediate recharging of a Printing Pod, and causes new printables to be metals and metal ores.";
            }

            public static class VACILLATING_BIO_INK
            {
                public static LocString NAME = "Vacillating Bio-Ink";
                public static LocString DESC = "Collected unused printing material of a Printing pod. Allows immediate recharging of a Printing Pod, and causes new printables be exclusively Duplicants. " +
                    "These duplicants are likely to have much higher stats than normal, and can have Vacillator exclusive starting traits.";
            }

            public static class GERMINATED_BIO_INK
            {
                public static LocString NAME = "Eggy Bio-Ink";
                public static LocString DESC = "Collected unused printing material of a Printing pod. Allows immediate recharging of a Printing Pod, and causes the new selection to only include eggs and baby creatures.";
            }

            public static class SEEDED_BIO_INK
            {
                public static LocString NAME = "Seeded Bio-Ink";
                public static LocString DESC = "Collected unused printing material of a Printing pod. Allows immediate recharging of a Printing Pod, and causes the new printables to be seeds of plants.";
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

            public static class SETTINGSDIALOG
            {
                public static class TITLE
                {
                    public static LocString TITLETEXT = "Bio-Inks Settings";
                }

                public static class BUTTONS
                {
                    public static class STEAMBUTTON
                    {
                        public static LocString TEXT = "Steam Workshop";
                    }

                    public static class CANCELBUTTON
                    {
                        public static LocString TEXT = "Cancel";
                    }

                    public static class GITHUBBUTTON
                    {
                        public static LocString TEXT = "Github";
                    }

                    public static class OK
                    {
                        public static LocString TEXT = "Apply";
                    }
                }

                public static class CONTENT
                {
                    public static class SLIDERPANEL
                    {
                        public static class SLIDER
                        {
                            public static LocString LABEL = "Random Dupe chance";
                            public static LocString RANGELABEL = "{0}";
                            public static LocString TOOLTIP = "% chance of randomized dupes showing up in prints, not using the Suspicious Ink.";
                        }
                    }

                    public static class REFUND
                    {
                        public static LocString LABEL = "Refund Quantity:";
                        public static LocString QUANTITY = "{0}";
                        public static LocString TOOLTIP = "How much ink to refund for a rejected print, in kgs.";
                    }

                    public static class REFUNDACTIVETOGGLE
                    {
                        public static LocString LABEL = "Refund Active Ink";
                        public static LocString TOOLTIP = "If enabled, when rejecting a print the refunded ink will be the same type as the print.";
                    }
                    public static class DEBUGMODETOGGLE
                    {
                        public static LocString LABEL = "Debug mode (for testing)";
                        public static LocString TOOLTIP = "If enabled, an additional panel will show with debug tools. Do not use for gameplay.";
                    }
                }
            }
        }
    }
}
