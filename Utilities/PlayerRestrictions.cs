using Player;
using SNetwork;
using System;
namespace AchievementsAPI.Utilities
{
    /// <summary>
    /// Representation of restrictions for players.
    /// </summary>
    public class PlayerRestrictions
    {
        /// <summary>
        /// Restrictions for the local player.
        /// </summary>
        public PlayerGroupRestrictions? LocalPlayer { get; set; }
        /// <summary>
        /// Restrictions for the non-local players.
        /// </summary>
        public PlayerGroupRestrictions? Players { get; set; }
        /// <summary>
        /// Restrictions for bots.
        /// </summary>
        public PlayerGroupRestrictions? Bots { get; set; }

        /// <summary>
        /// Returns whether or not this restriction is valid.
        /// </summary>
        /// <param name="isMe">Checking for local player</param>
        /// <param name="isBot">Checking for bot.</param>
        /// <returns><see langword="true"/> if valid, otherwise
        /// <see langword="false"/>.</returns>
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

        /// <summary>
        /// Attempts to check for a player that is valid for this restriction
        /// and passes the verifier.
        /// </summary>
        /// <param name="playerVerifier">The verifier.</param>
        /// <returns><see langword="true"/> if one player is valid
        /// for this restriction and got past the verifier, otherwise
        /// <see langword="false"/>.</returns>
        public bool CheckForOnePlayer(Func<PlayerAgent, bool> playerVerifier)
        {
            SNet_Lobby? lobby = SNet.Lobby;
            for (int index = 0; index < lobby.Players.Count; index++)
            {
                SNet_Player? player = lobby.Players[index];

                PlayerAgent? playerAgent = player.PlayerAgent?.TryCast<PlayerAgent>();
                if (playerAgent == null || !playerVerifier(playerAgent))
                {
                    continue;
                }

                return this.IsValid(playerAgent);
            }
            return false;
        }

        /// <summary>
        /// Returns whether or not the given player is valid for these restrictions.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <returns><see langword="true"/> if the player is valid, otherwise
        /// <see langword="false"/>.</returns>
        public bool IsValid(SNet_Player player)
        {
            return this.IsValid(player.IsLocal, player.IsBot);
        }
        /// <summary>
        /// Returns whether or not the given player is valid for these restrictions.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <returns><see langword="true"/> if the player is valid, otherwise
        /// <see langword="false"/>.</returns>
        public bool IsValid(PlayerAgent player)
        {
            return this.IsValid(player.Owner);
        }
    }
}
