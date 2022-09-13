namespace VacationRental.Data
{
    internal class SequenceEmulator
    {
        private int _number;

        public int GetNextValue()
            => Interlocked.Increment(ref _number);
    }
}
