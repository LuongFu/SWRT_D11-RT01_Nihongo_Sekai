// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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

// Sau khi đã định nghĩa class ScrollAnimationManager
document.addEventListener("DOMContentLoaded", () => {
    new ScrollAnimationManager();
});
