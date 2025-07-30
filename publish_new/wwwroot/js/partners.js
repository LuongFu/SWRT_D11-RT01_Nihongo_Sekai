/**
 * Partners Page JavaScript
 * Handles partner listing, chat modal, and partnership form
 */

class PartnersPage {
  constructor() {
    this.featuredPartners = [];
    this.allPartners = [];
    this.filteredPartners = [];
    this.currentFilters = {
      search: "",
      expertise: "",
      rating: "",
      availability: "",
    };
    this.currentChatPartner = null;

    this.init();
  }

  init() {
    this.loadFeaturedPartners();
    this.loadAllPartners();
    this.setupEventListeners();
    this.setupChatModal();
  }

  async loadFeaturedPartners() {
    try {
      // Load 3 featured partners with photo, testimonial, rating
      this.featuredPartners = [
        {
          id: 1,
          name: "Hiroshi Tanaka",
          photo:
            "https://api.dicebear.com/7.x/avataaars/svg?seed=HiroshiSensei&backgroundColor=b6e3f4",
          expertise: "Business Japanese Expert",
          rating: 4.9,
          testimonial:
            "Hiroshi's business Japanese classes helped me ace my job interview at a Japanese company. His real-world experience is invaluable!",
          experience: "8 years",
          students: 250,
          hourlyRate: 45,
        },
        {
          id: 2,
          name: "Akiko Sato",
          photo:
            "https://api.dicebear.com/7.x/avataaars/svg?seed=AkikoSensei&backgroundColor=ffd93d",
          expertise: "Conversation Specialist",
          rating: 4.8,
          testimonial:
            "Akiko makes learning Japanese conversation so natural and fun. I went from shy beginner to confident speaker in just 3 months!",
          experience: "6 years",
          students: 180,
          hourlyRate: 35,
        },
        {
          id: 3,
          name: "Kenji Yamamoto",
          photo:
            "https://api.dicebear.com/7.x/avataaars/svg?seed=KenjiSensei&backgroundColor=c0aede",
          expertise: "JLPT Preparation",
          rating: 4.9,
          testimonial:
            "Thanks to Kenji's structured approach, I passed JLPT N2 on my first try. His test strategies are incredibly effective!",
          experience: "10 years",
          students: 320,
          hourlyRate: 50,
        },
      ];

      this.renderFeaturedPartners();
    } catch (error) {
      console.error("Error loading featured partners:", error);
    }
  }

  async loadAllPartners() {
    try {
      // Load full directory of partners
      this.allPartners = [
        ...this.featuredPartners,
        {
          id: 4,
          name: "Yuki Nakamura",
          photo:
            "https://api.dicebear.com/7.x/avataaars/svg?seed=YukiSensei&backgroundColor=ffdfbf",
          expertise: "Grammar Specialist",
          rating: 4.7,
          experience: "5 years",
          students: 145,
          hourlyRate: 30,
          bio: "Specializing in Japanese grammar structures and patterns. I help students understand the logic behind Japanese grammar rules.",
          availability: "Available",
        },
        {
          id: 5,
          name: "Mai Suzuki",
          photo:
            "https://api.dicebear.com/7.x/avataaars/svg?seed=MaiSensei&backgroundColor=fce7f3",
          expertise: "Cultural Studies",
          rating: 4.8,
          experience: "7 years",
          students: 210,
          hourlyRate: 40,
          bio: "Learn Japanese through cultural immersion. I combine language learning with deep cultural insights and traditions.",
          availability: "Available",
        },
        {
          id: 6,
          name: "Tomoko Watanabe",
          photo:
            "https://api.dicebear.com/7.x/avataaars/svg?seed=TomokoSensei&backgroundColor=dbeafe",
          expertise: "Test Preparation",
          rating: 4.9,
          experience: "9 years",
          students: 280,
          hourlyRate: 48,
          bio: "JLPT specialist with proven track record. My students have 95% pass rate on their target JLPT levels.",
          availability: "Busy",
        },
        {
          id: 7,
          name: "Ryo Taniguchi",
          photo:
            "https://api.dicebear.com/7.x/avataaars/svg?seed=RyoSensei&backgroundColor=f0fdf4",
          expertise: "Travel Japanese",
          rating: 4.6,
          experience: "4 years",
          students: 120,
          hourlyRate: 28,
          bio: "Former tour guide specializing in practical Japanese for travelers. Learn essential phrases for your Japan trip!",
          availability: "Available",
        },
        {
          id: 8,
          name: "Emiko Yoshida",
          photo:
            "https://api.dicebear.com/7.x/avataaars/svg?seed=EmikoSensei&backgroundColor=fef3c7",
          expertise: "Pronunciation",
          rating: 4.8,
          experience: "6 years",
          students: 190,
          hourlyRate: 38,
          bio: "Voice coach and pronunciation expert. I help students achieve natural Japanese pronunciation and intonation.",
          availability: "Available",
        },
      ];

      this.filteredPartners = [...this.allPartners];
      this.renderPartnersDirectory();
    } catch (error) {
      console.error("Error loading partners directory:", error);
    }
  }

