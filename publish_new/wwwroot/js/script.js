// Nihongo Sekai - Comprehensive JavaScript with Role-Based Authorization

// Global Configuration
const CONFIG = {
  API_BASE_URL: null, // Set to null for static deployment
  TOAST_DURATION: 5000,
  LOADING_MIN_DURATION: 500,
  USE_MOCK_DATA: true,
};

// Utility Functions
const Utils = {
  debounce(func, wait, immediate) {
    let timeout;
    return function executedFunction(...args) {
      const later = () => {
        timeout = null;
        if (!immediate) func(...args);
      };
      const callNow = immediate && !timeout;
      clearTimeout(timeout);
      timeout = setTimeout(later, wait);
      if (callNow) func(...args);
    };
  },

  formatCurrency(amount, currency = "USD") {
    return new Intl.NumberFormat("en-US", {
      style: "currency",
      currency: currency,
    }).format(amount);
  },

  formatDate(date, options = {}) {
    const defaultOptions = {
      year: "numeric",
      month: "short",
      day: "numeric",
    };
    return new Intl.DateTimeFormat("en-US", {
      ...defaultOptions,
      ...options,
    }).format(new Date(date));
  },

  truncate(text, length = 100) {
    if (text.length <= length) return text;
    return text.substring(0, length) + "...";
  },

  generateId() {
    return Math.random().toString(36).substr(2, 9);
  },

  scrollTo(element, offset = 0) {
    const targetPosition =
      element.getBoundingClientRect().top + window.pageYOffset - offset;
    window.scrollTo({
      top: targetPosition,
      behavior: "smooth",
    });
  },
};

