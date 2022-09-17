using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Util.Threads;

public static class Scheduler
{
    /// <param name="delay">in milliseconds</param>
    public static CancellationTokenSource Delay(int delay, Action action, Dispatcher? dispatcher = null)
    {
        var tokenSource = new CancellationTokenSource();
        Task.Run(async () =>
        {
            if (tokenSource.IsCancellationRequested) {
                return;
            }

            // wait for the given time period
            await Task.Delay(delay, tokenSource.Token);
                
            // invoke via dispatcher or directly
            if (dispatcher != null) {
                dispatcher.Invoke(action);
            } else {
                action.Invoke();                    
            }
        }, tokenSource.Token);

        return tokenSource;
    }
}