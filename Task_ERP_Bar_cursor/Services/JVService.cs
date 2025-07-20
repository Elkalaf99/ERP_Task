using System.Net.Http.Json;
using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public class JVService : IJVService
    {
        private readonly HttpClient _httpClient;

        public JVService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<JV>> GetAllJVsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("jv");
                if (response.IsSuccessStatusCode)
                {
                    var jvs = await response.Content.ReadFromJsonAsync<List<JV>>();
                    return jvs ?? new List<JV>();
                }
                else
                {
                    Console.WriteLine($"API returned status code: {response.StatusCode}");
                    return new List<JV>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching JVs: {ex.Message}");
                return new List<JV>();
            }
        }

        public async Task<JV?> GetJVByIdAsync(int jvId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"jv/{jvId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<JV>();
                }
                else
                {
                    Console.WriteLine($"API returned status code: {response.StatusCode} for JV {jvId}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching JV {jvId}: {ex.Message}");
                return null;
            }
        }

        public async Task<JV> CreateJVAsync(JV jv)
        {
            try
            {
                // استخدام DTO للتنسيق الصحيح - يطابق الباك إند بالضبط
                var jvDto = new JVDto
                {
                    jvID = 0,
                    jvNo = jv.JVNumber,
                    jvDate = jv.JVDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    jvTypeID = jv.JVTypeID,
                    totalDebit = jv.TotalDebit,
                    totalCredit = jv.TotalCredit,
                    receiptNo = jv.ReceiptNo ?? "",
                    notes = jv.Notes ?? "",
                    branchID = jv.BranchID,
                    jvDetails = new List<object>()
                };

                Console.WriteLine($"Creating JV: {jv.JVNumber}");
                Console.WriteLine($"JV DTO: {System.Text.Json.JsonSerializer.Serialize(jvDto)}");
                
                var response = await _httpClient.PostAsJsonAsync("api/jv", jvDto);
                
                if (response.IsSuccessStatusCode)
                {
                    var createdJVResponse = await response.Content.ReadFromJsonAsync<JVResponse>();
                    if (createdJVResponse != null)
                    {
                        var createdJV = new JV
                        {
                            JVID = createdJVResponse.jvID,
                            JVNumber = createdJVResponse.jvNo,
                            JVDate = createdJVResponse.jvDate,
                            JVTypeID = createdJVResponse.jvTypeID,
                            TotalDebit = createdJVResponse.totalDebit,
                            TotalCredit = createdJVResponse.totalCredit,
                            ReceiptNo = createdJVResponse.receiptNo,
                            Notes = createdJVResponse.notes,
                            BranchID = createdJVResponse.branchID
                        };
                        
                        Console.WriteLine($"JV created successfully: {createdJV.JVID}");
                        
                        // إضافة التفاصيل لاحقاً إذا كان هناك تفاصيل
                        if (jv.JVDetails?.Any() == true)
                        {
                            await AddJVDetailsAsync(createdJV.JVID, jv.JVDetails);
                        }
                        
                        return createdJV;
                    }
                    return jv;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                    throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating JV: {ex.Message}");
                throw;
            }
        }

        public async Task<JV> UpdateJVAsync(JV jv)
        {
            try
            {
                // تنظيف البيانات قبل الإرسال
                var cleanJV = new
                {
                    jvID = jv.JVID,
                    jvNo = jv.JVNumber,
                    jvDate = jv.JVDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    jvTypeID = jv.JVTypeID,
                    totalDebit = jv.TotalDebit,
                    totalCredit = jv.TotalCredit,
                    receiptNo = jv.ReceiptNo,
                    notes = jv.Notes,
                    branchID = jv.BranchID,
                    jvDetails = jv.JVDetails?.Where(d => d.AccountID > 0).Select(d => new
                    {
                        accountID = d.AccountID,
                        subAccountID = d.SubAccountID,
                        debit = d.Debit,
                        credit = d.Credit,
                        isDocumented = d.IsDocumented,
                        notes = d.Notes ?? "",
                        branchID = d.BranchID ?? jv.BranchID ?? 1
                    }).ToList()
                };

                var response = await _httpClient.PutAsJsonAsync($"api/jv/{jv.JVID}", cleanJV);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<JV>() ?? jv;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                    throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating JV {jv.JVID}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteJVAsync(int jvId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/jv/{jvId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting JV {jvId}: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GenerateJVNumberAsync()
        {
            try
            {
                // محاولة الحصول على رقم من الباك إند أولاً
                var response = await _httpClient.GetAsync("api/jv/generate-number");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                
                // إذا لم يكن الـ endpoint موجود، نولد الرقم محلياً
                Console.WriteLine("Generate number endpoint not found, generating locally");
                var year = DateTime.Now.Year;
                var jvs = await GetAllJVsAsync();
                var nextNumber = jvs.Count + 1;
                return $"JV-{year}-{nextNumber:D3}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating JV number: {ex.Message}");
                // توليد رقم افتراضي في حالة الخطأ
                var year = DateTime.Now.Year;
                var timestamp = DateTime.Now.ToString("HHmmss");
                return $"JV-{year}-{timestamp}";
            }
        }

        public async Task<List<Account>> GetAccountsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/account");
                if (response.IsSuccessStatusCode)
                {
                    var accounts = await response.Content.ReadFromJsonAsync<List<Account>>();
                    return accounts ?? new List<Account>();
                }
                else
                {
                    Console.WriteLine($"API returned status code: {response.StatusCode} for accounts");
                    return new List<Account>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching accounts: {ex.Message}");
                return new List<Account>();
            }
        }

        public async Task<List<SubAccount>> GetSubAccountsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/subaccount");
                if (response.IsSuccessStatusCode)
                {
                    var subAccounts = await response.Content.ReadFromJsonAsync<List<SubAccount>>();
                    return subAccounts ?? new List<SubAccount>();
                }
                else
                {
                    Console.WriteLine($"API returned status code: {response.StatusCode} for sub-accounts");
                    return new List<SubAccount>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sub-accounts: {ex.Message}");
                return new List<SubAccount>();
            }
        }

        public async Task<List<JVType>> GetJVTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/jvtype");
                if (response.IsSuccessStatusCode)
                {
                    var jvTypes = await response.Content.ReadFromJsonAsync<List<JVType>>();
                    return jvTypes ?? new List<JVType>();
                }
                else
                {
                    Console.WriteLine($"API returned status code: {response.StatusCode} for JV types");
                    return new List<JVType>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching JV types: {ex.Message}");
                return new List<JVType>();
            }
        }

        public async Task<List<Branch>> GetBranchesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/branch");
                if (response.IsSuccessStatusCode)
                {
                    var branches = await response.Content.ReadFromJsonAsync<List<Branch>>();
                    return branches ?? new List<Branch>();
                }
                else
                {
                    Console.WriteLine($"API returned status code: {response.StatusCode} for branches");
                    return new List<Branch>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching branches: {ex.Message}");
                return new List<Branch>();
            }
        }

        public async Task<JV> CreateJVWithoutDetailsAsync(JV jv)
        {
            try
            {
                // استخدام DTO للتنسيق الصحيح - يطابق الباك إند بالضبط
                var jvDto = new JVDto
                {
                    jvID = 0,
                    jvNo = jv.JVNumber,
                    jvDate = jv.JVDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    jvTypeID = jv.JVTypeID,
                    totalDebit = jv.TotalDebit,
                    totalCredit = jv.TotalCredit,
                    receiptNo = jv.ReceiptNo ?? "",
                    notes = jv.Notes ?? "",
                    branchID = jv.BranchID,
                    jvDetails = new List<object>()
                };

                Console.WriteLine($"Creating JV without details: {jv.JVNumber}");
                Console.WriteLine($"JV DTO: {System.Text.Json.JsonSerializer.Serialize(jvDto)}");
                
                var response = await _httpClient.PostAsJsonAsync("api/jv", jvDto);
                
                if (response.IsSuccessStatusCode)
                {
                    var createdJVResponse = await response.Content.ReadFromJsonAsync<JVResponse>();
                    if (createdJVResponse != null)
                    {
                        var createdJV = new JV
                        {
                            JVID = createdJVResponse.jvID,
                            JVNumber = createdJVResponse.jvNo,
                            JVDate = createdJVResponse.jvDate,
                            JVTypeID = createdJVResponse.jvTypeID,
                            TotalDebit = createdJVResponse.totalDebit,
                            TotalCredit = createdJVResponse.totalCredit,
                            ReceiptNo = createdJVResponse.receiptNo,
                            Notes = createdJVResponse.notes,
                            BranchID = createdJVResponse.branchID
                        };
                        
                        Console.WriteLine($"JV created successfully: {createdJV.JVID}");
                        return createdJV;
                    }
                    return jv;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                    throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating JV without details: {ex.Message}");
                throw;
            }
        }

        private async Task AddJVDetailsAsync(int jvId, List<JVDetail> details)
        {
            try
            {
                foreach (var detail in details.Where(d => d.AccountID > 0))
                {
                    var jvDetailDto = new JVDetailDto
                    {
                        jVID = jvId,
                        accountID = detail.AccountID,
                        subAccountID = detail.SubAccountID,
                        debit = detail.Debit,
                        credit = detail.Credit,
                        isDocumented = detail.IsDocumented,
                        notes = detail.Notes ?? "",
                        branchID = detail.BranchID ?? 1
                    };

                    Console.WriteLine($"Sending JV Detail: {System.Text.Json.JsonSerializer.Serialize(jvDetailDto)}");

                    var response = await _httpClient.PostAsJsonAsync("api/JVDetail", jvDetailDto);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"JV Detail added successfully for account {detail.AccountID}");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error adding JV detail: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding JV details: {ex.Message}");
            }
        }
    }
} 