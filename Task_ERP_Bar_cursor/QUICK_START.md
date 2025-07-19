# 🚀 دليل البدء السريع - JVForm Integration

## ⚡ التشغيل السريع

### 1. تشغيل الباك إند

```bash
# تأكد من تشغيل الباك إند على
https://localhost:7240
```

### 2. تشغيل الفرونت إند

```bash
dotnet run
# سيفتح على https://localhost:7000
```

### 3. الوصول للصفحة

```
http://localhost:7000/JVFormComponent
```

## 🔧 إصلاح المشاكل الشائعة

### مشكلة 404 في generate-number

✅ **تم الحل**: الـ service الآن يولد الرقم محلياً إذا لم يكن الـ endpoint موجود

### مشكلة 400 Bad Request

✅ **تم الحل**: تم تنظيف تنسيق البيانات المرسلة للـ API

### مشكلة Entity Changes Error

✅ **تم الحل**:

- إضافة دالة `CreateSimpleJVAsync` للاختبار
- تنظيف البيانات قبل الإرسال
- معالجة القيم الفارغة

### خطأ في نوع البيانات (CS0019)

✅ **تم الحل**: إزالة `?? new List<object>()` من LINQ queries

### مشكلة CORS

```csharp
// في الباك إند، أضف:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp",
        builder => builder
            .WithOrigins("https://localhost:7000")
            .AllowAnyMethod()
            .AllowAnyHeader());
});
```

## 📊 اختبار النظام

### 1. اختبار الاتصال

```bash
# استخدم ملف api-test.http في wwwroot/
```

### 2. مراقبة الـ Console

```csharp
// في JVService.cs
Console.WriteLine($"API Status: {response.StatusCode}");
```

### 3. التحقق من البيانات

- تأكد من وجود بيانات في الباك إند
- تحقق من صحة الـ endpoints
- راجع الـ error messages

## 🎯 الميزات الجاهزة

✅ **عرض القيود**: جدول تفاعلي مع فلترة وبحث  
✅ **إضافة قيد جديد**: نموذج كامل مع التحقق من التوازن  
✅ **تعديل القيود**: تحميل وتعديل البيانات  
✅ **حذف القيود**: حذف مع تأكيد  
✅ **توليد الأرقام**: تلقائي أو محلي  
✅ **معالجة الأخطاء**: رسائل واضحة للمستخدم  
✅ **اختبار مبسط**: زر "Test Simple JV" للاختبار السريع  
✅ **اختبار بدون تفاصيل**: زر "Test JV No Details" لحفظ بدون تفاصيل  
✅ **اختبار تفاصيل القيد**: زر "Test JV Detail" لاختبار إنشاء التفاصيل  
✅ **اختبار التنسيق الصحيح**: زر "Test Working Detail" للتنسيق المختبر  
✅ **حفظ القيد**: Save و Save And Close يعملان الآن مع التنسيق الصحيح

## 🚨 إذا لم تعمل البيانات

1. **تحقق من الباك إند**: `https://localhost:7240/api/jv`
2. **راجع الـ Console**: للـ error messages
3. **اختبر الـ API**: استخدم ملف `api-test.http`
4. **تحقق من CORS**: في إعدادات الباك إند

## 📞 الدعم السريع

```csharp
// إضافة logging للتتبع
Console.WriteLine($"Loading data from: {_httpClient.BaseAddress}");
```

---

**النظام جاهز للاستخدام!** 🎉
