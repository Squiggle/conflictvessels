using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using ConflictVessels.Engine;

namespace ConflictVessels.Engine
{
    /// <summary>
    /// Extension methods for serializing Game instances to JSON.
    /// </summary>
    public static class GameSerializationExtensions
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        /// <summary>
        /// Serializes the Game instance to a JSON string.
        /// </summary>
        public static string ToJson(this Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            return JsonSerializer.Serialize(game, Options);
        }

        /// <summary>
        /// Serializes the Game instance and writes the JSON to a file.
        /// </summary>
        public static void ToJsonFile(this Game game, string filePath)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path must not be null or empty.", nameof(filePath));
            var json = game.ToJson();
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Deserializes a Game instance from a JSON string.
        /// </summary>
        public static Game FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentException("JSON must not be null or empty.", nameof(json));
            return JsonSerializer.Deserialize<Game>(json, Options)
                ?? throw new InvalidOperationException("Deserialization returned null Game instance.");
        }

        /// <summary>
        /// Deserializes a Game instance from a JSON file.
        /// </summary>
        public static Game FromJsonFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path must not be null or empty.", nameof(filePath));
            var json = File.ReadAllText(filePath);
            return FromJson(json);
        }
    }
}
