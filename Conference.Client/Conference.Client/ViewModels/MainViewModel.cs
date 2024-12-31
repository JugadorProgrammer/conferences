using Conference.Core.WebServices;
using Prism.Regions;
using ReactiveUI;
using System.Windows.Input;
using ReactiveUI.SourceGenerators;
using Conference.Client.Views;
using System.Threading.Tasks;
using Conference.Core.DTOModels;
using Conference.Client.Validators;
using System;
using System.Reactive.Linq;
using Conference.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace Conference.Client.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IConnectionService _connectionService;

        private const string MainContentFrame = nameof(MainContentFrame);

        #region Reactive
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
        #endregion

        #region Commands
        public ICommand NavigateToEnterCommand { get; }

        public ICommand NavigateToCreationNewUserCommand { get; }

        public ICommand ExitCommand { get; }

        public ICommand EnterCommand { get; }

        public ICommand CreateNewUserCommand { get; }
        #endregion

#pragma warning disable CS8618 //ClearParameters fix it
        public MainViewModel(IRegionManager regionManager, IConnectionService connectionService)
#pragma warning restore CS8618 
        {
            ClearParameters();

            _regionManager = regionManager;
            _connectionService = connectionService;

            NavigateToEnterCommand = ReactiveCommand.Create(NavigateToEnter);
            NavigateToCreationNewUserCommand = ReactiveCommand.Create(NavigateToCreationNewUser);

            ExitCommand = ReactiveCommand.CreateFromTask(Exit);
            EnterCommand = ReactiveCommand.CreateFromTask(Enter, CanExecuteEnterCommand());
            CreateNewUserCommand = ReactiveCommand.CreateFromTask(CreateNewUser, CanExecuteCreateNewUserCommnad());

            NavigateToEnter();
        }

        private async Task Exit()
            => await _connectionService.ExitAsync();

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

        private async Task CreateNewUser()
        {
            // TODO: dialog service
            try
            {
                var user = new User(_name, _email, _password);
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
                            .Select((_, _) => SimpleValidator.IsValid(new User(_name, _email, _password)));

        private IObservable<bool> CanExecuteEnterCommand() 
            => this.WhenAnyValue(_ => _.Email, _ => _.Password)
                            .Select((_, _) => SimpleValidator.IsValid(new SingInModel(_email, _password)));

        private void ClearParameters()
        {
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }

        #region NavigationMethods
        private void NavigateToEnter()
        {
            ClearParameters();
            _regionManager.RequestNavigate(MainContentFrame, nameof(EnterView));
        }

        private void NavigateToCreationNewUser()
        {
            ClearParameters();
            _regionManager.RequestNavigate(MainContentFrame, nameof(CreationNewUserView));
        }

        private void NavigateToUserSpace()
            => _regionManager.RequestNavigate(MainContentFrame, nameof(UserSpaceView));
        #endregion
    }
}
