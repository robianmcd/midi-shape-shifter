namespace MidiShapeShifter.Mss.Mapping
{
    public interface IMappingManager : IGraphableMappingManager<IMappingEntry>
    {
        bool MoveEntryDown(int id);
        bool MoveEntryUp(int id);
    }
}
