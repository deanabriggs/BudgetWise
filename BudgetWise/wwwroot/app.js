// Simple dark-mode helper for BudgetWise.
// Uses body.dark-theme + localStorage("bw-theme").

window.bwTheme = {
    init: function () {
        try {
            const saved = localStorage.getItem("bw-theme") || "light";
            const isDark = saved === "dark";
            document.body.classList.toggle("dark-theme", isDark);
        } catch {
            // ignore
        }
    },
    toggle: function () {
        try {
            const isDarkNow = !document.body.classList.contains("dark-theme");
            document.body.classList.toggle("dark-theme", isDarkNow);
            localStorage.setItem("bw-theme", isDarkNow ? "dark" : "light");
            return isDarkNow ? "dark" : "light";
        } catch {
            // fallback: just flip without storage
            const isDarkNow = !document.body.classList.contains("dark-theme");
            document.body.classList.toggle("dark-theme", isDarkNow);
            return isDarkNow ? "dark" : "light";
        }
    },
    get: function () {
        try {
            if (document.body.classList.contains("dark-theme")) {
                return "dark";
            }
            const saved = localStorage.getItem("bw-theme");
            return saved === "dark" ? "dark" : "light";
        } catch {
            return "light";
        }
    }
};

document.addEventListener("DOMContentLoaded", () => {
    if (window.bwTheme && window.bwTheme.init) {
        window.bwTheme.init();
    }
});

// --- Light / Dark Theme Manager ---
// Uses localStorage to remember the user's choice across reloads.

window.themeManager = (function () {
    function applyTheme(theme) {
        if (theme === "dark") {
            document.body.classList.add("dark-theme");
        } else {
            document.body.classList.remove("dark-theme");
        }
    }

    return {
        initializeTheme: function () {
            var saved = localStorage.getItem("theme");
            if (!saved) {
                // default to light
                localStorage.setItem("theme", "light");
                saved = "light";
            }
            applyTheme(saved);
        },
        toggleTheme: function () {
            var isDark = document.body.classList.toggle("dark-theme");
            localStorage.setItem("theme", isDark ? "dark" : "light");
        }
    };
})();
