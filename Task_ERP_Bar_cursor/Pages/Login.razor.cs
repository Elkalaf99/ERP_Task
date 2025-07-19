using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Pages
{
    public partial class Login
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsLoading { get; set; } = false;

        protected async Task HandleLogin()
        {
            ErrorMessage = string.Empty;
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(500); // Simulate async login

            // Mock authentication: username = admin, password = 1234
            if (Username == "admin" && Password == "1234")
            {
                NavigationManager.NavigateTo("/JVFormComponent");
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
            }
            IsLoading = false;
            StateHasChanged();
        }
    }
} 