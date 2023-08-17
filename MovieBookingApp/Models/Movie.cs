using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MovieBookingApp.Models
{
    [BsonIgnoreExtraElements]
    public class Movie
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("theatreName")]
        public string TheatreName { get; set; } = string.Empty;
        [BsonElement("ticketsAlloted")]
        public int TicketsAlloted { get; set; }
        [BsonElement("ticketsBooked")]
        public int TicketsBooked { get; set; }
        [BsonElement("movieImage")]
        public string MovieImage { get; set; }
        [BsonElement("ticketStatus")]
        public string ticketStatus { get; set; }

        [BsonElement("casts")]
        public List<string> Casts { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }


        [BsonElement("director")]
        public string Director { get; set; }

        [BsonElement("language")]
        public string Language { get; set; }

        [BsonElement("releaseDate")]
        public string ReleaseDate { get; set; }

        [BsonElement("releaseStatus")]
        public string ReleaseStatus { get; set; }

        [BsonElement("trailerUrl")]
        public string TrailerUrl { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

    }

    //[BsonIgnoreExtraElements]
    public class TicketStatusResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class DeleteMovieByNameAndIdResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
