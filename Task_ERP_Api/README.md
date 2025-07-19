# 🚀 Task ERP API - Frontend Integration Guide

## 🔗 الـ Base URL

```
https://localhost:7001/api
```

## 📋 الـ Headers المطلوبة

```http
Content-Type: application/json
Accept: application/json
```

## ⚠️ الـ Error Handling

- `200` - Success
- `201` - Created
- `400` - Bad Request
- `404` - Not Found
- `500` - Internal Server Error

## 🔌 الـ Endpoints الأساسية

### 1. 📊 Account Management

```http
GET    /api/Account          # جلب جميع الحسابات
GET    /api/Account/{id}     # جلب حساب محدد
POST   /api/Account          # إنشاء حساب جديد
PUT    /api/Account/{id}     # تحديث حساب
DELETE /api/Account/{id}     # حذف حساب
```

**Request Body (Create/Update):**

```json
{
  "accountNumber": "ACC001",
  "accountNameAr": "حساب تجريبي",
  "accountNameEn": "Test Account",
  "branchID": 1,
  "isActive": true
}
```

### 2. 🏢 Branch Management

```http
GET    /api/Branch           # جلب جميع الفروع
GET    /api/Branch/{id}      # جلب فرع محدد
POST   /api/Branch           # إنشاء فرع جديد
PUT    /api/Branch/{id}      # تحديث فرع
DELETE /api/Branch/{id}      # حذف فرع
```

**Request Body:**

```json
{
  "branchNameAr": "فرع تجريبي",
  "branchNameEn": "Test Branch",
  "isActive": true
}
```

### 3. 🏙️ City Management

```http
GET    /api/City             # جلب جميع المدن
GET    /api/City/{id}        # جلب مدينة محددة
POST   /api/City             # إنشاء مدينة جديدة
PUT    /api/City/{id}        # تحديث مدينة
DELETE /api/City/{id}        # حذف مدينة
```

**Request Body:**

```json
{
  "cityNameAr": "مدينة تجريبية",
  "cityNameEn": "Test City",
  "branchID": 1,
  "isActive": true
}
```

### 4. 📝 Journal Voucher (JV)

```http
GET    /api/JV               # جلب جميع القيود
GET    /api/JV/{id}          # جلب قيد محدد
POST   /api/JV               # إنشاء قيد جديد
PUT    /api/JV/{id}          # تحديث قيد
DELETE /api/JV/{id}          # حذف قيد
```

**Request Body:**

```json
{
  "jvNumber": "JV001",
  "jvDate": "2024-01-15",
  "description": "Test Journal Voucher",
  "branchID": 1,
  "jvTypeID": 1,
  "isActive": true
}
```

### 5. 📋 JV Detail

```http
GET    /api/JVDetail         # جلب جميع تفاصيل القيود
GET    /api/JVDetail/{id}    # جلب تفصيل محدد
POST   /api/JVDetail         # إنشاء تفصيل جديد
PUT    /api/JVDetail/{id}    # تحديث تفصيل
DELETE /api/JVDetail/{id}    # حذف تفصيل
```

**Request Body:**

```json
{
  "jvID": 1,
  "accountID": 1,
  "subAccountID": 1,
  "branchID": 1,
  "debitAmount": 1000.0,
  "creditAmount": 0.0,
  "description": "Test JV Detail"
}
```

### 6. 💼 Sub Account

```http
GET    /api/SubAccount       # جلب جميع الحسابات الفرعية
GET    /api/SubAccount/{id}  # جلب حساب فرعي محدد
POST   /api/SubAccount       # إنشاء حساب فرعي جديد
PUT    /api/SubAccount/{id}  # تحديث حساب فرعي
DELETE /api/SubAccount/{id}  # حذف حساب فرعي
```

**Request Body:**

```json
{
  "subAccountNumber": "SUB001",
  "subAccountNameAr": "حساب فرعي تجريبي",
  "subAccountNameEn": "Test Sub Account",
  "branchID": 1,
  "levelID": 1,
  "parentID": null,
  "subAccountTypeID": 1,
  "isActive": true
}
```

## 💻 أمثلة الاستخدام في Blazor

### 1. إنشاء Service للـ HTTP Calls

