using AchievementsAPI.Registries;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AchievementsAPI.Converters
{
    /// <summary>
    /// An abstract JSON Converter that handles Registry Lists.
    /// </summary>
    /// <typeparam name="TList">The List</typeparam>
    /// <typeparam name="TElement">The elements in the list</typeparam>
    public abstract class InternalRegistryListConverter<TList, TElement> : JsonConverter<TList>
        where TList : IRegistryList<TElement>, new()
        where TElement : IRegisterable
    {
        /// <summary>
        /// The write mode of this converter.
        /// </summary>
        public InternalRegistryListWriteMode WriteMode { get; set; }

        /// <inheritdoc/>
        public override TList? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TList list = new();
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    break;
                case JsonTokenType.StartObject:
                    this.ReadAsMap(list, ref reader, typeToConvert, options);
                    break;
                case JsonTokenType.StartArray:
                    this.ReadAsList(list, ref reader, typeToConvert, options);
                    break;
                default:
                    throw new JsonException($"Unexpected token '{reader.TokenType}' (expected {JsonTokenType.StartObject}, or {JsonTokenType.StartArray})");
            }
            return list;
        }

        private void ReadAsList(TList list, ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            int elementIndex = 0;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return;
                }
                else if (reader.TokenType == JsonTokenType.StartObject)
                {
                    TElement element;
                    try
                    {
                        element = this.ReadElement(ref reader, typeToConvert, options);
                    }
                    catch (JsonException ex)
                    {
                        throw new JsonException($"Uncaught json exception whilst parsing element index {elementIndex}", ex);
                    }
                    list.Add(element);
                    elementIndex++;
                }
                else
                {
                    throw new JsonException($"Unexpected token {reader.TokenType} for element index {elementIndex}. (Expected {JsonTokenType.EndArray} or {JsonTokenType.StartObject})");
                }
            }
            throw new JsonException($"Unexpected end of input at element index {elementIndex}");
        }

        private void ReadAsMap(TList list, ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            int elementIndex = 0;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException($"Unexpected token {reader.TokenType}. Expected {JsonTokenType.PropertyName} at element index {elementIndex}");
                }

                string id = reader.GetString() ?? throw new JsonException($"ID was null at element index {elementIndex}");

                if (!reader.Read())
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.StartObject ||
                    reader.TokenType == JsonTokenType.Null)
                {
                    TElement element;
                    try
                    {
                        element = this.ReadElement(id, ref reader, typeToConvert, options);
                    }
                    catch (JsonException ex)
                    {
                        throw new JsonException($"Uncaught json exception whilst parsing element index {elementIndex} (ID: {id})", ex);
                    }

                    list.Add(element);
                    elementIndex++;
                }
                else
                {
                    throw new JsonException($"Unexpected token {reader.TokenType} for element index {elementIndex}. (Expected {JsonTokenType.EndArray} or {JsonTokenType.StartObject})");
                }
            }
            throw new JsonException($"Unexpected end of input at element index {elementIndex}");
        }

        private TElement ReadElement(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!reader.Read())
            {
                goto UNEXPECTED_EOI;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException($"Unexpected token {reader.TokenType}. Expected {JsonTokenType.PropertyName}");
            }

            string property = reader.GetString() ?? throw new JsonException("ID Property name null!");
            if (property.ToLower() != "id")
            {
                throw new JsonException($"First property of an element should be an ID field! Instead got {property}.");
            }

            if (!reader.Read())
            {
                goto UNEXPECTED_EOI;
            }

            if (reader.TokenType == JsonTokenType.Null)
            {
                throw new JsonException($"An ID cannot be 'null'");
            }

            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Unexpected token {reader.TokenType}. Expected {JsonTokenType.String}");
            }

            string id = reader.GetString()!;

            return this.ReadElement(id, ref reader, typeToConvert, options);

        UNEXPECTED_EOI:
            throw new JsonException("Unexpected end of input!");
        }

        private TElement ReadElement(string id, ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TElement element = this.CreateElementFromID(id);
            this.FillElement(element, ref reader, options);
            return element;
        }

        /// <summary>
        /// Create an element with the specific ID.
        /// </summary>
        /// <param name="id">The ID of the element to create.</param>
        /// <returns>The created element.</returns>
        protected abstract TElement CreateElementFromID(string id);
        /// <summary>
        /// Fills the <paramref name="element"/> with values from the <paramref name="reader"/>.
        /// </summary>
        /// <param name="element">The element to fill.</param>
        /// <param name="reader">The JSON reader</param>
        /// <param name="options">The Json Serializer options.</param>
        protected abstract void FillElement(TElement element, ref Utf8JsonReader reader, JsonSerializerOptions options);


        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TList value, JsonSerializerOptions options)
        {
            switch (this.WriteMode)
            {
                case InternalRegistryListWriteMode.List:
                    this.WriteAsList(writer, value, options);
                    break;
                case InternalRegistryListWriteMode.Map:
                    this.WriteAsMap(writer, value, options);
                    break;
            }
        }

        private void WriteAsList(Utf8JsonWriter writer, TList value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            if (value is null)
            {
                writer.WriteEndArray();
                return;
            }

            foreach (TElement element in value)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("ID");
                writer.WriteStringValue(element.GetID());

                this.WriteElementProperties(writer, element, options);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        private void WriteAsMap(Utf8JsonWriter writer, TList value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (value is null)
            {
                writer.WriteEndObject();
                return;
            }

            foreach (TElement element in value)
            {
                writer.WritePropertyName(element.GetID());

                writer.WriteStartObject();

                this.WriteElementProperties(writer, element, options);

                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Write the elements properties to the <paramref name="writer"/>. Don't worry about
        /// writing <see cref="JsonTokenType.StartObject"/> and <see cref="JsonTokenType.EndObject"/>,
        /// as that's already handled for you.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="element">The element to write.</param>
        /// <param name="options">The options.</param>
        protected abstract void WriteElementProperties(Utf8JsonWriter writer, TElement element, JsonSerializerOptions options);
    }
}
