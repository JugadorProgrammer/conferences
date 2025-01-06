using Conference.Client.Validators;
using Conference.Client.Views;
using Conference.Core.Models;
using Conference.Core.WebServices;
using Prism.Regions;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Conference.Client.ViewModels
{
    public partial class CreationNewUserViewModel : ViewModelBase
    {
        private const string MainContentFrame = nameof(MainContentFrame);

        private readonly IRegionManager _regionManager;
        private readonly IConnectionService _connectionService;

        [Reactive]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Length should be between 3 and 40 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        private string _email;

        [Reactive]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Length should be between 8 and 40 characters")]
        private string _password;

        [Reactive]
        [StringLength(20, MinimumLength = 4)]
        private string _name;

        public ICommand NavigateToEnterCommand { get; }

        public ICommand CreateNewUserCommand { get; }

        public CreationNewUserViewModel(IRegionManager regionManager, IConnectionService connectionService) 
        {
            _regionManager = regionManager;
            _connectionService = connectionService;

            _name = string.Empty;
            _email = string.Empty;
            _password = string.Empty;

            NavigateToEnterCommand = ReactiveCommand.Create(NavigateToEnter);
            CreateNewUserCommand = ReactiveCommand.CreateFromTask(CreateNewUser, CanExecuteCreateNewUserCommnad());

        }
        private async Task CreateNewUser()
        {
            // TODO: dialog service
            try
            {
                var user = new User()
                {
                    Name = _name,
                    Email = _email,
                    Password = _password
                };

                var result = await _connectionService.CreateNewUserAsync(user);

                if (result)
                {
                    NavigateToUserSpace();
                    return;
                }
            }
            catch (InvalidOperationException exception)
            {
                // TODO: dialog with message "The server does not respond, please try again later"
            }
            catch (HttpRequestException exception)
            {
                // TODO: dialog with message "The server does not respond, please try again later"
            }
            catch (TaskCanceledException exception)
            {
                // TODO: dialog with message "The server does not respond, please try again later"
            }
            catch (UriFormatException exception)
            {
                // TODO: dialog with message "The application does not has uncorrect path to server"
            }
        }

        private IObservable<bool> CanExecuteCreateNewUserCommnad()
            => this.WhenAnyValue(_ => _.Name, _ => _.Email, _ => _.Password)
                                    .Select((_, _) => SimpleValidator.IsValid(new User()
                                    {
                                        Name = _name,
                                        Email = _email,
                                        Password = _password
                                    }));

        private void NavigateToEnter()
            => _regionManager.RequestNavigate(MainContentFrame, nameof(EnterView));

        private void NavigateToUserSpace()
            => _regionManager.RequestNavigate(MainContentFrame, nameof(UserSpaceView));
    }
}
