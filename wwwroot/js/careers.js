// wwwroot/js/careers.js

// Khi DOM sẵn sàng, khởi tạo trang Careers
document.addEventListener("DOMContentLoaded", function () {
    initializeCareersPage();
    loadPositions();
});

/// Khởi tạo các bộ lọc (filter buttons)
function initializeCareersPage() {
    const filterButtons = document.querySelectorAll(".filter-btn");
    filterButtons.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            // Bỏ trạng thái active ở các nút khác
            filterButtons.forEach((b) => b.classList.remove("active"));
            // Đánh dấu nút hiện tại là active
            e.target.classList.add("active");
            // Lọc vị trí theo category của nút
            filterPositions(e.target.dataset.category);
        });
    });
}

/// Load toàn bộ vị trí (positions) từ mock data và render lên lưới
function loadPositions() {
    const positionsGrid = document.getElementById("positionsGrid");
    positionsGrid.innerHTML = mockPositions
        .map((position) => createPositionCard(position))
        .join("");
}

/// Tạo HTML cho từng vị trí
function createPositionCard(position) {
    return `
    <div class="position-card" data-category="${position.category}">
      <div class="position-header">
        <h3 class="position-title">${position.title}</h3>
        <div class="position-badges">
          <span class="position-type">${position.type}</span>
          <span class="position-location">${position.location}</span>
        </div>
      </div>
      <p class="position-description">${position.description}</p>
      <div class="position-skills">
        ${position.skills
            .map((skill) => `<span class="skill-tag">${skill}</span>`)
            .join("")}
      </div>
      <div class="position-footer">
        <span class="position-department">${position.department}</span>
        <button class="btn btn-primary" onclick="applyForPosition('${position.title}')">
          Apply Now
        </button>
      </div>
    </div>
  `;
}

/// Lọc hiển thị các position-card theo category
function filterPositions(category) {
    const positions = document.querySelectorAll(".position-card");
    positions.forEach((position) => {
        if (category === "all" || position.dataset.category === category) {
            position.style.display = "block";
        } else {
            position.style.display = "none";
        }
    });
}

/// Xử lý khi click Apply Now
function applyForPosition(title) {
    // Hiển thị toast thành công
    showToast(`Application process started for: ${title}`, "success");
}

/// Hiển thị toast (dùng chung với site.js hoặc bạn có thể copy hàm showToast từ global.js)
function showToast(message, type = "info") {
    const toast = document.createElement("div");
    toast.style.cssText = `
    position: fixed;
    top: 20px;
    right: 20px;
    background: ${type === "success"
            ? "#22c55e"
            : type === "error"
                ? "#ef4444"
                : type === "warning"
                    ? "#f59e0b"
                    : "#3b82f6"
        };
    color: white;
    padding: 12px 20px;
    border-radius: 8px;
    font-weight: 500;
    z-index: 10000;
    min-width: 200px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
    animation: slideInFromRight 0.3s ease;
  `;
    toast.innerHTML = `
    <div style="display: flex; align-items: center; justify-content: space-between;">
      <span>${message}</span>
      <button onclick="this.parentElement.parentElement.remove()" style="background: none; border: none; color: white; font-size: 18px; cursor: pointer; margin-left: 10px;">×</button>
    </div>
  `;
    document.body.appendChild(toast);
    setTimeout(() => {
        if (toast.parentElement) toast.remove();
    }, 5000);
}

/// Dữ liệu mock cho các vị trí tuyển dụng
const mockPositions = [
    {
        title: "Senior Frontend Engineer",
        category: "engineering",
        type: "Full-time",
        location: "Remote",
        department: "Engineering",
        description:
            "Build beautiful and responsive user interfaces for our learning platform using React, TypeScript, and modern web technologies.",
        skills: ["React", "TypeScript", "Next.js", "CSS", "REST APIs"],
    },
    {
        title: "Japanese Language Instructor",
        category: "education",
        type: "Full-time",
        location: "Tokyo/Remote",
        department: "Education",
        description:
            "Teach Japanese to international students through live classes and create engaging curriculum content.",
        skills: ["Native Japanese", "Teaching Experience", "JLPT", "Online Education"],
    },
    {
        title: "Product Designer",
        category: "design",
        type: "Full-time",
        location: "Remote",
        department: "Design",
        description:
            "Design intuitive learning experiences and user interfaces that make Japanese learning accessible and enjoyable.",
        skills: ["UI/UX Design", "Figma", "Design Systems", "User Research", "Prototyping"],
    },
    {
        title: "DevOps Engineer",
        category: "engineering",
        type: "Full-time",
        location: "Remote",
        department: "Engineering",
        description:
            "Manage and scale our cloud infrastructure to support millions of learning sessions worldwide.",
        skills: ["AWS", "Docker", "Kubernetes", "CI/CD", "Monitoring"],
    },
    {
        title: "Content Marketing Manager",
        category: "marketing",
        type: "Full-time",
        location: "San Francisco/Remote",
        department: "Marketing",
        description:
            "Create compelling content that showcases Japanese culture and our learning methodology to global audiences.",
        skills: ["Content Strategy", "SEO", "Social Media", "Analytics", "Japanese Culture"],
    },
    {
        title: "Curriculum Designer",
        category: "education",
        type: "Contract",
        location: "Remote",
        department: "Education",
        description:
            "Design structured learning paths and interactive exercises for different Japanese proficiency levels.",
        skills: ["Curriculum Design", "Japanese Pedagogy", "EdTech", "Learning Sciences"],
    },
];
