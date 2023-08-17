namespace MovieBookingApp.Models.Dtos
{
    public class MovieDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string TheatreName { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }

        public string ticketStatus { get; set; }
        public string MovieImage { get; set; }

        public List<string> Casts { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public string Language { get; set; }
        public string ReleaseDate { get; set; }
        public string ReleaseStatus { get; set; }
        public string TrailerUrl { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
