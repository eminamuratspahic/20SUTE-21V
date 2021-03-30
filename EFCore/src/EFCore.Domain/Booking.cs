using System;

namespace EFCore.Domain
{
    public class Booking
    {
        public string Id { get; private set; }
        public DateTime Date { get; private set; }

        public Booking(string id, DateTime date)
        {
            this.Id = id;
            this.Date = date;
        }
    }
}