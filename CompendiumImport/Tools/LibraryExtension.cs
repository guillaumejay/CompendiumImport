using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Masterplan.Data;

namespace CompendiumImport.Tools
{
    internal static class LibraryExtension
    {
        public static Creature FindCreatureCaseInsensitive(this Library lib,string creature_name, int level)
        {
            foreach (Creature creature in lib.Creatures)
            {
                if (((creature != null) && (creature.Name.ToLower() == creature_name.ToLower())) && (creature.Level == level))
                {
                    return creature;
                }
            }
            return null;
        }

        public static Trap FindTrapCaseInsensitive(this Library lib, string creature_name, int level)
        {
            foreach (Trap t in lib.Traps)
            {
                if (((t != null) && (t.Name.ToLower() == creature_name.ToLower())) && (t.Level == level))
                {
                    return t;
                }
            }
            return null;
        }
    }
}
