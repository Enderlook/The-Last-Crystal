namespace System.Collections.Generic
{
    /// <summary>
    /// Compatibility prior .Net Standard 2.1
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.queue-1.trydequeue?view=netcore-3.1#applies-to"/>
    /// <seealso href="https://raw.githubusercontent.com/dotnet/standard/master/docs/versions/netstandard2.1_diff.md"></see> 
    /// </summary>
    public static class QueueExtension
    {
        public static bool TryDequeue<T>(this Queue<T> source, out T result)
        {
            if (source.Count > 0)
            {
                result = source.Dequeue();
                return true;
            }
            result = default;
            return false;
        }
    }
}
