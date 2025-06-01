using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class AccountDAO
    {
        public static List<AccountMember> GetAccounts()
        {
            var listAccounts = new List<AccountMember>();
            try
            {
                using var db = new MyStoreDBContext();
                listAccounts = db.AccountMembers.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listAccounts;
        }

        public static void SaveAccount(AccountMember acc)
        {
            try
            {
                using var context = new MyStoreDBContext();
                context.AccountMembers.Add(acc);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void CreateAccount(AccountMember acc)
        {
            try
            {
                using var context = new MyStoreDBContext();
                context.AccountMembers.Add(acc);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateAccount(AccountMember acc)
        {
            try
            {
                using var context = new MyStoreDBContext();
                context.Entry<AccountMember>(acc).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteAccount(AccountMember acc)
        {
            try
            {
                using var context = new MyStoreDBContext();
                var foundAccount =
                    context.AccountMembers.SingleOrDefault(c => c.MemberId == acc.MemberId);

                if (foundAccount != null)
                    context.AccountMembers.Remove(foundAccount);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static AccountMember? GetAccountById(string accountId)
        {
            using var db = new MyStoreDBContext();
            return db.AccountMembers.FirstOrDefault(c => c.MemberId.Equals(accountId));
        }

        public static List<AccountMember> SearchAccount(string fullName, string email, int roleId)
        {
            using var db = new MyStoreDBContext();
            var result = from member in db.AccountMembers
                         where (string.IsNullOrEmpty(fullName) || member.FullName.Contains(fullName)) &&
                               (string.IsNullOrEmpty(email) || member.EmailAddress.Contains(email)) &&
                               (roleId == 0 || member.MemberRole == roleId)
                         select member;

            return result.ToList();
        }
    }
}
