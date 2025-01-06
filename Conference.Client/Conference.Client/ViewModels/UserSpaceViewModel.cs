using Conference.Core.Models;
using Conference.Core.WebServices;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Conference.Client.ViewModels
{
    public partial class UserSpaceViewModel : ViewModelBase
    {
        private readonly IConnectionService _connectionService;

        [Reactive]
        private IEnumerable<Group> _groups;

        public ICommand GetGroupsCommand { get; set; }

        public UserSpaceViewModel(IConnectionService connectionService) 
        {
            _connectionService = connectionService;

            _groups = [];

            GetGroupsCommand = ReactiveCommand.CreateFromTask<string?>(GetGroups);
        }

        private async Task GetGroups(string? groupName) 
            => Groups = await _connectionService.GetGroups(groupName);
    }
}
