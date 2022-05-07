namespace TrueTiles
{
    public static class ElementGrid
    {
        public static int[] elementIdx;

        public static void Add(int cell, int elementIndex)
        {
            elementIdx[cell] = elementIndex;
        }

        public static void Add(int cell, SimHashes element)
        {
            elementIdx[cell] = SimMessages.GetElementIndex(element);
        }

        public static void Remove(int cell)
        {
            elementIdx[cell] = -1;
        }

        public static void Initialize()
        {
            elementIdx = new int[Grid.CellCount];

            for(int i = 0; i < elementIdx.Length; i++)
            {
                elementIdx[i] = -1;
            }
        }
    }
}
