namespace SketchPad.History
{
    public interface ICommand
    {
        void Execute();

        void Undo();
    }
}
