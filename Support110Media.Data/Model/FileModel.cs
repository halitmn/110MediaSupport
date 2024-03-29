﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Support110Media.Data.Model
{
    public class FileModel
    {
        [Key]
        public int FileId { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public string FileUploadDate { get; set; }
        [Required]
        public string CallDate { get; set; }
        [Required]
        public string CallTime { get; set; }
        public int CostumerId { get; set; }
        public CostumerModel CostumerModel { get; set; }
    }
}
