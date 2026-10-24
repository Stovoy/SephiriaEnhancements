using System;
using System.Collections.Generic;
using SephiriaEnhancements.Configuration;

namespace SephiriaEnhancements.CrashRecovery
{
    internal static class CrashRecoveryLocalization
    {
        internal const string Resume = "SephiriaEnhancements.CrashRecovery.Resume";
        internal const string OlderBackup = "SephiriaEnhancements.CrashRecovery.OlderBackup";
        internal const string Failed = "SephiriaEnhancements.CrashRecovery.Failed";
        internal const string Restored = "SephiriaEnhancements.CrashRecovery.Restored";
        internal const string Invite = "SephiriaEnhancements.CrashRecovery.Invite";
        internal const string LobbyFailed = "SephiriaEnhancements.CrashRecovery.LobbyFailed";

        internal static void Register(Action<string, string, string> addText, IEnumerable<string> languages) =>
            LocalizationGroup.Register(addText, languages, new[] { Resume, OlderBackup, Failed, Restored, Invite, LobbyFailed },
                new Dictionary<string, string[]>
                {
                    ["en-US"] = new[]
                    {
                        "Resume the saved area checkpoint?\nArea: {0}\nSaved: {1:g}\nYes: restore this checkpoint, including run progress and your profile at that point.\nNo: continue with the game's normal save. Progress within the area will be lost.",
                        "The newest checkpoint is damaged. This is the previous intact checkpoint.",
                        "The checkpoint could not be restored. Your checkpoint files have been kept. See the Mod log for details.",
                        "Area checkpoint restored. Progress within the area was rolled back.",
                        "Area checkpoint restored. A new private Steam room is ready. Send fresh Steam invitations to your friends; their saved run slots are retained.",
                        "Area checkpoint restored, but the Steam room could not be recreated. See the Mod log before continuing multiplayer."
                    },
                    ["zh-CN"] = new[]
                    {
                        "恢复区域检查点？\n区域：{0}\n保存时间：{1:g}\n是：恢复当时的个人存档和本局进度。\n否：使用游戏的普通存档。当前区域内的进度会丢失。",
                        "最新检查点已损坏，将使用上一个完整检查点。",
                        "无法恢复检查点。检查点文件已保留，请查看模组日志。",
                        "区域检查点已恢复，区域内的进度已回退。",
                        "检查点已恢复，新的 Steam 私人房间已建立。请重新邀请好友，原有玩家存档槽已保留。",
                        "检查点已恢复，但无法重建 Steam 房间。继续联机前请查看模组日志。"
                    },
                    ["zh-TW"] = new[]
                    {
                        "復原區域檢查點？\n區域：{0}\n儲存時間：{1:g}\n是：復原當時的個人存檔與本局進度。\n否：使用遊戲的一般存檔。目前區域內的進度會遺失。",
                        "最新檢查點已損壞，將使用上一個完整檢查點。",
                        "無法復原檢查點。檢查點檔案已保留，請查看模組記錄。",
                        "區域檢查點已復原，區域內的進度已回退。",
                        "檢查點已復原，新的 Steam 私人房間已建立。請重新邀請好友，原有玩家存檔欄位已保留。",
                        "檢查點已復原，但無法重建 Steam 房間。繼續連線前請查看模組記錄。"
                    },
                    ["ko-KR"] = new[]
                    {
                        "지역 체크포인트를 복구할까요?\n지역: {0}\n저장: {1:g}\n예: 당시의 개인 저장과 탐험 진행을 복구합니다.\n아니요: 일반 저장을 불러옵니다. 지역 내 진행은 사라집니다.",
                        "최신 체크포인트가 손상되어 이전의 정상 체크포인트를 사용합니다.",
                        "체크포인트를 복구하지 못했습니다. 파일은 보존되었습니다. 모드 로그를 확인하세요.",
                        "지역 체크포인트를 복구했습니다. 지역 내 진행은 되돌아갔습니다.",
                        "복구 후 새 Steam 비공개 방을 만들었습니다. 친구에게 새 초대를 보내세요. 기존 플레이어 저장 슬롯은 유지됩니다.",
                        "체크포인트는 복구했지만 Steam 방을 만들지 못했습니다. 멀티플레이를 계속하기 전에 모드 로그를 확인하세요."
                    },
                    ["ja-JP"] = new[]
                    {
                        "エリアのチェックポイントを復元しますか？\nエリア：{0}\n保存：{1:g}\nはい：当時の個人セーブと探索の進行を復元します。\nいいえ：通常のセーブを使用します。エリア内の進行は失われます。",
                        "最新のチェックポイントが破損しているため、前の正常なものを使用します。",
                        "復元できませんでした。チェックポイントは保持されています。Modログを確認してください。",
                        "チェックポイントを復元しました。エリア内の進行は巻き戻りました。",
                        "復元後、新しいSteamプライベートルームを作成しました。フレンドに新しく招待を送ってください。保存済みプレイヤー枠は保持されています。",
                        "チェックポイントは復元しましたが、Steamルームを作成できませんでした。マルチプレイを続ける前にModログを確認してください。"
                    },
                    ["de-DE"] = new[]
                    {
                        "Gebiets-Kontrollpunkt laden?\nGebiet: {0}\nGespeichert: {1:g}\nJa: Profil und Lauf auf diesen Stand zurücksetzen.\nNein: normalen Spielstand verwenden. Fortschritt innerhalb des Gebiets geht verloren.",
                        "Der neueste Kontrollpunkt ist beschädigt. Der vorherige intakte wird verwendet.",
                        "Wiederherstellung fehlgeschlagen. Die Kontrollpunkte bleiben erhalten. Siehe Mod-Protokoll.",
                        "Kontrollpunkt geladen. Fortschritt innerhalb des Gebiets wurde zurückgesetzt.",
                        "Kontrollpunkt geladen und neuer privater Steam-Raum erstellt. Freunde erneut einladen; gespeicherte Spielerplätze bleiben erhalten.",
                        "Kontrollpunkt geladen, Steam-Raum konnte nicht erstellt werden. Vor weiterem Mehrspielerspiel das Mod-Protokoll prüfen."
                    },
                    ["es-ES"] = new[]
                    {
                        "¿Restaurar el punto de control?\nZona: {0}\nGuardado: {1:g}\nSí: restaurar el perfil y la partida de ese momento.\nNo: usar el guardado normal. Se pierde el progreso dentro de la zona.",
                        "El punto más reciente está dañado. Se usará el anterior intacto.",
                        "No se pudo restaurar. Los archivos se conservan. Consulta el registro del mod.",
                        "Punto restaurado. Se ha revertido el progreso dentro de la zona.",
                        "Punto restaurado y nueva sala privada de Steam creada. Envía nuevas invitaciones; se conservan las ranuras de los jugadores.",
                        "Punto restaurado, pero no se pudo crear la sala de Steam. Consulta el registro antes de seguir en multijugador."
                    },
                    ["fr-FR"] = new[]
                    {
                        "Reprendre au point de contrôle ?\nZone : {0}\nSauvegarde : {1:g}\nOui : restaurer le profil et la partie à cet instant.\nNon : utiliser la sauvegarde normale. La progression dans la zone sera perdue.",
                        "Le dernier point est endommagé. Le précédent point intact sera utilisé.",
                        "Restauration impossible. Les fichiers sont conservés. Consultez le journal du mod.",
                        "Point restauré. La progression dans la zone a été annulée.",
                        "Point restauré et nouveau salon Steam privé créé. Envoyez de nouvelles invitations ; les emplacements des joueurs sont conservés.",
                        "Point restauré, mais création du salon Steam impossible. Consultez le journal avant de poursuivre en multijoueur."
                    },
                    ["it-IT"] = new[]
                    {
                        "Ripristinare il checkpoint?\nArea: {0}\nSalvato: {1:g}\nSì: ripristina profilo e partita a quel momento.\nNo: usa il salvataggio normale. I progressi dentro questa area andranno persi.",
                        "Il checkpoint più recente è danneggiato. Verrà usato il precedente integro.",
                        "Ripristino non riuscito. I file sono conservati. Consulta il registro della mod.",
                        "Checkpoint ripristinato. I progressi dentro questa area sono stati annullati.",
                        "Checkpoint ripristinato e nuova stanza Steam privata creata. Invia nuovi inviti; gli slot salvati dei giocatori sono conservati.",
                        "Checkpoint ripristinato, ma impossibile creare la stanza Steam. Controlla il registro prima di continuare in multigiocatore."
                    },
                    ["pl-PL"] = new[]
                    {
                        "Przywrócić punkt kontrolny?\nObszar: {0}\nZapis: {1:g}\nTak: przywróć profil i wyprawę z tej chwili.\nNie: użyj zwykłego zapisu. Postęp wewnątrz obszaru zostanie utracony.",
                        "Najnowszy punkt jest uszkodzony. Zostanie użyty poprzedni poprawny punkt.",
                        "Nie udało się przywrócić punktu. Pliki zachowano. Sprawdź dziennik moda.",
                        "Punkt przywrócony. Postęp wewnątrz obszaru został cofnięty.",
                        "Punkt przywrócony i utworzono nowy prywatny pokój Steam. Wyślij nowe zaproszenia; zapisane miejsca graczy zachowano.",
                        "Punkt przywrócony, lecz nie udało się utworzyć pokoju Steam. Sprawdź dziennik przed dalszą grą wieloosobową."
                    },
                    ["pt-BR"] = new[]
                    {
                        "Restaurar o ponto de controle?\nÁrea: {0}\nSalvo: {1:g}\nSim: restaurar o perfil e a partida daquele momento.\nNão: usar o salvamento normal. O progresso dentro da área será perdido.",
                        "O ponto mais recente está danificado. Será usado o anterior intacto.",
                        "Não foi possível restaurar. Os arquivos foram preservados. Consulte o registro do mod.",
                        "Ponto restaurado. O progresso dentro da área foi desfeito.",
                        "Ponto restaurado e nova sala privada do Steam criada. Envie novos convites; os espaços salvos dos jogadores foram preservados.",
                        "Ponto restaurado, mas não foi possível criar a sala Steam. Consulte o registro antes de continuar no multijogador."
                    },
                    ["ru-RU"] = new[]
                    {
                        "Восстановить контрольную точку?\nОбласть: {0}\nСохранено: {1:g}\nДа: вернуть профиль и забег к этому моменту.\nНет: использовать обычное сохранение. Прогресс внутри области будет потерян.",
                        "Последняя точка повреждена. Будет использована предыдущая исправная точка.",
                        "Не удалось восстановить точку. Файлы сохранены. Проверьте журнал мода.",
                        "Точка восстановлена. Прогресс внутри области отменён.",
                        "Точка восстановлена, создана новая закрытая комната Steam. Отправьте новые приглашения; сохранённые слоты игроков оставлены.",
                        "Точка восстановлена, но создать комнату Steam не удалось. Перед продолжением совместной игры проверьте журнал мода."
                    },
                    ["sv-SE"] = new[]
                    {
                        "Återställ kontrollpunkten?\nOmråde: {0}\nSparad: {1:g}\nJa: återställ profil och spelomgång till denna tidpunkt.\nNej: använd den vanliga sparfilen. Framsteg inom området går förlorade.",
                        "Den senaste kontrollpunkten är skadad. Den föregående hela används.",
                        "Kunde inte återställa. Filerna har behållits. Se moddens logg.",
                        "Kontrollpunkten återställd. Framsteg inom området har återställts.",
                        "Kontrollpunkten återställd och ett nytt privat Steam-rum skapat. Skicka nya inbjudningar; spelarnas sparade platser finns kvar.",
                        "Kontrollpunkten återställd, men Steam-rummet kunde inte skapas. Läs loggen innan ni fortsätter i flerspelarläge."
                    },
                    ["th-TH"] = new[]
                    {
                        "กู้คืนจุดบันทึกหรือไม่?\nพื้นที่: {0}\nบันทึกเมื่อ: {1:g}\nใช่: กู้คืนโปรไฟล์และการเล่น ณ เวลานั้น\nไม่: ใช้ไฟล์บันทึกปกติ ความคืบหน้าภายในพื้นที่จะหายไป",
                        "จุดบันทึกล่าสุดเสียหาย จะใช้จุดบันทึกก่อนหน้าที่สมบูรณ์",
                        "กู้คืนไม่สำเร็จ ไฟล์ยังคงอยู่ โปรดดูบันทึกของม็อด",
                        "กู้คืนจุดบันทึกแล้ว ความคืบหน้าภายในพื้นที่ถูกย้อนกลับ",
                        "กู้คืนแล้วและสร้างห้องส่วนตัว Steam ใหม่ โปรดส่งคำเชิญใหม่ให้เพื่อน ช่องบันทึกผู้เล่นเดิมยังคงอยู่",
                        "กู้คืนแล้วแต่สร้างห้อง Steam ไม่สำเร็จ โปรดดูบันทึกก่อนเล่นหลายคนต่อ"
                    },
                    ["tr-TR"] = new[]
                    {
                        "Kontrol noktası geri yüklensin mi?\nBölge: {0}\nKayıt: {1:g}\nEvet: o andaki profili ve koşuyu geri yükle.\nHayır: normal kaydı kullan. Bölge içindeki ilerleme kaybolur.",
                        "En yeni kontrol noktası bozuk. Önceki sağlam nokta kullanılacak.",
                        "Geri yüklenemedi. Dosyalar korundu. Mod günlüğünü kontrol edin.",
                        "Kontrol noktası yüklendi. Bölge içindeki ilerleme geri alındı.",
                        "Kontrol noktası yüklendi ve yeni özel Steam odası oluşturuldu. Yeni davetler gönderin; oyuncuların kayıt yuvaları korunur.",
                        "Kontrol noktası yüklendi ancak Steam odası oluşturulamadı. Çok oyunculuya devam etmeden önce günlüğü kontrol edin."
                    }
                });
    }
}
