using System.Net.Http.Json;
using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public class JVDetailService : IJVDetailService
    {
        private readonly HttpClient _httpClient;

        public JVDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<JVDetail>> GetAllJVDetailsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/JVDetail");
                if (response.IsSuccessStatusCode)
                {
                    var details = await response.Content.ReadFromJsonAsync<List<JVDetail>>();
                    return details ?? new List<JVDetail>();
                }
                else
                {
                    Console.WriteLine($"API returned status code: {response.StatusCode} for JV details");
                    return new List<JVDetail>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching JV details: {ex.Message}");
                return new List<JVDetail>();
            }
        }

        public async Task<JVDetail?> GetJVDetailByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/JVDetail/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<JVDetail>();
                }
                else
                {
                    Console.WriteLine($"API returned status code: {response.StatusCode} for JV detail {id}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching JV detail {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<JVDetail> CreateJVDetailAsync(JVDetail jvDetail)
        {
            try
            {
                // استخدام DTO للتنسيق الصحيح - يطابق Curl بالضبط
                var jvDetailDto = new JVDetailDto
                {
                    jVID = jvDetail.JVID,
                    accountID = jvDetail.AccountID,
                    subAccountID = jvDetail.SubAccountID,
                    debit = jvDetail.Debit,
                    credit = jvDetail.Credit,
                    isDocumented = jvDetail.IsDocumented,
                    notes = jvDetail.Notes ?? "",
                    branchID = jvDetail.BranchID ?? 1
                };

                Console.WriteLine($"Creating JV Detail: {System.Text.Json.JsonSerializer.Serialize(jvDetailDto)}");

                var response = await _httpClient.PostAsJsonAsync("api/JVDetail", jvDetailDto);
                
                if (response.IsSuccessStatusCode)
                {
                    var createdDetail = await response.Content.ReadFromJsonAsync<JVDetailResponse>();
                    if (createdDetail != null)
                    {
                        return new JVDetail
                        {
                            JVDetailID = createdDetail.jvDetailID,
                            JVID = createdDetail.jvid,
                            AccountID = createdDetail.accountID,
                            SubAccountID = createdDetail.subAccountID,
                            Debit = createdDetail.debit,
                            Credit = createdDetail.credit,
                            IsDocumented = createdDetail.isDocumented,
                            Notes = createdDetail.notes,
                            BranchID = createdDetail.branchID
                        };
                    }
                    return jvDetail;
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
                Console.WriteLine($"Error creating JV detail: {ex.Message}");
                throw;
            }
        }

        public async Task<JVDetail> UpdateJVDetailAsync(int id, JVDetail jvDetail)
        {
            try
            {
                var cleanDetail = new
                {
                    jVID = jvDetail.JVID,
                    accountID = jvDetail.AccountID,
                    subAccountID = jvDetail.SubAccountID,
                    debit = jvDetail.Debit,
                    credit = jvDetail.Credit,
                    isDocumented = jvDetail.IsDocumented,
                    notes = jvDetail.Notes ?? "",
                    branchID = jvDetail.BranchID ?? 1
                };

                var response = await _httpClient.PutAsJsonAsync($"api/JVDetail/{id}", cleanDetail);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<JVDetail>() ?? jvDetail;
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
                Console.WriteLine($"Error updating JV detail {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteJVDetailAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/JVDetail/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting JV detail {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<JVDetail>> GetJVDetailsByJVIdAsync(int jvId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/JVDetail/jv/{jvId}");
                if (response.IsSuccessStatusCode)
                {
                    var details = await response.Content.ReadFromJsonAsync<List<JVDetail>>();
                    return details ?? new List<JVDetail>();
                }
                else
                {
                    Console.WriteLine($"API returned status code: {response.StatusCode} for JV details of JV {jvId}");
                    return new List<JVDetail>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching JV details for JV {jvId}: {ex.Message}");
                return new List<JVDetail>();
            }
        }

        public async Task<bool> CreateJVDetailAsync(int jvid, int accountID, int? subAccountID, decimal? debit, decimal? credit, bool isDocumented, string notes, int branchID)
        {
            try
            {
                var jvDetailDto = new JVDetailDto
                {
                    jVID = jvid,
                    accountID = accountID,
                    subAccountID = subAccountID,
                    debit = debit,
                    credit = credit,
                    isDocumented = isDocumented,
                    notes = notes,
                    branchID = branchID
                };

                Console.WriteLine($"Creating JV Detail: {System.Text.Json.JsonSerializer.Serialize(jvDetailDto)}");

                var response = await _httpClient.PostAsJsonAsync("api/JVDetail", jvDetailDto);
                
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"JV Detail created successfully");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating JV detail: {ex.Message}");
                return false;
            }
        }
    }
} 