// blog.js
// Nihongo Sekai – Blog Page Functionality

let currentPage = 1;
let filteredArticles = [];

// Initialize when DOM is ready
document.addEventListener("DOMContentLoaded", function () {
    initializeBlogPage();
    loadArticles();
});

function initializeBlogPage() {
    // Search functionality
    const searchInput = document.getElementById("blogSearch");
    searchInput.addEventListener(
        "input",
        Utils.debounce((e) => searchArticles(e.target.value), 300)
    );

    // Filter functionality
    const categoryFilter = document.getElementById("categoryFilter");
    const sortFilter = document.getElementById("sortFilter");
    categoryFilter.addEventListener("change", filterArticles);
    sortFilter.addEventListener("change", filterArticles);

    // Category cards click-to-filter
    document.querySelectorAll(".category-card").forEach((card) => {
        card.addEventListener("click", () => {
            const category = card.dataset.category;
            categoryFilter.value = category;
            filterArticles();
            document
                .querySelector(".articles-section")
                .scrollIntoView({ behavior: "smooth" });
        });
    });

    // Load more button
    document.getElementById("loadMoreBtn").addEventListener("click", loadMoreArticles);

    // Newsletter signup
    document.getElementById("newsletterForm").addEventListener("submit", handleNewsletterSignup);
}

function loadArticles() {
    filteredArticles = [...mockArticles];
    renderArticles();
}

function renderArticles() {
    const articlesGrid = document.getElementById("articlesGrid");
    const articlesToShow = filteredArticles.slice(0, currentPage * 6);

    articlesGrid.innerHTML = articlesToShow
        .map(createArticleCard)
        .join("");

    // Toggle "Load More" button
    const loadMoreBtn = document.getElementById("loadMoreBtn");
    loadMoreBtn.style.display = articlesToShow.length < filteredArticles.length
        ? "block"
        : "none";
}

function createArticleCard(article) {
    return `
    <article class="article-card">
      <div class="article-image">
        <img src="${article.image}" alt="${article.title}" />
        <div class="article-category-badge">${article.category}</div>
      </div>
      <div class="article-content">
        <div class="article-meta">
          <span class="article-date">${article.date}</span>
          <span class="article-reading-time">${article.readingTime} min read</span>
        </div>
        <h3 class="article-title">${article.title}</h3>
        <p class="article-excerpt">${article.excerpt}</p>
        <div class="article-footer">
          <div class="article-author">
            <img src="${article.author.avatar}" alt="${article.author.name}" class="author-avatar-small" />
            <span class="author-name-small">${article.author.name}</span>
          </div>
          <a href="#" class="read-more-link">Read More →</a>
        </div>
      </div>
    </article>
  `;
}

function searchArticles(query) {
    const term = query.trim().toLowerCase();
    filteredArticles = mockArticles.filter(
        (a) =>
            a.title.toLowerCase().includes(term) ||
            a.excerpt.toLowerCase().includes(term) ||
            a.category.toLowerCase().includes(term)
    );
    currentPage = 1;
    renderArticles();
}

function filterArticles() {
    const category = document.getElementById("categoryFilter").value;
    const sortBy = document.getElementById("sortFilter").value;

    // Filter
    filteredArticles = category
        ? mockArticles.filter((a) => a.categorySlug === category)
        : [...mockArticles];

    // Sort
    if (sortBy === "oldest") {
        filteredArticles.sort((a, b) => new Date(a.dateObject) - new Date(b.dateObject));
    } else if (sortBy === "popular") {
        filteredArticles.sort((a, b) => b.views - a.views);
    } else {
        // newest
        filteredArticles.sort((a, b) => new Date(b.dateObject) - new Date(a.dateObject));
    }

    currentPage = 1;
    renderArticles();
}

function loadMoreArticles() {
    currentPage++;
    renderArticles();
}

function handleNewsletterSignup(e) {
    e.preventDefault();
    const email = document.getElementById("newsletterEmail").value;
    NihongoSekai.showToast(
        "Thanks for subscribing! You'll receive our weekly newsletter soon.",
        "success"
    );
    e.target.reset();
}

