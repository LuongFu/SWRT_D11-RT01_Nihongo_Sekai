﻿using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Models.Partner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class EditPartnerProfileVM
    {
        // User fields
        [Display(Name = "Tên đầy đủ")]
        [Required]
        public string FullName { get; set; } = "";

        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        // PartnerProfile fields
        [Display(Name = "Năm kinh nghiệm")]
        public YearsOfExperience YearsOfExperience { get; set; }

        [Display(Name = "Chuyên môn")]
        public List<SpecializationType> SelectedSpecializations { get; set; }
            = new();

        // For rendering dropdown & checkboxes
        public List<SelectListItem> ExperienceOptions { get; set; }
            = new();

        public List<SpecializationType> AllSpecializations { get; set; }
            = new();

        // File uploads
        [Display(Name = "Tải tài liệu mới")]
        public IFormFileCollection? NewDocuments { get; set; }
        public List<int> DeletedDocumentIds { get; set; } = new();


        // Show existing docs
        public List<PartnerDocument> ExistingDocuments { get; set; }
            = new();
    }
}
