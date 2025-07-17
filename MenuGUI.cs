using UnityEngine;

namespace QoLTeleportKit
{
    public class MenuGUI
    {
        private readonly QoLTeleportKit _mod;
        private Texture2D _glowTexture;
        private Texture2D _textGlowTexture;
        private Texture2D _subtleGlowTexture;
        private float _menuAlpha = 0f;
        private const float FadeInSpeed = 8f;
        private const float FadeOutSpeed = 6f;
        private bool _wasMenuVisible = false;

        public MenuGUI(QoLTeleportKit mod)
        {
            _mod = mod;
            CreateGlowTexture();
            CreateTextGlowTexture();
            CreateSubtleGlowTexture();
            var wrapper = new GameObject("QoL_GUI_Wrapper");
            wrapper.AddComponent<GUIComponent>().Init(this);
            UnityEngine.Object.DontDestroyOnLoad(wrapper);
        }

        private void CreateGlowTexture()
        {
            _glowTexture = new Texture2D(1, 1);
            _glowTexture.SetPixel(0, 0, new Color(1, 1, 1, 0.15f));
            _glowTexture.Apply();
        }

        private void CreateTextGlowTexture()
        {
            _textGlowTexture = new Texture2D(1, 1);
            _textGlowTexture.SetPixel(0, 0, new Color(1, 1, 1, 0.3f));
            _textGlowTexture.Apply();
        }

        private void CreateSubtleGlowTexture()
        {
            _subtleGlowTexture = new Texture2D(1, 1);
            _subtleGlowTexture.SetPixel(0, 0, new Color(1, 1, 1, 0.05f));
            _subtleGlowTexture.Apply();
        }

        public void DrawMenu()
        {
            bool isMenuVisible = _mod.Input.ShowMenu && !GameManager.instance.IsGamePaused();

            if (isMenuVisible)
            {
                if (!_wasMenuVisible)
                {
                    _menuAlpha = Mathf.Max(0.1f, _menuAlpha);
                }
                _menuAlpha = Mathf.Min(1f, _menuAlpha + Time.unscaledDeltaTime * FadeInSpeed);
            }
            else
            {
                if (_wasMenuVisible)
                {
                    _menuAlpha = Mathf.Min(0.9f, _menuAlpha);
                }
                _menuAlpha = Mathf.Max(0f, _menuAlpha - Time.unscaledDeltaTime * FadeOutSpeed);
            }

            _wasMenuVisible = isMenuVisible;

            if (_menuAlpha <= 0f) return;

            var originalStyle = GUI.skin.label.fontStyle;
            var originalBoxStyle = GUI.skin.box.fontStyle;

            GUI.skin.label.fontStyle = FontStyle.Bold;
            GUI.skin.box.fontStyle = FontStyle.BoldAndItalic;

            float screenHeight = Screen.height;
            float menuHeight = 390;
            float startY = (screenHeight - menuHeight) / 2;

            GUI.color = new Color(1f, 1f, 1f, _menuAlpha);

            DrawGlow(new Rect(5, startY - 5, 610, menuHeight + 10), 5);

            GUI.Box(new Rect(10, startY, 600, menuHeight), $"[Page {_mod.Input.CurrentPage}] - {GetPageTitle()}");

            DrawMenuContent(startY);
            DrawKeyBindInfo(startY);
            DrawRebindingPrompts();

            GUI.skin.label.fontStyle = originalStyle;
            GUI.skin.box.fontStyle = originalBoxStyle;
            GUI.color = Color.white;
        }

