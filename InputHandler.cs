using HutongGames.PlayMaker;
using Modding;
using System;
using System.Linq;
using UnityEngine;

namespace QoLTeleportKit
{
    public class InputHandler
    {
        private readonly QoLTeleportKit _mod;
        private string _inputBuffer = "";
        private int _currentPage = 1;
        private bool _gameWasPaused = false;

        public InputHandler(QoLTeleportKit mod)
        {
            _mod = mod;
            ModHooks.HeroUpdateHook += CheckInput;
            On.HeroController.Pause += OnPause;
        }

        public bool IsRebindingKey { get; set; }
        public bool IsRebindingSaveKey { get; set; }
        public bool IsRebindingTeleportKey { get; set; }
        public bool ShowMenu { get; set; }
        public string InputBuffer => _inputBuffer;
        public int CurrentPage => _currentPage;

        private void CheckInput()
        {
            bool isGamePaused = GameManager.instance.IsGamePaused();

            if (isGamePaused && !_gameWasPaused)
            {
                ShowMenu = false;
                _inputBuffer = "";
                _mod.Log.Write("Menu closed due to game pause");
            }
            _gameWasPaused = isGamePaused;

            if (isGamePaused)
                return;

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(_mod.Settings.MenuHotkey))
            {
                IsRebindingKey = true;
                _mod.Log.Write("Started rebinding menu hotkey");
                return;
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(_mod.Settings.SaveTeleportKey))
            {
                IsRebindingSaveKey = true;
                _mod.Log.Write("Started rebinding save teleport key");
                return;
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(_mod.Settings.TeleportKey))
            {
                IsRebindingTeleportKey = true;
                _mod.Log.Write("Started rebinding teleport key");
                return;
            }

            if (IsRebindingKey)
            {
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(keyCode) && keyCode != KeyCode.None)
                        {
                            _mod.Settings.MenuHotkey = keyCode;
                            IsRebindingKey = false;
                            _mod.Log.Write($"Menu hotkey rebound to: {keyCode}");
                            return;
                        }
                    }
                }
                return;
            }

            if (IsRebindingSaveKey)
            {
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(keyCode) && keyCode != KeyCode.None &&
                            keyCode != KeyCode.LeftControl && keyCode != KeyCode.RightControl)
                        {
                            _mod.Settings.SaveTeleportKey = keyCode;
                            IsRebindingSaveKey = false;
                            _mod.Log.Write($"Save teleport key rebound to: {keyCode}");
                            return;
                        }
                    }
                }
                return;
            }

            if (IsRebindingTeleportKey)
            {
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(keyCode) && keyCode != KeyCode.None &&
                            keyCode != KeyCode.LeftControl && keyCode != KeyCode.RightControl)
                        {
                            _mod.Settings.TeleportKey = keyCode;
                            IsRebindingTeleportKey = false;
                            _mod.Log.Write($"Teleport key rebound to: {keyCode}");
                            return;
                        }
                    }
                }
                return;
            }

            if (Input.GetKeyDown(_mod.Settings.MenuHotkey))
            {
                ShowMenu = !ShowMenu;
                _inputBuffer = "";
                _currentPage = 1;
                _mod.Log.Write($"Menu {(ShowMenu ? "opened" : "closed")}");
            }

            if (ShowMenu)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    _currentPage--;
                    if (_currentPage < 1) _currentPage = 4;
                    _inputBuffer = "";
                    _mod.Log.Write($"Switched to page {_currentPage} (left)");
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _currentPage++;
                    if (_currentPage > 4) _currentPage = 1;
                    _inputBuffer = "";
                    _mod.Log.Write($"Switched to page {_currentPage} (right)");
                }

                for (int i = 0; i <= 9; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
                    {
                        _inputBuffer += i.ToString();
                        _mod.Log.Write($"Input buffer: {_inputBuffer}");
                    }
                }

                if (Input.GetKeyDown(KeyCode.Return) && _inputBuffer.Length > 0)
                {
                    if (int.TryParse(_inputBuffer, out int bossId))
                    {
                        var boss = _mod.Data.Bosses.FirstOrDefault(b => b.id == bossId);
                        if (boss != default && !string.IsNullOrEmpty(boss.scene))
                        {
                            _mod.Log.Write($"Attempting teleport to boss ID {bossId} ({boss.name})");
                            _mod.Teleport.StartTeleport(boss.position, boss.scene);
                        }
                        else
                        {
                            _mod.Log.Write($"Invalid boss ID: {bossId}");
                        }
                    }
                    else if (_inputBuffer == "12151920815165")
                    {
                        _mod.Log.Write("Attempting teleport to Dream_Room_Believer_Shrine");
                        _mod.Teleport.StartTeleport(new Vector3(56.486f, 40.388f, 0f), "Dream_Room_Believer_Shrine");
                    }
                    _inputBuffer = "";
                }

                if (Input.GetKeyDown(KeyCode.Backspace))
                    _inputBuffer = _inputBuffer.Length > 0 ? _inputBuffer.Substring(0, _inputBuffer.Length - 1) : "";
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ShowMenu = false;
                    _inputBuffer = "";
                    _mod.Log.Write("Menu closed via Escape");
                }
            }

            if (!ShowMenu && !_mod.Teleport.IsBusy)
            {
                if (Input.GetKeyDown(_mod.Settings.SaveTeleportKey))
                {
                    SetCustomTeleportPoint();
                }
                else if (Input.GetKeyDown(_mod.Settings.TeleportKey))
                {
                    TeleportToCustomPoint();
                }
            }
        }

        private void SetCustomTeleportPoint()
        {
            if (HeroController.instance == null) return;

            _mod.Data.CustomTeleportPosition = HeroController.instance.transform.position;
            _mod.Data.CustomTeleportScene = GameManager.instance.sceneName;
            _mod.Log.Write($"Custom teleport point set at {_mod.Data.CustomTeleportPosition} in {_mod.Data.CustomTeleportScene}");
        }

        private void TeleportToCustomPoint()
        {
            if (_mod.Data.CustomTeleportPosition == null || string.IsNullOrEmpty(_mod.Data.CustomTeleportScene))
            {
                _mod.Log.Write("Attempted teleport to custom point but none was set");
                return;
            }

            if (GameManager.instance.sceneName != _mod.Data.CustomTeleportScene)
            {
                _mod.Log.Write($"Custom teleport point is in another scene ({_mod.Data.CustomTeleportScene}), point cleared");
                _mod.Data.CustomTeleportPosition = null;
                _mod.Data.CustomTeleportScene = null;
                return;
            }

            _mod.Log.Write($"Teleporting to custom point at {_mod.Data.CustomTeleportPosition} in {_mod.Data.CustomTeleportScene}");
            _mod.Teleport.StartTeleport(_mod.Data.CustomTeleportPosition.Value, _mod.Data.CustomTeleportScene);
        }

        private void OnPause(On.HeroController.orig_Pause orig, HeroController self)
        {
            orig(self);
            ShowMenu = false;
            _inputBuffer = "";
            IsRebindingKey = false;
            IsRebindingSaveKey = false;
            IsRebindingTeleportKey = false;
            _mod.Log.Write("Game paused, resetting menu state");
        }
    }
}
