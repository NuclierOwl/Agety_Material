using System;
using System.Collections.Generic;
using Agents_BD_Tres.Hardik.Connect.Dao;
using Npgsql;

namespace Agents_BD_Tres.Hardik.Connect
{
    public class DatabaseConnection : IDisposable
    {
        private readonly NpgsqlConnection _connection;

        public DatabaseConnection() // подключение к БД
        {
            string connectionString = "Server=45.67.56.214;Port=5421;Database=user16;User Id=user16;Password=dZ28IVE5;";
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
        }

        public NpgsqlConnection GetConnection() => _connection;

        public List<AgentDao> GetAllAgents()    //для работы с агентами
        {
            var agents = new List<AgentDao>();
            using (var cmd = new NpgsqlCommand("SELECT * FROM \"Tres\".agent", _connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    agents.Add(new AgentDao
                    {
                        id = reader.GetInt32(0),
                        title = reader.GetString(1),
                        agenttypeid = reader.GetInt32(2),
                        address = reader.IsDBNull(3) ? null : reader.GetString(3),
                        inn = reader.GetString(4),
                        kpp = reader.IsDBNull(5) ? null : reader.GetString(5),
                        directorname = reader.IsDBNull(6) ? null : reader.GetString(6),
                        phone = reader.GetString(7),
                        email = reader.IsDBNull(8) ? null : reader.GetString(8),
                        logo = reader.IsDBNull(9) ? null : reader.GetString(9),
                        priority = reader.GetInt32(10)
                    });
                }
            }
            return agents;
        }


        public List<ProductDao> GetAllProducts()  //для работы с продуктами
        {
            var products = new List<ProductDao>();
            using (var cmd = new NpgsqlCommand("SELECT * FROM \"Tres\".product", _connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    products.Add(new ProductDao
                    {
                        id = reader.GetInt32(0),
                        title = reader.GetString(1),
                        producttypeid = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                        articlenumber = reader.GetString(3),
                        description = reader.IsDBNull(4) ? null : reader.GetString(4),
                        image = reader.IsDBNull(5) ? null : reader.GetString(5),
                        productionpersoncount = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                        productionworkshopnumber = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7),
                        mincostforagent = reader.GetDecimal(8)
                    });
                }
            }
            return products;
        }


        public List<MaterialDao> GetAllMaterials()  //для работы с матерьялами
        {
            var materials = new List<MaterialDao>();
            using (var cmd = new NpgsqlCommand("SELECT * FROM \"Tres\".material", _connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    materials.Add(new MaterialDao
                    {
                        id = reader.GetInt32(0),
                        title = reader.GetString(1),
                        countinpack = reader.GetInt32(2),
                        unit = reader.GetString(3),
                        countinstock = reader.GetInt32(4),
                        mincount = reader.GetInt32(5),
                        description = reader.IsDBNull(6) ? null : reader.GetString(6),
                        image = reader.IsDBNull(7) ? null : reader.GetString(7),
                        materialtypeid = reader.GetInt32(8)
                    });
                }
            }
            return materials;
        }


        public List<AgentTypeDao> GetAgentTypes()  //для работы с типами
        {
            var types = new List<AgentTypeDao>();
            using (var cmd = new NpgsqlCommand("SELECT id, title, image FROM \"Tres\".agenttype", _connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    types.Add(new AgentTypeDao
                    {
                        id = reader.GetInt32(0),
                        title = reader.GetString(1),
                        image = reader.IsDBNull(2) ? null : reader.GetString(2)
                    });
                }
            }
            return types;
        }

        public int GetAgentSalesCount(int agentId, DateTime startDate, DateTime endDate) //счетчик продаж
        {
            using (var cmd = new NpgsqlCommand(
                "SELECT COUNT(*) FROM \"Tres\".productsale WHERE agentid = @agentId AND saleDate BETWEEN @startDate AND @endDate",
                _connection))
            {
                cmd.Parameters.AddWithValue("@agentId", agentId);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public decimal GetAgentTotalSalesAmount(int agentId) // сумма продаж
        {
            using (var cmd = new NpgsqlCommand(
                "SELECT SUM(ps.quantity * p.mincostforagent) FROM \"Tres\".productsale ps " +
                "JOIN \"Tres\".product p ON ps.productid = p.id WHERE ps.agentid = @agentId",
                _connection))
            {
                cmd.Parameters.AddWithValue("@agentId", agentId);
                var result = cmd.ExecuteScalar();
                return result is DBNull ? 0 : Convert.ToDecimal(result);
            }
        }

        public void UpdateAgent(AgentDao agent) // добавление агентов
        {
            using (var cmd = new NpgsqlCommand(
                "UPDATE \"Tres\".agent SET title = @title, agenttypeid = @agenttypeid, address = @address, " +
                "inn = @inn, kpp = @kpp, directorname = @directorname, phone = @phone, email = @email, " +
                "logo = @logo, priority = @priority WHERE id = @id",
                _connection))
            {
                cmd.Parameters.AddWithValue("@id", agent.id);
                cmd.Parameters.AddWithValue("@priority", agent.priority);
                cmd.ExecuteNonQuery();
            }
        }





        public void Dispose() // Отключение от БД
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}