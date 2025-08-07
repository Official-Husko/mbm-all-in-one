using mbm_all_in_one.src.Managers;

namespace mbm_all_in_one.src.modules.utils
{
    public interface IRegisterableCheat
    {
        void Register(mbm_all_in_one.src.Managers.CheatManager cheatManager);
    }
}