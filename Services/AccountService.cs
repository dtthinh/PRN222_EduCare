﻿using BOs.Models;
using Microsoft.Extensions.Caching.Distributed;
using Repos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _accountRepo;

        public AccountService(IAccountRepo accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _accountRepo.GetAccountByEmailAsync(email);
        }

        public async Task<Account?> GetAccountByIdAsync(int accountId)
        {
            return await _accountRepo.GetAccountByIdAsync(accountId);
        }

        public async Task<Account?> Login(string email, string password)
        {
            return await _accountRepo.Login(email, password);
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _accountRepo.GetAllAccountsAsync();
        }

        public async Task<List<Account>> SearchAccountsByFullNameAsync(string searchTerm)
        {
            return await _accountRepo.SearchAccountsByFullNameAsync(searchTerm);
        }

        public async Task<Account?> SearchAccountByIdAsync(int accountId)
        {
            return await _accountRepo.SearchAccountByIdAsync(accountId);
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            return await _accountRepo.DeleteAccountAsync(accountId);
        }

        public async Task<Account> SignUpByAdminAsync(Account newAccountDetails)
        {
            if (newAccountDetails == null)
            {
                throw new ArgumentNullException(nameof(newAccountDetails));
            }

            var existingAccount = await _accountRepo.GetAccountByEmailAsync(newAccountDetails.Email);
            if (existingAccount != null)
            {
                throw new InvalidOperationException("Một tài khoản với email này đã tồn tại.");
            }

            newAccountDetails.Password = BCrypt.Net.BCrypt.HashPassword(newAccountDetails.Password);
            newAccountDetails.CreatedAt = DateTime.UtcNow;
            newAccountDetails.UpdateAt = DateTime.UtcNow;

            if (string.IsNullOrWhiteSpace(newAccountDetails.Status))
            {
                newAccountDetails.Status = "Active";
            }

            return await _accountRepo.CreateAccountAsync(newAccountDetails);
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _accountRepo.GetAllRolesAsync();
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            return await _accountRepo.UpdateAccountAsync(account);
        }

        public async Task<bool> UpdateAccountStatusAsync(Account account, string status)
        {
            return await _accountRepo.UpdateAccountStatusAsync(account, status);
        }

        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return await _accountRepo.GetRoleByIdAsync(roleId);
        }

        public Task SavePasswordResetTokenAsync(int accountId, string token, DateTime expiration)
        {
            return _accountRepo.SavePasswordResetTokenAsync(accountId, token, expiration);
        }

        public Task<bool> VerifyPasswordResetTokenAsync(int accountId, string token)
        {
            return _accountRepo.VerifyPasswordResetTokenAsync(accountId, token);
        }

        public Task InvalidatePasswordResetTokenAsync(int accountId, string token)
        {
            return _accountRepo.InvalidatePasswordResetTokenAsync(accountId, token);
        }

        public async Task<int> GetParentCountAsync()
        {
            return await _accountRepo.GetParentCountAsync();
        }

        public async Task<int> GetNurseCountAsync()
        {
            return await _accountRepo.GetNurseCountAsync();
        }

        public async Task<List<Account>> GetActiveNursesAsync()
        {
            return await _accountRepo.GetActiveNursesAsync();
        }

    }

}
