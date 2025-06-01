using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModel.Accounts
{
    public class PaginatedAccountMembers
    {
        public int PageNum { get; set; } = 1; 
        public int TotalPage { get; set; } = 1;
        public int PageSize { get; set; } = PAGE_SIZE;
        public int TotalItems { get; set; } = 0;
        public List<AccountMemberListItem> AccountMembers { get; set; } = new List<AccountMemberListItem>();

        private const int PAGE_SIZE = 5;

        public static PaginatedAccountMembers ToPaginatedAccountMembers(IEnumerable<AccountMember> dataList, int pageNum)
        {
            var totalPages = (int)Math.Ceiling((double)dataList.Count() / PAGE_SIZE);
            var paginatedList = dataList.Skip((pageNum - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();

            return new PaginatedAccountMembers
            {
                PageNum = pageNum,
                TotalPage = totalPages,
                PageSize = PAGE_SIZE,
                TotalItems = dataList.Count(),
                AccountMembers = paginatedList.Select(acc => new AccountMemberListItem
                {
                    MemberId = acc.MemberId,
                    FullName = acc.FullName,
                    EmailAddress = acc.EmailAddress,
                    MemberRole = acc.MemberRole
                }).ToList()
            };
        }
    }

    public class AccountMemberListItem
    {
        public string MemberId { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public int MemberRole { get; set; }
    }
}
