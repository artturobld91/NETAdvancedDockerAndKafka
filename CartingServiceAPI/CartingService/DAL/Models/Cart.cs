using MongoDB.Bson.Serialization.Attributes;

namespace CartingService.DAL.Models
{
    public sealed class Cart
    {
        [BsonId] // Specifies a field that must be unique
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public Guid Id { get; set; } = Guid.Empty;
    }
}
