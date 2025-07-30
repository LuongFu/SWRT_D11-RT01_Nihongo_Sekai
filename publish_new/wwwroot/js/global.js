/**
 * Nihongo Sekai - Global JavaScript Utilities
 * Shared functionality across all pages
 */

// Global Configuration
const CONFIG = {
  API_BASE_URL: null, // Set to null for static deployment
  TOAST_DURATION: 5000,
  LOADING_MIN_DURATION: 500,
  USE_MOCK_DATA: true,
};

// Global Utilities
const Utils = {
  // Debounce function for search inputs
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

  // Format currency
  formatCurrency(amount, currency = "USD") {
    return new Intl.NumberFormat("en-US", {
      style: "currency",
      currency: currency,
    }).format(amount);
  },

  // Format date
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

  // Truncate text
  truncate(text, length = 100) {
    if (text.length <= length) return text;
    return text.substring(0, length) + "...";
  },

  // Generate star rating HTML
  generateStars(rating, maxStars = 5) {
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;
    let stars = "";

    for (let i = 0; i < fullStars; i++) {
      stars += "★";
    }

    if (hasHalfStar) {
      stars += "☆";
    }

    const emptyStars = maxStars - fullStars - (hasHalfStar ? 1 : 0);
    for (let i = 0; i < emptyStars; i++) {
      stars += "☆";
    }

    return stars;
  },

  // Get current user data
  getCurrentUser() {
    try {
      return JSON.parse(localStorage.getItem("user_data"));
    } catch {
      return null;
    }
  },

  // Get auth token
  getAuthToken() {
    return localStorage.getItem("auth_token") || "";
  },

  // Check if user is logged in
  isLoggedIn() {
    return localStorage.getItem("user_logged_in") === "true";
  },
};

// Toast Notification System
class ToastManager {
  constructor() {
    this.container = this.createContainer();
  }

  createContainer() {
    let container = document.querySelector(".toast-container");
    if (!container) {
      container = document.createElement("div");
      container.className = "toast-container";
      document.body.appendChild(container);
    }
    return container;
  }

  show(message, type = "info", duration = CONFIG.TOAST_DURATION) {
    const toast = document.createElement("div");
    toast.className = `toast toast-${type}`;

    const iconMap = {
      success: "✓",
      error: "✕",
      warning: "⚠",
      info: "ℹ",
    };

    toast.innerHTML = `
      <div class="toast-content">
        <span class="toast-icon">${iconMap[type] || "ℹ"}</span>
        <span class="toast-message">${message}</span>
        <button class="toast-close" onclick="this.parentElement.parentElement.remove()">×</button>
      </div>
    `;

    this.container.appendChild(toast);

    // Auto remove
    setTimeout(() => {
      if (toast.parentElement) {
        toast.remove();
      }
    }, duration);

    return toast;
  }

  success(message, duration) {
    return this.show(message, "success", duration);
  }

  error(message, duration) {
    return this.show(message, "error", duration);
  }

  warning(message, duration) {
    return this.show(message, "warning", duration);
  }

  info(message, duration) {
    return this.show(message, "info", duration);
  }
}

// Global toast instance
const Toast = new ToastManager();

// Navigation Toggle for Mobile
class NavigationManager {
  constructor() {
    this.init();
  }

  init() {
    this.setupMobileToggle();
    this.setupActiveNavigation();
    this.setupSmoothScrolling();
  }

  setupMobileToggle() {
    const navToggle = document.querySelector(".nav-toggle");
    const navMenu = document.querySelector(".nav-menu");

    if (navToggle && navMenu) {
      navToggle.addEventListener("click", () => {
        navMenu.classList.toggle("active");
        this.toggleNavIcon(navToggle);
      });

      // Close menu when clicking outside
      document.addEventListener("click", (e) => {
        if (!navToggle.contains(e.target) && !navMenu.contains(e.target)) {
          navMenu.classList.remove("active");
          this.resetNavIcon(navToggle);
        }
      });
    }
  }

