﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace MovieBookingApp.Models
{
    [BsonIgnoreExtraElements]
    public class Ticket
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string Id { get; set; } = string.Empty;

        [BsonElement("movieName")]
        public string MovieName { get; set; } = string.Empty;
        [BsonElement("theatreName")]
        public string TheatreName { get; set; } = string.Empty;
        [BsonElement("numberOfTickets")]
        public int NumberOfTickets { get; set; }
        [BsonElement("seatNumber")]
        public string SeatNumber { get; set; } = string.Empty;
        [BsonElement("customerId")]
        public string UserId { get; set; } 
    }
}
