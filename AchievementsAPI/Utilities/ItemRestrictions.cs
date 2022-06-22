using GameData;
using System.Collections.Generic;
using System.Linq;

namespace AchievementsAPI.Utilities
{
    /// <summary>
    /// Restrictions for specific items
    /// </summary>
    public sealed class ItemRestrictions
    {
        /// <summary>
        /// Whether or not to use the whitelist or blacklist.
        /// </summary>
        public bool UseWhiteList { get; set; }
        /// <summary>
        /// Enemies allowed.
        /// </summary>
        public List<DatablockReference?>? WhiteList { get; set; }
        /// <summary>
        /// Enemies not allowed.
        /// </summary>
        public List<DatablockReference?>? BlackList { get; set; }

        /// <summary>
        /// Returns whether or not the given item is valid by checking if it's in
        /// the whitelist or blacklist with it's item data block.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns><see langword="true"/> if the item is valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(Item item)
            => this.IsValid(item.ItemDataBlock);

        /// <summary>
        /// Returns whether or not the given item datablock is valid by checking if it's in
        /// the whitelist or blacklist.
        /// </summary>
        /// <param name="itemDB">The item DB to check.</param>
        /// <returns><see langword="true"/> if the enemy DB is valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(ItemDataBlock itemDB)
        {
            return this.IsValid(itemDB.persistentID) ||
                this.IsValid(itemDB.name);
        }

        /// <summary>
        /// Returns whether or not the given item ID is valid by checking if it's in
        /// the whitelist or blacklist with it's enemy data id.
        /// </summary>
        /// <param name="itemID">The item ID to check.</param>
        /// <returns><see langword="true"/> if the item ID is valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(uint itemID)
        {
            if (this.UseWhiteList)
            {
                return EntryWithID(this.WhiteList, itemID);
            }
            else
            {
                return !EntryWithID(this.BlackList, itemID);
            }
        }

        /// <summary>
        /// Returns whether or not the given item datablock name is valid.
        /// </summary>
        /// <param name="itemDBName">The item datablock name.</param>
        /// <returns><see langword="true"/> if the item db name is valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(string itemDBName)
        {
            if (this.UseWhiteList)
            {
                return EntryWithName(this.WhiteList, itemDBName);
            }
            else
            {
                return !EntryWithName(this.BlackList, itemDBName);
            }
        }

        private static bool EntryWithName(List<DatablockReference?>? entries, string name)
        {
            return entries?.Any(r =>
            {
                ItemDataBlock? block = r?.GetBlock<ItemDataBlock>();
                if (block == null)
                {
                    return false;
                }

                return block.name == name;
            }) ?? false;
        }

        private static bool EntryWithID(List<DatablockReference?>? entries, uint id)
        {
            return entries?.Any(r =>
            {
                ItemDataBlock? block = r?.GetBlock<ItemDataBlock>();
                if (block == null)
                {
                    return false;
                }

                return block.persistentID == id;
            }) ?? false;
        }
    }
}
