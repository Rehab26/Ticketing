﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.Web.Employee.Models
{
    public class ReplyModel
    {
        [Required]
        public int TicketId { get; set; }
        [MinLength(3)]
        public string Content { get; set; }
        public int UserId { get; set; }
        public DateTime Time { get; set; }
        [FileExtensions(Extensions = "jpg,png,gif,jpeg,bmp", ErrorMessage = "Please only upload image file")]
        public string Attachment { get; set; }
        public ClientView User { get; set; }
    }
}