  renderFeaturedPartners() {
    const container = document.getElementById("featuredPartnersGrid");
    if (!container) return;

    const partnersHTML = this.featuredPartners
      .map(
        (partner) => `
      <div class="featured-partner-card animate-on-scroll">
        <div class="partner-photo">
          <img src="${partner.photo}" alt="${partner.name}" />
        </div>
        <h3 class="partner-name">${partner.name}</h3>
        <div class="partner-expertise">${partner.expertise}</div>
        <div class="partner-rating">
          <span class="rating-stars">${this.generateStars(partner.rating)}</span>
          <span class="rating-value">${partner.rating}</span>
        </div>
        <p class="partner-testimonial">"${partner.testimonial}"</p>
        <button class="btn btn-primary partner-connect-btn" onclick="partnersPage.openChat(${partner.id})">
          Connect with ${partner.name.split(" ")[0]}
        </button>
      </div>
    `,
      )
      .join("");

    container.innerHTML = partnersHTML;
  }

  renderPartnersDirectory() {
    const container = document.getElementById("partnersGrid");
    if (!container) return;

    if (this.filteredPartners.length === 0) {
      this.showEmpty();
      return;
    }

    const partnersHTML = this.filteredPartners
      .map(
        (partner) => `
      <div class="partner-card animate-on-scroll">
        <div class="partner-card-header">
          <div class="partner-avatar">
            <img src="${partner.photo}" alt="${partner.name}" />
          </div>
          <div class="partner-info">
            <h3>${partner.name}</h3>
            <div class="partner-specialization">${partner.expertise}</div>
            <div class="partner-meta">
              <div class="meta-item">
                <span>⭐</span>
                <span>${partner.rating} (${partner.students} students)</span>
              </div>
              <div class="meta-item">
                <span>🎓</span>
                <span>${partner.experience} experience</span>
              </div>
            </div>
          </div>
        </div>
        
        <div class="partner-card-body">
          ${partner.bio ? `<p class="partner-bio">${partner.bio}</p>` : ""}
          
          <div class="partner-stats">
            <div class="partner-rating-display">
              <span class="rating-stars">${this.generateStars(partner.rating)}</span>
              <span class="rating-value">${partner.rating}</span>
            </div>
            <div class="partner-students">${partner.students} students</div>
          </div>
          
          <div class="partner-footer">
            <div class="partner-hourly-rate">$${partner.hourlyRate}/hr</div>
            <button class="btn btn-primary" onclick="partnersPage.openChat(${partner.id})">
              Connect
            </button>
          </div>
        </div>
      </div>
    `,
      )
      .join("");

    container.innerHTML = partnersHTML;

    // Add staggered animation
    const cards = container.querySelectorAll(".partner-card");
    cards.forEach((card, index) => {
      card.style.opacity = "0";
      card.style.transform = "translateY(30px)";

      setTimeout(() => {
        card.style.transition = "all 0.6s ease";
        card.style.opacity = "1";
        card.style.transform = "translateY(0)";
      }, index * 100);
    });
  }

