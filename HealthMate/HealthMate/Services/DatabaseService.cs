using HealthMate.Constants;
using SQLite;
using System.Linq.Expressions;

namespace HealthMate.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _dummyDatabase;

    #region Create
    public async Task<CreateTablesResult> InitializeTables(params Type[] tables)
    {
        try
        {
            var database = await InternalAsyncDatabase();
            var tableCreationResult = await database.CreateTablesAsync(CreateFlags.None, tables);
            return tableCreationResult;
        }
        catch (Exception ex)
        {
            return default;
        }
    }

    public async Task<int> Upsert<T>(T entity) where T : new()
    {
        try
        {
            var database = await InternalAsyncDatabase();
            var rows = await database.UpdateAsync(entity);
            if (rows == 0)
                rows = await database.InsertAsync(entity);
            return rows;
        }
        catch (Exception ex)
        {
            return default;
        }
    }
    #endregion

    #region Read
    public async Task<IEnumerable<T>> Find<T>() where T : new()
    {
        try
        {
            var database = await InternalAsyncDatabase();
            var data = await database
                .Table<T>()
                .ToListAsync();
            return data;
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<T>();
        }
    }

    public async Task<IEnumerable<T>> Find<T>(Expression<Func<T, bool>> predicate) where T : new()
    {
        try
        {
            var database = await InternalAsyncDatabase();
            var data = await database
                .Table<T>()
                .Where(predicate)
                .ToListAsync();
            return data;
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<T>();
        }
    }
    #endregion

    #region Update
    public async Task<int> Update<T>(T entity) where T : new()
    {
        try
        {
            var database = await InternalAsyncDatabase();
            var data = await database.UpdateAsync(entity);
            return data;
        }
        catch (Exception ex)
        {
            return default;
        }
    }
    #endregion

    #region Delete
    public async Task<int> Delete<T>(int id) where T : new()
    {
        try
        {
            var database = await InternalAsyncDatabase();
            var data = await database.DeleteAsync<T>(id);
            return data;
        }
        catch (Exception ex)
        {
            return default;
        }
    }

    public async Task<int> Delete<T>(string id) where T : new()
    {
        try
        {
            var database = await InternalAsyncDatabase();
            var data = await database.DeleteAsync<T>(id);
            return data;
        }
        catch (Exception ex)
        {
            return default;
        }
    }

    public async Task<int> Delete<T>(T entity) where T : new()
    {
        try
        {
            var database = await InternalAsyncDatabase();
            var data = await database.DeleteAsync(entity);
            return data;
        }
        catch (Exception ex)
        {
            return default;
        }
    }

    public async Task<int> DeleteAll<T>() where T : new()
    {
        try
        {
            var database = await InternalAsyncDatabase();
            var data = await database.DeleteAllAsync<T>();
            return data;
        }
        catch (Exception ex)
        {
            return default;
        }
    }
    #endregion

    #region Internal functions
    private async Task<SQLiteAsyncConnection> InternalAsyncDatabase()
    {
        if (_dummyDatabase != null)
            return _dummyDatabase;

        var database = new SQLiteAsyncConnection(SQLiteConstants.DatabasePath, SQLiteConstants.Flags);
        await database.EnableWriteAheadLoggingAsync();
        _dummyDatabase = database;

        return _dummyDatabase;
    }
    #endregion
}