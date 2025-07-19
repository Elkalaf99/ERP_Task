-- =====================================================
-- SAMPLE DATA INSERTION WITH PROPER RELATIONSHIPS
-- =====================================================

-- Clear all existing data in correct order (respecting foreign keys)
PRINT '=== CLEARING EXISTING DATA ==='

-- Delete JVDetails first (depends on JV, Account, SubAccount, Branch)
DELETE FROM JVDetails;
PRINT 'JVDetails cleared'

-- Delete JVs (depends on JVType, Branch)
DELETE FROM JVs;
PRINT 'JVs cleared'

-- Delete SubAccounts_Details (depends on SubAccount, Account)
DELETE FROM SubAccounts_Details;
PRINT 'SubAccounts_Details cleared'

-- Delete SubAccountsClients (depends on SubAccount)
DELETE FROM SubAccountsClients;
PRINT 'SubAccountsClients cleared'

-- Delete SubAccounts (depends on SubAccounts_Level, SubAccountsType, Branch)
DELETE FROM SubAccounts;
PRINT 'SubAccounts cleared'

-- Delete Accounts (depends on Branch)
DELETE FROM Accounts;
PRINT 'Accounts cleared'

-- Delete JVTypes (depends on Branch)
DELETE FROM JVTypes;
PRINT 'JVTypes cleared'

-- Delete SubAccounts_Levels (depends on Branch)
DELETE FROM SubAccounts_Levels;
PRINT 'SubAccounts_Levels cleared'

-- Delete SubAccountsTypes (depends on Branch)
DELETE FROM SubAccountsTypes;
PRINT 'SubAccountsTypes cleared'

-- Delete Branches (depends on Cities)
DELETE FROM Branches;
PRINT 'Branches cleared'

-- Delete Cities
DELETE FROM Cities;
PRINT 'Cities cleared'

PRINT '=== ALL DATA CLEARED SUCCESSFULLY ==='

-- =====================================================
-- INSERT SAMPLE DATA IN CORRECT ORDER
-- =====================================================

PRINT '=== INSERTING CITIES ==='
INSERT INTO Cities (CityID, CityNameAr, CityNameEn) VALUES
(1, 'القاهرة', 'Cairo'),
(2, 'الإسكندرية', 'Alexandria'),
(3, 'الجيزة', 'Giza');

PRINT 'Cities inserted successfully'

PRINT '=== INSERTING BRANCHES ==='
INSERT INTO Branches (BranchID, BranchCode, BranchNameAr, BranchNameEn) VALUES
(1, 'MB001', 'الفرع الرئيسي', 'Main Branch'),
(2, 'SB001', 'الفرع الفرعي', 'Sub Branch');

PRINT 'Branches inserted successfully'

PRINT '=== INSERTING JV TYPES ==='
INSERT INTO JVTypes (JVTypeID, JVTypeNameAr, JVTypeNameEn, BranchID) VALUES
(1, 'قيد يومية عادي', 'Regular Journal Entry', 1),
(2, 'قيد إقفال', 'Closing Entry', 1),
(3, 'قيد افتتاح', 'Opening Entry', 1);

PRINT 'JVTypes inserted successfully'

PRINT '=== INSERTING SUB ACCOUNTS TYPES ==='
INSERT INTO SubAccountsTypes (SubAccountTypeID, SubAccountTypeNameAr, SubAccountTypeNameEn, BranchID) VALUES
(1, 'عملاء', 'Customers', 1),
(2, 'موردين', 'Suppliers', 1),
(3, 'موظفين', 'Employees', 1),
(4, 'بنوك', 'Banks', 1);

PRINT 'SubAccountsTypes inserted successfully'

PRINT '=== INSERTING SUB ACCOUNTS LEVELS ==='
INSERT INTO SubAccounts_Levels (LevelID, LevelNameAr, LevelNameEn, LevelNumber, BranchID) VALUES
(1, 'المستوى الأول', 'Level 1', 1, 1),
(2, 'المستوى الثاني', 'Level 2', 2, 1),
(3, 'المستوى الثالث', 'Level 3', 3, 1);

PRINT 'SubAccounts_Levels inserted successfully'

