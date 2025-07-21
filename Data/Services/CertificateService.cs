using JapaneseLearningPlatform.Data;               // AppDbContext
using JapaneseLearningPlatform.Models;             // CourseCertificate
using JapaneseLearningPlatform.Data.Services;      // ICertificateService
using Microsoft.AspNetCore.Hosting;                // IWebHostEnvironment
using Microsoft.AspNetCore.Mvc;                    // IUrlHelper
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;               // Include()
using JapaneseLearningPlatform.Data;
using Microsoft.AspNetCore.Mvc;
namespace JapaneseLearningPlatform.Data.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CertificateService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<CourseCertificate> GenerateAsync(string userId, int courseId, IUrlHelper urlHelper)
        {
            // Tạo tên file cho chứng chỉ
            var fileName = $"{userId}_{courseId}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
            var folder = Path.Combine(_env.WebRootPath, "uploads", "certificates");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            var filePath = Path.Combine(folder, fileName);

            // Nếu chưa có logic render PDF (chưa làm thì cứ để đường dẫn là null)
            // Thay vì bỏ qua, ta có thể để null hoặc string trống cho các trường bị thiếu
            string fileUrl = $"/uploads/certificates/{fileName}";  // Đây là URL mặc định, bạn có thể thay đổi sau khi tạo file PDF thực sự.

            // Tạo một CourseCertificate với giá trị mặc định nếu thiếu
            var cert = new CourseCertificate
            {
                UserId = userId,
                CourseId = courseId,
                IssuedAt = DateTime.UtcNow, // Nếu chưa có giá trị cho ngày, ta có thể để mặc định là giờ hiện tại
                FileUrl = fileUrl ?? ""  // Nếu FileUrl thiếu, ta có thể để là chuỗi rỗng hoặc null
            };

            // Thêm chứng chỉ vào cơ sở dữ liệu
            _context.CourseCertificates.Add(cert);
            await _context.SaveChangesAsync();

            return cert;
        }


        public async Task<List<CourseCertificate>> GetByUserAsync(string userId)
            => await _context.CourseCertificates
                             .Include(c => c.Course)
                             .Where(c => c.UserId == userId)
                             .OrderByDescending(c => c.IssuedAt)
                             .ToListAsync();
    }
}