namespace Blog.Storage
{
    using System;
    using System.Threading.Tasks;
    using Business.Entity;
    using Business.Logic.Interfaces;
    using Google.Apis.Json;
    using Google.Apis.Util.Store;

    public class DatabaseDataStore : IDataStore
    {

        private readonly IGoogleApiDataStoreBL _GoogleApiDataStoreBL;
        public DatabaseDataStore(IGoogleApiDataStoreBL GoogleApiDataStoreBL)
        {
            _GoogleApiDataStoreBL = GoogleApiDataStoreBL;
        }
        /// <summary>
        /// Stores the given value for the given key. It creates a new file (named <see cref="GenerateStoredKey"/>) in 
        /// <see cref="FolderPath"/>.
        /// </summary>
        /// <typeparam name="T">The type to store in the data store</typeparam>
        /// <param name="key">The key</param>
        /// <param name="value">The value to store in the data store</param>
        public Task StoreAsync<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }
            var serialized = NewtonsoftJsonSerializer.Instance.Serialize(value);


            var GoogleApiDataStore = _GoogleApiDataStoreBL.GetByUserName(key);

            if (GoogleApiDataStore == null)
            {
                _GoogleApiDataStoreBL.Insert(new GoogleApiDataStore { UserName = key, RefreshToken = serialized });

            }
            return TaskEx.Delay(0);
        }

        /// <summary>
        /// Deletes the given key. It deletes the <see cref="GenerateStoredKey"/> named file in <see cref="FolderPath"/>.
        /// </summary>
        /// <param name="key">The key to delete from the data store</param>
        public Task DeleteAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }

            var GoogleApiDataStore = _GoogleApiDataStoreBL.GetByUserName(key);
            _GoogleApiDataStoreBL.Delete(GoogleApiDataStore.Id);

            return TaskEx.Delay(0);
        }

        /// <summary>
        /// Returns the stored value for the given key or <c>null</c> if the matching file (<see cref="GenerateStoredKey"/>
        /// in <see cref="FolderPath"/> doesn't exist.
        /// </summary>
        /// <typeparam name="T">The type to retrieve</typeparam>
        /// <param name="key">The key to retrieve from the data store</param>
        /// <returns>The stored object</returns>
        public Task<T> GetAsync<T>(string key)
        {
            //Key is the user string sent with AuthorizeAsync
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

            var GoogleApiDataStore = _GoogleApiDataStoreBL.GetByUserName(key);

            if (GoogleApiDataStore == null)
            {
                tcs.SetResult(default(T));

            }
            else
            {
                string refreshToken = GoogleApiDataStore.RefreshToken;

                if (string.IsNullOrEmpty(refreshToken))
                {
                    // we don't have a record so we request it of the user.
                    tcs.SetResult(default(T));
                }
                else
                {
                    try
                    {
                        // we have it we use that.
                        tcs.SetResult(NewtonsoftJsonSerializer.Instance.Deserialize<T>(refreshToken));
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }
            }
            return tcs.Task;
        }

        /// <summary>
        /// Clears all values in the data store. This method deletes all files in <see cref="FolderPath"/>.
        /// </summary>
        public Task ClearAsync()
        {

            _GoogleApiDataStoreBL.TruncateTable();
            return TaskEx.Delay(0);
        }

        /// <summary>Creates a unique stored key based on the key and the class type.</summary>
        /// <param name="key">The object key</param>
        /// <param name="t">The type to store or retrieve</param>
        public static string GenerateStoredKey(string key, Type t)
        {
            return string.Format("{0}-{1}", t.FullName, key);
        }

    }
}
