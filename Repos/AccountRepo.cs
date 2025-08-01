﻿using BOs.Models;
using DAOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public class AccountRepo : IAccountRepo
    {

        public Task<Account?> GetAccountByEmailAsync(string email) => AccountDAO.Instance.GetAccountByEmailAsync(email);
        public Task<Account> CreateAccountAsync(Account account) => AccountDAO.Instance.CreateAccountAsync(account);
        public Task<List<Role>> GetAllRolesAsync()
        {
            return AccountDAO.Instance.GetAllRolesAsync();
        }

        public Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return AccountDAO.Instance.GetRoleByIdAsync(roleId);
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await AccountDAO.Instance.GetAllAccountsAsync();
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            return await AccountDAO.Instance.DeleteAccountAsync(accountId);
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            return await AccountDAO.Instance.UpdateAccountAsync(account);
        }

        public async Task<Account?> Login(string email, string password)
        {
            return await AccountDAO.Instance.Login(email, password);
        }

        public async Task<Account?> GetAccountByIdAsync(int accountId)
        {
            return await AccountDAO.Instance.GetAccountByIdAsync(accountId);
        }

        public async Task<Account?> SearchAccountByIdAsync(int accountId)
        {
            return await AccountDAO.Instance.SearchAccountByIdAsync(accountId);
        }

        public async Task<List<Account>> SearchAccountsByFullNameAsync(string searchTerm)
        {
            return await AccountDAO.Instance.SearchAccountsByFullNameAsync(searchTerm);
        }

        public async Task<bool> UpdateAccountStatusAsync(Account account, string status)
        {
            return await AccountDAO.Instance.UpdateAccountStatusAsync(account, status);
        }

        public async Task SavePasswordResetTokenAsync(int accountId, string token, DateTime expiration)
        {
            await AccountDAO.Instance.SavePasswordResetTokenAsync(accountId, token, expiration);
        }

        public async Task<bool> VerifyPasswordResetTokenAsync(int accountId, string token)
        {
            return await AccountDAO.Instance.VerifyPasswordResetTokenAsync(accountId, token);
        }

        public async Task InvalidatePasswordResetTokenAsync(int accountId, string token)
        {
            await AccountDAO.Instance.InvalidatePasswordResetTokenAsync(accountId, token);
        }

        public async Task<int> GetParentCountAsync()
        {
            return await AccountDAO.Instance.GetParentCountAsync();
        }

        public async Task<int> GetNurseCountAsync()
        {
            return await AccountDAO.Instance.GetNurseCountAsync();
        }

        public async Task<List<Account>> GetActiveNursesAsync()
        {
            return await AccountDAO.Instance.GetActiveNursesAsync();
        }
    }

}
