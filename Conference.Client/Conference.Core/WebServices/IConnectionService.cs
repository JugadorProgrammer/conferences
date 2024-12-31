using Conference.Core.DTOModels;
using Conference.Core.Models;

namespace Conference.Core.WebServices
{
    public interface IConnectionService
    {
        public Task<bool> IsUserVerifiedAsync(SingInModel singInModel);

        public Task<bool> CreateNewUserAsync(User user);

        public Task<bool> ExitAsync();
    }
}
