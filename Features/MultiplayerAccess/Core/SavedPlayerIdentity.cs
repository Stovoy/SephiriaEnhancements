using System;
using System.Collections.Generic;

namespace SephiriaEnhancements.MultiplayerAccess
{
    internal readonly struct SavedPlayerIdentity
    {
        internal string Guid { get; }
        internal ulong SteamId { get; }

        internal SavedPlayerIdentity(string guid, ulong steamId)
        {
            Guid = guid;
            SteamId = steamId;
        }

        internal static int Resolve(IReadOnlyList<SavedPlayerIdentity> players,
            string guid, ulong steamId)
        {
            int steamSlot = -1;
            int guidSlot = -1;
            for (int slot = 0; slot < players.Count; slot++)
            {
                var saved = players[slot];
                if (steamId != 0 && saved.SteamId == steamId)
                {
                    if (steamSlot != -1) return -2;
                    steamSlot = slot;
                }
                if (!string.IsNullOrWhiteSpace(guid) &&
                    string.Equals(saved.Guid, guid, StringComparison.Ordinal))
                {
                    if (guidSlot != -1) return -2;
                    guidSlot = slot;
                }
            }

            if (steamSlot >= 0)
                return guidSlot >= 0 && guidSlot != steamSlot ? -2 : steamSlot;
            if (guidSlot >= 0 && steamId != 0 && players[guidSlot].SteamId != 0)
                return -2;
            return guidSlot;
        }
    }
}