  setupEventListeners() {
    // Search input
    const searchInput = document.getElementById("partnerSearch");
    if (searchInput) {
      searchInput.addEventListener(
        "input",
        Utils.debounce((e) => {
          this.currentFilters.search = e.target.value;
          this.applyFilters();
        }, 300),
      );
    }

    // Filter selects
    const expertiseFilter = document.getElementById("expertiseFilter");
    const ratingFilter = document.getElementById("ratingFilter");
    const availabilityFilter = document.getElementById("availabilityFilter");

    if (expertiseFilter) {
      expertiseFilter.addEventListener("change", (e) => {
        this.currentFilters.expertise = e.target.value;
        this.applyFilters();
      });
    }

    if (ratingFilter) {
      ratingFilter.addEventListener("change", (e) => {
        this.currentFilters.rating = e.target.value;
        this.applyFilters();
      });
    }

    if (availabilityFilter) {
      availabilityFilter.addEventListener("change", (e) => {
        this.currentFilters.availability = e.target.value;
        this.applyFilters();
      });
    }

    // Partnership form
    const partnershipForm = document.getElementById("partnershipForm");
    if (partnershipForm) {
      partnershipForm.addEventListener(
        "submit",
        this.handlePartnershipForm.bind(this),
      );
    }
  }

  applyFilters() {
    this.filteredPartners = this.allPartners.filter((partner) => {
      // Search filter
      if (this.currentFilters.search) {
        const searchTerm = this.currentFilters.search.toLowerCase();
        const matchesSearch =
          partner.name.toLowerCase().includes(searchTerm) ||
          partner.expertise.toLowerCase().includes(searchTerm) ||
          (partner.bio && partner.bio.toLowerCase().includes(searchTerm));

        if (!matchesSearch) return false;
      }

      // Expertise filter
      if (
        this.currentFilters.expertise &&
        partner.expertise !== this.currentFilters.expertise
      ) {
        return false;
      }

      // Rating filter
      if (this.currentFilters.rating) {
        const minRating = parseFloat(this.currentFilters.rating);
        if (partner.rating < minRating) return false;
      }

      // Availability filter
      if (
        this.currentFilters.availability &&
        partner.availability !== this.currentFilters.availability
      ) {
        return false;
      }

      return true;
    });

    this.renderPartnersDirectory();
  }

  setupChatModal() {
    const modal = document.getElementById("chatModal");
    const closeBtn = document.getElementById("chatCloseBtn");
    const chatForm = document.getElementById("chatForm");
    const chatInput = document.getElementById("chatInput");

    if (closeBtn) {
      closeBtn.addEventListener("click", () => {
        this.closeChat();
      });
    }

    if (modal) {
      modal.addEventListener("click", (e) => {
        if (e.target === modal) {
          this.closeChat();
        }
      });
    }

    if (chatForm) {
      chatForm.addEventListener("submit", (e) => {
        e.preventDefault();
        this.sendMessage();
      });
    }

    if (chatInput) {
      chatInput.addEventListener("keypress", (e) => {
        if (e.key === "Enter" && !e.shiftKey) {
          e.preventDefault();
          this.sendMessage();
        }
      });
    }
  }

  openChat(partnerId) {
    const partner = this.allPartners.find((p) => p.id === partnerId);
    if (!partner) return;

    this.currentChatPartner = partner;

    // Update chat header
    const partnerName = document.getElementById("chatPartnerName");
    const partnerStatus = document.getElementById("chatPartnerStatus");
    const partnerAvatar = document.getElementById("chatPartnerAvatar");

    if (partnerName) partnerName.textContent = partner.name;
    if (partnerStatus)
      partnerStatus.textContent = partner.availability || "Available";
    if (partnerAvatar) {
      partnerAvatar.innerHTML = `<img src="${partner.photo}" alt="${partner.name}" />`;
    }

    // Clear and setup initial messages
    this.setupInitialMessages();

    // Show modal
    const modal = document.getElementById("chatModal");
    if (modal) {
      modal.classList.add("active");
      document.body.style.overflow = "hidden";
    }

    // Focus chat input
    const chatInput = document.getElementById("chatInput");
    if (chatInput) {
      setTimeout(() => chatInput.focus(), 300);
    }
  }

