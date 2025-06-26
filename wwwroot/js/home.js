/**
 * Home Page JavaScript
 * Handles featured content, animations, and page-specific functionality
 */

class HomePage {
  constructor() {
    this.featuredCourses = [];
    this.featuredClassrooms = [];
    this.init();
  }

  init() {
    this.loadFeaturedContent();
    this.initializeAnimations();
    this.setupSakuraEffect();
  }

  // Load featured courses and classrooms
  async loadFeaturedContent() {
    try {
      // Show loading states
      LoadingManager.show("featuredCourses", "Loading featured courses...");
      LoadingManager.show("featuredClassrooms", "Loading live classrooms...");

      // Simulate API calls with mock data
      await Promise.all([
        this.loadFeaturedCourses(),
        this.loadFeaturedClassrooms(),
      ]);
    } catch (error) {
      console.error("Error loading featured content:", error);
      Toast.error("Failed to load featured content");
    }
  }

  async loadFeaturedCourses() {
    // Mock featured courses data - exactly 3 featured courses
    const courses = [
      {
        id: 1,
        title: "Japanese for Beginners",
        instructor: "Hiroshi Tanaka",
        level: "Beginner",
        price: 49.99,
        students: 1205,
        rating: 4.8,
        lessons: 24,
        duration: "6 weeks",
        image: "🎌",
        description:
          "Start your Japanese journey with comprehensive basics covering hiragana, katakana, and essential vocabulary.",
        isEnrolled: this.checkEnrollment("course", 1),
      },
      {
        id: 2,
        title: "Conversational Japanese",
        instructor: "Akiko Sato",
        level: "Intermediate",
        price: 79.99,
        students: 856,
        rating: 4.9,
        lessons: 36,
        duration: "8 weeks",
        image: "💬",
        description:
          "Master everyday conversations and natural speech patterns used by native Japanese speakers.",
        isEnrolled: this.checkEnrollment("course", 2),
      },
      {
        id: 3,
        title: "Business Japanese Mastery",
        instructor: "Kenji Yamamoto",
        level: "Advanced",
        price: 129.99,
        students: 423,
        rating: 4.7,
        lessons: 48,
        duration: "12 weeks",
        image: "💼",
        description:
          "Professional Japanese communication for business meetings, presentations, and formal correspondence.",
        isEnrolled: this.checkEnrollment("course", 3),
      },
    ];

    this.featuredCourses = courses;
    this.renderFeaturedCourses();
  }

  async loadFeaturedClassrooms() {
    // Mock featured classrooms data - exactly 2 popular classrooms
    const classrooms = [
      {
        id: 1,
        title: "Morning Conversation Circle",
        instructor: "Yuki Tanaka",
        level: "Intermediate",
        price: 25.0,
        maxStudents: 8,
        enrolledStudents: 6,
        schedule: "Mon, Wed, Fri 9:00 AM JST",
        duration: "60 minutes",
        image: "☀️",
        description:
          "Start your day with friendly Japanese conversations in a supportive group environment.",
        nextSession: "Tomorrow 9:00 AM",
        isEnrolled: this.checkEnrollment("classroom", 1),
      },
      {
        id: 2,
        title: "Weekend Cultural Exchange",
        instructor: "Mai Suzuki",
        level: "All Levels",
        price: 30.0,
        maxStudents: 12,
        enrolledStudents: 9,
        schedule: "Saturdays 2:00 PM JST",
        duration: "90 minutes",
        image: "🎎",
        description:
          "Explore Japanese culture while practicing language skills with native speakers and fellow learners.",
        nextSession: "This Saturday 2:00 PM",
        isEnrolled: this.checkEnrollment("classroom", 2),
      },
    ];

    this.featuredClassrooms = classrooms;
    this.renderFeaturedClassrooms();
  }

