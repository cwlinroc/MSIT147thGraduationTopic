

//更改RatingData
$('#rangeEvaluationWeight').change(async e => {
    const weight = e.currentTarget.value
    $('#spanEvaluationWeight').text(weight)
    await updateRateData(weight, 'evaluationweight')
})
$('#rangePurchaseWeight').change(async e => {
    const weight = e.currentTarget.value
    $('#spanPurchaseWeight').text(weight)
    await updateRateData(weight, 'purchasedeight')
})
$('#rangeManuallyWeight').change(async e => {
    const weight = e.currentTarget.value
    $('#spanManuallyWeight').text(weight)
    await updateRateData(weight, 'manuallyweight')
})
$('#rateEvaluationFunc').change(async e => {
    const func = e.currentTarget.value
    await updateRateData(func, 'rateEvaluationFunc')
})
$('#ratePurchaseFunc').change(async e => {
    const func = e.currentTarget.value
    await updateRateData(func, 'ratePurchaseFunc')
})
$('#recentEvaluationTimes').change(async e => {
    const func = e.currentTarget.value
    await updateRateData(func, 'recentevaluationtimes')
})
$('#recentEvaluationDays').change(async e => {
    const func = e.currentTarget.value
    await updateRateData(func, 'recentevaluationdays')
})
$('#recentPurchaseTimes').change(async e => {
    const func = e.currentTarget.value
    await updateRateData(func, 'recentpurchasedtimes')
})
$('#recentPurchaseDays').change(async e => {
    const func = e.currentTarget.value
    await updateRateData(func, 'recentpurchaseddays')
})


async function updateRateData(num, data) {
    const response = await fetch(`${ROOT}/api/apirecommend/ratedata`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ 'num': num, 'data': data })
    })
    return await response.json()
}

//顯示熱門度最高的十名商品
displayMostPopularSpecs()

async function displayMostPopularSpecs() {
    const specNames = await getMostPopularSpecs()
    const htmlStr = specNames.map(value => `<li>${value}</li>`)
    $('#listPopularSpecs').html(htmlStr)
}
async function getMostPopularSpecs() {
    const response = await fetch(`${ROOT}/api/apirecommend/mostpopularspecs`)
    return await response.json()
}

//立即刷新熱門度
$('#btnRefreshPopularity').click(async e => {
    const response = await fetch(`${ROOT}/api/apirecommend/refreshpopularity`)
    const result = await response.json()
    console.log(result)
    displayMostPopularSpecs()
    getLastExecuteTime()
})

/****自動更新****/

//取得自動更新間隔時間
getTimeInterval()
async function getTimeInterval() {
    const response = await fetch(`${ROOT}/api/apirecommend/TimeIntervalMinutes`)
    const timeInterval = await response.json()
    $('#selectTimeInterval').val(timeInterval)
}

//設定自動更新間隔時間
$('#selectTimeInterval').change(async e => {
    const timeInterval = $('#selectTimeInterval').val()
    const response = await fetch(`${ROOT}/api/apirecommend/TimeIntervalMinutes/${timeInterval}`, { method: 'PUT' })
    const result = await response.json()
    console.log(result)
})


//定時取得上次更新時間
setInterval(getLastExecuteTime, 10000)
//取得上一次更新時間
getLastExecuteTime()
async function getLastExecuteTime() {
    const response = await fetch(`${ROOT}/api/apirecommend/LastExecuteTimeMinuteBefore`)
    const minuteBefore = await response.json()
    displayLastExecuteTime(minuteBefore)
    if (minuteBefore == 0) displayMostPopularSpecs()
}
function displayLastExecuteTime(minuteBefore) {
    let displayStr = '無法取得上次更新時間'
    if (minuteBefore > 0 && minuteBefore < 60) displayStr = `${minuteBefore}分鐘前`
    if (minuteBefore >= 60) displayStr = `${Math.floor(minuteBefore / 60).toFixed(0)}小時前`
    if (minuteBefore == 0 || minuteBefore > 1000) displayStr = `剛才`
    console.log(displayStr)
    $('#lastExecuteTime').text(displayStr)
}

//const selectTimeInterval = document.querySelector('#selectTimeInterval')



/*****Modal*****/
//跳出Modal
const insertModal = new bootstrap.Modal(document.getElementById('insertModal'))
$('#btnShowInsertModal').click(e => {
    $('#tableSearchedItems').find('tbody').html('')
    $('#btnChooseSearchType').text('選擇類別').attr('data-searchtype', '')
    $('#inputSearch').val('')
    insertModal.show()
})
//選擇搜尋種類
$('#btnChooseTag').click(e => {
    $('#btnChooseSearchType').text('標籤').attr('data-searchtype', 'tag')
    searchItems()
})
$('#btnChooseMerchandise').click(e => {
    $('#btnChooseSearchType').text('商品').attr('data-searchtype', 'merchandise')
    searchItems()
})
$('#btnChooseSpec').click(e => {
    $('#btnChooseSearchType').text('規格').attr('data-searchtype', 'spec')
    searchItems()
})

