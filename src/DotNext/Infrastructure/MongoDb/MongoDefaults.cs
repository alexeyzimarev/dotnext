using DotNext.Lib;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace DotNext.Infrastructure.MongoDb
{
    public static class MongoDefaults
    {
        public static readonly BulkWriteOptions DefaultBulkWriteOptions = new BulkWriteOptions { IsOrdered = false };
        public static readonly UpdateOptions    DefaultUpdateOptions    = new UpdateOptions { IsUpsert     = true };
        public static readonly ReplaceOptions   DefaultReplaceOptions   = new ReplaceOptions { IsUpsert    = true };

        public static void RegisterConventions() {
            if (BsonClassMap.IsClassMapRegistered(typeof(Document))) return;

            var pack = new ConventionPack {
                new CamelCaseElementNameConvention(),
                new IgnoreIfNullConvention(true),
                new IgnoreExtraElementsConvention(true)
            };

            ConventionRegistry.Register("DotNext", pack, type => true);
            
        }
    }
}