// Role-Based Authorization System
const NihongoSekai = {
  user: null,
  isLoggedIn: false,

  // User roles and permissions
  roles: {
    Admin: {
      name: "Admin",
      permissions: [
        "dashboard",
        "manage_users",
        "manage_courses",
        "manage_classrooms",
        "view_analytics",
      ],
    },
    Partner: {
      name: "Partner",
      permissions: [
        "view_courses",
        "view_classrooms",
        "teach_classrooms",
        "manage_own_content",
      ],
    },
    Learner: {
      name: "Learner",
      permissions: [
        "purchase_courses",
        "enroll_classrooms",
        "rate_courses",
        "rate_classrooms",
        "view_progress",
      ],
    },
  },

  // Initialize user session
  init() {
    this.checkUserSession();
    this.updateUIBasedOnRole();
  },

  // Check if user is logged in and load user data
  checkUserSession() {
    const isLoggedIn = localStorage.getItem("user_logged_in") === "true";
    const userData = localStorage.getItem("user_data");

    if (isLoggedIn && userData) {
      try {
        this.user = JSON.parse(userData);
        this.isLoggedIn = true;
      } catch (error) {
        console.error("Error parsing user data:", error);
        this.logout();
      }
    }
  },

  // Check if user has specific permission
  hasPermission(permission) {
    if (!this.isLoggedIn || !this.user) return false;

    const userRole = this.roles[this.user.role];
    return userRole && userRole.permissions.includes(permission);
  },

  // Check if user can rate a course
  canRateCourse(courseId, userProgress = null) {
    if (!this.hasPermission("rate_courses")) return false;

    // Check if user has purchased and completed ≥50% of lessons
    const hasPurchased = this.hasUserPurchased("course", courseId);
    const completionRate = userProgress ? userProgress.completionRate || 0 : 0;

    return hasPurchased && completionRate >= 0.5;
  },

  // Check if user can rate a classroom
  canRateClassroom(classroomId, attendanceWeeks = 0) {
    if (!this.hasPermission("rate_classrooms")) return false;

    // Check if user has attended ≥1 week
    return attendanceWeeks >= 1;
  },

  // Check if user has purchased/enrolled in content
  hasUserPurchased(type, contentId) {
    // Mock data - in real app, fetch from API
    const mockPurchases = {
      courses: ["1", "4"], // Course IDs user has purchased
      classrooms: ["1", "2"], // Classroom IDs user has enrolled in
    };

    const purchases = mockPurchases[type + "s"] || [];
    return purchases.includes(contentId.toString());
  },

  // Get user's course progress
  getUserProgress(courseId) {
    // Mock progress data - in real app, fetch from API
    const mockProgress = {
      1: { completionRate: 0.75, lessonsCompleted: 18, totalLessons: 24 },
      4: { completionRate: 0.6, lessonsCompleted: 29, totalLessons: 48 },
    };

    return (
      mockProgress[courseId.toString()] || {
        completionRate: 0,
        lessonsCompleted: 0,
        totalLessons: 0,
      }
    );
  },

  // Get user's classroom attendance
  getClassroomAttendance(classroomId) {
    // Mock attendance data - in real app, fetch from API
    const mockAttendance = {
      1: { weeksAttended: 3, totalSessions: 12, attendanceRate: 0.85 },
      2: { weeksAttended: 2, totalSessions: 8, attendanceRate: 0.9 },
    };

    return (
      mockAttendance[classroomId.toString()] || {
        weeksAttended: 0,
        totalSessions: 0,
        attendanceRate: 0,
      }
    );
  },

  // Setup autocomplete functionality for course search
  setupAutocomplete() {
    const searchInput =
      document.getElementById("courseSearch") ||
      document.getElementById("classroomSearch");
    const dropdown = document.getElementById("autocompleteDropdown");

    if (!searchInput || !dropdown) return;

    let selectedIndex = -1;
    const suggestions = [];

    searchInput.addEventListener(
      "input",
      Utils.debounce((e) => {
        const query = e.target.value.trim().toLowerCase();

        if (query.length < 2) {
          hideDropdown();
          return;
        }

        // Get course/classroom data based on page
        const isCoursePage = window.location.pathname.includes("courses");
        const data = isCoursePage
          ? this.getCourseData()
          : this.getClassroomData();

        // Filter and limit to 5 suggestions
        const filtered = data
          .filter(
            (item) =>
              item.title.toLowerCase().includes(query) ||
              item.description.toLowerCase().includes(query) ||
              (item.category && item.category.toLowerCase().includes(query)),
          )
          .slice(0, 5);

        suggestions.length = 0;
        suggestions.push(...filtered);
        showSuggestions(query);
      }, 300),
    );

    searchInput.addEventListener("keydown", (e) => {
      if (!dropdown.style.display || dropdown.style.display === "none") return;

      switch (e.key) {
        case "ArrowDown":
          e.preventDefault();
          selectedIndex = Math.min(selectedIndex + 1, suggestions.length - 1);
          updateHighlight();
          break;
        case "ArrowUp":
          e.preventDefault();
          selectedIndex = Math.max(selectedIndex - 1, -1);
          updateHighlight();
          break;
        case "Enter":
          e.preventDefault();
          if (selectedIndex >= 0) {
            selectSuggestion(suggestions[selectedIndex]);
          }
          break;
        case "Escape":
          hideDropdown();
          break;
      }
    });

    document.addEventListener("click", (e) => {
      if (!searchInput.contains(e.target) && !dropdown.contains(e.target)) {
        hideDropdown();
      }
    });

    function showSuggestions(query) {
      if (suggestions.length === 0) {
        hideDropdown();
        return;
      }

      const isCoursePage = window.location.pathname.includes("courses");
      const html = suggestions
        .map(
          (item, index) => `
              <div class="autocomplete-suggestion" data-index="${index}" onclick="window.NihongoSekai.selectSuggestion(${JSON.stringify(item).replace(/"/g, "&quot;")})">
                <div class="suggestion-title">${highlightMatch(item.title, query)}</div>
                <div class="suggestion-meta">
                  <span>${item.level || "All Levels"}</span>
                  ${isCoursePage ? `<span>$${item.price}</span>` : `<span>${item.currentStudents}/${item.maxStudents} enrolled</span>`}
                  <span>${item.duration || item.schedule}</span>
                </div>
              </div>
            `,
        )
        .join("");

      dropdown.innerHTML = html;
      dropdown.style.display = "block";
      selectedIndex = -1;
    }

    function highlightMatch(text, query) {
      const regex = new RegExp(`(${query})`, "gi");
      return text.replace(
        regex,
        '<span class="suggestion-highlight">$1</span>',
      );
    }

    function updateHighlight() {
      const items = dropdown.querySelectorAll(".autocomplete-suggestion");
      items.forEach((item, index) => {
        item.classList.toggle("highlighted", index === selectedIndex);
      });
    }

    function hideDropdown() {
      dropdown.style.display = "none";
      selectedIndex = -1;
    }
  },

  // Get course data for autocomplete
  getCourseData() {
    return [
      {
        id: 1,
        title: "Complete Japanese for Beginners",
        description:
          "Start your Japanese journey with hiragana, katakana, and basic grammar concepts.",
        level: "Beginner",
        category: "grammar",
        price: 49,
        duration: "8 weeks",
      },
      {
        id: 2,
        title: "Japanese Grammar Mastery",
        description:
          "Deep dive into Japanese grammar structures and patterns for intermediate learners.",
        level: "Intermediate",
        category: "grammar",
        price: 89,
        duration: "12 weeks",
      },
      {
        id: 3,
        title: "Business Japanese Essentials",
        description:
          "Professional Japanese for workplace communication and business contexts.",
        level: "Advanced",
        category: "business",
        price: 129,
        duration: "10 weeks",
      },
      {
        id: 4,
        title: "JLPT N5 Preparation",
        description:
          "Complete preparation course for the Japanese Language Proficiency Test N5 level.",
        level: "Beginner",
        category: "jlpt",
        price: 79,
        duration: "16 weeks",
      },
      {
        id: 5,
        title: "Japanese Conversation Practice",
        description:
          "Improve your speaking skills with practical conversation scenarios and role-plays.",
        level: "Intermediate",
        category: "conversation",
        price: 69,
        duration: "8 weeks",
      },
      {
        id: 6,
        title: "Kanji Mastery Course",
        description:
          "Systematic approach to learning and remembering Japanese kanji characters.",
        level: "Elementary",
        category: "writing",
        price: 59,
        duration: "20 weeks",
      },
    ];
  },

  // Get classroom data for autocomplete
  getClassroomData() {
    return [
      {
        id: 1,
        title: "Morning Conversation Practice",
        description:
          "Join our morning conversation circle to practice speaking Japanese in a supportive environment.",
        level: "Beginner",
        currentStudents: 5,
        maxStudents: 8,
        schedule: "Mon, Wed, Fri 9:00 AM JST",
      },
      {
        id: 2,
        title: "JLPT Study Group",
        description: "Intensive study sessions focused on JLPT preparation.",
        level: "Intermediate",
        currentStudents: 7,
        maxStudents: 10,
        schedule: "Tue, Thu 7:00 PM JST",
      },
      {
        id: 3,
        title: "Business Japanese Workshop",
        description: "Advanced workshop for professionals.",
        level: "Advanced",
        currentStudents: 6,
        maxStudents: 6,
        schedule: "Sat 2:00 PM JST",
      },
      {
        id: 4,
        title: "Anime Japanese Club",
        description: "Learn Japanese through popular anime and manga.",
        level: "Elementary",
        currentStudents: 9,
        maxStudents: 12,
        schedule: "Sun 4:00 PM JST",
      },
      {
        id: 5,
        title: "Grammar Bootcamp",
        description:
          "Intensive grammar sessions covering particles, verb forms, and complex sentence structures.",
        level: "Intermediate",
        currentStudents: 4,
        maxStudents: 8,
        schedule: "Wed, Fri 6:00 PM JST",
      },
      {
        id: 6,
        title: "Cultural Exchange Chat",
        description:
          "Relaxed conversation sessions focusing on Japanese culture, traditions, and modern life.",
        level: "Beginner",
        currentStudents: 6,
        maxStudents: 10,
        schedule: "Sat 10:00 AM JST",
      },
    ];
  },

  // Select autocomplete suggestion
  selectSuggestion(item) {
    const searchInput =
      document.getElementById("courseSearch") ||
      document.getElementById("classroomSearch");
    const dropdown = document.getElementById("autocompleteDropdown");

    if (searchInput) {
      searchInput.value = item.title;
      // Trigger search
      const event = new Event("input", { bubbles: true });
      searchInput.dispatchEvent(event);
    }

    if (dropdown) {
      dropdown.style.display = "none";
    }
  },

  // Update classroom card hover behavior for Partners
  updateClassroomCardBehavior() {
    if (!this.isLoggedIn) return;

    const overlayButtons = document.querySelectorAll('[id^="overlayButton-"]');
    overlayButtons.forEach((button) => {
      const buttonText = button.querySelector(".button-text");
      if (buttonText && this.user && this.user.role === "Partner") {
        button.addEventListener("mouseenter", () => {
          buttonText.textContent = "View Classroom";
        });
        button.addEventListener("mouseleave", () => {
          buttonText.textContent = "Join Classroom";
        });
      }
    });
  },

  // Update UI based on user role
  updateUIBasedOnRole() {
    if (!this.isLoggedIn) return;

    // Update navigation
    this.updateNavigation();

    // Hide/show purchase buttons based on role
    this.updatePurchaseButtons();

    // Update rating forms
    this.updateRatingForms();
  },

  // Update navigation based on user role
  updateNavigation() {
    const navActions =
      document.querySelector(".nav-actions") ||
      document.getElementById("navActions");
    if (!navActions) return;

    if (this.isLoggedIn && this.user) {
      navActions.innerHTML = `
        <div class="user-profile-menu">
          <div class="user-avatar-container" id="userAvatarContainer">
            <div class="user-avatar" id="userAvatar">
              <svg class="cartoon-avatar" viewBox="0 0 100 100" fill="none" xmlns="http://www.w3.org/2000/svg">
                <circle cx="50" cy="50" r="50" fill="#4F46E5"/>
                <circle cx="50" cy="35" r="12" fill="#FFFFFF"/>
                <ellipse cx="50" cy="70" rx="20" ry="18" fill="#FFFFFF"/>
                <circle cx="45" cy="32" r="2" fill="#1F2937"/>
                <circle cx="55" cy="32" r="2" fill="#1F2937"/>
                <path d="M45 38 Q50 42 55 38" stroke="#1F2937" stroke-width="1.5" fill="none"/>
              </svg>
            </div>
            <div class="online-indicator"></div>
          </div>

          <div class="profile-dropdown" id="profileDropdown" style="display: none;">
            <div class="dropdown-header">
              <div class="dropdown-avatar">
                <svg class="cartoon-avatar" viewBox="0 0 100 100" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <circle cx="50" cy="50" r="50" fill="#4F46E5"/>
                  <circle cx="50" cy="35" r="12" fill="#FFFFFF"/>
                  <ellipse cx="50" cy="70" rx="20" ry="18" fill="#FFFFFF"/>
                  <circle cx="45" cy="32" r="2" fill="#1F2937"/>
                  <circle cx="55" cy="32" r="2" fill="#1F2937"/>
                  <path d="M45 38 Q50 42 55 38" stroke="#1F2937" stroke-width="1.5" fill="none"/>
                </svg>
              </div>
              <div class="dropdown-user-info">
                <div class="dropdown-name">${this.user.name}</div>
                <div class="dropdown-role">${this.user.role}</div>
              </div>
            </div>

            <div class="dropdown-menu">
              ${this.getDropdownMenuItems()}
            </div>
          </div>
        </div>
      `;

      // Setup dropdown functionality
      this.setupProfileDropdown();
    }
  },

  // Get dropdown menu items based on user role
  getDropdownMenuItems() {
    const baseItems = `
      <a href="#" class="dropdown-item" onclick="NihongoSekai.navigateToProfile()">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
          <circle cx="12" cy="7" r="4"></circle>
        </svg>
        My Profile
      </a>
    `;

    let roleSpecificItems = "";

    switch (this.user.role) {
      case "Learner":
        roleSpecificItems = `
          <a href="#" class="dropdown-item" onclick="NihongoSekai.navigateToMyCourses()">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M2 3h6a4 4 0 0 1 4 4v14a3 3 0 0 0-3-3H2z"></path>
              <path d="M22 3h-6a4 4 0 0 0-4 4v14a3 3 0 0 1 3-3h7z"></path>
            </svg>
            My Courses
            <div class="menu-progress-preview">3 active</div>
          </a>
          <a href="#" class="dropdown-item" onclick="NihongoSekai.navigateToMyClassrooms()">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M17 10.5V7a1 1 0 0 0-1-1H4a1 1 0 0 0-1 1v10a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1v-3.5l4 4v-11l-4 4z"></path>
            </svg>
            My Classrooms
            <div class="menu-join-indicator">2 available</div>
          </a>
          <a href="#" class="dropdown-item" onclick="NihongoSekai.navigateToTransactions()">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <rect x="1" y="4" width="22" height="16" rx="2" ry="2"></rect>
              <line x1="1" y1="10" x2="23" y2="10"></line>
            </svg>
            Transactions
          </a>
        `;
        break;

      case "Partner":
      case "Teacher":
        roleSpecificItems = `
          <a href="#" class="dropdown-item" onclick="NihongoSekai.navigateToMyClassrooms()">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M17 10.5V7a1 1 0 0 0-1-1H4a1 1 0 0 0-1 1v10a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1v-3.5l4 4v-11l-4 4z"></path>
            </svg>
            My Classrooms
            <div class="menu-edit-indicator">5 hosted</div>
          </a>
        `;
        break;

      case "Admin":
        roleSpecificItems = `
          <a href="dashboard.html" class="dropdown-item">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <rect x="3" y="3" width="7" height="9"></rect>
              <rect x="14" y="3" width="7" height="5"></rect>
              <rect x="14" y="12" width="7" height="9"></rect>
              <rect x="3" y="16" width="7" height="5"></rect>
            </svg>
            Dashboard
          </a>
        `;
        break;
    }

    const settingsAndLogout = `
      <div class="dropdown-divider"></div>
      <a href="#" class="dropdown-item" onclick="NihongoSekai.navigateToSettings()">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <circle cx="12" cy="12" r="3"></circle>
          <path d="M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 0 1 0 2.83 2 2 0 0 1-2.83 0l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-2 2 2 2 0 0 1-2-2v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 0 1-2.83 0 2 2 0 0 1 0-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1-2-2 2 2 0 0 1 2-2h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 0 1 0-2.83 2 2 0 0 1 2.83 0l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1 1.51V3a2 2 0 0 1 2-2 2 2 0 0 1 2 2v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 0 1 2.83 0 2 2 0 0 1 0 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 2 2 2 2 0 0 1-2 2h-.09a1.65 1.65 0 0 0-1.51 1z"></path>
        </svg>
        Settings
      </a>
      <a href="#" class="dropdown-item logout" onclick="NihongoSekai.showLogoutConfirmation()">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"></path>
          <polyline points="16,17 21,12 16,7"></polyline>
          <line x1="21" y1="12" x2="9" y2="12"></line>
        </svg>
        Log out
      </a>
    `;

    return baseItems + roleSpecificItems + settingsAndLogout;
  },

  // Setup profile dropdown functionality
  setupProfileDropdown() {
    const avatarContainer = document.getElementById("userAvatarContainer");
    const dropdown = document.getElementById("profileDropdown");

    if (avatarContainer && dropdown) {
      avatarContainer.addEventListener("click", (e) => {
        e.stopPropagation();
        const isVisible = dropdown.style.display === "block";
        dropdown.style.display = isVisible ? "none" : "block";
      });

      // Close dropdown when clicking outside
      document.addEventListener("click", (e) => {
        if (
          !avatarContainer.contains(e.target) &&
          !dropdown.contains(e.target)
        ) {
          dropdown.style.display = "none";
        }
      });
    }
  },

  // Show logout confirmation modal
  showLogoutConfirmation() {
    const modal = document.createElement("div");
    modal.className = "logout-confirmation-modal";
    modal.innerHTML = `
      <div class="modal-overlay" onclick="this.parentElement.remove()">
        <div class="modal-content" onclick="event.stopPropagation()">
          <div class="modal-header">
            <h3>Confirm Logout</h3>
          </div>
          <div class="modal-body">
            <p>Are you sure you want to log out?</p>
          </div>
          <div class="modal-actions">
            <button class="btn btn-outline" onclick="this.closest('.logout-confirmation-modal').remove()">Cancel</button>
            <button class="btn btn-primary" onclick="NihongoSekai.confirmLogout()">Confirm</button>
          </div>
        </div>
      </div>
    `;
    document.body.appendChild(modal);
  },

  // Confirm logout action
  confirmLogout() {
    const modal = document.querySelector(".logout-confirmation-modal");
    if (modal) modal.remove();
    this.logout();
  },

  // Navigation methods
  navigateToProfile() {
    this.showToast("Navigating to profile page...", "info");
    // In real app: window.location.href = "profile.html";
  },

  navigateToMyCourses() {
    this.showToast("Loading your courses...", "info");
    // In real app: window.location.href = "my-courses.html";
  },

  navigateToMyClassrooms() {
    this.showToast("Loading your classrooms...", "info");
    // In real app: window.location.href = "my-classrooms.html";
  },

  navigateToTransactions() {
    this.showToast("Loading transaction history...", "info");
    // In real app: window.location.href = "transactions.html";
  },

  navigateToSettings() {
    this.showToast("Opening settings...", "info");
    // In real app: window.location.href = "settings.html";
  },

  // Update purchase/enroll buttons based on role
  updatePurchaseButtons() {
    const purchaseButtons = document.querySelectorAll(
      '.course-enroll-btn, .cta-button, [onclick*="enroll"]',
    );

    purchaseButtons.forEach((button) => {
      if (
        !this.hasPermission("purchase_courses") &&
        !this.hasPermission("enroll_classrooms")
      ) {
        // Partners cannot purchase/enroll
        button.disabled = true;
        button.style.background = "var(--color-gray-400)";
        button.style.cursor = "not-allowed";
        button.innerHTML = `
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="12" cy="12" r="10"></circle>
            <path d="M15 9l-6 6M9 9l6 6"></path>
          </svg>
          View Only (${this.user.role})
        `;

        // Remove onclick handlers
        button.onclick = () => {
          this.showRoleMessage("purchase_denied");
        };
      }
    });
  },

  // Update rating forms based on permissions
  updateRatingForms() {
    // Get current page context
    const urlParams = new URLSearchParams(window.location.search);
    const courseId = urlParams.get("id");
    const currentPage = window.location.pathname;

    if (currentPage.includes("course-detail.html") && courseId) {
      this.updateCourseRatingForm(courseId);
    } else if (currentPage.includes("classroom-detail.html") && courseId) {
      this.updateClassroomRatingForm(courseId);
    }
  },

  // Update course rating form
  updateCourseRatingForm(courseId) {
    const ratingForms = document.querySelectorAll(
      '.rating-form, [data-rating-type="course"]',
    );

    ratingForms.forEach((form) => {
      if (!this.hasPermission("rate_courses")) {
        form.style.display = "none";
        return;
      }

      const userProgress = this.getUserProgress(courseId);
      const canRate = this.canRateCourse(courseId, userProgress);

      if (!canRate) {
        const completionRate = Math.round(userProgress.completionRate * 100);
        form.innerHTML = `
          <div style="background: var(--color-warning-50); border: 1px solid var(--color-warning-200); border-radius: var(--radius-lg); padding: var(--spacing-lg); text-align: center;">
            <div style="color: var(--color-warning-700); font-weight: 600; margin-bottom: var(--spacing-sm);">
              Complete more lessons to rate this course
            </div>
            <div style="color: var(--color-warning-600); font-size: 0.875rem;">
              Progress: ${completionRate}% (need 50% to rate)
            </div>
            <div style="margin-top: var(--spacing-sm);">
              <div style="width: 100%; height: 6px; background: var(--color-warning-200); border-radius: 3px; overflow: hidden;">
                <div style="width: ${completionRate}%; height: 100%; background: var(--color-warning-500); transition: width 0.3s ease;"></div>
              </div>
            </div>
          </div>
        `;
      }
    });
  },

  // Update classroom rating form
  updateClassroomRatingForm(classroomId) {
    const ratingForms = document.querySelectorAll(
      '.rating-form, [data-rating-type="classroom"]',
    );

    ratingForms.forEach((form) => {
      if (!this.hasPermission("rate_classrooms")) {
        form.style.display = "none";
        return;
      }

      const attendance = this.getClassroomAttendance(classroomId);
      const canRate = this.canRateClassroom(
        classroomId,
        attendance.weeksAttended,
      );

      if (!canRate) {
        form.innerHTML = `
          <div style="background: var(--color-warning-50); border: 1px solid var(--color-warning-200); border-radius: var(--radius-lg); padding: var(--spacing-lg); text-align: center;">
            <div style="color: var(--color-warning-700); font-weight: 600; margin-bottom: var(--spacing-sm);">
              Attend more sessions to rate this classroom
            </div>
            <div style="color: var(--color-warning-600); font-size: 0.875rem;">
              Attendance: ${attendance.weeksAttended} weeks (need 1+ weeks to rate)
            </div>
          </div>
        `;
      }
    });
  },

  // Show enrollment button with role check
  showEnrollmentButton(contentType, contentId) {
    if (!this.isLoggedIn) {
      return `
        <button onclick="window.location.href='login.html'" class="cta-button">
          Sign In to ${contentType === "course" ? "Purchase" : "Enroll"}
        </button>
      `;
    }

    const hasPermission =
      contentType === "course"
        ? this.hasPermission("purchase_courses")
        : this.hasPermission("enroll_classrooms");

    if (!hasPermission) {
      return `
        <button disabled class="cta-button" style="background: var(--color-gray-400); cursor: not-allowed;">
          View Only (${this.user.role})
        </button>
      `;
    }

    const hasPurchased = this.hasUserPurchased(contentType, contentId);
    if (hasPurchased) {
      return `
        <button disabled class="cta-button" style="background: var(--color-success);">
          ${contentType === "course" ? "Purchased" : "Enrolled"}
        </button>
      `;
    }

    return `
      <button onclick="NihongoSekai.handleEnrollment('${contentType}', '${contentId}')" class="cta-button">
        ${contentType === "course" ? "Purchase Course" : "Enroll in Classroom"}
      </button>
    `;
  },

  // Handle enrollment/purchase with role checking
  handleEnrollment(contentType, contentId) {
    if (!this.isLoggedIn) {
      localStorage.setItem("redirect_after_login", window.location.href);
      window.location.href = "login.html";
      return;
    }

    const hasPermission =
      contentType === "course"
        ? this.hasPermission("purchase_courses")
        : this.hasPermission("enroll_classrooms");

    if (!hasPermission) {
      this.showRoleMessage(
        contentType === "course" ? "purchase_denied" : "enroll_denied",
      );
      return;
    }

    // Proceed with enrollment/purchase
    this.showToast(
      `Successfully ${contentType === "course" ? "purchased" : "enrolled in"} ${contentType}!`,
      "success",
    );

    // Update UI to reflect purchase/enrollment
    this.updatePurchaseButtons();
  },

  // Logout user
  logout() {
    localStorage.removeItem("user_logged_in");
    localStorage.removeItem("user_data");
    localStorage.removeItem("auth_token");
    localStorage.removeItem("redirect_after_login");

    this.user = null;
    this.isLoggedIn = false;

    // Redirect to home
    window.location.href = "index.html";
  },

  // Show role-based messages
  showRoleMessage(action, requiredRole = null) {
    const messages = {
      purchase_denied: `Only students can purchase courses. ${this.user.role}s can view course content but cannot make purchases.`,
      enroll_denied: `Only students can enroll in classrooms. ${this.user.role}s can view classroom information but cannot enroll.`,
      rate_denied:
        "You need to complete at least 50% of the course to leave a rating.",
      rate_classroom_denied:
        "You need to attend at least 1 week of classes to rate this classroom.",
      login_required: "Please sign in to access this feature.",
    };

    const message = messages[action] || "Access denied.";
    this.showToast(message, "warning");
  },

  // Enhanced toast with role-based styling
  showToast(message, type = "info") {
    const toast = document.createElement("div");
    toast.style.cssText = `
      position: fixed;
      top: 20px;
      right: 20px;
      background: ${type === "success" ? "#22c55e" : type === "error" ? "#ef4444" : type === "warning" ? "#f59e0b" : "#3b82f6"};
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
      if (toast.parentElement) {
        toast.remove();
      }
    }, CONFIG.TOAST_DURATION);
  },

  // Show logout confirmation modal
  showLogoutConfirmation() {
    const modal = document.createElement("div");
    modal.className = "logout-confirmation-modal";
    modal.innerHTML = `
      <div class="modal-overlay" onclick="this.parentElement.remove()">
        <div class="modal-content" onclick="event.stopPropagation()">
          <div class="modal-header">
            <h3>Confirm Logout</h3>
          </div>
          <div class="modal-body">
            <p>Are you sure you want to log out?</p>
          </div>
          <div class="modal-actions">
            <button class="btn btn-outline" onclick="this.closest('.logout-confirmation-modal').remove()">Cancel</button>
            <button class="btn btn-primary" onclick="NihongoSekai.confirmLogout()">Confirm</button>
          </div>
        </div>
      </div>
    `;
    document.body.appendChild(modal);
  },

  // Confirm logout action
  confirmLogout() {
    const modal = document.querySelector(".logout-confirmation-modal");
    if (modal) modal.remove();
    this.logout();
  },

  // Get enrollment date for classroom
  getEnrollmentDate(type, contentId) {
    // Mock data - in real app, fetch from API
    const mockEnrollments = {
      classroom: {
        1: "2024-01-01",
        2: "2024-01-15",
      },
    };

    return mockEnrollments[type]?.[contentId.toString()] || null;
  },

  // Enhanced purchase/enroll button logic
  getPurchaseButtonHTML(contentType, contentId, price) {
    const hasPurchased = this.hasUserPurchased(contentType, contentId);

    if (hasPurchased) {
      return `
        <button disabled class="purchased-button">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M9 12l2 2 4-4"></path>
            <circle cx="12" cy="12" r="9"></circle>
          </svg>
          ${contentType === "course" ? "Purchased" : "Enrolled"}
        </button>
      `;
    }

    return `
      <button class="cta-button" onclick="NihongoSekai.handlePurchase('${contentType}', '${contentId}', ${price})">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <circle cx="9" cy="21" r="1"></circle>
          <circle cx="20" cy="21" r="1"></circle>
          <path d="M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6"></path>
        </svg>
        ${contentType === "course" ? "Buy Now" : "Enroll"} - $${price}
      </button>
    `;
  },

  // API Functions
  async fetchAPI(endpoint, options = {}) {
    if (!CONFIG.API_BASE_URL || CONFIG.USE_MOCK_DATA) {
      return { success: false, error: "Static deployment - using mock data" };
    }

    const url = `${CONFIG.API_BASE_URL}${endpoint}`;
    const defaultOptions = {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("auth_token")}`,
      },
    };

    try {
      const response = await fetch(url, { ...defaultOptions, ...options });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const data = await response.json();
      return { success: true, data };
    } catch (error) {
      console.error("API Error:", error);
      return { success: false, error: error.message };
    }
  },
};

// Initialize application when DOM is ready
document.addEventListener("DOMContentLoaded", () => {
  NihongoSekai.init();
});

// Export for global access
window.NihongoSekai = NihongoSekai;
window.Utils = Utils;
window.CONFIG = CONFIG;

// Global helper functions
window.selectSuggestion = function (item) {
  if (window.NihongoSekai && window.NihongoSekai.selectSuggestion) {
    window.NihongoSekai.selectSuggestion(item);
  }
};
