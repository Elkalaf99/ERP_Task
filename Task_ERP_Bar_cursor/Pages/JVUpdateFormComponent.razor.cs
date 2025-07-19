using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Task_ERP_Bar.Models;
using Task_ERP_Bar.Services;
using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Pages
{
    public partial class JVUpdateFormComponent : ComponentBase
    {
        [Inject] private IJVService JVService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [Parameter] public string? Id { get; set; }

        // Form data
        private JV currentJV = new();
        private List<JVDetail> jvDetails = new();
        private EditContext editContext = default!;

        // Dropdown data
        private List<Account> accounts = new();
        private List<SubAccount> subAccounts = new();
        private List<JVType> jvTypes = new();
        private List<Branch> branches = new();

        // UI state
        private string selectedLanguage = "English";
        private string branchName = "Loading...";
        private string userName = "User";
        private bool isLoading = false;
        private string errorMessage = string.Empty;
        private string successMessage = string.Empty;

        // Calculated totals
        private decimal TotalDebit => jvDetails.Sum(d => d.Debit ?? 0);
        private decimal TotalCredit => jvDetails.Sum(d => d.Credit ?? 0);
        private decimal Difference => TotalDebit - TotalCredit;

        protected override async Task OnInitializedAsync()
        {
            await LoadDropdownDataAsync();
            await LoadJVAsync();
            editContext = new EditContext(currentJV);
        }

        private async Task LoadDropdownDataAsync()
        {
            try
            {
                isLoading = true;
                StateHasChanged();

                // Load all dropdown data in parallel
                var accountsTask = JVService.GetAccountsAsync();
                var subAccountsTask = JVService.GetSubAccountsAsync();
                var jvTypesTask = JVService.GetJVTypesAsync();
                var branchesTask = JVService.GetBranchesAsync();

                await Task.WhenAll(accountsTask, subAccountsTask, jvTypesTask, branchesTask);

                accounts = await accountsTask;
                subAccounts = await subAccountsTask;
                jvTypes = await jvTypesTask;
                branches = await branchesTask;

                // Set branch name dynamically
                var firstBranch = branches.FirstOrDefault();
                if (firstBranch != null)
                {
                    branchName = selectedLanguage == "Arabic" ? firstBranch.BranchNameAr : firstBranch.BranchNameEn ?? firstBranch.BranchNameAr;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading data: {ex.Message}";
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private async Task LoadJVAsync()
        {
            if (string.IsNullOrEmpty(Id) || !int.TryParse(Id, out int jvId))
            {
                errorMessage = "Invalid JV ID.";
                return;
            }

            try
            {
                isLoading = true;
                StateHasChanged();

                var jv = await JVService.GetJVByIdAsync(jvId);
                if (jv == null)
                {
                    errorMessage = "JV not found.";
                    return;
                }

                currentJV = jv;
                jvDetails = jv.JVDetails?.ToList() ?? new List<JVDetail>();

                // Ensure we have at least one detail row
                if (jvDetails.Count == 0)
                {
                    AddNewDetailRow();
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading JV: {ex.Message}";
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private void AddNewDetailRow()
        {
            var newDetail = new JVDetail
            {
                JVDetailID = jvDetails.Count + 1,
                JVID = currentJV.JVID,
                Debit = 0,
                Credit = 0,
                IsDocumented = false,
                BranchID = currentJV.BranchID
            };

            jvDetails.Add(newDetail);
            StateHasChanged();
        }

        private void RemoveDetailRow(int index)
        {
            if (index >= 0 && index < jvDetails.Count)
            {
                jvDetails.RemoveAt(index);
                // Reorder IDs
                for (int i = 0; i < jvDetails.Count; i++)
                {
                    jvDetails[i].JVDetailID = i + 1;
                }
                StateHasChanged();
            }
        }

        private void OnAccountChanged(int detailIndex, object value)
        {
            if (detailIndex >= 0 && detailIndex < jvDetails.Count && value != null && int.TryParse(value.ToString(), out int accountId))
            {
                jvDetails[detailIndex].AccountID = accountId;
                jvDetails[detailIndex].SubAccountID = null;
                StateHasChanged();
            }
        }

        private void OnSubAccountChanged(int detailIndex, object value)
        {
            if (detailIndex >= 0 && detailIndex < jvDetails.Count && value != null && int.TryParse(value.ToString(), out int subAccountId))
            {
                jvDetails[detailIndex].SubAccountID = subAccountId;
                StateHasChanged();
            }
        }

        private void OnDebitChanged(int detailIndex, object value)
        {
            if (detailIndex >= 0 && detailIndex < jvDetails.Count)
            {
                if (value != null && decimal.TryParse(value.ToString(), out decimal debit))
                {
                    jvDetails[detailIndex].Debit = debit;
                    if (debit > 0)
                    {
                        jvDetails[detailIndex].Credit = null;
                    }
                }
                else
                {
                    jvDetails[detailIndex].Debit = null;
                }
                StateHasChanged();
            }
        }

        private void OnCreditChanged(int detailIndex, object value)
        {
            if (detailIndex >= 0 && detailIndex < jvDetails.Count)
            {
                if (value != null && decimal.TryParse(value.ToString(), out decimal credit))
                {
                    jvDetails[detailIndex].Credit = credit;
                    if (credit > 0)
                    {
                        jvDetails[detailIndex].Debit = null;
                    }
                }
                else
                {
                    jvDetails[detailIndex].Credit = null;
                }
                StateHasChanged();
            }
        }

        private async Task UpdateAsync()
        {
            try
            {
                if (!ValidateForm())
                {
                    return;
                }

                isLoading = true;
                errorMessage = string.Empty;
                StateHasChanged();

                // Prepare JV for updating
                currentJV.JVDetails = jvDetails.Where(d => d.AccountID > 0).ToList();
                currentJV.TotalDebit = TotalDebit;
                currentJV.TotalCredit = TotalCredit;

                var updatedJV = await JVService.UpdateJVAsync(currentJV);
                successMessage = $"JV {updatedJV.JVNumber} updated successfully!";
                
                // Navigate to the main JV form after a short delay
                await Task.Delay(1500);
                NavigationManager.NavigateTo("/JVFormComponent");
            }
            catch (Exception ex)
            {
                errorMessage = $"Error updating JV: {ex.Message}";
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private async Task UpdateAndCloseAsync()
        {
            await UpdateAsync();
            if (string.IsNullOrEmpty(errorMessage))
            {
                NavigationManager.NavigateTo("/JVFormComponent");
            }
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/JVFormComponent");
        }

        private void Refresh()
        {
            _ = LoadJVAsync();
            errorMessage = string.Empty;
            successMessage = string.Empty;
            StateHasChanged();
        }

        private bool ValidateForm()
        {
            // Basic validation
            if (currentJV.JVDate == default)
            {
                errorMessage = "Please select a valid date.";
                return false;
            }

            if (currentJV.JVTypeID == null)
            {
                errorMessage = "Please select a transaction type.";
                return false;
            }

            if (jvDetails.Count == 0)
            {
                errorMessage = "Please add at least one detail row.";
                return false;
            }

            // Validate details
            var validDetails = jvDetails.Where(d => d.AccountID > 0).ToList();
            if (validDetails.Count == 0)
            {
                errorMessage = "Please select accounts for all detail rows.";
                return false;
            }

            // Check if debit equals credit
            if (Math.Abs(Difference) > 0.01m)
            {
                errorMessage = $"Total Debit ({TotalDebit:C}) must equal Total Credit ({TotalCredit:C}). Current difference: {Difference:C}";
                return false;
            }

            // Validate each detail row
            foreach (var detail in validDetails)
            {
                if (detail.AccountID == 0)
                {
                    errorMessage = "Please select an account for all detail rows.";
                    return false;
                }

                if ((detail.Debit ?? 0) == 0 && (detail.Credit ?? 0) == 0)
                {
                    errorMessage = "Please enter either a debit or credit amount for all detail rows.";
                    return false;
                }

                if ((detail.Debit ?? 0) > 0 && (detail.Credit ?? 0) > 0)
                {
                    errorMessage = "A detail row cannot have both debit and credit amounts.";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }

        private void ChangeLanguage(ChangeEventArgs e)
        {
            selectedLanguage = e.Value?.ToString() ?? "English";
            
            // Update branch name based on selected language
            var firstBranch = branches.FirstOrDefault();
            if (firstBranch != null)
            {
                branchName = selectedLanguage == "Arabic" ? firstBranch.BranchNameAr : firstBranch.BranchNameEn ?? firstBranch.BranchNameAr;
            }
            
            StateHasChanged();
        }

        private void Logout()
        {
            // Logout logic here
            NavigationManager.NavigateTo("/");
        }

        private string GetAccountDisplayName(Account account)
        {
            return selectedLanguage == "Arabic" ? account.AccountNameAr : account.AccountNameEn;
        }

        private string GetSubAccountDisplayName(SubAccount subAccount)
        {
            return selectedLanguage == "Arabic" ? subAccount.SubAccountNameAr : subAccount.SubAccountNameEn;
        }
    }
} 