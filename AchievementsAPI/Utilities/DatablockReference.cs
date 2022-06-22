using AchievementsAPI.Converters;
using GameData;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AchievementsAPI.Utilities
{
    /// <summary>
    /// A reference to a datablock. What more do you want in information? It literally
    /// just holds either an ID or Name
    /// </summary>
    [JsonConverter(typeof(DatablockReferenceConverter))]
    public sealed class DatablockReference
    {
        /// <summary>
        /// A name reference
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// An id reference
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        /// Creates a datablock reference for a name.
        /// </summary>
        /// <param name="name">The name.</param>
        public DatablockReference(string name)
        {
            this.Name = name;
            this.ID = 0;
        }

        /// <summary>
        /// Creates a datablock reference for an id.
        /// </summary>
        /// <param name="id">The id.</param>
        public DatablockReference(uint id)
        {
            this.Name = null;
            this.ID = id;
        }

        /// <summary>
        /// Gets the DataBlock this is referencing. It will use
        /// <see cref="ID"/> if <see cref="Name"/> is <see langword="null"/>
        /// </summary>
        /// <typeparam name="TBlock">The data block type.</typeparam>
        /// <returns>The data block entry.</returns>
        public TBlock? GetBlock<TBlock>()
            where TBlock : GameDataBlockBase<TBlock>
        {
            if (this.Name is null)
            {
                return GameDataBlockBase<TBlock>.GetBlock(this.ID);
            }
            else
            {
                return GameDataBlockBase<TBlock>.GetBlock(this.Name);
            }
        }
    }

    /// <summary>
    /// Some extension util methods for DataBlock References
    /// </summary>
    public static class DatablockReferenceExtensions
    {
        /// <summary>
        /// Returns whether or not the given list has a datablock entry with the given id.
        /// </summary>
        /// <typeparam name="TBlock">The type of DataBlock</typeparam>
        /// <param name="entries">The list.</param>
        /// <param name="id">The ID of the datablock to check.</param>
        /// <returns><see langword="true"/> if it exists in the list, otherwise
        /// <see langword="false"/>.</returns>
        public static bool HasEntryWithID<TBlock>(this IList<DatablockReference?> entries, uint id)
            where TBlock : GameDataBlockBase<TBlock>
        {
            if (entries == null)
            {
                return false;
            }

            foreach (DatablockReference entry in entries)
            {
                TBlock? block = entry?.GetBlock<TBlock>();
                if (block == null)
                {
                    continue;
                }

                if (block.persistentID == id)
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Returns whether or not the given list has a datablock entry with the given name.
        /// </summary>
        /// <typeparam name="TBlock">The type of DataBlock</typeparam>
        /// <param name="entries">The list.</param>
        /// <param name="name">The name of the datablock to check.</param>
        /// <returns><see langword="true"/> if it exists in the list, otherwise
        /// <see langword="false"/>.</returns>
        public static bool HasEntryWithName<TBlock>(this IList<DatablockReference?> entries, string name)
            where TBlock : GameDataBlockBase<TBlock>
        {
            if (entries == null)
            {
                return false;
            }

            foreach (DatablockReference entry in entries)
            {
                TBlock? block = entry?.GetBlock<TBlock>();
                if (block == null)
                {
                    continue;
                }

                if (block.name == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