//搜尋keyup
$('#inputSearch').keyup(searchItems)
//document.getElementById('inputSearch').addEventListener('compositionend', searchItems)
async function searchItems() {
    const table = document.querySelector('#tableSearchedItems')
    $(table).find('tbody').html('')
    const text = document.querySelector('#inputSearch').value
    const type = $('#btnChooseSearchType').attr('data-searchtype')
    if (!text || !type) return
    const data = await getSearchedItems(text, type)
    displaySearchedItems(data)
}

async function getSearchedItems(text, type) {
    const response = await fetch(`${ROOT}/api/apirecommend/getsearcheditems?` + new URLSearchParams({
        "type": type,
        "text": text,
    }))
    return await response.json()
}
async function displaySearchedItems(data) {
    const trs = data.map(value => `<tr class="tr-searched-item"><td>
    <input type="checkbox" name="rateItems" style="pointer-events:none" value="${value.id}"/>
    <label class="ms-3 text-nowrap">${value.name}</label></td></tr>`)
    $('#tableSearchedItems').find('tbody').html(trs.join(''))
    searchItemEvents()
}

//點選searchItem事件
function searchItemEvents() {
    $('.tr-searched-item').click(e => {
        e.stopPropagation()
        const checkbox = $(e.currentTarget).find('input[type=checkbox]')
        checkbox.prop('checked', !checkbox.prop('checked'))
    })
    $('.tr-searched-item').find('input[type=checkbox]').click(e => e.preventDefault())
}

//新增自訂項目
$('#btnInsertItems').click(async e => {
    let ids = []
    $('.tr-searched-item').find('input[type=checkbox]:checked')
        .each((index, element) => ids.push(+element.value))
    if (!ids.length) return
    const weight = $('#itemsWeight').val()
    const type = $('#btnChooseSearchType').attr('data-searchtype')
    await insertItems(ids, weight, type)
    displayWeightedEntries()
    insertModal.hide()
})

async function insertItems(ids, weight, type) {
    const response = await fetch(`${ROOT}/api/apirecommend/insertweightentries`, {
        method: 'POST',
        body: JSON.stringify({ ids, weight, type }),
        headers: { 'Content-Type': 'application/json', },
    })
    const result = await response.json()
    console.log(result)
}

//顯示自訂評分物件

displayWeightedEntries()
async function displayWeightedEntries() {
    $('#tableEntries').find('tbody').html('')
    const data = await getWeightedEntries()
    const htmlStr = getEntriesDataHtml(data)
    $('#tableEntries').find('tbody').html(htmlStr)
    $('select.entry-changeweight').change(updateEntryWeight)
    $('button.entry-delete').click(deleteEntry)
}

async function getWeightedEntries() {
    const response = await fetch(`${ROOT}/api/apirecommend/getallweightedentries`)
    return await response.json()
}
function getEntriesDataHtml(data) {
    const strs = data.map((o, index) => {
        const options = Array.from({ length: 21 }, (v, i) => `<option value="${-10 + i}" ${(o.weight == -10 + i) ? 'selected' : ''}>${-10 + i}</option>`)
        const typename = ['標籤', '商品', '規格'][['tag', 'merchandise', 'spec'].indexOf(o.type)]
        return `<tr data-entryid="${o.id}">
                    <th scope="row">${index + 1}</th>
                    <td>${typename}</td>
                    <td>${o.name}</td>
                    <td>
                        <select class="entry-changeweight" style="width: 100px;">
                            ${options.join('')}
                        </select>
                    </td>
                    <td><button class="btn btn-danger entry-delete"><i class="fa-solid fa-trash-can"></i></button></td>
                </tr>`
    })
    return strs.join('')
}

//更改自訂評分物件評分
async function updateEntryWeight(e) {
    const weight = e.currentTarget.value
    const id = e.currentTarget.closest('tr').dataset.entryid
    const response = await fetch(`${ROOT}/api/apirecommend/updateentryweight`, {
        method: 'PUT',
        body: JSON.stringify({ id, weight }),
        headers: { 'Content-Type': 'application/json', },
    })
    const result = await response.json()
    console.log(result)
}

//刪除自訂評分物件

async function deleteEntry(e) {
    const id = e.currentTarget.closest('tr').dataset.entryid
    const response = await fetch(`${ROOT}/api/apirecommend/deleteweightentry/${id}`, { method: 'DELETE' })
    const result = await response.json()
    console.log(result)
    displayWeightedEntries()
}

//demo按鈕
$('#demoCat').click(e => {
    $('#inputSearch').val('貓')
    searchItems()
})
$('#demoDog').click(e => {
    $('#inputSearch').val('狗')
    searchItems()
})
