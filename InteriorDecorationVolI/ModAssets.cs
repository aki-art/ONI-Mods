using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteriorDecorationv1
{
    // Various assets I need accessible
    public static class ModAssets
    {
        public const string PREFIX = "ID1_";

        // Compatibility for MaterialColors
        // Buildings with this tag will be ignored
        public static Tag NoPaintTag = TagManager.Create("NoPaint");
    }
}
