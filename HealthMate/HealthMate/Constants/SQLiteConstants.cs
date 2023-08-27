using SQLite;

namespace HealthMate.Constants;

public static class SQLiteConstants
{
    public const string DatabaseFilename = "HealthMate.db3";

    public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite |
        SQLiteOpenFlags.Create |
        SQLiteOpenFlags.FullMutex |
        SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
}