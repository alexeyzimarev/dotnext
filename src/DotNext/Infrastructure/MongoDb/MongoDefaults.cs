using MongoDB.Driver;

namespace DotNext.Infrastructure.MongoDb
{
    public static class MongoDefaults
    {
        public static readonly BulkWriteOptions DefaultBulkWriteOptions = new BulkWriteOptions { IsOrdered = false };
        public static readonly UpdateOptions    DefaultUpdateOptions    = new UpdateOptions { IsUpsert     = true };
        public static readonly ReplaceOptions   DefaultReplaceOptions   = new ReplaceOptions { IsUpsert    = true };
    }
}
