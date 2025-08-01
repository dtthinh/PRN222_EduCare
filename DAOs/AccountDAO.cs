﻿using BOs.Data;
using BOs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class AccountDAO
    {
        private static AccountDAO instance = null;
        private readonly DataContext _context;

        private AccountDAO()
        {
            _context = new DataContext();
        }

        public static AccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountDAO();
                }
                return instance;
            }
        }


        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _context.Accounts
                                 .AsNoTracking()
                                 .Include(a => a.Role)
                                 .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts.AsNoTracking().Include(a => a.Role).ToListAsync();
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountID == accountId);
            if (account == null)
                return false;
            account.Status = "InActive";
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.AsNoTracking().ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }
        public async Task<bool> UpdateAccountAsync(Account account)
        {
            var existingAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountID == account.AccountID);

            if (existingAccount == null || existingAccount.RoleID == 0)
                return false;

            bool hasChanges = false;

            // Update các trường thông tin cơ bản
            if (!string.IsNullOrEmpty(account.Fullname) && !account.Fullname.Equals(existingAccount.Fullname))
            {
                existingAccount.Fullname = account.Fullname;
                hasChanges = true;
            }

            if (!string.IsNullOrEmpty(account.Email) && !account.Email.Equals(existingAccount.Email))
            {
                existingAccount.Email = account.Email;
                hasChanges = true;
            }

            if (account.Address != null && !account.Address.Equals(existingAccount.Address))
            {
                existingAccount.Address = account.Address;
                hasChanges = true;
            }

            if (!string.IsNullOrEmpty(account.PhoneNumber) && !account.PhoneNumber.Equals(existingAccount.PhoneNumber))
            {
                existingAccount.PhoneNumber = account.PhoneNumber;
                hasChanges = true;
            }

            if (account.DateOfBirth != default && account.DateOfBirth != existingAccount.DateOfBirth)
            {
                existingAccount.DateOfBirth = account.DateOfBirth;
                hasChanges = true;
            }

            // Update password nếu có
            if (!string.IsNullOrEmpty(account.Password))
            {
                if (account.Password.StartsWith("$2a$") || account.Password.StartsWith("$2b$") || account.Password.StartsWith("$2x$") || account.Password.StartsWith("$2y$"))
                {
                    if (!account.Password.Equals(existingAccount.Password))
                    {
                        existingAccount.Password = account.Password;
                        hasChanges = true;
                    }
                }
                else
                {
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(account.Password);
                    if (!hashedPassword.Equals(existingAccount.Password))
                    {
                        existingAccount.Password = hashedPassword;
                        hasChanges = true;
                    }
                }
            }

            // Update Status nếu có
            if (!string.IsNullOrEmpty(account.Status) && !account.Status.Equals(existingAccount.Status, StringComparison.OrdinalIgnoreCase))
            {
                existingAccount.Status = account.Status;
                hasChanges = true;
            }

            // --- Cập nhật ảnh đại diện (Image) ---
            if (account.Image != null && (existingAccount.Image == null || !account.Image.SequenceEqual(existingAccount.Image)))
            {
                existingAccount.Image = account.Image;
                hasChanges = true;
            }

            // Always update UpdatedAt when there are changes
            if (hasChanges)
            {
                existingAccount.UpdateAt = account.UpdateAt;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Account?> Login(string email, string password)
        {
            var account = await _context.Accounts
                                        .AsNoTracking()
                                        .Include(a => a.Role)
                                        .FirstOrDefaultAsync(a => a.Email == email && a.RoleID != 0);
            if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password))
                return null;
            return account;
        }

        public async Task<Account?> GetAccountByIdAsync(int accountId)
        {
            return await _context.Accounts
                .Include(a => a.Role)
                .Include(a => a.Students)
                    .ThenInclude(s => s.Class) 
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AccountID == accountId);
        }

        public async Task<Account?> SearchAccountByIdAsync(int accountId)
        {
            return await _context.Accounts.Include(a => a.Role).AsNoTracking()
                                   .FirstOrDefaultAsync(a => a.AccountID == accountId);
        }

        public async Task<List<Account>> SearchAccountsByFullNameAsync(string searchTerm)
        {
            return await _context.Accounts.AsNoTracking()
           .Include(a => a.Role)
                .Where(a => EF.Functions.Like(a.Fullname, $"%{searchTerm}%"))
                .ToListAsync();
        }


        public async Task<bool> UpdateAccountStatusAsync(Account account, string status)
        {
            // Danh sách các trạng thái được phép
            var allowedStatuses = new[] { "Active", "InActive", "Pending", "Reject" };

            // Kiểm tra trạng thái hợp lệ
            if (!allowedStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
                return false;

            // Kiểm tra nếu trạng thái mới giống trạng thái hiện tại
            if (account.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                return true; // Không cần update, xem như thành công

            // Cập nhật trạng thái và thời gian cập nhật
            account.Status = status;
            account.UpdateAt = DateTime.UtcNow;

            // Gọi hàm cập nhật thực tế
            return await UpdateAccountAsync(account);
        }

        public async Task SavePasswordResetTokenAsync(int accountId, string token, DateTime expiration)
        {
            var resetToken = new PasswordResetToken
            {
                AccountID = accountId,
                Token = token,
                Expiration = expiration,
                CreatedAt = DateTime.UtcNow
            };

            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VerifyPasswordResetTokenAsync(int accountId, string token)
        {
            var resetToken = await _context.PasswordResetTokens.AsNoTracking()
        .FirstOrDefaultAsync(t => t.AccountID == accountId && t.Token == token);

            return resetToken != null && resetToken.Expiration >= DateTime.UtcNow;
        }

        public async Task InvalidatePasswordResetTokenAsync(int accountId, string token)
        {
            var resetToken = await _context.PasswordResetTokens.FirstOrDefaultAsync(t => t.AccountID == accountId && t.Token == token);

            if (resetToken != null)
            {
                _context.PasswordResetTokens.Remove(resetToken);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetParentCountAsync()
        {
            return await _context.Accounts.AsNoTracking().CountAsync(a =>
                a.RoleID == 3 &&
                a.Status.ToLower() == "active");
        }
        public async Task<int> GetNurseCountAsync()
        {
            return await _context.Accounts.AsNoTracking().CountAsync(a =>
                a.RoleID == 2 &&
                a.Status.ToLower() == "active");
        }
        
        public async Task<List<Account>> GetActiveNursesAsync()
        {
            return await _context.Accounts
                .Where(a => a.RoleID == 2 && a.Status == "Active")
                .ToListAsync();
        }
    }
}
