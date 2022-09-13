namespace VacationRental.Domain.Booking
{
    //todo:
    public class Booking
    {
        public int Id { get; set; }

        public int RentalId { get; set; }

        public DateOnly Start { get; set; }

        public DateOnly End => Start.AddDays(Nights);

        public int Nights { get; set; }


        public bool Overlap(Booking other)
        {
            return Start < other.End && other.Start < End;
        }
    }
}