PRINT '=== INSERTING ACCOUNTS ==='
INSERT INTO Accounts (AccountID, AccountNumber, AccountNameAr, AccountNameEn, BranchID) VALUES
(1001, '1000', 'الصندوق', 'Cash', 1),
(1002, '1100', 'البنوك', 'Banks', 1),
(2001, '2000', 'المدينون', 'Accounts Receivable', 1),
(2002, '2100', 'أوراق القبض', 'Notes Receivable', 1),
(3001, '3000', 'الدائنون', 'Accounts Payable', 1),
(3002, '3100', 'أوراق الدفع', 'Notes Payable', 1),
(4001, '4000', 'الإيرادات', 'Revenue', 1),
(5001, '5000', 'المصروفات', 'Expenses', 1);

PRINT 'Accounts inserted successfully'

PRINT '=== INSERTING SUB ACCOUNTS ==='
INSERT INTO SubAccounts (SubAccountID, SubAccountNumber, SubAccountNameAr, SubAccountNameEn, ParentID, IsMain, LevelID, SubAccountTypeID, BranchID) VALUES
-- Level 1 - Main SubAccounts
(1, '1001', 'صندوق الفرع الرئيسي', 'Main Branch Cash', NULL, 1, 1, 4, 1),
(2, '1101', 'البنك الأهلي', 'National Bank', NULL, 1, 1, 4, 1),
(3, '2001', 'عملاء نقدي', 'Cash Customers', NULL, 1, 1, 1, 1),
(4, '3001', 'موردين محليين', 'Local Suppliers', NULL, 1, 1, 2, 1);

PRINT 'SubAccounts inserted successfully'

PRINT '=== INSERTING JVs ==='
INSERT INTO JVs (JVID, JVNo, JVDate, JVTypeID, TotalDebit, TotalCredit, ReceiptNo, Notes, BranchID) VALUES
(1, 'JV-2024-001', '2024-01-15', 1, 10000.00, 10000.00, 'RCP-001', 'قيد إيداع نقدي', 1),
(2, 'JV-2024-002', '2024-01-16', 1, 5000.00, 5000.00, 'RCP-002', 'قيد تحصيل من عميل', 1);

PRINT 'JVs inserted successfully'

PRINT '=== INSERTING JV DETAILS ==='
INSERT INTO JVDetails (JVDetailID, JVID, AccountID, SubAccountID, Debit, Credit, IsDocumented, Notes, BranchID) VALUES
-- JV-2024-001 Details (Cash Deposit)
(1, 1, 1001, 1, 10000.00, 0.00, 1, 'إيداع نقدي في الصندوق', 1),
(2, 1, 4001, NULL, 0.00, 10000.00, 1, 'إيراد نقدي', 1),

-- JV-2024-002 Details (Customer Collection)
(3, 2, 1001, 1, 5000.00, 0.00, 1, 'تحصيل نقدي من عميل', 1),
(4, 2, 2001, 3, 0.00, 5000.00, 1, 'تسوية حساب العميل', 1);

PRINT 'JVDetails inserted successfully'

-- =====================================================
-- VERIFICATION QUERIES
-- =====================================================

PRINT '=== DATA VERIFICATION ==='

PRINT 'Cities count:'
SELECT COUNT(*) as CitiesCount FROM Cities;

PRINT 'Branches count:'
SELECT COUNT(*) as BranchesCount FROM Branches;

PRINT 'JVTypes count:'
SELECT COUNT(*) as JVTypesCount FROM JVTypes;

PRINT 'SubAccountsTypes count:'
SELECT COUNT(*) as SubAccountsTypesCount FROM SubAccountsTypes;

PRINT 'SubAccounts_Levels count:'
SELECT COUNT(*) as SubAccounts_LevelsCount FROM SubAccounts_Levels;

PRINT 'Accounts count:'
SELECT COUNT(*) as AccountsCount FROM Accounts;

PRINT 'SubAccounts count:'
SELECT COUNT(*) as SubAccountsCount FROM SubAccounts;

PRINT 'JVs count:'
SELECT COUNT(*) as JVsCount FROM JVs;

PRINT 'JVDetails count:'
SELECT COUNT(*) as JVDetailsCount FROM JVDetails;

PRINT '=== BALANCE VERIFICATION ==='
SELECT 
    JV.JVNo,
    JV.TotalDebit,
    JV.TotalCredit,
    CASE 
        WHEN JV.TotalDebit = JV.TotalCredit THEN 'BALANCED'
        ELSE 'UNBALANCED'
    END as BalanceStatus
FROM JVs JV;

PRINT '=== ALL OPERATIONS COMPLETED SUCCESSFULLY ==='
PRINT 'Completion time: ' + CAST(GETDATE() AS VARCHAR(50)) 