  toggleNavIcon(toggle) {
    const spans = toggle.querySelectorAll("span");
    toggle.classList.toggle("active");

    if (toggle.classList.contains("active")) {
      spans[0].style.transform = "rotate(45deg) translate(5px, 5px)";
      spans[1].style.opacity = "0";
      spans[2].style.transform = "rotate(-45deg) translate(7px, -6px)";
    } else {
      this.resetNavIcon(toggle);
    }
  }

  resetNavIcon(toggle) {
    const spans = toggle.querySelectorAll("span");
    toggle.classList.remove("active");
    spans.forEach((span) => {
      span.style.transform = "";
      span.style.opacity = "";
    });
  }

  setupActiveNavigation() {
    const currentPage =
      window.location.pathname.split("/").pop() || "index.html";
    const navLinks = document.querySelectorAll(".nav-link");

    navLinks.forEach((link) => {
      const href = link.getAttribute("href");
      if (
        href === currentPage ||
        (currentPage === "" && href === "index.html")
      ) {
        link.classList.add("active");
      }
    });
  }

  setupSmoothScrolling() {
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
  }
}

// Scroll Animation Observer
class ScrollAnimationManager {
  constructor() {
    this.observer = null;
    this.init();
  }

  init() {
    if ("IntersectionObserver" in window) {
      this.observer = new IntersectionObserver(
        this.handleIntersection.bind(this),
        {
          threshold: 0.1,
          rootMargin: "0px 0px -50px 0px",
        },
      );

      this.observeElements();
    }
  }

  observeElements() {
    const animatedElements = document.querySelectorAll(".animate-on-scroll");
    animatedElements.forEach((el) => {
      this.observer.observe(el);
    });
  }

  handleIntersection(entries) {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        entry.target.classList.add("visible");
        this.observer.unobserve(entry.target);
      }
    });
  }

  // Method to add new elements for observation
  observe(element) {
    if (this.observer && element) {
      this.observer.observe(element);
    }
  }
}

// API Client for making requests
class APIClient {
  constructor() {
    this.baseURL = CONFIG.API_BASE_URL;
    this.useMockData = CONFIG.USE_MOCK_DATA;
  }

