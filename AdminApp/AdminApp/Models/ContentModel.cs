﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminApp.Models
{
    public class ContentModel
    {
        public int Id { get; set; }
        [Display(Name = "Headline")]
        [Required]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)] // Make it a textare in the form
        [Display(Name = "Message")]
        [Required]
        public string BodyText { get; set; }
    }
}