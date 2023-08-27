using HealthMate.Constants;
using SQLite;
using System.Linq.Expressions;

namespace HealthMate.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _dummyDatabase;
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    #region Create
    public async Task<CreateTableResult> CreateTable<T>() where T : new()
    {
        return await WithSemaphore(async () =>
        {
            try
            {
                var database = await InternalAsyncDatabase();
                var table = await database.CreateTableAsync<T>(CreateFlags.None);
                return table;
            }
            catch (Exception ex)
            {
                return default;
            }
        });
    }

    public async Task<int> Upsert<T>(T entity) where T : new()
    {
        return await WithSemaphore(async () =>
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
        });
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

        return await WithSemaphore(async () =>
        {
            var database = new SQLiteAsyncConnection(SQLiteConstants.DatabasePath, SQLiteConstants.Flags);
            await database.EnableWriteAheadLoggingAsync();
            _dummyDatabase = database;

            return _dummyDatabase;
        });
    }

    private async Task<T> WithSemaphore<T>(Func<Task<T>> action)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            return await action().ConfigureAwait(false);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
    #endregion
}