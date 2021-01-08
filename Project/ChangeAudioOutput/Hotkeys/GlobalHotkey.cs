using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace ChangeAudioOutput.Hotkeys
{
    public class GlobalHotkey
    {
        public ModifierKeys Modifier;
        public Key Key;
        public Action Callback;

        public GlobalHotkey(ModifierKeys modifier, Key key, Action callbackMethod)
        {
            this.Modifier = modifier;
            this.Key = key;
            this.Callback = callbackMethod;
        }
    }
}
