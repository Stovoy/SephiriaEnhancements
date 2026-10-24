using System;
using System.Collections.Generic;

namespace SephiriaEnhancements.Configuration
{
    internal static class FeatureActivationLocalization
    {
        internal const string Help = "SephiriaEnhancements.OptIn.Help";
        private static readonly Dictionary<string, string> Text = new()
        {
            ["en-US"] = "Restart the game after changing this switch. Crash recovery is on by default; other optional features are off. Individual settings still apply.",
            ["zh-CN"] = "更改此开关后请重启游戏。崩溃恢复默认开启，其他可选功能默认关闭。各项设置仍然生效。",
            ["zh-TW"] = "更改此開關後請重新啟動遊戲。當機復原預設開啟，其他選用功能預設關閉。各項設定仍然適用。",
            ["ja-JP"] = "変更後はゲームを再起動してください。クラッシュ復元は初期状態でオン、他の任意機能はオフです。個別設定も適用されます。",
            ["ko-KR"] = "변경 후 게임을 다시 시작하세요. 충돌 복구는 기본적으로 켜지고 다른 선택 기능은 꺼집니다. 개별 설정도 적용됩니다.",
            ["de-DE"] = "Nach Änderungen das Spiel neu starten. Absturzwiederherstellung ist standardmäßig an, andere optionale Funktionen sind aus. Einzelne Einstellungen gelten weiterhin.",
            ["es-ES"] = "Reinicia el juego tras cambiar esta opción. La recuperación tras fallos está activada por defecto; las demás funciones opcionales están desactivadas. Se aplican los ajustes individuales.",
            ["fr-FR"] = "Redémarrez le jeu après modification. La récupération après plantage est activée par défaut ; les autres fonctions facultatives sont désactivées. Les réglages individuels restent applicables.",
            ["it-IT"] = "Riavvia il gioco dopo la modifica. Il recupero dopo un arresto è attivo per impostazione predefinita; le altre funzioni opzionali sono disattivate. Restano validi i singoli parametri.",
            ["pl-PL"] = "Po zmianie uruchom grę ponownie. Odzyskiwanie po awarii jest domyślnie włączone; inne opcjonalne funkcje są wyłączone. Nadal obowiązują osobne ustawienia.",
            ["pt-BR"] = "Reinicie o jogo após alterar esta opção. A recuperação após falhas está ativada por padrão; os outros recursos opcionais estão desativados. As configurações individuais continuam válidas.",
            ["ru-RU"] = "После изменения перезапустите игру. Восстановление после сбоя включено по умолчанию; остальные необязательные функции выключены. Отдельные настройки продолжают действовать.",
            ["sv-SE"] = "Starta om spelet efter ändringar. Återställning efter krasch är på som standard; andra valfria funktioner är av. Enskilda inställningar gäller fortfarande.",
            ["th-TH"] = "โปรดเริ่มเกมใหม่หลังเปลี่ยนตัวเลือกนี้ การกู้คืนหลังเกมขัดข้องเปิดตามค่าเริ่มต้น ส่วนคุณสมบัติเสริมอื่นปิด การตั้งค่าแต่ละรายการยังมีผล",
            ["tr-TR"] = "Değiştirdikten sonra oyunu yeniden başlatın. Çökme kurtarma varsayılan olarak açık, diğer isteğe bağlı özellikler kapalıdır. Ayrı ayarlar geçerliliğini korur.",
        };

        internal static void Register(Action<string, string, string> addText)
        {
            foreach (string language in LocalizationLanguages.All)
                addText(language, Help, Text[language]);
        }
    }
}
