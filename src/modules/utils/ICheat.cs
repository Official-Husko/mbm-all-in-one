namespace mbm_all_in_one.src.modules.utils
{
    public interface ICheat
    {
        string Name { get; }
        CheatType Type { get; }
        void Execute(int amount);
    }
} 