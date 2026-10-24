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

        internal static void Register(Action<string, string, string> addText, IEnumerable<string> languages) =>
            LocalizationGroup.Register(addText, languages, new[] { Resume, OlderBackup, Failed },
                new Dictionary<string, string[]>
                {
                    ["en-US"] = new[]
                    {
                        "Resume the saved area checkpoint?\nArea: {0}\nSaved: {1:g}\nYes: restore this checkpoint, including run progress and your profile at that point.\nNo: continue with the game's normal save. Progress within the area will be lost.",
                        "The newest checkpoint is damaged. This is the previous intact checkpoint.",
                        "The checkpoint could not be restored. Your checkpoint files have been kept. See the Mod log for details.",
                    },
                    ["zh-CN"] = new[]
                    {
                        "恢复区域检查点？\n区域：{0}\n保存时间：{1:g}\n是：恢复当时的个人存档和本局进度。\n否：使用游戏的普通存档。当前区域内的进度会丢失。",
                        "最新检查点已损坏，将使用上一个完整检查点。",
                        "无法恢复检查点。检查点文件已保留，请查看模组日志。",
                    },
                    ["zh-TW"] = new[]
                    {
                        "復原區域檢查點？\n區域：{0}\n儲存時間：{1:g}\n是：復原當時的個人存檔與本局進度。\n否：使用遊戲的一般存檔。目前區域內的進度會遺失。",
                        "最新檢查點已損壞，將使用上一個完整檢查點。",
                        "無法復原檢查點。檢查點檔案已保留，請查看模組記錄。",
                    },
                    ["ko-KR"] = new[]
                    {
                        "지역 체크포인트를 복구할까요?\n지역: {0}\n저장: {1:g}\n예: 당시의 개인 저장과 탐험 진행을 복구합니다.\n아니요: 일반 저장을 불러옵니다. 지역 내 진행은 사라집니다.",
                        "최신 체크포인트가 손상되어 이전의 정상 체크포인트를 사용합니다.",
                        "체크포인트를 복구하지 못했습니다. 파일은 보존되었습니다. 모드 로그를 확인하세요.",
                    },
                    ["ja-JP"] = new[]
                    {
                        "エリアのチェックポイントを復元しますか？\nエリア：{0}\n保存：{1:g}\nはい：当時の個人セーブと探索の進行を復元します。\nいいえ：通常のセーブを使用します。エリア内の進行は失われます。",
                        "最新のチェックポイントが破損しているため、前の正常なものを使用します。",
                        "復元できませんでした。チェックポイントは保持されています。Modログを確認してください。",
                    },
                    ["de-DE"] = new[]
                    {
                        "Gebiets-Kontrollpunkt laden?\nGebiet: {0}\nGespeichert: {1:g}\nJa: Profil und Lauf auf diesen Stand zurücksetzen.\nNein: normalen Spielstand verwenden. Fortschritt innerhalb des Gebiets geht verloren.",
                        "Der neueste Kontrollpunkt ist beschädigt. Der vorherige intakte wird verwendet.",
                        "Wiederherstellung fehlgeschlagen. Die Kontrollpunkte bleiben erhalten. Siehe Mod-Protokoll.",
                    },
                    ["es-ES"] = new[]
                    {
                        "¿Restaurar el punto de control?\nZona: {0}\nGuardado: {1:g}\nSí: restaurar el perfil y la partida de ese momento.\nNo: usar el guardado normal. Se pierde el progreso dentro de la zona.",
                        "El punto más reciente está dañado. Se usará el anterior intacto.",
                        "No se pudo restaurar. Los archivos se conservan. Consulta el registro del mod.",
                    },
                    ["fr-FR"] = new[]
                    {
                        "Reprendre au point de contrôle ?\nZone : {0}\nSauvegarde : {1:g}\nOui : restaurer le profil et la partie à cet instant.\nNon : utiliser la sauvegarde normale. La progression dans la zone sera perdue.",
                        "Le dernier point est endommagé. Le précédent point intact sera utilisé.",
                        "Restauration impossible. Les fichiers sont conservés. Consultez le journal du mod.",
                    },
                    ["it-IT"] = new[]
                    {
                        "Ripristinare il checkpoint?\nArea: {0}\nSalvato: {1:g}\nSì: ripristina profilo e partita a quel momento.\nNo: usa il salvataggio normale. I progressi dentro questa area andranno persi.",
                        "Il checkpoint più recente è danneggiato. Verrà usato il precedente integro.",
                        "Ripristino non riuscito. I file sono conservati. Consulta il registro della mod.",
                    },
                    ["pl-PL"] = new[]
                    {
                        "Przywrócić punkt kontrolny?\nObszar: {0}\nZapis: {1:g}\nTak: przywróć profil i wyprawę z tej chwili.\nNie: użyj zwykłego zapisu. Postęp wewnątrz obszaru zostanie utracony.",
                        "Najnowszy punkt jest uszkodzony. Zostanie użyty poprzedni poprawny punkt.",
                        "Nie udało się przywrócić punktu. Pliki zachowano. Sprawdź dziennik moda.",
                    },
                    ["pt-BR"] = new[]
                    {
                        "Restaurar o ponto de controle?\nÁrea: {0}\nSalvo: {1:g}\nSim: restaurar o perfil e a partida daquele momento.\nNão: usar o salvamento normal. O progresso dentro da área será perdido.",
                        "O ponto mais recente está danificado. Será usado o anterior intacto.",
                        "Não foi possível restaurar. Os arquivos foram preservados. Consulte o registro do mod.",
                    },
                    ["ru-RU"] = new[]
                    {
                        "Восстановить контрольную точку?\nОбласть: {0}\nСохранено: {1:g}\nДа: вернуть профиль и забег к этому моменту.\nНет: использовать обычное сохранение. Прогресс внутри области будет потерян.",
                        "Последняя точка повреждена. Будет использована предыдущая исправная точка.",
                        "Не удалось восстановить точку. Файлы сохранены. Проверьте журнал мода.",
                    },
                    ["sv-SE"] = new[]
                    {
                        "Återställ kontrollpunkten?\nOmråde: {0}\nSparad: {1:g}\nJa: återställ profil och spelomgång till denna tidpunkt.\nNej: använd den vanliga sparfilen. Framsteg inom området går förlorade.",
                        "Den senaste kontrollpunkten är skadad. Den föregående hela används.",
                        "Kunde inte återställa. Filerna har behållits. Se moddens logg.",
                    },
                    ["th-TH"] = new[]
                    {
                        "กู้คืนจุดบันทึกหรือไม่?\nพื้นที่: {0}\nบันทึกเมื่อ: {1:g}\nใช่: กู้คืนโปรไฟล์และการเล่น ณ เวลานั้น\nไม่: ใช้ไฟล์บันทึกปกติ ความคืบหน้าภายในพื้นที่จะหายไป",
                        "จุดบันทึกล่าสุดเสียหาย จะใช้จุดบันทึกก่อนหน้าที่สมบูรณ์",
                        "กู้คืนไม่สำเร็จ ไฟล์ยังคงอยู่ โปรดดูบันทึกของม็อด",
                    },
                    ["tr-TR"] = new[]
                    {
                        "Kontrol noktası geri yüklensin mi?\nBölge: {0}\nKayıt: {1:g}\nEvet: o andaki profili ve koşuyu geri yükle.\nHayır: normal kaydı kullan. Bölge içindeki ilerleme kaybolur.",
                        "En yeni kontrol noktası bozuk. Önceki sağlam nokta kullanılacak.",
                        "Geri yüklenemedi. Dosyalar korundu. Mod günlüğünü kontrol edin.",
                    }
                });
    }
}
