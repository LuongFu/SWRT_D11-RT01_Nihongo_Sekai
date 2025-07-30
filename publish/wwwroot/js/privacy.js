// privacy.js

document.addEventListener("DOMContentLoaded", () => {
    const OFFSET = 20; // adjust to match your fixed header height

    // 1. Smooth-scroll when you click a TOC link
    document.querySelectorAll(".legal-nav-link").forEach(link => {
        link.addEventListener("click", e => {
            e.preventDefault();
            const targetId = link.getAttribute("href").slice(1);
            const section = document.getElementById(targetId);
            if (section) {
                // use the existing Utils.scrollTo if you've made it global;
                // otherwise polyfill here:
                const y = section.getBoundingClientRect().top + window.pageYOffset - OFFSET;
                window.scrollTo({ top: y, behavior: "smooth" });
            }
        });
    });

    // 2. (Optional) Highlight TOC item as you scroll
    const tocLinks = Array.from(document.querySelectorAll(".legal-nav-link"));
    const sections = tocLinks.map(l => document.getElementById(l.getAttribute("href").slice(1)));

    window.addEventListener("scroll", () => {
        const scrollPos = window.pageYOffset + OFFSET + 5;
        sections.forEach((sec, i) => {
            if (!sec) return;
            const top = sec.offsetTop;
            const bottom = top + sec.offsetHeight;
            tocLinks[i].classList.toggle("active", scrollPos >= top && scrollPos < bottom);
        });
    });
});
