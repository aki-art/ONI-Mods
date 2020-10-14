namespace Curtain
{
    public static class NavigatorExtension
    {
        public static bool TryGetNextTransition(this Navigator navigator, out NavGrid.Transition transition)
        {
            transition = default;

            // (╯°□°）╯︵ ┻━┻
            if (navigator == null ||
                navigator.path.nodes == null ||
                navigator.NavGrid == null ||
                navigator.NavGrid.transitions == null ||
                navigator.path.nodes.Count < 2)
                return false;

            int nextid = navigator.path.nodes[1].transitionId;

            if (nextid >= navigator.NavGrid.transitions.Length)
                return false;

            transition = navigator.GetNextTransition();
            return true;
        }
    }
}