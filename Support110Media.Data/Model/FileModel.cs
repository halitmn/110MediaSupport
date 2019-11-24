using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Support110Media.Data.Model
{
    public class FileModel
    {
        [Key]
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileUploadDate { get; set; }
        public string CallDate { get; set; }
        public string CallTime { get; set; }
        public int CostumerId { get; set; }
        public CostumerModel CostumerModel { get; set; }
    }
}
