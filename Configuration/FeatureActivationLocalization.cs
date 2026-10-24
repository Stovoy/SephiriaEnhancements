using System;
using System.Collections.Generic;

namespace SephiriaEnhancements.Configuration
{
    internal static class FeatureActivationLocalization
    {
        internal const string Help = "SephiriaEnhancements.OptIn.Help";
        private static readonly Dictionary<string, string> Text = new()
        {
            ["en-US"] = "Off by default. Restart the game after changing this switch. Individual settings still apply. Only joining and reconnecting start enabled.",
            ["zh-CN"] = "默认关闭。更改此开关后请重启游戏。各项设置仍然生效。默认仅启用加入与重连。",
            ["zh-TW"] = "預設關閉。更改此開關後請重新啟動遊戲。各項設定仍然適用。預設僅啟用加入與重新連線。",
            ["ja-JP"] = "初期設定はオフです。変更後はゲームを再起動してください。個別設定も適用されます。参加と再接続のみ初期状態で有効です。",
            ["ko-KR"] = "기본적으로 꺼져 있습니다. 변경 후 게임을 다시 시작하세요. 개별 설정도 적용됩니다. 참가와 재접속만 기본적으로 켜집니다.",
            ["de-DE"] = "Standardmäßig aus. Nach Änderungen das Spiel neu starten. Einzelne Einstellungen gelten weiterhin. Nur Beitritt und Wiederverbindung sind anfangs aktiv.",
            ["es-ES"] = "Desactivado por defecto. Reinicia el juego tras cambiar esta opción. Se aplican los ajustes individuales. Solo unirse y reconectarse están activos inicialmente.",
            ["fr-FR"] = "Désactivé par défaut. Redémarrez le jeu après modification. Les réglages individuels restent applicables. Seules la participation et la reconnexion sont actives au départ.",
            ["it-IT"] = "Disattivato per impostazione predefinita. Riavvia il gioco dopo la modifica. Restano validi i singoli parametri. Solo ingresso e riconnessione sono attivi inizialmente.",
            ["pl-PL"] = "Domyślnie wyłączone. Po zmianie uruchom grę ponownie. Nadal obowiązują osobne ustawienia. Domyślnie aktywne są tylko dołączanie i ponowne łączenie.",
            ["pt-BR"] = "Desativado por padrão. Reinicie o jogo após alterar esta opção. As configurações individuais continuam válidas. Apenas entrada e reconexão começam ativas.",
            ["ru-RU"] = "По умолчанию выключено. После изменения перезапустите игру. Отдельные настройки продолжают действовать. Изначально включены только вход и переподключение.",
            ["sv-SE"] = "Av som standard. Starta om spelet efter ändringar. Enskilda inställningar gäller fortfarande. Endast anslutning och återanslutning är aktiva från början.",
            ["th-TH"] = "ปิดตามค่าเริ่มต้น โปรดเริ่มเกมใหม่หลังเปลี่ยนตัวเลือกนี้ การตั้งค่าแต่ละรายการยังมีผล เปิดเฉพาะการเข้าร่วมและการเชื่อมต่อใหม่ตามค่าเริ่มต้น",
            ["tr-TR"] = "Varsayılan olarak kapalıdır. Değiştirdikten sonra oyunu yeniden başlatın. Ayrı ayarlar geçerliliğini korur. Başlangıçta yalnızca katılma ve yeniden bağlanma açıktır."
        };

        internal static void Register(Action<string, string, string> addText)
        {
            foreach (string language in LocalizationLanguages.All)
                addText(language, Help, Text[language]);
        }
    }
}
