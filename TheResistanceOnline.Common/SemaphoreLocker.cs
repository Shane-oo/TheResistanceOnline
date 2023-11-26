namespace TheResistanceOnline.Common;

public class SemaphoreLocker
{
    #region Fields

    private readonly SemaphoreSlim _semaphore = new(1, 1);

    #endregion

    #region Public Methods

    public async Task LockAsync(Func<Task> worker)
    {
        var isTaken = false;
        try
        {
            do
            {
                try
                {
                }
                finally
                {
                    isTaken = await _semaphore.WaitAsync(TimeSpan.FromSeconds(1));
                }
            } while(!isTaken);

            await worker();
        }
        finally
        {
            if (isTaken)
            {
                _semaphore.Release();
            }
        }
    }

    // overloading variant for non-void methods with return type (generic T)
    public async Task<T> LockAsync<T>(Func<Task<T>> worker)
    {
        var isTaken = false;
        try
        {
            do
            {
                try
                {
                }
                finally
                {
                    isTaken = await _semaphore.WaitAsync(TimeSpan.FromSeconds(1));
                }
            } while(!isTaken);

            return await worker();
        }
        finally
        {
            if (isTaken)
            {
                _semaphore.Release();
            }
        }
    }

    #endregion
}
