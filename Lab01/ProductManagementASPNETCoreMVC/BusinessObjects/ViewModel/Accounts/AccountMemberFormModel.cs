using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModel.Accounts
{
    public class AccountMemberFormModel
    {
        [Required]
        public string MemberId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int MemberRole { get; set; }

        public AccountMember ToAccountMember()
        {
            return new AccountMember
            {
                MemberId = MemberId,
                FullName = FullName,
                EmailAddress = EmailAddress,
                MemberPassword = Password,
                MemberRole = MemberRole,
            };
        }

        public static AccountMemberFormModel ToFormModel(AccountMember account)
        {
            return new AccountMemberFormModel()
            {
                MemberId = account.MemberId,
                ConfirmPassword = account.MemberPassword,
                Password = account.MemberPassword,
                EmailAddress = account.EmailAddress,
                MemberRole = account.MemberRole,
                FullName = account.FullName,
            };
        }
    }
}