  async request(endpoint, options = {}) {
    if (this.useMockData || !this.baseURL) {
      return this.getMockResponse(endpoint, options);
    }

    const url = `${this.baseURL}${endpoint}`;
    const defaultOptions = {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${Utils.getAuthToken()}`,
      },
    };

    try {
      const response = await fetch(url, { ...defaultOptions, ...options });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      return await response.json();
    } catch (error) {
      console.error("API request failed:", error);
      throw error;
    }
  }

  async get(endpoint) {
    return this.request(endpoint, { method: "GET" });
  }

  async post(endpoint, data) {
    return this.request(endpoint, {
      method: "POST",
      body: JSON.stringify(data),
    });
  }

  async put(endpoint, data) {
    return this.request(endpoint, {
      method: "PUT",
      body: JSON.stringify(data),
    });
  }

  async delete(endpoint) {
    return this.request(endpoint, { method: "DELETE" });
  }

  getMockResponse(endpoint, options) {
    // Return mock data based on endpoint
    console.log(`Mock API call: ${options.method || "GET"} ${endpoint}`);
    return Promise.resolve({
      success: true,
      data: null,
      message: "Mock response",
    });
  }
}

// Global API client instance
const API = new APIClient();

// Form Validation Utilities
class FormValidator {
  static validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
  }

  static validateRequired(value) {
    return value && value.trim().length > 0;
  }

  static validateMinLength(value, minLength) {
    return value && value.length >= minLength;
  }

  static validatePassword(password) {
    // At least 8 characters, 1 uppercase, 1 lowercase, 1 number
    const re = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d@$!%*?&]{8,}$/;
    return re.test(password);
  }

  static showFieldError(fieldId, message) {
    const field = document.getElementById(fieldId);
    if (!field) return;

    // Remove existing error
    this.clearFieldError(fieldId);

    // Add error styling
    field.classList.add("error");

    // Create error message
    const errorDiv = document.createElement("div");
    errorDiv.className = "field-error";
    errorDiv.id = `${fieldId}-error`;
    errorDiv.textContent = message;

    field.parentNode.appendChild(errorDiv);
  }

  static clearFieldError(fieldId) {
    const field = document.getElementById(fieldId);
    const errorElement = document.getElementById(`${fieldId}-error`);

    if (field) {
      field.classList.remove("error");
    }

    if (errorElement) {
      errorElement.remove();
    }
  }

  static clearAllErrors(formElement) {
    const errorElements = formElement.querySelectorAll(".field-error");
    const errorFields = formElement.querySelectorAll(".error");

    errorElements.forEach((el) => el.remove());
    errorFields.forEach((field) => field.classList.remove("error"));
  }
}

// Loading State Manager
class LoadingManager {
  static show(elementId, message = "Loading...") {
    const element = document.getElementById(elementId);
    if (!element) return;

    element.innerHTML = `
      <div class="loading-state">
        <div class="loading-spinner"></div>
        <p>${message}</p>
      </div>
    `;
  }

  static hide(elementId, content = "") {
    const element = document.getElementById(elementId);
    if (!element) return;

    element.innerHTML = content;
  }

  static showButton(buttonElement, message = "Loading...") {
    if (!buttonElement) return;

    buttonElement.disabled = true;
    buttonElement.originalHTML = buttonElement.innerHTML;
    buttonElement.innerHTML = `
      <span class="loading-spinner-sm"></span>
      ${message}
    `;
  }

  static hideButton(buttonElement) {
    if (!buttonElement || !buttonElement.originalHTML) return;

    buttonElement.disabled = false;
    buttonElement.innerHTML = buttonElement.originalHTML;
    delete buttonElement.originalHTML;
  }
}

// Local Storage Helper
class StorageManager {
  static set(key, value) {
    try {
      localStorage.setItem(key, JSON.stringify(value));
      return true;
    } catch (error) {
      console.error("Failed to save to localStorage:", error);
      return false;
    }
  }

  static get(key, defaultValue = null) {
    try {
      const item = localStorage.getItem(key);
      return item ? JSON.parse(item) : defaultValue;
    } catch (error) {
      console.error("Failed to read from localStorage:", error);
      return defaultValue;
    }
  }

  static remove(key) {
    try {
      localStorage.removeItem(key);
      return true;
    } catch (error) {
      console.error("Failed to remove from localStorage:", error);
      return false;
    }
  }

  static clear() {
    try {
      localStorage.clear();
      return true;
    } catch (error) {
      console.error("Failed to clear localStorage:", error);
      return false;
    }
  }
}

// Initialize global functionality when DOM is loaded
document.addEventListener("DOMContentLoaded", () => {
  // Initialize navigation
  new NavigationManager();

  // Initialize scroll animations
  new ScrollAnimationManager();

  // Add loading spinner styles if not present
  if (!document.querySelector("#loading-styles")) {
    const styles = document.createElement("style");
    styles.id = "loading-styles";
    styles.textContent = `
      .loading-state {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding: 2rem;
        color: var(--color-gray-600);
      }
      
      .loading-spinner {
        width: 40px;
        height: 40px;
        border: 3px solid var(--color-gray-200);
        border-top: 3px solid var(--color-primary-600);
        border-radius: 50%;
        animation: spin 1s linear infinite;
        margin-bottom: 1rem;
      }
      
      .loading-spinner-sm {
        width: 16px;
        height: 16px;
        border: 2px solid transparent;
        border-top: 2px solid currentColor;
        border-radius: 50%;
        animation: spin 1s linear infinite;
        display: inline-block;
      }
      
      @keyframes spin {
        0% { transform: rotate(0deg); }
        100% { transform: rotate(360deg); }
      }
      
      .field-error {
        color: var(--color-error);
        font-size: 0.875rem;
        margin-top: 0.25rem;
      }
      
      .form-input.error,
      .form-select.error,
      .form-textarea.error {
        border-color: var(--color-error);
        box-shadow: 0 0 0 3px rgba(239, 68, 68, 0.1);
      }
    `;
    document.head.appendChild(styles);
  }
});

// Export utilities for use in other scripts
window.Utils = Utils;
window.Toast = Toast;
window.API = API;
window.FormValidator = FormValidator;
window.LoadingManager = LoadingManager;
window.StorageManager = StorageManager;