// Mock articles data
const mockArticles = [
    {
        id: 1,
        title: "5 Essential Japanese Phrases for Your First Conversation",
        excerpt:
            "Start your Japanese speaking journey with these fundamental phrases that will help you in any conversation situation.",
        category: "Learning Tips",
        categorySlug: "learning-tips",
        image:
            "https://images.unsplash.com/photo-1545569341-9eb8b30979d9?w=400&h=250&fit=crop",
        author: {
            name: "Kenji Yamamoto",
            avatar:
                "https://api.dicebear.com/7.x/avataaars/svg?seed=Kenji&backgroundColor=ffd93d",
        },
        date: "March 15, 2024",
        dateObject: "2024-03-15",
        readingTime: 5,
        views: 2847,
    },
    {
        id: 2,
        title: "The Mysteries of Japanese Particles: は vs が Explained",
        excerpt:
            "Finally understand the difference between these two crucial particles and when to use each one correctly.",
        category: "Grammar Guides",
        categorySlug: "grammar",
        image:
            "https://images.unsplash.com/photo-1434030216411-0b793f4b4173?w=400&h=250&fit=crop",
        author: {
            name: "Akiko Sato",
            avatar:
                "https://api.dicebear.com/7.x/avataaars/svg?seed=Akiko&backgroundColor=b6e3f4",
        },
        date: "March 12, 2024",
        dateObject: "2024-03-12",
        readingTime: 7,
        views: 3521,
    },
    {
        id: 3,
        title: "Cherry Blossom Season: Understanding Hanami Culture",
        excerpt:
            "Explore the deep cultural significance of cherry blossoms in Japan and how to participate in hanami traditions.",
        category: "Cultural Insights",
        categorySlug: "culture",
        image:
            "https://images.unsplash.com/photo-1522383225653-ed111181a951?w=400&h=250&fit=crop",
        author: {
            name: "Yuki Tanaka",
            avatar:
                "https://api.dicebear.com/7.x/avataaars/svg?seed=YukiSensei&backgroundColor=b6e3f4",
        },
        date: "March 10, 2024",
        dateObject: "2024-03-10",
        readingTime: 8,
        views: 4167,
    },
    {
        id: 4,
        title: "From Zero to JLPT N5: Sarah's 6-Month Journey",
        excerpt:
            "Follow Sarah's inspiring story of how she went from knowing no Japanese to passing the JLPT N5 in just six months.",
        category: "Success Stories",
        categorySlug: "success-stories",
        image:
            "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400&h=250&fit=crop",
        author: {
            name: "Sarah Kim",
            avatar:
                "https://api.dicebear.com/7.x/avataaars/svg?seed=Sarah&backgroundColor=ffdfbf",
        },
        date: "March 8, 2024",
        dateObject: "2024-03-08",
        readingTime: 6,
        views: 1893,
    },
    {
        id: 5,
        title:
            "Memory Techniques for Learning Kanji: Interview with Hiroshi-sensei",
        excerpt:
            "Our senior instructor shares his proven methods for memorizing kanji characters effectively and permanently.",
        category: "Teacher Interviews",
        categorySlug: "teacher-interviews",
        image:
            "https://images.unsplash.com/photo-1481627834876-b7833e8f5570?w=400&h=250&fit=crop",
        author: {
            name: "Hiroshi Tanaka",
            avatar:
                "https://api.dicebear.com/7.x/avataaars/svg?seed=HiroshiSensei&backgroundColor=c0aede",
        },
        date: "March 5, 2024",
        dateObject: "2024-03-05",
        readingTime: 10,
        views: 2674,
    },
    {
        id: 6,
        title: "JLPT N3 Listening Strategy: How to Improve Your Score",
        excerpt:
            "Master the listening section of JLPT N3 with these proven strategies and practice techniques.",
        category: "JLPT Preparation",
        categorySlug: "jlpt",
        image:
            "https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=400&h=250&fit=crop",
        author: {
            name: "Mai Suzuki",
            avatar:
                "https://api.dicebear.com/7.x/avataaars/svg?seed=Mai&backgroundColor=ffd93d",
        },
        date: "March 3, 2024",
        dateObject: "2024-03-03",
        readingTime: 9,
        views: 3012,
    },
];
