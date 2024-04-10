using backendPrueba.Data;
using backendPrueba.Models;
using Dapper;
using System.Data;

namespace backendPrueba.Repository
{
    public class CustomerRepository : ICustomerRespository
    {
        private readonly Connection _connection;
        public CustomerRepository(Connection connection) 
        {
            _connection = connection;
        }

        public async Task CreateCustomer(Customer customer)
        {
            using (var connection = _connection.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Name", customer.Name, DbType.String);
                parameters.Add("@Email", customer.Email, DbType.String);
                parameters.Add("@PhoneNumber", customer.PhoneNumber, DbType.String);
                parameters.Add("@Address", customer.Address, DbType.String);
                parameters.Add("@City", customer.City, DbType.String);
                parameters.Add("@Country", customer.Country, DbType.String);
                parameters.Add("@IsActive", customer.IsActive, DbType.Boolean);

                // Establecer las fechas de creación y actualización
                customer.CreatedAt = DateTime.UtcNow;
                customer.UpdatedAt = DateTime.UtcNow;

                parameters.Add("@CreatedAt", customer.CreatedAt, DbType.DateTime);
                parameters.Add("@UpdatedAt", customer.UpdatedAt, DbType.DateTime);

                await connection.ExecuteAsync("sp_create_customer", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateCustomer(Customer customer)
        {
            using (var connection = _connection.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", customer.Id, DbType.Int32);
                parameters.Add("@Name", customer.Name, DbType.String);
                parameters.Add("@Email", customer.Email, DbType.String);
                parameters.Add("@PhoneNumber", customer.PhoneNumber, DbType.String);
                parameters.Add("@Address", customer.Address, DbType.String);
                parameters.Add("@City", customer.City, DbType.String);
                parameters.Add("@Country", customer.Country, DbType.String);
                parameters.Add("@IsActive", customer.IsActive, DbType.Boolean);

                // Establecer la fecha de actualización
                customer.UpdatedAt = DateTime.UtcNow;

                parameters.Add("@UpdatedAt", customer.UpdatedAt, DbType.DateTime);

                await connection.ExecuteAsync("sp_update_customer", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteCustomer(int id)
        {
            using (var connection = _connection.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);

                await connection.ExecuteAsync("sp_delete_customer", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            using (var connection = _connection.GetConnection())
            {
                return await connection.QueryAsync<Customer>("sp_get_all_customers", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            using (var connection = _connection.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);

                return await connection.QueryFirstOrDefaultAsync<Customer>("sp_get_customer_by_id", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
