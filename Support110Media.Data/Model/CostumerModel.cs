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
        public string CostumerName { get; set; }
        public string CostumerSurname { get; set; }
        public string CostumerPhoneNumber { get; set; }
        public string CostumerAddreess { get; set; }
        public string CostumerType { get; set; }
        public string CostumerMailAddress { get; set; }
        public string CostumerPassword { get; set; }
        public string CompanyName { get; set; }
        public ICollection<FileModel> FileModel { get; set; }
    }
}
