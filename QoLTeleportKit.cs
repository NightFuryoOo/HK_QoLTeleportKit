using Modding;
using UnityEngine;

namespace QoLTeleportKit
{
    public class QoLTeleportKit : Mod, IGlobalSettings<GlobalSettings>
    {
        public static QoLTeleportKit Instance { get; private set; }
        public TeleportData Data { get; private set; }
        public InputHandler Input { get; private set; }
        public TeleportManager Teleport { get; private set; }
        public MenuGUI GUI { get; private set; }
        public QoLLogger Log { get; private set; } // Изменено с Logger на QoLLogger

        public GlobalSettings Settings { get; set; } = new GlobalSettings();

        public override string GetVersion() => "1.0.0.0";

        public override void Initialize()
        {
            Instance = this;
            Log = new QoLLogger(); // Изменено с Logger на QoLLogger
            Data = new TeleportData();
            Input = new InputHandler(this);
            Teleport = new TeleportManager(this);
            GUI = new MenuGUI(this);

            Log.Write("Mod initialized");
        }

        public void OnLoadGlobal(GlobalSettings s) => Settings = s;
        public GlobalSettings OnSaveGlobal() => Settings;
    }

    public class GlobalSettings
    {
        public KeyCode MenuHotkey = KeyCode.F6;
        public KeyCode SaveTeleportKey = KeyCode.R;
        public KeyCode TeleportKey = KeyCode.T;
    }
}
