using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyFirstMCP;

public class ProductService
{
    private readonly IMongoCollection<Product> collection;

    public ProductService()
    {
        var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION") ?? "mongodb://localhost:27017/";
        var dbName = Environment.GetEnvironmentVariable("MONGO_DB") ?? "store";
        var client = new MongoClient(connectionString);
        var db = client.GetDatabase(dbName);
        collection = db.GetCollection<Product>("products");
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await collection.Find(Builders<Product>.Filter.Empty).ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(string id)
    {
        var filter = Builders<Product>.Filter.Eq(p => p._id, id);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<Product?> GetProductByNameAsync(string name)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.name, name);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product._id))
            product._id = ObjectId.GenerateNewId().ToString();

        await collection.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> UpdateProductAsync(string id, Product updated)
    {
        var result = await collection.ReplaceOneAsync(p => p._id == id, updated);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        var result = await collection.DeleteOneAsync(p => p._id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public int id { get; set; }
    public string name { get; set; }
    public string category { get; set; }
    public int price { get; set; }
    public string image { get; set; }
}
