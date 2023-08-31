using MongoDB.Bson;
using Realms;
using System.Linq.Expressions;

namespace HealthMate.Services;

public class RealmService
{
    private readonly Realm _realm;

    #region Create
    public async Task<T> Upsert<T>(T itemToAdd) where T : IRealmObject
    {
        var realm = await InternalAsyncRealm();
        var writeOperation = await realm.WriteAsync(() => realm.Add(itemToAdd, true));
        return writeOperation;
    }
    #endregion

    #region Read
    public async Task<T> Find<T>(ObjectId primaryKey) where T : IRealmObject
    {
        var realm = await InternalAsyncRealm();
        var foundData = realm.Find<T>(primaryKey);
        return foundData;
    }

    public async Task<IQueryable<T>> Find<T>(Expression<Func<T, bool>> expression) where T : IRealmObject
    {
        var realm = await InternalAsyncRealm();
        var foundData = realm.All<T>().Where(expression);
        return foundData;
    }

    public async Task<IQueryable<T>> FindAll<T>() where T : IRealmObject
    {
        var realm = await InternalAsyncRealm();
        var foundData = realm.All<T>();
        return foundData;
    }
    #endregion

    #region Delete
    public async Task Delete<T>(T objToDelete) where T : IRealmObject
    {
        var realm = await InternalAsyncRealm();
        await realm.WriteAsync(() => realm.Remove(objToDelete));
    }

    public async Task Delete<T>(Expression<Func<T, bool>> expression) where T : IRealmObject
    {
        var realm = await InternalAsyncRealm();
        var objsToDelete = realm.All<T>().Where(expression);
        await realm.WriteAsync(() => realm.RemoveRange(objsToDelete));
    }

    public async Task DeleteAll<T>() where T : IRealmObject
    {
        var realm = await InternalAsyncRealm();
        await realm.WriteAsync(() => realm.RemoveAll<T>());
    }
    #endregion

    #region Internal functions
    private async Task<Realm> InternalAsyncRealm()
    {
        if (_realm != null)
            return _realm;

        //var dbPath = Path.Combine(FileSystem.AppDataDirectory, "HealthMate.realm");
        var realm = await Realm.GetInstanceAsync();
        return realm;
    }
    #endregion
}
