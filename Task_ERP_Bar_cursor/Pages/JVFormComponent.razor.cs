using Microsoft.AspNetCore.Components;
using Task_ERP_Bar.Models;
using Task_ERP_Bar.Services;

namespace Task_ERP_Bar.Pages
{
    public partial class JVFormComponent : ComponentBase
    {
        [Inject] private IJVService JVService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        // Data
        private List<JV> jvs = new();
        private IEnumerable<JV> filteredJVs = new List<JV>();
        private JV? selectedJV;
        private List<JVType> jvTypes = new();
        private List<Branch> branches = new();

        // UI state
        private string selectedLanguage = "English";
        private string branchName = "Loading...";
        private string userName = "User";
        private bool isLoading = false;
        private string errorMessage = string.Empty;
        private string searchTerm = string.Empty;

        // Search and filter
        private DateTime? fromDate;
        private DateTime? toDate;
        private int? selectedJVTypeId;

        protected override async Task OnInitializedAsync()
        {
            await LoadDropdownDataAsync();
            await LoadJVsAsync();
        }

        private async Task LoadDropdownDataAsync()
        {
            try
            {
                // Load dropdown data in parallel
                var jvTypesTask = JVService.GetJVTypesAsync();
                var branchesTask = JVService.GetBranchesAsync();

                await Task.WhenAll(jvTypesTask, branchesTask);

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
                errorMessage = $"Error loading dropdown data: {ex.Message}";
            }
        }

        private async Task LoadJVsAsync()
        {
            try
            {
                isLoading = true;
                errorMessage = string.Empty;
                StateHasChanged();

                Console.WriteLine("Loading JVs from API...");
                jvs = await JVService.GetAllJVsAsync();
                Console.WriteLine($"Loaded {jvs.Count} JVs");
                
                ApplyFilters();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading JVs: {ex.Message}");
                errorMessage = $"خطأ في تحميل البيانات: {ex.Message}";
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private void ApplyFilters()
        {
            filteredJVs = jvs.AsEnumerable();

            // Apply date range filter
            if (fromDate.HasValue)
            {
                filteredJVs = filteredJVs.Where(j => j.JVDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                filteredJVs = filteredJVs.Where(j => j.JVDate <= toDate.Value);
            }

            // Apply JV type filter
            if (selectedJVTypeId.HasValue)
            {
                filteredJVs = filteredJVs.Where(j => j.JVTypeID == selectedJVTypeId.Value);
            }

            // Apply search term
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredJVs = filteredJVs.Where(j =>
                    j.JVNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    j.ReceiptNo?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true ||
                    j.Notes?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true
                );
            }

            filteredJVs = filteredJVs.ToList();
        }

        private void OnSearchChanged(ChangeEventArgs e)
        {
            searchTerm = e.Value?.ToString() ?? string.Empty;
            ApplyFilters();
            StateHasChanged();
        }

        private void OnFromDateChanged(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out var date))
            {
                fromDate = date;
            }
            else
            {
                fromDate = null;
            }
            ApplyFilters();
            StateHasChanged();
        }

        private void OnToDateChanged(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out var date))
            {
                toDate = date;
            }
            else
            {
                toDate = null;
            }
            ApplyFilters();
            StateHasChanged();
        }

        private void OnJVTypeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var jvTypeId))
            {
                selectedJVTypeId = jvTypeId;
            }
            else
            {
                selectedJVTypeId = null;
            }
            ApplyFilters();
            StateHasChanged();
        }

        private void ClearFilters()
        {
            searchTerm = string.Empty;
            fromDate = null;
            toDate = null;
            selectedJVTypeId = null;
            ApplyFilters();
            StateHasChanged();
        }

        private void SelectJV(JV jv)
        {
            selectedJV = jv;
            StateHasChanged();
        }

        private void NavigateToAdd()
        {
            NavigationManager.NavigateTo("/JVAddFormComponent");
        }

        private void NavigateToUpdate()
        {
            if (selectedJV != null)
            {
                NavigationManager.NavigateTo($"/JVUpdateFormComponent/{selectedJV.JVID}");
            }
            else
            {
                errorMessage = "Please select a JV to update.";
            }
        }

        private async Task DeleteSelectedJV()
        {
            if (selectedJV == null)
            {
                errorMessage = "Please select a JV to delete.";
                return;
            }

            try
            {
                isLoading = true;
                StateHasChanged();

                var success = await JVService.DeleteJVAsync(selectedJV.JVID);
                if (success)
                {
                    await LoadJVsAsync();
                    selectedJV = null;
                    errorMessage = string.Empty;
                }
                else
                {
                    errorMessage = "Failed to delete JV.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error deleting JV: {ex.Message}";
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private void PrintJV()
        {
            if (selectedJV != null)
            {
                // Implement print functionality
                // For now, just show a message
                errorMessage = "Print functionality not implemented yet.";
            }
            else
            {
                errorMessage = "Please select a JV to print.";
            }
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

        private string GetJVTypeName(int? jvTypeId)
        {
            if (!jvTypeId.HasValue) return "-";
            
            var jvType = jvTypes.FirstOrDefault(t => t.JVTypeID == jvTypeId.Value);
            if (jvType != null)
            {
                return selectedLanguage == "Arabic" ? jvType.JVTypeNameAr : jvType.JVTypeNameEn ?? jvType.JVTypeNameAr;
            }
            
            return "غير محدد";
        }

        private string GetFormattedAmount(decimal amount)
        {
            return amount.ToString("C");
        }

        private string GetRowClass(JV jv)
        {
            return selectedJV?.JVID == jv.JVID ? "table-active" : "";
        }
    }
} 