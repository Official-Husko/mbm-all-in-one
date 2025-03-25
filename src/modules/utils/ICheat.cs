using mbm_all_in_one.src.modules.utils;

namespace mbm_all_in_one.src.modules.utils
{
    public interface ICheat
    {
        string Name { get; }
        CheatType Type { get; }
        void Execute(int amount);
        Tab DisplayTab { get; }
    }
} 