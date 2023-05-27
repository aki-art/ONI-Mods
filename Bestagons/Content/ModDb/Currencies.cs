namespace Bestagons.Content.ModDb
{
    public class Currencies : ResourceSet<Currency>
    {
        public static Currency rock;
        public static Currency metalBar;
        public static Currency food;

        public Currencies()
        {
            rock = Add(new Currency("Rock", "bestagons_rock", 1));
            metalBar = Add(new Currency("MetalBar", "bestagons_metalBar", 1));
            food = Add(new Currency("Food", "bestagons_food", 1));
        }
    }
}
