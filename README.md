# ERP Fullstack Blazor Project

## نظرة عامة على المشروع

هذا المشروع عبارة عن نظام إدارة موارد المؤسسة (ERP) مبني باستخدام:

- **Backend**: ASP.NET Core Web API
- **Frontend**: Blazor Server

## هيكل المشروع

```
ERP_Fullstack_Blazor/
├── Task_ERP_Api/          # Backend API
│   ├── Controllers/       # API Controllers
│   ├── DTOs/             # Data Transfer Objects
│   ├── Models/           # Entity Models
│   ├── Services/         # Business Logic Services
│   ├── Repositories/     # Data Access Layer
│   └── Mapper/           # AutoMapper Profiles
└── Task_ERP_Bar_cursor/  # Frontend Blazor
    ├── Pages/            # Blazor Pages
    ├── Services/         # Client-side Services
    ├── Models/           # Client Models
    └── wwwroot/          # Static Files
```

## الميزات الرئيسية

### Backend API

- **Account Management**: إدارة الحسابات
- **Branch Management**: إدارة الفروع
- **City Management**: إدارة المدن
- **JV (Journal Voucher) Management**: إدارة القيود اليومية
- **SubAccount Management**: إدارة الحسابات الفرعية
- **Generic Repository Pattern**: نمط المستودع العام
- **AutoMapper**: تحويل البيانات
- **Global Exception Handling**: معالجة الأخطاء العامة

### Frontend Blazor

- **Modern UI**: واجهة مستخدم حديثة
- **Responsive Design**: تصميم متجاوب
- **CRUD Operations**: عمليات إنشاء وقراءة وتحديث وحذف
- **Real-time Updates**: تحديثات فورية
- **Bootstrap Styling**: تنسيق باستخدام Bootstrap

## متطلبات النظام

- .NET 8.0 SDK أو أحدث
- Visual Studio 2022 أو Visual Studio Code
- SQL Server (LocalDB أو Express أو Full)

## كيفية التشغيل

### 1. تشغيل Backend API

```bash
cd Task_ERP_Api
dotnet restore
dotnet build
dotnet run
```

### 2. تشغيل Frontend Blazor

```bash
cd Task_ERP_Bar_cursor
dotnet restore
dotnet build
dotnet run
```

### 3. الوصول للتطبيق

- **API**: https://localhost:7001 (أو المنفذ المحدد)
- **Blazor App**: https://localhost:5001 (أو المنفذ المحدد)

## إعداد قاعدة البيانات

1. تأكد من تشغيل SQL Server
2. قم بتحديث connection string في `appsettings.json`
3. قم بتشغيل Entity Framework migrations:

```bash
cd Task_ERP_Api
dotnet ef database update
```

## هيكل قاعدة البيانات

المشروع يتضمن الجداول التالية:

- **Accounts**: الحسابات الرئيسية
- **Branches**: الفروع
- **Cities**: المدن
- **JVs**: القيود اليومية
- **JVDetails**: تفاصيل القيود
- **JVTypes**: أنواع القيود
- **SubAccounts**: الحسابات الفرعية
- **SubAccountsDetails**: تفاصيل الحسابات الفرعية
- **SubAccountsLevels**: مستويات الحسابات الفرعية
- **SubAccountsClients**: عملاء الحسابات الفرعية
- **SubAccountsTypes**: أنواع الحسابات الفرعية

## API Endpoints

### Accounts

- `GET /api/accounts` - جلب جميع الحسابات
- `GET /api/accounts/{id}` - جلب حساب محدد
- `POST /api/accounts` - إنشاء حساب جديد
- `PUT /api/accounts/{id}` - تحديث حساب
- `DELETE /api/accounts/{id}` - حذف حساب

### Branches

- `GET /api/branches` - جلب جميع الفروع
- `GET /api/branches/{id}` - جلب فرع محدد
- `POST /api/branches` - إنشاء فرع جديد
- `PUT /api/branches/{id}` - تحديث فرع
- `DELETE /api/branches/{id}` - حذف فرع

### Cities

- `GET /api/cities` - جلب جميع المدن
- `GET /api/cities/{id}` - جلب مدينة محددة
- `POST /api/cities` - إنشاء مدينة جديدة
- `PUT /api/cities/{id}` - تحديث مدينة
- `DELETE /api/cities/{id}` - حذف مدينة

### JVs (Journal Vouchers)

- `GET /api/jvs` - جلب جميع القيود
- `GET /api/jvs/{id}` - جلب قيد محدد
- `POST /api/jvs` - إنشاء قيد جديد
- `PUT /api/jvs/{id}` - تحديث قيد
- `DELETE /api/jvs/{id}` - حذف قيد

## التقنيات المستخدمة

### Backend

- **ASP.NET Core 8.0**: إطار العمل الرئيسي
- **Entity Framework Core**: ORM للتعامل مع قاعدة البيانات
- **AutoMapper**: تحويل البيانات بين DTOs والـ Models
- **Swagger/OpenAPI**: توثيق API
- **Global Exception Handler**: معالجة الأخطاء

### Frontend

- **Blazor Server**: إطار العمل للواجهة الأمامية
- **Bootstrap 5**: إطار العمل للتصميم
- **JavaScript**: للتفاعلات المتقدمة
- **CSS3**: للتنسيقات المخصصة

## المساهمة

1. Fork المشروع
2. إنشاء branch جديد للميزة (`git checkout -b feature/AmazingFeature`)
3. Commit التغييرات (`git commit -m 'Add some AmazingFeature'`)
4. Push إلى Branch (`git push origin feature/AmazingFeature`)
5. فتح Pull Request

## الترخيص

هذا المشروع مرخص تحت رخصة MIT - انظر ملف [LICENSE](LICENSE) للتفاصيل.

## الدعم

إذا واجهت أي مشاكل أو لديك أسئلة، يرجى فتح issue في GitHub repository.

## المؤلف

تم تطوير هذا المشروع كجزء من دورة ITI 9 Months.