  renderFeaturedCourses() {
    const container = document.getElementById("featuredCourses");
    if (!container) return;

    const coursesHTML = this.featuredCourses
      .map(
        (course) => `
      <div class="featured-card" data-course-id="${course.id}">
        ${course.isEnrolled ? '<div class="enrolled-badge">Enrolled</div>' : ""}
        <div class="featured-card-image">
          <span style="font-size: 4rem;">${course.image}</span>
        </div>
        <div class="featured-card-content">
          <h3 class="featured-card-title">${course.title}</h3>
          <div class="featured-card-meta">
            <span class="card-meta-item">
              <span>👨‍🏫</span>
              ${course.instructor}
            </span>
            <span class="card-meta-item">
              <span>📊</span>
              ${course.level}
            </span>
            <span class="card-meta-item">
              <span>⭐</span>
              ${course.rating} (${course.students})
            </span>
          </div>
          <p class="featured-card-description">${course.description}</p>
          <div class="featured-card-footer">
            <span class="card-price">$${course.price}</span>
            <a href="course-detail.html?id=${course.id}" class="btn btn-primary btn-sm">
              ${course.isEnrolled ? "Continue Learning" : "Learn More"}
            </a>
          </div>
        </div>
      </div>
    `,
      )
      .join("");

    container.innerHTML = coursesHTML;
    this.addCardAnimations();
  }

  renderFeaturedClassrooms() {
    const container = document.getElementById("featuredClassrooms");
    if (!container) return;

    const classroomsHTML = this.featuredClassrooms
      .map(
        (classroom) => `
      <div class="featured-card" data-classroom-id="${classroom.id}">
        ${classroom.isEnrolled ? '<div class="enrolled-badge">Enrolled</div>' : ""}
        <div class="featured-card-image">
          <span style="font-size: 4rem;">${classroom.image}</span>
        </div>
        <div class="featured-card-content">
          <h3 class="featured-card-title">${classroom.title}</h3>
          <div class="featured-card-meta">
            <span class="card-meta-item">
              <span>👨‍🏫</span>
              ${classroom.instructor}
            </span>
            <span class="card-meta-item">
              <span>📊</span>
              ${classroom.level}
            </span>
            <span class="card-meta-item">
              <span>👥</span>
              ${classroom.enrolledStudents}/${classroom.maxStudents}
            </span>
          </div>
          <p class="featured-card-description">${classroom.description}</p>
          <div class="featured-card-meta">
            <span class="card-meta-item">
              <span>🕒</span>
              ${classroom.schedule}
            </span>
            <span class="card-meta-item">
              <span>⏱️</span>
              ${classroom.duration}
            </span>
          </div>
          <div class="featured-card-footer">
            <span class="card-price">$${classroom.price}</span>
            <a href="classrooms-detail.html?id=${classroom.id}" class="btn btn-primary btn-sm">
              ${classroom.isEnrolled ? "Join Session" : "View Details"}
            </a>
          </div>
        </div>
      </div>
    `,
      )
      .join("");

    container.innerHTML = classroomsHTML;
    this.addCardAnimations();
  }

  checkEnrollment(type, id) {
    // Mock enrollment check
    const enrollments = {
      course: [1], // User is enrolled in course 1
      classroom: [2], // User is enrolled in classroom 2
    };

    const user = Utils.getCurrentUser();
    if (!user) return false;

    return enrollments[type]?.includes(id) || false;
  }

  addCardAnimations() {
    const cards = document.querySelectorAll(".featured-card");
    cards.forEach((card, index) => {
      card.style.opacity = "0";
      card.style.transform = "translateY(30px)";

      setTimeout(() => {
        card.style.transition = "all 0.6s ease";
        card.style.opacity = "1";
        card.style.transform = "translateY(0)";
      }, index * 200);
    });
  }

