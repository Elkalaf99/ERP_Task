# 🎯 JVDetail Endpoint Test Guide

## 📋 التنسيق المطلوب

### Endpoint

```
POST https://localhost:7240/api/JVDetail
```

### Headers

```
Content-Type: application/json
Accept: application/json
```

### Body Format

```json
{
  "jVID": 1,
  "accountID": 4001,
  "subAccountID": null,
  "debit": null,
  "credit": 10000.0,
  "isDocumented": true,
  "notes": "إيراد نقدي",
  "branchID": 1
}
```

## 🧪 اختبارات مختلفة

### 1. إدخال دائن (Credit Entry)

```json
{
  "jVID": 1,
  "accountID": 4001,
  "subAccountID": null,
  "debit": null,
  "credit": 10000.0,
  "isDocumented": true,
  "notes": "إيراد نقدي",
  "branchID": 1
}
```

### 2. إدخال مدين (Debit Entry)

```json
{
  "jVID": 1,
  "accountID": 3002,
  "subAccountID": 2003,
  "debit": 10000.0,
  "credit": null,
  "isDocumented": true,
  "notes": "مدين نقدي",
  "branchID": 1
}
```

### 3. إدخال مع SubAccount

```json
{
  "jVID": 1,
  "accountID": 3002,
  "subAccountID": 2003,
  "debit": 5000.0,
  "credit": null,
  "isDocumented": false,
  "notes": "مدين مع حساب فرعي",
  "branchID": 1
}
```

## ⚠️ حالات الفشل المتوقعة

### 1. مدين ودائن معاً (يجب أن يفشل)

```json
{
  "jVID": 1,
  "accountID": 3002,
  "subAccountID": 2003,
  "debit": 1000.0,
  "credit": 1000.0,
  "isDocumented": true,
  "notes": "مدين ودائن معاً",
  "branchID": 1
}
```

### 2. لا مدين ولا دائن (يجب أن يفشل)

```json
{
  "jVID": 1,
  "accountID": 3002,
  "subAccountID": 2003,
  "debit": null,
  "credit": null,
  "isDocumented": true,
  "notes": "لا مدين ولا دائن",
  "branchID": 1
}
```

## 🔧 كيفية الاختبار

### باستخدام HTTP File

1. افتح ملف `wwwroot/jvdetail-test.http`
2. اختر الاختبار المطلوب
3. اضغط `Send Request`

### باستخدام Curl

```bash
curl -X POST https://localhost:7240/api/JVDetail \
  -H "Content-Type: application/json" \
  -d '{
    "jVID": 1,
    "accountID": 4001,
    "subAccountID": null,
    "debit": null,
    "credit": 10000.0,
    "isDocumented": true,
    "notes": "إيراد نقدي",
    "branchID": 1
  }'
```

### باستخدام Postman

1. Method: `POST`
2. URL: `https://localhost:7240/api/JVDetail`
3. Headers: `Content-Type: application/json`
4. Body: `raw` + `JSON`

## ✅ النتائج المتوقعة

### نجح (200 OK)

```json
{
  "jvDetailID": 1,
  "jvid": 1,
  "accountID": 4001,
  "subAccountID": null,
  "debit": null,
  "credit": 10000.0,
  "isDocumented": true,
  "notes": "إيراد نقدي",
  "branchID": 1
}
```

### فشل (400 Bad Request)

```json
{
  "error": "Validation failed",
  "details": "Either debit or credit must be provided, but not both"
}
```

## 📝 ملاحظات مهمة

1. **jVID**: معرف القيد الرئيسي (مطلوب)
2. **accountID**: معرف الحساب (مطلوب)
3. **subAccountID**: معرف الحساب الفرعي (اختياري)
4. **debit**: المبلغ المدين (اختياري)
5. **credit**: المبلغ الدائن (اختياري)
6. **isDocumented**: هل موثق (مطلوب)
7. **notes**: ملاحظات (اختياري)
8. **branchID**: معرف الفرع (مطلوب)

### قواعد التحقق

- يجب أن يكون هناك إما `debit` أو `credit`، وليس كلاهما
- لا يمكن أن يكون كل من `debit` و `credit` فارغين
- `jVID` و `accountID` و `branchID` مطلوبة
