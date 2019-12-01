using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Support110Media.Data.Model
{
    public class CostumerModel
    {
        [Key]
        public int CostumerId { get; set; }
        [Required]
        public string CostumerName { get; set; }
        [Required]
        public string CostumerSurname { get; set; }
        [Phone]
        public string CostumerPhoneNumber { get; set; }
        public string CostumerAddreess { get; set; }
        public string CostumerType { get; set; }
        [EmailAddress]
        public string CostumerMailAddress { get; set; }
        [Required]
        public string CostumerPassword { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public ICollection<FileModel> FileModel { get; set; }
    }
}
