using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using mbm_all_in_one.src.modules.cheats;
using mbm_all_in_one.src.modules.utils;

namespace mbm_all_in_one.src.Managers
{
    public class CheatManager
    {
        private readonly List<ICheat> _cheats = new List<ICheat>();
        private readonly Dictionary<string, ICheat> _cheatDict = new Dictionary<string, ICheat>();

        public void RegisterCheat(ICheat cheat)
        {
            _cheats.Add(cheat);
            if (!_cheatDict.ContainsKey(cheat.Name))
                _cheatDict.Add(cheat.Name, cheat);
        }

        public IEnumerable<ICheat> GetCheats()
        {
            return _cheats;
        }

        public void ExecuteCheat(string name, int amount)
        {
            if (_cheatDict.TryGetValue(name, out var cheat))
            {
                cheat.Execute(amount);
            }
        }

        public void RegisterAllCheats()
        {
            var cheatTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IRegisterableCheat).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in cheatTypes)
            {
                var cheat = (IRegisterableCheat)Activator.CreateInstance(type);
                cheat.Register(this);
            }
        }
    }
}
