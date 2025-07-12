using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Models.Partner;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ReviewPartnerVM
    {
        // chính là Id của PartnerProfile
        public int Id { get; set; }

        // có sẵn PartnerProfile và navigation về User
        public PartnerProfile Profile { get; set; } = null!;

        // để tiện dùng trong view
        public string FullName => Profile.User.FullName;
        public string Email => Profile.User.Email;
        public YearsOfExperience YearsOfExperience => Profile.YearsOfExperience;
        public IEnumerable<SpecializationType> Specializations
            => Profile.Specializations.Select(s => s.Specialization);
        public IEnumerable<Models.Partner.PartnerDocument> Documents
            => Profile.Documents;

    //    public int Id { get; set; }                             // → PartnerProfile.Id
    //    public string FullName { get; set; } = null!;           // → từ partner.User.FullName
    //    public string Email { get; set; } = null!;              // → từ partner.User.Email
    //    public YearsOfExperience YearsOfExperience { get; set; }// → partner.YearsOfExperience.Value
    //    public List<SpecializationType> Specializations { get; set; } = new();
    //    public List<PartnerDocument> Documents { get; set; } = new();
    }
}
