using Conference.Client.Validators;
using Conference.Client.Views;
using Conference.Core.DTOModels;
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
    public partial class EnterViewModel : ViewModelBase
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

        public ICommand EnterCommand { get; }

        public ICommand NavigateToCreationNewUserCommand { get; }

        public EnterViewModel(IRegionManager regionManager, IConnectionService connectionService)
        {
            _email = string.Empty;
            _password = string.Empty;

            _regionManager = regionManager;
            _connectionService = connectionService;

            EnterCommand = ReactiveCommand.CreateFromTask(Enter, CanExecuteEnterCommand());
            NavigateToCreationNewUserCommand = ReactiveCommand.Create(NavigateToCreationNewUser);
        }

        private async Task Enter()
        {
            // TODO: dialog service
            try
            {
                var singInModel = new SingInModel(_email, _password);
                var result = await _connectionService.IsUserVerifiedAsync(singInModel);

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

        private IObservable<bool> CanExecuteEnterCommand()
            => this.WhenAnyValue(_ => _.Email, _ => _.Password)
                            .Select((_, _) => SimpleValidator.IsValid(new SingInModel(_email, _password)));

        private void NavigateToCreationNewUser()
            => _regionManager.RequestNavigate(MainContentFrame, nameof(CreationNewUserView));

        private void NavigateToUserSpace()
            => _regionManager.RequestNavigate(MainContentFrame, nameof(UserSpaceView));
    }
}
