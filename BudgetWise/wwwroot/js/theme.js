// ======================
// DARK MODE AUTOMATIC LOAD
// ======================
(function () {
    const savedTheme = localStorage.getItem("budgetwise-theme");
    if (savedTheme === "dark") {
        document.documentElement.setAttribute("data-theme", "dark");
    }
})();

// ======================
// TOGGLE DARK MODE
// ======================
window.toggleTheme = function () {
    const root = document.documentElement;
    const current = root.getAttribute("data-theme");
    const next = current === "dark" ? "light" : "dark";

    root.setAttribute("data-theme", next);
    localStorage.setItem("budgetwise-theme", next);
};

// ======================
// RENDER CATEGORY CHART (Pie Chart)
// ======================
window.renderCategoryChart = function (labels, values) {
    const ctx = document.getElementById("categoryChart");

    if (!ctx) {
        console.warn("categoryChart canvas not found");
        return;
    }

    if (window.categoryChartInstance) {
        window.categoryChartInstance.destroy();
    }

    window.categoryChartInstance = new Chart(ctx, {
        type: "pie",
        data: {
            labels: labels,
            datasets: [{
                data: values,
                borderWidth: 1
            }]
        }
    });
};

// ======================
// RENDER DAILY TREND CHART (Bar Chart)
// ======================
window.renderDailyChart = function (labels, income, expenses) {
    const ctx = document.getElementById("dailyChart");

    if (!ctx) {
        console.warn("dailyChart canvas not found");
        return;
    }

    if (window.dailyChartInstance) {
        window.dailyChartInstance.destroy();
    }

    window.dailyChartInstance = new Chart(ctx, {
        type: "bar",
        data: {
            labels: labels,
            datasets: [
                {
                    label: "Income",
                    data: income,
                    backgroundColor: "rgba(75,192,192,0.6)"
                },
                {
                    label: "Expenses",
                    data: expenses,
                    backgroundColor: "rgba(255,99,132,0.6)"
                }
            ]
        }
    });
};
