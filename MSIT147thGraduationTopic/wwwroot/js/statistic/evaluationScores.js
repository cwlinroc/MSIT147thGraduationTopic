import chartJs from 'https://cdn.jsdelivr.net/npm/chart.js@4.3.3/+esm'


const ctxScore = document.getElementById('evaluationChart').getContext('2d');

const evaluationChart = new Chart(ctxScore, {
    type: 'bar',
    data: {
        labels: ['五星', '四星', '三星', '二星', '一星'],
        datasets: [{
            label: '評分統計',
            data: [0,0,0,0,0],
            borderWidth: 4,
            backgroundColor: [
                'rgba(255, 205, 86, 0.7)',
                'rgba(255, 205, 86, 0.7)',
                'rgba(255, 205, 86, 0.7)',
                'rgba(255, 205, 86, 0.7)',
                'rgba(255, 205, 86, 0.7)',
            ],
            borderColor: [
                'rgba(255, 205, 86, 0.9)',
                'rgba(255, 205, 86, 0.9)',
                'rgba(255, 205, 86, 0.9)',
                'rgba(255, 205, 86, 0.9)',
                'rgba(255, 205, 86, 0.9)',
            ],
        }]
    },
    options: {
        indexAxis: 'y',
        // Elements options apply to all of the options unless overridden in a dataset
        // In this case, we are setting the border of each horizontal bar to be 2px wide
        elements: {
            bar: {
                borderWidth: 2,
            }
        },
        responsive: true,
        plugins: {
            //legend: {
            //    position: 'right',
            //},
            //title: {
            //    display: true,
            //    text: '評分統計'
            //}
        },
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }    
})

//evaluationChart
console.log('test start')

let chartMerchandiseId = $('#evaluationChart').attr('data-merchandiseid')

displayScores()
async function displayScores() {
    const scores = await getEvaluationScores()
    evaluationChart.data.datasets[0].data = scores
    evaluationChart.update()
}
async function getEvaluationScores() {
    const response = await fetch(`${ROOT}/api/apistatistic/evaluationscores/${chartMerchandiseId}`)
    const data = await response.json()
    console.log(data)
    return data
}


