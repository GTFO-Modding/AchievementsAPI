using Player;
using SNetwork;
using System;
namespace AchievementsAPI.Utilities
{
    public class PlayerRestrictions
    {
        public PlayerGroupRestrictions? LocalPlayer { get; set; }
        public PlayerGroupRestrictions? Players { get; set; }
        public PlayerGroupRestrictions? Bots { get; set; }

        public bool IsValid(bool isMe, bool isBot = false)
        {
            if (isMe)
            {
                return this.LocalPlayer?.Include ?? true;
            }
            else if (isBot)
            {
                return this.Bots?.Include ?? true;
            }
            else
            {
                return this.Players?.Include ?? true;
            }
        }

        public bool CheckForOnePlayer(Func<PlayerAgent, bool> playerVerifier)
        {
            var lobby = SNet.Lobby;
            for (int index = 0; index < lobby.Players.Count; index++)
            {
                var player = lobby.Players[index];

                var playerAgent = player.PlayerAgent?.TryCast<PlayerAgent>();
                if (playerAgent == null || !playerVerifier(playerAgent))
                {
                    continue;
                }

                return this.IsValid(playerAgent);
            }
            return false;
        }

        public bool IsValid(SNet_Player player)
        {
            return this.IsValid(player.IsLocal, player.IsBot);
        }
        public bool IsValid(PlayerAgent player)
        {
            return this.IsValid(player.Owner);
        }
    }
}
