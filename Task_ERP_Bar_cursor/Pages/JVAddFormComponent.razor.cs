using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Task_ERP_Bar.Models;
using Task_ERP_Bar.Services;
using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Pages
{
    public partial class JVAddFormComponent : ComponentBase
    {
        [Inject] private IJVService JVService { get; set; } = default!;
        [Inject] private IJVDetailService JVDetailService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

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

        // Validation properties
        public bool IsDateValid => currentJV.JVDate != default;
        public bool IsTypeValid => currentJV.JVTypeID > 0;
        public bool IsBranchValid => currentJV.BranchID > 0;
        
        public bool IsAccountValid(int index) => index >= 0 && index < jvDetails.Count && jvDetails[index].AccountID > 0;
        public bool IsAmountValid(int index) => index >= 0 && index < jvDetails.Count && 
            ((jvDetails[index].Debit ?? 0) > 0 || (jvDetails[index].Credit ?? 0) > 0) &&
            !((jvDetails[index].Debit ?? 0) > 0 && (jvDetails[index].Credit ?? 0) > 0);
        
        public bool IsDetailRowValid(int index) => IsAccountValid(index) && IsAmountValid(index);

        protected override async Task OnInitializedAsync()
        {
            await LoadDropdownDataAsync();
            InitializeNewJV();
            editContext = new EditContext(currentJV);
        }

        private async Task LoadDropdownDataAsync()
        {
            try
            {
                isLoading = true;
                StateHasChanged();

                Console.WriteLine("Starting to load dropdown data...");

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

                Console.WriteLine($"Loaded {accounts.Count} accounts, {subAccounts.Count} sub-accounts, {jvTypes.Count} JV types, {branches.Count} branches");

                // Set branch name dynamically
                var firstBranch = branches.FirstOrDefault();
                if (firstBranch != null)
                {
                    branchName = selectedLanguage == "Arabic" ? firstBranch.BranchNameAr : firstBranch.BranchNameEn ?? firstBranch.BranchNameAr;
                    Console.WriteLine($"Set branch name to: {branchName}");
                }
                else
                {
                    Console.WriteLine("No branches found, keeping default branch name");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadDropdownDataAsync: {ex.Message}");
                Console.WriteLine($"Full exception: {ex}");
                errorMessage = $"Error loading data: {ex.Message}";
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private void InitializeNewJV()
        {
            currentJV = new JV
            {
                JVDate = DateTime.Today,
                BranchID = branches.FirstOrDefault()?.BranchID, // Use first available branch
                JVDetails = new List<JVDetail>()
            };

            jvDetails = new List<JVDetail>();
            AddNewDetailRow();
        }

        private void AddNewDetailRow()
        {
            var newDetail = new JVDetail
            {
                JVDetailID = jvDetails.Count + 1,
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

        private async Task SaveAsync()
        {
            try
            {
                if (!ValidateForm())
                    return;

                isLoading = true;
                errorMessage = string.Empty;
                successMessage = string.Empty;
                StateHasChanged();

                Console.WriteLine("Preparing JV data for saving...");

                // Generate JVNumber before saving
                currentJV.JVNumber = await JVService.GenerateJVNumberAsync();
                currentJV.JVID = 0;
                currentJV.TotalDebit = TotalDebit;
                currentJV.TotalCredit = TotalCredit;
                
                // Ensure branch ID is set
                if (!currentJV.BranchID.HasValue && branches.Any())
                {
                    currentJV.BranchID = branches.First().BranchID;
                }

                Console.WriteLine($"Saving JV: {currentJV.JVNumber}, Details: {jvDetails.Count(d => d.AccountID > 0)}");

                // إنشاء JV بدون تفاصيل أولاً
                var savedJV = await JVService.CreateJVWithoutDetailsAsync(currentJV);
                Console.WriteLine($"JV created successfully: {savedJV.JVID}");

                // إضافة التفاصيل باستخدام التنسيق الصحيح
                var validDetails = jvDetails.Where(d => d.AccountID > 0).ToList();
                if (validDetails.Any())
                {
                    Console.WriteLine($"Adding {validDetails.Count} details...");
                    
                    foreach (var detail in validDetails)
                    {
                        var success = await JVDetailService.CreateJVDetailAsync(
                            jvid: savedJV.JVID,
                            accountID: detail.AccountID,
                            subAccountID: detail.SubAccountID,
                            debit: detail.Debit,
                            credit: detail.Credit,
                            isDocumented: detail.IsDocumented,
                            notes: detail.Notes ?? "",
                            branchID: detail.BranchID ?? savedJV.BranchID ?? 1
                        );

                        if (!success)
                        {
                            Console.WriteLine($"Failed to add detail for account {detail.AccountID}");
                        }
                    }
                }

                successMessage = $"تم حفظ القيد {savedJV.JVNumber} بنجاح!";

                Console.WriteLine($"JV saved successfully: {savedJV.JVID}");

                await Task.Delay(1500);
                NavigationManager.NavigateTo("/JVFormComponent");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SaveAsync: {ex.Message}");
                errorMessage = $"خطأ في حفظ القيد: {ex.Message}";
                
                // إذا كان الخطأ من الـ API، أضف تفاصيل أكثر
                if (ex.Message.Contains("400") || ex.Message.Contains("Bad Request"))
                {
                    errorMessage += "\nيرجى التحقق من صحة البيانات المدخلة";
                }
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private async Task SaveAndCloseAsync()
        {
            await SaveAsync();
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
            InitializeNewJV();
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

        private void ChangeLanguage(string language)
        {
            selectedLanguage = language;
            
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
            return selectedLanguage == "Arabic" ? account.AccountNameAr : account.AccountNameEn ?? "";
        }

        private string GetSubAccountDisplayName(SubAccount subAccount)
        {
            return selectedLanguage == "Arabic" ? subAccount.SubAccountNameAr : subAccount.SubAccountNameEn ?? "";
        }


    }
} 