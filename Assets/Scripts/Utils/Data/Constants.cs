using SaveLoad;
using World.Generation;

namespace Utils.Data
{
    public static class GlobalDataKeys
    {
        public static readonly DataSignature<GameState> GameState = new("GameState");
        public static readonly DataSignature<WorldSettings> CurrentWorldSettings = new("WorldSetting");
        public static readonly DataSignature<FileLocation> JoinWorldLocation = new("JoinWorldLocation");
    }
    
    public static class FileNameConstants
    {
        public const string WorldSettingsFile = "settings";
        public const string WorldChunk = "chunk";
        public const string MachineData = "machine_data";
        public const string PlayerData = "player_data";
        public const string GroundItems = "ground_items";
    }

    public static class Statistics
    {
        public const int AccessDistance = 6;
    }
}