  closeChat() {
    const modal = document.getElementById("chatModal");
    if (modal) {
      modal.classList.remove("active");
      document.body.style.overflow = "";
    }
    this.currentChatPartner = null;
  }

  setupInitialMessages() {
    const messagesContainer = document.getElementById("chatMessages");
    if (!messagesContainer || !this.currentChatPartner) return;

    const initialMessages = [
      {
        sender: "partner",
        text: `Hello! I'm ${this.currentChatPartner.name}. I'd love to help you with your Japanese learning journey!`,
        time: this.getCurrentTime(),
      },
      {
        sender: "partner",
        text: `I specialize in ${this.currentChatPartner.expertise.toLowerCase()}. What specific goals do you have for learning Japanese?`,
        time: this.getCurrentTime(),
      },
    ];

    const messagesHTML = initialMessages
      .map((message) => this.createMessageHTML(message))
      .join("");
    messagesContainer.innerHTML = messagesHTML;

    this.scrollToBottom();
  }

  sendMessage() {
    const chatInput = document.getElementById("chatInput");
    const messagesContainer = document.getElementById("chatMessages");

    if (!chatInput || !messagesContainer) return;

    const messageText = chatInput.value.trim();
    if (!messageText) return;

    // Add user message
    const userMessage = {
      sender: "user",
      text: messageText,
      time: this.getCurrentTime(),
    };

    messagesContainer.innerHTML += this.createMessageHTML(userMessage);
    chatInput.value = "";
    this.scrollToBottom();

    // Simulate partner response after delay
    setTimeout(
      () => {
        const partnerResponse = this.generatePartnerResponse(messageText);
        const partnerMessage = {
          sender: "partner",
          text: partnerResponse,
          time: this.getCurrentTime(),
        };

        messagesContainer.innerHTML += this.createMessageHTML(partnerMessage);
        this.scrollToBottom();
      },
      1000 + Math.random() * 2000,
    );
  }

  createMessageHTML(message) {
    const isUser = message.sender === "user";
    const avatarSrc = isUser
      ? "https://api.dicebear.com/7.x/avataaars/svg?seed=User&backgroundColor=b6e3f4"
      : this.currentChatPartner.photo;

    return `
      <div class="chat-message ${isUser ? "sent" : ""}">
        <div class="message-avatar">
          <img src="${avatarSrc}" alt="${message.sender}" />
        </div>
        <div class="message-content">
          <p class="message-text">${message.text}</p>
          <div class="message-time">${message.time}</div>
        </div>
      </div>
    `;
  }

  generatePartnerResponse(userMessage) {
    const responses = [
      "That's a great question! I'd be happy to help you with that.",
      "I understand what you're looking for. Based on my experience, I recommend...",
      "That's exactly what many of my students ask about. Let me explain...",
      "I've helped many students with similar goals. Would you like to schedule a trial lesson?",
      "Great! I think my teaching style would be perfect for your learning objectives.",
      "That sounds like a wonderful goal! I can definitely help you achieve that.",
    ];

    // Simple response logic based on keywords
    const lowerMessage = userMessage.toLowerCase();

    if (lowerMessage.includes("price") || lowerMessage.includes("cost")) {
      return `My rate is $${this.currentChatPartner.hourlyRate} per hour. I offer flexible scheduling and can create a customized learning plan for you.`;
    }

    if (lowerMessage.includes("schedule") || lowerMessage.includes("time")) {
      return "I have flexible availability throughout the week. Would you prefer morning, afternoon, or evening sessions?";
    }

    if (
      lowerMessage.includes("experience") ||
      lowerMessage.includes("qualification")
    ) {
      return `I have ${this.currentChatPartner.experience} of teaching experience and have helped ${this.currentChatPartner.students} students achieve their Japanese learning goals.`;
    }

    return responses[Math.floor(Math.random() * responses.length)];
  }

