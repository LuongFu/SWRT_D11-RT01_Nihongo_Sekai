// wwwroot/js/wordOfTheDay.js

document.addEventListener('DOMContentLoaded', function () {
    const container = document.getElementById('wordOfTheDayContainer');
    if (!container) return;

    fetch('/DailyWord/GetWordOfTheDay')
        .then(res => {
            if (!res.ok) throw new Error("Failed to fetch word of the day.");
            return res.text();
        })
        .then(html => {
            container.innerHTML = html;
        })
        .catch(err => {
            console.error("Error loading Word of the Day:", err);
        });
});
