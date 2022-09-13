namespace VacationRental.Domain.Booking
{
    public class Booking
    {
        public int Id { get; set; }

        public int RentalId { get; }

        public DateOnly Start { get; }

        public DateOnly End => Start.AddDays(Nights);

        public int Nights { get; }

        public Booking(int rentalId, DateOnly start, int nights)
        {
            if (nights < 0)
                throw new ArgumentOutOfRangeException(nameof(nights), "Must be positive");

            RentalId = rentalId;
            Start = start;
            Nights = nights;
        }

        public Booking(int id, int rentalId, DateOnly start, int nights) : this(rentalId, start, nights)
        {
            Id = id;
        }

        public bool Overlap(Booking other, int preparationTimeInDays)
        {
            return Start <= other.End.AddDays(preparationTimeInDays) && other.Start <= End.AddDays(preparationTimeInDays);
        }
    }
}