  initializeAnimations() {
    // Add intersection observer for scroll animations
    const observerOptions = {
      threshold: 0.1,
      rootMargin: "0px 0px -50px 0px",
    };

    const observer = new IntersectionObserver((entries) => {
      entries.forEach((entry) => {
        if (entry.isIntersecting) {
          entry.target.style.animationPlayState = "running";
          observer.unobserve(entry.target);
        }
      });
    }, observerOptions);

    // Observe all animated elements
    document.querySelectorAll(".animate-on-scroll").forEach((el) => {
      observer.observe(el);
    });

    // Stagger feature cards animation
    const featureCards = document.querySelectorAll(".feature-card");
    featureCards.forEach((card, index) => {
      card.style.animationDelay = `${index * 0.2}s`;
    });

    // Stagger testimonial cards animation
    const testimonialCards = document.querySelectorAll(".testimonial-card");
    testimonialCards.forEach((card, index) => {
      card.style.animationDelay = `${index * 0.3}s`;
    });
  }

  setupSakuraEffect() {
    const container = document.getElementById("sakuraContainer");
    if (!container) return;

    const createSakuraPetal = () => {
      const petal = document.createElement("div");
      petal.className = "sakura-petal";

      // Random horizontal position
      petal.style.left = Math.random() * 100 + "%";

      // Random size
      const size = Math.random() * 8 + 6;
      petal.style.width = size + "px";
      petal.style.height = size + "px";

      // Random animation duration
      const duration = Math.random() * 10 + 15;
      petal.style.animationDuration = duration + "s";

      // Random delay
      petal.style.animationDelay = Math.random() * 5 + "s";

      container.appendChild(petal);

      // Remove petal after animation
      setTimeout(
        () => {
          if (petal.parentNode) {
            petal.parentNode.removeChild(petal);
          }
        },
        (duration + 5) * 1000,
      );
    };

    // Create initial petals
    for (let i = 0; i < 15; i++) {
      setTimeout(createSakuraPetal, i * 1000);
    }

    // Continuously create new petals
    setInterval(createSakuraPetal, 3000);
  }
}

// Course interaction handlers
function handleCourseClick(courseId) {
  const course = homePage.featuredCourses.find((c) => c.id === courseId);
  if (course) {
    if (course.isEnrolled) {
      window.location.href = `course-detail.html?id=${courseId}&action=continue`;
    } else {
      window.location.href = `course-detail.html?id=${courseId}`;
    }
  }
}

function handleClassroomClick(classroomId) {
  const classroom = homePage.featuredClassrooms.find(
    (c) => c.id === classroomId,
  );
  if (classroom) {
    if (classroom.isEnrolled) {
      window.location.href = `classrooms-detail.html?id=${classroomId}&action=join`;
    } else {
      window.location.href = `classrooms-detail.html?id=${classroomId}`;
    }
  }
}

// Initialize home page when DOM is loaded
let homePage;

document.addEventListener("DOMContentLoaded", () => {
  homePage = new HomePage();

  // Add smooth scrolling for internal links
  document.querySelectorAll('a[href^="#"]').forEach((anchor) => {
    anchor.addEventListener("click", function (e) {
      e.preventDefault();
      const target = document.querySelector(this.getAttribute("href"));
      if (target) {
        target.scrollIntoView({
          behavior: "smooth",
          block: "start",
        });
      }
    });
  });

  // Add parallax effect to hero background
  let ticking = false;

  function updateParallax() {
    const scrolled = window.pageYOffset;
    const rate = scrolled * -0.5;
    const heroBackground = document.querySelector(".hero-background");

    if (heroBackground) {
      heroBackground.style.transform = `translateY(${rate}px)`;
    }

    ticking = false;
  }

  function requestTick() {
    if (!ticking) {
      requestAnimationFrame(updateParallax);
      ticking = true;
    }
  }

  window.addEventListener("scroll", requestTick, { passive: true });
});

// Handle window resize for responsive adjustments
window.addEventListener(
  "resize",
  Utils.debounce(() => {
    // Refresh animations and layouts if needed
    if (homePage) {
      homePage.addCardAnimations();
    }
  }, 250),
);
