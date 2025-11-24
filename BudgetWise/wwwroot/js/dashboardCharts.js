// dashboardCharts.js
// This file receives data from Blazor and renders charts on the dashboard.

/* ------------------------------
   PIE CHART — Spending by Category
--------------------------------*/
window.renderCategoryChart = (categories, amounts) => {

    const ctx = document.getElementById("categoryChart");

    if (!ctx) return;

    // Destroy existing chart instance (prevents duplicate canvas)
    if (window.categoryChartInstance) {
        window.categoryChartInstance.destroy();
    }

    window.categoryChartInstance = new Chart(ctx, {
        type: "pie",
        data: {
            labels: categories,
            datasets: [{
                data: amounts,
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { position: "bottom" }
            }
        }
    });
};


/* ------------------------------
   LINE CHART — Daily Trend
--------------------------------*/
window.renderDailyTrendChart = (days, incomeData, expenseData) => {

    const ctx = document.getElementById("dailyTrendChart");

    if (!ctx) return;

    // Destroy existing chart instance
    if (window.dailyTrendChartInstance) {
        window.dailyTrendChartInstance.destroy();
    }

    window.dailyTrendChartInstance = new Chart(ctx, {
        type: "line",
        data: {
            labels: days,
            datasets: [
                {
                    label: "Income",
                    data: incomeData,
                    borderWidth: 2,
                    tension: 0.3
                },
                {
                    label: "Expenses",
                    data: expenseData,
                    borderWidth: 2,
                    tension: 0.3
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { position: "bottom" }
            },
            scales: {
                y: { beginAtZero: true }
            }
        }
    });
};
