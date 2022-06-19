namespace AchievementsAPI.Registries
{
    public interface IRegisterable
    {
        /// <summary>
        /// Returns the ID of this registerable item.
        /// </summary>
        /// <returns>The ID of this registerable item.</returns>
        string GetID();
    }
}