```csharp
public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://localhost:7001/api";

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_baseUrl);
    }

    // Generic methods
    public async Task<List<T>> GetAllAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<T>>();
    }

    public async Task<T> GetByIdAsync<T>(string endpoint, int id)
    {
        var response = await _httpClient.GetAsync($"{endpoint}/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> CreateAsync<T>(string endpoint, T data)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> UpdateAsync<T>(string endpoint, int id, T data)
    {
        var response = await _httpClient.PutAsJsonAsync($"{endpoint}/{id}", data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<bool> DeleteAsync(string endpoint, int id)
    {
        var response = await _httpClient.DeleteAsync($"{endpoint}/{id}");
        return response.IsSuccessStatusCode;
    }
}
```

### 2. استخدام Service في Blazor Component

```csharp
@page "/accounts"
@inject ApiService ApiService

<h3>إدارة الحسابات</h3>

@if (accounts == null)
{
    <p>جاري التحميل...</p>
}
else
{
    <table class="table">
        <thead>
            <tr>
                <th>رقم الحساب</th>
                <th>اسم الحساب</th>
                <th>الفرع</th>
                <th>الإجراءات</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var account in accounts)
            {
                <tr>
                    <td>@account.AccountNumber</td>
                    <td>@account.AccountNameAr</td>
                    <td>@account.Branch?.BranchNameAr</td>
                    <td>
                        <button class="btn btn-primary btn-sm" @onclick="() => EditAccount(account)">تعديل</button>
                        <button class="btn btn-danger btn-sm" @onclick="() => DeleteAccount(account.AccountID)">حذف</button>
                    </td>
                </tr>
            }
        </tbody>
    </table>
}

@code {
    private List<Account> accounts;

    protected override async Task OnInitializedAsync()
    {
        await LoadAccounts();
    }

    private async Task LoadAccounts()
    {
        try
        {
            accounts = await ApiService.GetAllAsync<Account>("Account");
        }
        catch (Exception ex)
        {
            // Handle error
        }
    }

    private async Task DeleteAccount(int id)
    {
        try
        {
            var success = await ApiService.DeleteAsync("Account", id);
            if (success)
            {
                await LoadAccounts();
            }
        }
        catch (Exception ex)
        {
            // Handle error
        }
    }
}
```

### 3. نموذج إنشاء/تعديل

```csharp
@page "/account-form"
@inject ApiService ApiService
@inject NavigationManager Navigation

<EditForm Model="@accountModel" OnValidSubmit="HandleValidSubmit">
    <DataAnnotationsValidator />

    <div class="form-group">
        <label>رقم الحساب</label>
        <InputText @bind-Value="accountModel.AccountNumber" class="form-control" />
        <ValidationMessage For="@(() => accountModel.AccountNumber)" />
    </div>

    <div class="form-group">
        <label>اسم الحساب (عربي)</label>
        <InputText @bind-Value="accountModel.AccountNameAr" class="form-control" />
        <ValidationMessage For="@(() => accountModel.AccountNameAr)" />
    </div>

    <div class="form-group">
        <label>الفرع</label>
        <InputSelect @bind-Value="accountModel.BranchID" class="form-control">
            @foreach (var branch in branches)
            {
                <option value="@branch.BranchID">@branch.BranchNameAr</option>
            }
        </InputSelect>
    </div>

    <button type="submit" class="btn btn-primary">حفظ</button>
</EditForm>

@code {
    private CreateAccountRequest accountModel = new();
    private List<Branch> branches = new();

    protected override async Task OnInitializedAsync()
    {
        branches = await ApiService.GetAllAsync<Branch>("Branch");
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            var result = await ApiService.CreateAsync("Account", accountModel);
            Navigation.NavigateTo("/accounts");
        }
        catch (Exception ex)
        {
            // Handle error
        }
    }
}
```

## 📋 الـ Models الأساسية

```csharp
public class Account
{
    public int AccountID { get; set; }
    public string AccountNumber { get; set; }
    public string AccountNameAr { get; set; }
    public string AccountNameEn { get; set; }
    public int? BranchID { get; set; }
    public bool IsActive { get; set; }
    public Branch Branch { get; set; }
}

public class CreateAccountRequest
{
    [Required]
    public string AccountNumber { get; set; }

    [Required]
    public string AccountNameAr { get; set; }

    public string AccountNameEn { get; set; }

    [Required]
    public int BranchID { get; set; }

    public bool IsActive { get; set; } = true;
}
```

## 🚀 نصائح سريعة

1. **استخدم Dependency Injection** لتسجيل الـ services
2. **أضف Error Handling** شامل
3. **استخدم Loading States** لتحسين UX
4. **أضف Validation** على الـ client side
5. **استخدم Caching** للبيانات المتكررة

---

## 📞 الدعم

لأي استفسارات، يرجى التواصل مع فريق التطوير.
