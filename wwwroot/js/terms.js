// terms.js
// Smooth scrolling and Table of Contents highlighting for Terms of Service page

document.addEventListener("DOMContentLoaded", () => {
    const sidebarLinks = document.querySelectorAll('.legal-nav-link');
    const sections = Array.from(sidebarLinks).map(link => {
        const id = link.getAttribute('href').slice(1);
        return document.getElementById(id);
    });

    // Smooth scroll to section on click\ n  sidebarLinks.forEach(link => {
    link.addEventListener('click', e => {
        e.preventDefault();
        const targetId = link.getAttribute('href').slice(1);
        const targetSection = document.getElementById(targetId);
        if (targetSection) {
            const offset = 20; // adjust for fixed header height if needed
            const targetPosition = targetSection.getBoundingClientRect().top + window.pageYOffset - offset;
            window.scrollTo({ top: targetPosition, behavior: 'smooth' });
        }
    });
});

// Highlight active section in sidebar on scroll\ n  window.addEventListener('scroll', () => {
const scrollPosition = window.pageYOffset;
sections.forEach((section, idx) => {
    if (section) {
        const top = section.offsetTop - 40;
        const bottom = top + section.offsetHeight;
        if (scrollPosition >= top && scrollPosition < bottom) {
            sidebarLinks.forEach(link => link.classList.remove('active'));
            sidebarLinks[idx].classList.add('active');
        }
    }
});
  });
});
