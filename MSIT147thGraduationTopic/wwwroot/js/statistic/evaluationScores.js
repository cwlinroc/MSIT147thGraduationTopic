import chartJs from 'https://cdn.jsdelivr.net/npm/chart.js@4.3.3/+esm'

const ctxScore = document.getElementById('evaluationChart').getContext('2d');

const evaluationChart = new Chart(ctxScore, {
    type: 'bar',
    data: {
        labels: ['⭐⭐⭐⭐⭐', '⭐⭐⭐⭐', '⭐⭐⭐', '⭐⭐', '⭐'],
        datasets: [{
            label: '評分統計',
            data: [0, 0, 0, 0, 0],
            borderWidth: 1, //條形圖邊框大小
            backgroundColor: [
                'rgba(255, 205, 86, 1)', //條形圖顏色
                'rgba(255, 205, 86, 1)',
                'rgba(255, 205, 86, 1)',
                'rgba(255, 205, 86, 1)',
                'rgba(255, 205, 86, 1)',
            ],
            borderColor: [
                'rgba(0, 0, 0, 0.5)', //條形圖邊框顏色
                'rgba(0, 0, 0, 0.5)',
                'rgba(0, 0, 0, 0.5)',
                'rgba(0, 0, 0, 0.5)',
                'rgba(0, 0, 0, 0.5)',
                'rgba(0, 0, 0, 0.5)',
            ],
        }]
    },
    options: {

        barThickness: 16, // 條形圖粗度
        indexAxis: 'y',

        elements: {
            bar: {
                borderWidth: 2,
            }
        },
        responsive: true,
        plugins: {
            legend: {
                display: false
            },
        },
        scales: {
            x: {
                grid: {
                    display: false, // 隐藏 x 轴的格线
                    borderColor: 'black'
                },
                ticks: {
                    color: 'black',// 设置 x 轴坐标文字颜色
                }
            },
            y: {
                grid: {
                    display: false, // 隐藏 y 轴的格线
                    borderColor: 'black'
                },
                beginAtZero: true,
                ticks: {
                    color: 'rgba(255, 205, 86, 1)', // 设置 x 轴坐标文字颜色
                    font: {
                        size: 10 // 设置 y 轴坐标文字大小
                    }
                }
            }
        }
    }
})

//evaluationChart

let chartMerchandiseId = document.getElementById('evaluationChart').dataset.merchandiseid

await displayScores()
async function displayScores() {
    const scores = await getEvaluationScores()
    evaluationChart.data.datasets[0].data = scores
    evaluationChart.update()
}
async function getEvaluationScores() {
    const response = await fetch(`${ROOT}/api/apistatistic/evaluationscores/${chartMerchandiseId}`)
    const data = await response.json()
    return data
}


