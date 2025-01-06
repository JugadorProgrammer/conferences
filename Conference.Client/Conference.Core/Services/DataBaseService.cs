using Conference.Core.Interfaces;
using Conference.Core.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace Conference.Core.Services
{
    public class DataBaseService : IDataBaseService
    {
        private readonly string _connectionString;

        public DataBaseService(string? connectionString)
        {
            ArgumentNullException.ThrowIfNull(connectionString);
            _connectionString = connectionString;
        }

        #region User
        public async Task<long> CreateUser(User user)
        {
            using IDbConnection connection = new NpgsqlConnection(_connectionString);
            return (await connection.QueryAsync<long>("INSERT INTO \"User\"(\"Name\", \"Email\", \"Password\", \"Status\") VALUES(@Name, @Email, @Password, @Status) RETURNING \"Id\"",
                user)).FirstOrDefault();
        }

        public async Task<bool> ExistUser(User compareUser)
        {
            using IDbConnection connection = new NpgsqlConnection(_connectionString);
            var user = (await connection.QueryAsync<User>("SELECT * FROM \"User\" WHERE \"Name\"=@Name OR \"Email\"=@Email",
                compareUser)).FirstOrDefault();
            return user is not null;
        }

        public async Task<User?> GetUser(int id)
        {
            using IDbConnection connection = new NpgsqlConnection(_connectionString);
            return (await connection.QueryAsync<User>("SELECT * FROM \"User\" WHERE \"Id\"=@id",
                new { id })).FirstOrDefault();
        }

        public async Task<User?> GetUser(string email, string password)
        {
            using IDbConnection connection = new NpgsqlConnection(_connectionString);
            return (await connection.QueryAsync<User>("SELECT * FROM \"User\" WHERE \"Email\"=@email AND \"Password\"=@password",
                new { email, password })).FirstOrDefault();
        }
        #endregion

        #region Group

        public async Task<IEnumerable<Group>> GetGroups(string? name)
        {
            name = name is null
                    ? string.Empty
                    : ArgumentToLike(name.ToLower());

            using IDbConnection connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<Group>("SELECT * FROM \"Group\" WHERE LOWER(\"Name\") LIKE @name", new { name });
        }

        #endregion

        private string ArgumentToLike(string argument) 
            => $"%{argument.Replace("[", "[[]").Replace("%", "[%]")}%";
    }
}