        private void DrawGlow(Rect rect, int thickness)
        {
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, thickness), _glowTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - thickness, rect.width, thickness), _glowTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.y + thickness, thickness, rect.height - thickness * 2), _glowTexture);
            GUI.DrawTexture(new Rect(rect.x + rect.width - thickness, rect.y + thickness, thickness, rect.height - thickness * 2), _glowTexture);
        }

        private string GetPageTitle()
        {
            return _mod.Input.CurrentPage switch
            {
                1 => "Hall of Gods [1 Floor]",
                2 => "Hall of Gods [2 Floor]",
                3 => "Pantheon's",
                4 => "PoP Segments",
                _ => ""
            };
        }

        private void DrawMenuContent(float startY)
        {
            int yPos = (int)startY + 40;
            int startIndex = (_mod.Input.CurrentPage - 1) * 18;

            if (_mod.Input.CurrentPage == 3) startIndex = 36;
            if (_mod.Input.CurrentPage == 4) startIndex = 43;

            int endIndex = _mod.Input.CurrentPage == 3 ? 43 :
                          _mod.Input.CurrentPage == 4 ? 56 :
                          startIndex + 18;

            for (int i = startIndex; i < endIndex && i < _mod.Data.Bosses.Length; i++)
            {
                int column = (i - startIndex) / 9;
                int row = (i - startIndex) % 9;
                var boss = _mod.Data.Bosses[i];

                if (!string.IsNullOrEmpty(boss.scene))
                {
                    Rect textRect = new Rect(20 + column * 280, yPos + row * 22, 260, 20);
                    GUI.Label(textRect, $"{boss.id} - {boss.name}");
                }
            }
        }

        private void DrawKeyBindInfo(float startY)
        {
            Rect inputRect = new Rect(20, startY + 270, 560, 20);
            GUI.Label(inputRect, $"Input (1–{_mod.Data.Bosses.Length}): {_mod.Input.InputBuffer}");

            Rect navRect = new Rect(20, startY + 290, 560, 20);
            GUI.Label(navRect, "Q - Previous Page | E - Next Page | Enter - Confirm | Esc - Close");

            Rect hotkeyRect = new Rect(20, startY + 310, 560, 20);
            GUI.Label(hotkeyRect, $"Current Hotkey: {_mod.Settings.MenuHotkey} (Press Ctrl+{_mod.Settings.MenuHotkey} to rebind)");

            Rect saveRect = new Rect(20, startY + 330, 560, 20);
            GUI.Label(saveRect, $"Save Teleport Key: {_mod.Settings.SaveTeleportKey} (Press Ctrl+{_mod.Settings.SaveTeleportKey} to rebind)");

            Rect teleportRect = new Rect(20, startY + 350, 560, 20);
            GUI.Label(teleportRect, $"Teleport Key: {_mod.Settings.TeleportKey} (Press Ctrl+{_mod.Settings.TeleportKey} to rebind)");

            if (_mod.Data.CustomTeleportPosition != null)
            {
                Rect customRect = new Rect(20, startY + 370, 560, 20);
                GUI.Label(customRect, $"Custom TP: Set in {_mod.Data.CustomTeleportScene}");
            }
        }

        private void DrawRebindingPrompts()
        {
            if (_mod.Input.IsRebindingKey)
            {
                DrawCenteredPrompt("Press any key to rebind menu hotkey... (Esc to cancel)");
            }
            else if (_mod.Input.IsRebindingSaveKey)
            {
                DrawCenteredPrompt("Press any key to rebind save teleport key... (Esc to cancel)");
            }
            else if (_mod.Input.IsRebindingTeleportKey)
            {
                DrawCenteredPrompt("Press any key to rebind teleport key... (Esc to cancel)");
            }
        }

        private void DrawCenteredPrompt(string message)
        {
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 25, 300, 50), "");
            Rect textRect = new Rect(Screen.width / 2 - 140, Screen.height / 2 - 10, 280, 20);
            GUI.Label(textRect, message);
        }

        private class GUIComponent : MonoBehaviour
        {
            private MenuGUI _menu;

            public GUIComponent Init(MenuGUI menu)
            {
                _menu = menu;
                return this;
            }

            private void OnGUI()
            {
                _menu?.DrawMenu();
            }
        }
    }
}
