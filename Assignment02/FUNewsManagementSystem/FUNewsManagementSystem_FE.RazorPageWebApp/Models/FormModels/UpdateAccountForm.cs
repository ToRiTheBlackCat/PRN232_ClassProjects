using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Models.FormModels
{
    public class UpdateAccountForm
    {
        [Required]
        public short AccountId { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\sÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẮẰẲẴẶắằẳẵặƯứừửữự]+$",
            ErrorMessage = "Text must not contain special characters.")]
        public string AccountName { get; set; }
        [Required]
        [EmailAddress]
        public string AccountEmail { get; set; }

        [Required]
        public int AccountRole { get; set; }

        [Required]
        public string AccountPassword { get; set; }
    }
}
