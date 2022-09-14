using System.Collections.Concurrent;

namespace VacationRental.Domain
{
    public class SemaphorService
    {
        private readonly ConcurrentDictionary<int, SemaphoreSlim> _semaphors
            = new ConcurrentDictionary<int, SemaphoreSlim>();

        public SemaphoreSlim GetSemaphor(int rentalId)
            => _semaphors.GetOrAdd(rentalId, new SemaphoreSlim(1, 1));
    }
}
