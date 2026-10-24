using System;
using System.Collections.Generic;
using SephiriaEnhancements.Configuration;

namespace SephiriaEnhancements.MultiplayerAccess
{
    internal static class RecoveredLobbyLocalization
    {
        internal const string Ready = "SephiriaEnhancements.MultiplayerAccess.RecoveryReady";
        internal const string Failed = "SephiriaEnhancements.MultiplayerAccess.RecoveryFailed";

        internal static void Register(Action<string, string, string> addText) =>
            LocalizationGroup.Register(addText, LocalizationLanguages.All, new[] { Ready, Failed },
                new Dictionary<string, string[]>
                {
                    ["en-US"] = new[] { "A friends-only Steam room for two players is ready. Your friend can use Join Game in Steam.", "The saved run loaded, but its Steam room could not be created. Rejoining is unavailable." },
                    ["zh-CN"] = new[] { "仅限好友的 Steam 双人房间已就绪。好友可在 Steam 中选择加入游戏。", "存档已加载，但无法创建 Steam 房间。暂时无法重新加入。" },
                    ["zh-TW"] = new[] { "僅限好友的 Steam 雙人房間已就緒。好友可在 Steam 中選擇加入遊戲。", "存檔已載入，但無法建立 Steam 房間。暫時無法重新加入。" },
                    ["ko-KR"] = new[] { "친구 전용 Steam 2인 방이 준비되었습니다. 친구가 Steam에서 게임 참가를 선택할 수 있습니다.", "저장된 탐험을 불러왔지만 Steam 방을 만들지 못했습니다. 재참가할 수 없습니다." },
                    ["ja-JP"] = new[] { "フレンド限定のSteam二人部屋を作成しました。フレンドはSteamからゲームに参加できます。", "セーブは読み込みましたが、Steamルームを作成できませんでした。再参加はできません。" },
                    ["de-DE"] = new[] { "Ein Steam-Raum nur für Freunde mit zwei Plätzen ist bereit. Dein Freund kann über Steam beitreten.", "Der Spielstand wurde geladen, aber der Steam-Raum konnte nicht erstellt werden. Erneuter Beitritt ist nicht möglich." },
                    ["es-ES"] = new[] { "Sala de Steam solo para amigos con dos plazas lista. Tu amigo puede unirse desde Steam.", "Partida cargada, pero no se pudo crear la sala de Steam. No se puede volver a entrar." },
                    ["fr-FR"] = new[] { "Un salon Steam pour deux joueurs réservé aux amis est prêt. Votre ami peut rejoindre via Steam.", "Sauvegarde chargée, mais création du salon Steam impossible. Impossible de rejoindre à nouveau." },
                    ["it-IT"] = new[] { "Stanza Steam per due giocatori riservata agli amici pronta. Il tuo amico può unirsi da Steam.", "Partita caricata, ma impossibile creare la stanza Steam. Non è possibile rientrare." },
                    ["pl-PL"] = new[] { "Pokój Steam tylko dla znajomych na dwie osoby jest gotowy. Znajomy może dołączyć przez Steam.", "Zapis wczytany, ale nie udało się utworzyć pokoju Steam. Ponowne dołączenie jest niedostępne." },
                    ["pt-BR"] = new[] { "Sala Steam para dois jogadores, apenas para amigos, pronta. Seu amigo pode entrar pelo Steam.", "Partida carregada, mas não foi possível criar a sala Steam. Não é possível entrar novamente." },
                    ["ru-RU"] = new[] { "Комната Steam только для друзей на двух игроков готова. Друг может присоединиться через Steam.", "Сохранение загружено, но создать комнату Steam не удалось. Повторное подключение недоступно." },
                    ["sv-SE"] = new[] { "Ett Steam-rum för två spelare, endast för vänner, är klart. Din vän kan ansluta via Steam.", "Sparfilen laddades, men Steam-rummet kunde inte skapas. Återanslutning är inte tillgänglig." },
                    ["th-TH"] = new[] { "ห้อง Steam สำหรับเพื่อนเท่านั้นจำนวนสองคนพร้อมแล้ว เพื่อนเข้าร่วมเกมผ่าน Steam ได้", "โหลดเกมที่บันทึกแล้วแต่สร้างห้อง Steam ไม่สำเร็จ ยังไม่สามารถเข้าร่วมอีกครั้งได้" },
                    ["tr-TR"] = new[] { "Yalnızca arkadaşlara açık iki kişilik Steam odası hazır. Arkadaşın Steam üzerinden katılabilir.", "Kayıt yüklendi ancak Steam odası oluşturulamadı. Yeniden katılma kullanılamıyor." },
                });
    }
}
