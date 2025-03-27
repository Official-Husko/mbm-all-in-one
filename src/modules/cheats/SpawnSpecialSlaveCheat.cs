using System;
using System.Linq;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;
using UnityEngine;

namespace mbm_all_in_one.src.modules.cheats
{
    public class SpawnSpecialSlaveCheat : ICheat, IRegisterableCheat
    {
        public string Name => "Spawn Special Slave";
        public CheatType Type => CheatType.Execute;
        public Tab DisplayTab => Tab.NPCs;

        private readonly EUnitType[] _spawnableTypes;
        private int _currentTypeIndex = 0;

        public SpawnSpecialSlaveCheat()
        {
            // Fetch all unit types except 'None'
            _spawnableTypes = Enum.GetValues(typeof(EUnitType))
                                  .Cast<EUnitType>()
                                  .Where(type => type != EUnitType.None)
                                  .ToArray();
        }

        public void Execute(int amount)
        {
            var selectedType = _spawnableTypes[_currentTypeIndex];
            GameManager.Instance.PlayerData.NewNewCharacterAtKeepingRoom(selectedType);
            GameManager.Instance.OpenWindow(EGameWindow.KeepingRoom);
            GameManager.Instance.AddSystemMessage($"Spawned {selectedType}");
        }

        public void Register(CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }

        public void IncrementTypeIndex()
        {
            _currentTypeIndex = (_currentTypeIndex + 1) % _spawnableTypes.Length;
        }

        public void DecrementTypeIndex()
        {
            _currentTypeIndex = (_currentTypeIndex - 1 + _spawnableTypes.Length) % _spawnableTypes.Length;
        }

        public string GetCurrentTypeName()
        {
            return _spawnableTypes[_currentTypeIndex].ToString();
        }

        public EUnitType[] GetSpawnableTypes()
        {
            return _spawnableTypes;
        }

        public void SetCurrentTypeIndex(int index)
        {
            _currentTypeIndex = index;
        }
    }
} 