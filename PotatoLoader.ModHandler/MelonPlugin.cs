using System;
#pragma warning disable 0108

namespace PotatoLoader
{
    public abstract class MelonPlugin : MelonBase
    {
        [Obsolete()]
        public MelonPluginInfoAttribute InfoAttribute { get => LegacyPluginInfo; }
        [Obsolete()]
        public MelonPluginGameAttribute[] GameAttributes { get => LegacyPluginGames; }

        public virtual void OnPreInitialization() { }
    }
}