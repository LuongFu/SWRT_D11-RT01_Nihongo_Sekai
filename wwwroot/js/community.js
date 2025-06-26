// Initialize community page logic when DOM is ready
document.addEventListener("DOMContentLoaded", initializeCommunityPage);

function initializeCommunityPage() {
    setupEventRegistration();
    setupForumNavigation();
}

function showJoinModal() {
    // Use the global toast from your main script
    window.NihongoSekai.showToast(
        "Join our community by creating a free account! Sign up to get started.",
        "info"
    );
}

function scrollToForums() {
    document.getElementById("forums").scrollIntoView({ behavior: "smooth" });
}

function setupEventRegistration() {
    const registerButtons = document.querySelectorAll(".event-card .btn");
    registerButtons.forEach((button) => {
        button.addEventListener("click", (e) => {
            const eventTitle = e.target
                .closest(".event-card")
                .querySelector(".event-title").textContent;
            window.NihongoSekai.showToast(
                `Registration confirmed for: ${eventTitle}`,
                "success"
            );
        });
    });
}

function setupForumNavigation() {
    const forumCategories = document.querySelectorAll(".forum-category");
    forumCategories.forEach((category) => {
        category.addEventListener("click", () => {
            const forumTitle = category.querySelector(".forum-title").textContent;
            window.NihongoSekai.showToast(`Opening ${forumTitle} forum...`, "info");
        });
    });
}