  getCurrentTime() {
    return new Date().toLocaleTimeString([], {
      hour: "2-digit",
      minute: "2-digit",
    });
  }

  scrollToBottom() {
    const messagesContainer = document.getElementById("chatMessages");
    if (messagesContainer) {
      messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }
  }

  handlePartnershipForm(e) {
    e.preventDefault();

    const formData = new FormData(e.target);
    const data = Object.fromEntries(formData.entries());

    console.log("Partnership application:", data);

    // Show success message
    Toast.success(
      "Thank you for your partnership application! We'll review and email you when approved.",
    );

    // Reset form
    e.target.reset();
  }

  generateStars(rating) {
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;
    let stars = "";

    for (let i = 0; i < fullStars; i++) {
      stars += "★";
    }

    if (hasHalfStar) {
      stars += "☆";
    }

    const emptyStars = 5 - fullStars - (hasHalfStar ? 1 : 0);
    for (let i = 0; i < emptyStars; i++) {
      stars += "☆";
    }

    return stars;
  }

  showEmpty() {
    const container = document.getElementById("partnersGrid");
    if (container) {
      container.innerHTML = `
        <div class="partners-empty">
          <div class="empty-icon">👥</div>
          <h3 class="empty-title">No partners found</h3>
          <p class="empty-description">
            Try adjusting your filters to find the perfect Japanese teacher for you.
          </p>
          <button class="btn btn-primary" onclick="partnersPage.clearFilters()">
            Clear Filters
          </button>
        </div>
      `;
    }
  }

  clearFilters() {
    // Reset all filters
    this.currentFilters = {
      search: "",
      expertise: "",
      rating: "",
      availability: "",
    };

    // Reset form elements
    const searchInput = document.getElementById("partnerSearch");
    const expertiseFilter = document.getElementById("expertiseFilter");
    const ratingFilter = document.getElementById("ratingFilter");
    const availabilityFilter = document.getElementById("availabilityFilter");

    if (searchInput) searchInput.value = "";
    if (expertiseFilter) expertiseFilter.value = "";
    if (ratingFilter) ratingFilter.value = "";
    if (availabilityFilter) availabilityFilter.value = "";

    // Reapply filters
    this.applyFilters();
  }
}

// Global reference for onclick handlers
let partnersPage;

// Initialize when DOM is loaded
document.addEventListener("DOMContentLoaded", () => {
  partnersPage = new PartnersPage();
});

// Handle escape key to close chat modal
document.addEventListener("keydown", (e) => {
  if (e.key === "Escape" && partnersPage) {
    partnersPage.closeChat();
  }
});

document.addEventListener("DOMContentLoaded", function () {
    initializePartnershipForm();
});

function initializePartnershipForm() {
    const form = document.getElementById("partnershipForm");
    form.addEventListener("submit", handlePartnershipSubmission);
}

function showPartnershipForm() {
    document
        .querySelector(".partner-contact-section")
        .scrollIntoView({ behavior: "smooth" });
}

function inquirePartnership(type) {
    const select = document.getElementById("partnershipType");
    select.value = type;
    showPartnershipForm();
}

async function handlePartnershipSubmission(e) {
    e.preventDefault();
    const formData = new FormData(e.target);
    const data = Object.fromEntries(formData.entries());

    try {
        showLoading();

        // Simulate API call
        await new Promise((resolve) => setTimeout(resolve, 1500));

        showToast(
            "Partnership inquiry submitted successfully! We'll contact you within 24 hours.",
            "success",
        );
        e.target.reset();
    } catch (error) {
        showToast("Failed to submit inquiry. Please try again.", "error");
    } finally {
        hideLoading();
    }
}