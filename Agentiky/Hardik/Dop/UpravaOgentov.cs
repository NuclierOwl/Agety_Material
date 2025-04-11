using System;
using System.Collections.Generic;
using System.Linq;
using Agents_BD_Tres.Hardik.Connect;
using Agents_BD_Tres.Hardik.Connect.Dao;
using Npgsql;

namespace Agentiky.Hardik.Dop
{
    public class UpravaOgentov : IDisposable
    {
        private readonly DatabaseConnection _dbConnection;
        private readonly NpgsqlConnection _connection;

        public UpravaOgentov()
        {
            _dbConnection = new DatabaseConnection();
            _connection = _dbConnection.GetConnection();
        }

        public List<ProductDao> GetAllProducts() =>
            _dbConnection.GetAllProducts();

        public ProductDao GetProductById(int id) =>
            GetAllProducts().FirstOrDefault(p => p.id == id);

        public void AddProduct(ProductDao product)
        {
            using (var cmd = new NpgsqlCommand("INSERT INTO \"Tres\".product (title, producttypeid, articlenumber, description, image, productionpersoncount, productionworkshopnumber, mincostforagent) VALUES (@title, @producttypeid, @articlenumber, @description, @image, @productionpersoncount, @productionworkshopnumber, @mincostforagent)", _connection))
            {
                cmd.Parameters.AddWithValue("@title", product.title);
                cmd.Parameters.AddWithValue("@producttypeid", product.producttypeid ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@articlenumber", product.articlenumber);
                cmd.Parameters.AddWithValue("@description", product.description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@image", product.image ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@productionpersoncount", product.productionpersoncount ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@productionworkshopnumber", product.productionworkshopnumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@mincostforagent", product.mincostforagent);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateProduct(ProductDao product)
        {
            using (var cmd = new NpgsqlCommand("UPDATE \"Tres\".product SET title = @title, producttypeid = @producttypeid, articlenumber = @articlenumber, description = @description, image = @image, productionpersoncount = @productionpersoncount, productionworkshopnumber = @productionworkshopnumber, mincostforagent = @mincostforagent WHERE id = @id", _connection))
            {
                cmd.Parameters.AddWithValue("@id", product.id);
                cmd.Parameters.AddWithValue("@title", product.title);
                cmd.Parameters.AddWithValue("@producttypeid", product.producttypeid ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@articlenumber", product.articlenumber);
                cmd.Parameters.AddWithValue("@description", product.description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@image", product.image ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@productionpersoncount", product.productionpersoncount ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@productionworkshopnumber", product.productionworkshopnumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@mincostforagent", product.mincostforagent);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int id)
        {
            using (var cmd = new NpgsqlCommand("DELETE FROM \"Tres\".product WHERE id = @id", _connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<ProductDao> FilterProducts(string searchTerm)
        {
            return GetAllProducts()
                .Where(p => p.title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            (p.description?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false))
                .ToList();
        }

        public void Dispose()
        {
            _dbConnection.Dispose();
        }
    }
}
