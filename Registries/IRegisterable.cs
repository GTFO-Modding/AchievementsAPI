namespace AchievementsAPI.Registries
{
    /// <summary>
    /// Represents something that's registerable. This means that
    /// it has a unique identifier that can be used to get this specific item.
    /// </summary>
    public interface IRegisterable
    {
        /// <summary>
        /// Returns the ID of this registerable item.
        /// </summary>
        /// <returns>The ID of this registerable item.</returns>
        string GetID();
    }
}
