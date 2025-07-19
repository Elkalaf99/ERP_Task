# Generic Repository and Service Pattern Implementation Guide

## Overview

This document explains the implementation of a generic repository and service pattern in the Task ERP API project. This pattern follows the DRY (Don't Repeat Yourself) principle and provides a consistent, maintainable approach to data access and business logic.

## Architecture Benefits

### 1. **Code Reusability**

- Eliminates duplicate CRUD operations across repositories
- Provides consistent interface for all entities
- Reduces boilerplate code significantly

### 2. **Maintainability**

- Single point of change for common operations
- Consistent error handling and validation
- Easier to implement cross-cutting concerns

### 3. **Extensibility**

- Easy to add new entities without duplicating code
- Domain-specific methods can be added to derived classes
- Supports both generic and specific operations

## Generic Repository Pattern

### Core Interface: `IGenericRepository<T>`

```csharp
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(object id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(object id);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
}
```

### Core Implementation: `GenericRepository<T>`

```csharp
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly TaskContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(TaskContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // Implementation of all interface methods
    // Provides common CRUD operations using Entity Framework
}
```

### Domain-Specific Repository Example

```csharp
// Interface
public interface IAccountRepository : IGenericRepository<Account>
{
    // Add Account-specific methods here
    Task<IEnumerable<Account>> GetAccountsByTypeAsync(string accountType);
}

// Implementation
public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(TaskContext context) : base(context)
    {
    }

    // Implement Account-specific methods
    public async Task<IEnumerable<Account>> GetAccountsByTypeAsync(string accountType)
    {
        return await _dbSet.Where(a => a.AccountType == accountType).ToListAsync();
    }
}
```

## Generic Service Pattern

### Core Interface: `IGenericService<T>`

```csharp
public interface IGenericService<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(object id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(object id);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
}
```

### Core Implementation: `GenericService<T>`

```csharp
public class GenericService<T> : IGenericService<T> where T : class
{
    protected readonly IGenericRepository<T> _repository;

    public GenericService(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    // Implementation with business logic validation
    // Includes input validation and error handling
}
```

### Domain-Specific Service Example

```csharp
// Interface
public interface IAccountService : IGenericService<Account>
{
    // DTO-specific methods
    Task<IEnumerable<AccountDto>> GetAllDtosAsync();
    Task<AccountDto> GetDtoByIdAsync(int id);
    Task<AccountDto> CreateFromDtoAsync(CreateAccountDto createAccountDto);
    Task<AccountDto> UpdateFromDtoAsync(int id, UpdateAccountDto updateAccountDto);
}

// Implementation
public class AccountService : GenericService<Account>, IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IBranchRepository _branchRepository;

    public AccountService(IAccountRepository accountRepository, IBranchRepository branchRepository)
        : base(accountRepository)
    {
        _accountRepository = accountRepository;
        _branchRepository = branchRepository;
    }

    // Implement DTO-specific methods with business logic
    public async Task<AccountDto> CreateFromDtoAsync(CreateAccountDto createAccountDto)
    {
        // Business logic validation
        if (createAccountDto.BranchID.HasValue)
        {
            var branch = await _branchRepository.GetByIdAsync(createAccountDto.BranchID.Value);
            if (branch == null)
            {
                throw new InvalidOperationException($"Branch with ID {createAccountDto.BranchID.Value} does not exist.");
            }
        }

        // Map DTO to entity and create
        var account = new Account { /* mapping logic */ };
        var createdAccount = await CreateAsync(account);
        return MapToDto(createdAccount);
    }
}
```

## Migration Guide

### Before (Traditional Approach)

```csharp
// Repository Interface
public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetAllAsync();
    Task<Account> GetByIdAsync(int id);
    Task<Account> AddAsync(Account account);
    Task<Account> UpdateAsync(Account account);
    Task<bool> DeleteAsync(int id);
}

// Repository Implementation
public class AccountRepository : IAccountRepository
{
    private readonly TaskContext _context;

    public AccountRepository(TaskContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _context.Accounts.ToListAsync();
    }

    // ... repeat for all methods
}
```

### After (Generic Pattern)

```csharp
// Repository Interface
public interface IAccountRepository : IGenericRepository<Account>
{
    // Only add Account-specific methods
}

// Repository Implementation
public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(TaskContext context) : base(context)
    {
    }

    // Only implement Account-specific methods
}
```

## Key Features

### 1. **Flexible ID Types**

- Generic repository supports `object` ID type
- Works with `int`, `Guid`, `string`, etc.
- Type-safe implementations in derived classes

### 2. **Advanced Querying**

- `FindAsync()` for complex queries
- `FirstOrDefaultAsync()` for single results
- `ExistsAsync()` for existence checks
- `CountAsync()` for counting records

### 3. **Business Logic Validation**

- Input validation in generic service
- Extensible validation hooks
- Domain-specific validation in derived services

### 4. **DTO Support**

- Separate methods for DTO operations
- Maintains clean separation of concerns
- Supports AutoMapper integration

## Best Practices

### 1. **Repository Layer**

- Keep repositories focused on data access
- Add domain-specific methods only when needed
- Use the generic base for common operations

### 2. **Service Layer**

- Implement business logic in services
- Use DTOs for external communication
- Extend generic service for domain-specific operations

### 3. **Error Handling**

- Generic service provides basic validation
- Add domain-specific validation in derived classes
- Use meaningful exception messages

### 4. **Performance**

- Generic repository uses efficient Entity Framework operations
- Supports async/await throughout
- Minimizes database round trips

## Usage Examples

### Basic CRUD Operations

```csharp
// Get all accounts
var accounts = await _accountService.GetAllAsync();

// Get account by ID
var account = await _accountService.GetByIdAsync(1);

// Create new account
var newAccount = await _accountService.CreateAsync(account);

// Update account
var updatedAccount = await _accountService.UpdateAsync(account);

// Delete account
var deleted = await _accountService.DeleteAsync(1);
```

### Advanced Querying

```csharp
// Find accounts by condition
var activeAccounts = await _accountService.FindAsync(a => a.IsActive);

// Check if account exists
var exists = await _accountService.ExistsAsync(a => a.AccountNumber == "ACC001");

// Count accounts
var count = await _accountService.CountAsync(a => a.BranchID == 1);
```

### DTO Operations

```csharp
// Get all accounts as DTOs
var accountDtos = await _accountService.GetAllDtosAsync();

// Create account from DTO
var accountDto = await _accountService.CreateFromDtoAsync(createAccountDto);

// Update account from DTO
var updatedDto = await _accountService.UpdateFromDtoAsync(1, updateAccountDto);
```

## Conclusion

The generic repository and service pattern provides a robust, maintainable foundation for the Task ERP API. It eliminates code duplication while maintaining flexibility for domain-specific requirements. This pattern follows SOLID principles and provides a consistent approach to data access and business logic across all entities.

## Files Created/Modified

### New Files

- `Repositories/IGenericRepository.cs` - Generic repository interface
- `Repositories/GenericRepository.cs` - Generic repository implementation
- `Services/IGenericService.cs` - Generic service interface
- `Services/GenericService.cs` - Generic service implementation
- `GENERIC_PATTERN_GUIDE.md` - This documentation

### Refactored Files

- `Repositories/Account/IAccountRepository.cs` - Now extends generic interface
- `Repositories/Account/AccountRepository.cs` - Now extends generic implementation
- `Services/Account/IAccountService.cs` - Now extends generic interface
- `Services/Account/AccountService.cs` - Now extends generic implementation
- `Repositories/Branch/IBranchRepository.cs` - Now extends generic interface
- `Repositories/Branch/BranchRepository.cs` - Now extends generic implementation
- `Services/Branch/IBranchService.cs` - Now extends generic interface
- `Services/Branch/BranchService.cs` - Now extends generic implementation
- `Repositories/City/ICityRepository.cs` - Now extends generic interface
- `Repositories/City/CityRepository.cs` - Now extends generic implementation
- `Services/City/ICityService.cs` - Now extends generic interface
- `Services/City/CityService.cs` - Now extends generic implementation
