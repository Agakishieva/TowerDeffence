public class Serialization
{
    public const int VERSION = 1;
    public const string DEFEND_PATH = "DefenseFiles";
    public const string DEFEND_EXTENSION = ".def";

    public const string LEVELS_PATH = "Levels";
    public const string LEVELS_EXTENSION = ".json";

    public enum ErrorCode
    {
        FileNotFound,
        VersionInvalid,
        Unknown
    }
}