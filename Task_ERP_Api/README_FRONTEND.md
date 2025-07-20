# 🚀 Task ERP API - دليل ربط الفرونت (Blazor)

## 🔗 Base URL

```
https://localhost:7001/api
```

## 📋 Headers

```http
Content-Type: application/json
Accept: application/json
```

---

## 📚 توثيق الـ Endpoints (CRUD)

### 1. الحسابات (Account)

- `GET    /api/Account` ← كل الحسابات
- `GET    /api/Account/{id}`← حساب محدد
- `POST   /api/Account` ← إضافة حساب
- `PUT    /api/Account/{id}` ← تحديث حساب
- `DELETE /api/Account/{id}` ← حذف حساب

**مثال Body (إنشاء):**

```json
{
  "accountNumber": "ACC001",
  "accountNameAr": "حساب تجريبي",
  "accountNameEn": "Test Account",
  "branchID": 1,
  "isActive": true
}
```

---

### 2. الفروع (Branch)

- `GET    /api/Branch`
- `GET    /api/Branch/{id}`
- `POST   /api/Branch`
- `PUT    /api/Branch/{id}`
- `DELETE /api/Branch/{id}`

**مثال Body (إنشاء):**

```json
{
  "branchNameAr": "فرع تجريبي",
  "branchNameEn": "Test Branch",
  "isActive": true
}
```

---

### 3. المدن (City)

- `GET    /api/City`
- `GET    /api/City/{id}`
- `POST   /api/City`
- `PUT    /api/City/{id}`
- `DELETE /api/City/{id}`

**مثال Body (إنشاء):**

```json
{
  "cityNameAr": "مدينة تجريبية",
  "cityNameEn": "Test City",
  "branchID": 1,
  "isActive": true
}
```

---

### 4. القيود اليومية (JV)

- `GET    /api/JV`
- `GET    /api/JV/{id}`
- `POST   /api/JV`
- `PUT    /api/JV/{id}`
- `DELETE /api/JV/{id}`

**مثال Body (إنشاء):**

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

---

### 5. تفاصيل القيود (JVDetail)

- `GET    /api/JVDetail`
- `GET    /api/JVDetail/{id}`
- `POST   /api/JVDetail`
- `PUT    /api/JVDetail/{id}`
- `DELETE /api/JVDetail/{id}`

**مثال Body (إنشاء):**

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

---

### 6. الحسابات الفرعية (SubAccount)

- `GET    /api/SubAccount`
- `GET    /api/SubAccount/{id}`
- `POST   /api/SubAccount`
- `PUT    /api/SubAccount/{id}`
- `DELETE /api/SubAccount/{id}`

**مثال Body (إنشاء):**

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

---

### 7. أنواع الحسابات الفرعية (SubAccountsType)

- `GET    /api/SubAccountsType`
- `GET    /api/SubAccountsType/{id}`
- `POST   /api/SubAccountsType`
- `PUT    /api/SubAccountsType/{id}`
- `DELETE /api/SubAccountsType/{id}`

**مثال Body (إنشاء):**

```json
{
  "typeNameAr": "نوع حساب فرعي",
  "typeNameEn": "Sub Account Type",
  "isActive": true
}
```

---

### 8. مستويات الحسابات الفرعية (SubAccounts_Level)

- `GET    /api/SubAccounts_Level`
- `GET    /api/SubAccounts_Level/{id}`
- `POST   /api/SubAccounts_Level`
- `PUT    /api/SubAccounts_Level/{id}`
- `DELETE /api/SubAccounts_Level/{id}`

**مثال Body (إنشاء):**

```json
{
  "levelNameAr": "مستوى 1",
  "levelNameEn": "Level 1",
  "isActive": true
}
```

---

### 9. تفاصيل الحسابات الفرعية (SubAccounts_Detail)

- `GET    /api/SubAccounts_Detail`
- `GET    /api/SubAccounts_Detail/{id}`
- `POST   /api/SubAccounts_Detail`
- `PUT    /api/SubAccounts_Detail/{id}`
- `DELETE /api/SubAccounts_Detail/{id}`

**مثال Body (إنشاء):**

```json
{
  "subAccountID": 1,
  "detailNameAr": "تفصيل",
  "detailNameEn": "Detail",
  "isActive": true
}
```

---

### 10. عملاء الحسابات الفرعية (SubAccountsClient)

- `GET    /api/SubAccountsClient`
- `GET    /api/SubAccountsClient/{id}`
- `POST   /api/SubAccountsClient`
- `PUT    /api/SubAccountsClient/{id}`
- `DELETE /api/SubAccountsClient/{id}`

**مثال Body (إنشاء):**

```json
{
  "clientNameAr": "عميل تجريبي",
  "clientNameEn": "Test Client",
  "subAccountID": 1,
  "isActive": true
}
```

---

## 💻 مثال Service في Blazor

```csharp
public class ApiService
{
    private readonly HttpClient _httpClient;
    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7001/api");
    }
    public async Task<List<T>> GetAllAsync<T>(string endpoint) =>
        await _httpClient.GetFromJsonAsync<List<T>>(endpoint);
    public async Task<T> GetByIdAsync<T>(string endpoint, int id) =>
        await _httpClient.GetFromJsonAsync<T>($"{endpoint}/{id}");
    public async Task<T> CreateAsync<T>(string endpoint, T data) =>
        (await _httpClient.PostAsJsonAsync(endpoint, data)).Content.ReadFromJsonAsync<T>().Result;
    public async Task<T> UpdateAsync<T>(string endpoint, int id, T data) =>
        (await _httpClient.PutAsJsonAsync($"{endpoint}/{id}", data)).Content.ReadFromJsonAsync<T>().Result;
    public async Task<bool> DeleteAsync(string endpoint, int id) =>
        (await _httpClient.DeleteAsync($"{endpoint}/{id}")).IsSuccessStatusCode;
}
```

## ⚡ ملاحظات هامة

- كل Endpoint له نفس نمط CRUD.
- كل الكيانات لها DTOs واضحة (انظر أمثلة JSON أعلاه).
- الأخطاء ترجع بصيغة JSON مع رسالة واضحة.
- جرب كل شيء عبر Swagger أو ملفات HTTP المرفقة.

---

**يكفي نسخ الكود أعلاه وبدء الربط مباشرة في Blazor أو أي فرونت آخر.**
