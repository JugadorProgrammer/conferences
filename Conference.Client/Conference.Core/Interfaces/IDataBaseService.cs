using Conference.Core.Models;

namespace Conference.Core.Interfaces
{
    public interface IDataBaseService
    {
        #region User
        #region Get
        public Task<User?> GetUser(int id);

        public Task<User?> GetUser(string email, string password);
        #endregion

        #region Check
        public Task<bool> ExistUser(User compareUser);
        #endregion

        #region Create
        public Task<long> CreateUser(User user);
        #endregion
        #endregion

        #region Group
        #endregion
    }
}
