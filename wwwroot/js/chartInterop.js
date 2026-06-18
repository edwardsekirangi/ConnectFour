window.smartSpendChart = {
    renderCategoryChart: function (canvasId, labels, values) {
        const ctx = document.getElementById(canvasId);
        if (!ctx) {
            return;
        }

        if (window.smartSpendChart.currentChart) {
            window.smartSpendChart.currentChart.destroy();
        }

        window.smartSpendChart.currentChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Category Spending',
                    data: values,
                    backgroundColor: 'rgba(54, 162, 235, 0.6)',
                    borderColor: 'rgba(54, 162, 235, 1)',
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }
};
