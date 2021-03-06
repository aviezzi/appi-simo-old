namespace AppiSimo.Client.Pages.CurrentUserBadge
{
    using System;
    using System.Threading.Tasks;
    using Factories.Abstract;
    using Microsoft.AspNetCore.Blazor.Components;
    using Microsoft.AspNetCore.Blazor.Services;
    using Shared.Services.Abstract;

    public class CurrentUserBadgeComponent : BlazorComponent, IDisposable
    {
        IDisposable _subscription;

        [Inject]
        IAsyncFactory<IAuthService> AuthFactory { get; set; }

        [Inject]
        IUriHelper UriHelper { get; set; }

        IAuthService Auth { get; set; }

        protected CurrentUserBadgeViewModel ViewModel { get; } = new CurrentUserBadgeViewModel();

        protected override async Task OnInitAsync()
        {
            Auth = await AuthFactory.CreateAsync();

            ViewModel.IsLogged = Auth.CurrentUser != null;
            ViewModel.CurrentUser = Auth?.CurrentUser?.Profile?.Email ?? "User";

            _subscription =
                Auth.User.Subscribe(user =>
                {
                    ViewModel.CurrentUser = user?.Profile.Email ?? "User";
                    ViewModel.IsLogged = user != null;

                    StateHasChanged();
                });
        }

        protected async Task SignIn() => await Auth.SignIn();

        protected void SignOut() => Auth.SignOut();

        protected void GoToDetail() => UriHelper.NavigateTo($"/user/{Auth.CurrentUser.Profile.Sub}");

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}