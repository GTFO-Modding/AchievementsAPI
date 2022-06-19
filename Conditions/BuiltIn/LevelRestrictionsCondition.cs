using AchievementsAPI.Utilities;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AchievementsAPI.Conditions.BuiltIn
{
    public class LevelRestrictionsCondition : AchievementCondition<LevelRestrictionsCondition.CustomData>
    {
        public const string ID = "LevelRestrictions";

        public override bool IsMet()
        {
            var expedition = SNet.GetLocalCustomData<pActiveExpedition>();
            return this.Data.IsValid(expedition.expeditionIndex, expedition.tier);
        }

        public override string GetID()
        {
            return ID;
        }

        [JsonConverter(typeof(Converter))]
        public sealed class CustomData : ConditionData
        {
            public LevelRestrictions? Restrictions { get; set; }

            public bool IsValid(int expeditionIndex, eRundownTier tier)
            {
                return this.Restrictions?.IsValid(expeditionIndex, tier) ?? true;
            }
        }

        private sealed class Converter : JsonConverter<CustomData>
        {
            public override CustomData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var data = new CustomData
                {
                    Restrictions = JsonSerializer.Deserialize<LevelRestrictions>(ref reader, options)
                };
                return data;
            }

            public override void Write(Utf8JsonWriter writer, CustomData value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value.Restrictions, options);
            }
        }
    }
}
