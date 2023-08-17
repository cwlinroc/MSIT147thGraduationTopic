import cityStruct from '../../datas/CityCountyData.json' assert {type: 'json'};

const orderForm = document.orderForm
const inputName = document.querySelector('#inputName')
const inputPhone = document.querySelector('#inputPhone')
const inputEmail = document.querySelector('#inputEmail')
const inputCity = document.querySelector('#inputCity')
const inputDistrict = document.querySelector('#inputDistrict')
const inputAddress = document.querySelector('#inputAddress')
const inputCoupons = document.querySelector('#inputCoupons')
const divPayment = document.querySelector('#divPayment')
const couponModal = new bootstrap.Modal(document.getElementById('couponModal'))

calculateTotalPrice()

//自動更改確認訂單資訊
$('#outputName').text(inputName.value)
inputName.addEventListener('change', e => $('#outputName').text(e.currentTarget.value))

$('#outputPhone').text(inputPhone.value)
inputPhone.addEventListener('change', e => $('#outputPhone').text(e.currentTarget.value))

$('#outputEmail').text(inputEmail.value)
inputEmail.addEventListener('change', e => $('#outputEmail').text(e.currentTarget.value))

$('#outputAddress').text(inputAddress.value)
inputAddress.addEventListener('change', e => $('#outputAddress').text(e.currentTarget.value))

const paymentRadios = [...document.orderForm.Payment]
paymentRadios.forEach(value =>
    value.addEventListener('change', () =>
        $('#outputPayment').text(divPayment.querySelector(':checked').nextElementSibling.innerText.trim())
    )
)


//上一頁按鈕
$('#prevPage1').click(e => {
    page1validator.endtValidate()
    $('.progress-bar').css('width', '0%')
    $('#circle2').addClass('border-white').removeClass('border-danger')
    $('.order-page').css('left', '0%')
    document.querySelector('#page1').customAnimate('backInLeft')
})
$('#prevPage2').click(e => {
    page2validator.endtValidate()
    $('.progress-bar').css('width', '33%')
    $('#circle3').addClass('border-white').removeClass('border-danger')
    $('.order-page').css('left', '-100%')
    document.querySelector('#page2').customAnimate('backInLeft')

})
$('#prevPage3').click(e => {
    page3validator.endtValidate()
    $('.progress-bar').css('width', '66%')
    $('#circle4').addClass('border-white').removeClass('border-danger')
    $('.order-page').css('left', '-200%')
    document.querySelector('#page3').customAnimate('backInLeft')
})

//下一頁按鈕
$('#nextPage2').click(async e => {
    page1validator.endtValidate()
    if (!await page1validator.startValidate()) return

    $('.progress-bar').css('width', '33%')
    $('#circle2').addClass('border-danger').removeClass('border-white')
    $('.order-page').css('left', '-100%')
    document.querySelector('#page2').customAnimate('backInRight')
})
$('#nextPage3').click(async e => {
    page2validator.endtValidate()
    if (!await page2validator.startValidate()) return

    $('.progress-bar').css('width', '66%')
    $('#circle3').addClass('border-danger').removeClass('border-white')
    $('.order-page').css('left', '-200%')
    document.querySelector('#page3').customAnimate('backInRight')

})
$('#nextPage4').click(async e => {
    page3validator.endtValidate()
    if (!await page3validator.startValidate()) return

    $('.progress-bar').css('width', '100%')
    $('#circle4').addClass('border-danger').removeClass('border-white')
    $('.order-page').css('left', '-300%')
    document.querySelector('#page4').customAnimate('backInRight')
})

//送出按鈕
$('#submitOrder').click(async event => {
    console.log()
    const result = await Swal.fire({
        title: '訂單提交',
        text: "確定將送出該筆訂單?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: '送出',
        cancelButtonText: '取消'
    })
    if (!result.isConfirmed) return
    const stockCheck = await checkStockQuantity()
    if (!stockCheck.enough) {
        await Swal.fire({
            icon: 'error',
            title: '商品庫存不足',
            text: stockCheck.message,
            footer: `<a href="${ROOT}/cart">回到購物車</a>`
        })
        console.log(stockCheck)
        return
    }
    submitEvent()
})

async function checkStockQuantity() {
    const response = await fetch(`${ROOT}/api/apibuy/checkstockquantity`)
    return await response.json()
}


async function submitEvent() {
    const formData = new FormData(document.orderForm)
    const response = await fetch(`${ROOT}/api/apibuy/sendorder`, {
        method: 'POST',
        body: formData
    })
    const result = await response.json()
    console.log(result)
    if(result.succeed) window.location = result.web
}


/***functions***/

//價格計算
function calculateTotalPrice() {
    const total = ([...document.querySelectorAll('.price-sum')])
        .map(o => +o.textContent)
        .reduce((accum, current) => accum + current, 0)
    const outputTotal = [...document.querySelectorAll('.price-total')]
    outputTotal.forEach(e => e.textContent = total)
}

//第一頁驗證

const page1validator = new MyBootsrapValidator(orderForm)
page1validator.validateFunction(async () => {
    const nameHasValue = !!inputName.value
    inputName.setValidate(() => nameHasValue)
    await new Promise(resolve => setTimeout(resolve, 300))

    const phoneHasValue = !!inputPhone.value
    inputPhone.setValidate(() => phoneHasValue)
    await new Promise(resolve => setTimeout(resolve, 300))

    const emailHasValue = !!inputEmail.value
    const emailPatternValid = /^\w+@\w+/.test(inputEmail.value)
    inputEmail.setValidate(() => emailHasValue, '請輸入Email').setValidate(() => emailPatternValid, 'Email格式錯誤')
    await new Promise(resolve => setTimeout(resolve, 500))

    return nameHasValue && phoneHasValue && emailHasValue && emailPatternValid
})

page1validator.keyupValidateFunction(() => {
    const nameHasValue = !!inputName.value
    inputName.setValidate(() => nameHasValue)

    const phoneHasValue = !!inputPhone.value
    inputPhone.setValidate(() => phoneHasValue)

    const emailHasValue = !!inputEmail.value
    const emailPatternValid = /^\w+@@\w+/.test(inputEmail.value)
    inputEmail.setValidate(() => emailHasValue, '請輸入Email').setValidate(() => emailPatternValid, 'Email格式錯誤')
})

//第二頁驗證

const page2validator = new MyBootsrapValidator(orderForm)
page2validator.validateFunction(async () => {
    const cityHasValue = !(inputCity.value == 0)
    inputCity.setValidate(() => cityHasValue)
    await new Promise(resolve => setTimeout(resolve, 300))

    const districtHasValue = !(inputDistrict.value == 0)
    inputDistrict.setValidate(() => districtHasValue)
    await new Promise(resolve => setTimeout(resolve, 300))

    const addressHasValue = !!inputAddress.value
    inputAddress.setValidate(() => addressHasValue)
    await new Promise(resolve => setTimeout(resolve, 500))

    return addressHasValue && cityHasValue && districtHasValue
})

page2validator.keyupValidateFunction(() => {
    const cityHasValue = !(inputCity.value == 0)
    inputCity.setValidate(() => cityHasValue)

    const districtHasValue = !(inputDistrict.value == 0)
    inputDistrict.setValidate(() => districtHasValue)

    const addressHasValue = !!inputAddress.value
    inputAddress.setValidate(() => addressHasValue)
})


//第三頁驗證

const page3validator = new MyBootsrapValidator(orderForm)
page3validator.validateFunction(async () => {
    inputCoupons.setValidate(() => true)
    await new Promise(resolve => setTimeout(resolve, 300))

    const paymentChecked = !!divPayment.querySelector('input[name="Payment"]:checked')
    console.log(divPayment.querySelector('input[name="Payment"]:checked'))
    document.querySelector('#radioPayment1').setValidate(() => paymentChecked)
    document.querySelector('#radioPayment2').setValidate(() => paymentChecked)
    document.querySelector('#radioPayment3').setValidate(() => paymentChecked)
    await new Promise(resolve => setTimeout(resolve, 500))

    return paymentChecked
})

page3validator.keyupValidateFunction(() => {
    const paymentChecked = !!divPayment.querySelector('input[name="Payment"]:checked')
    document.querySelector('#radioPayment1').setValidate(() => paymentChecked)
    document.querySelector('#radioPayment2').setValidate(() => paymentChecked)
    document.querySelector('#radioPayment3').setValidate(() => paymentChecked)
})



initSelectCities()
function initSelectCities() {
    var cityOptions = cityStruct.map(o => `<option value="${o.CityName}" >${o.CityName}</option>`)
    var htmlStr = '<option selected hidden disabled value="0">--請選擇縣市--</option>' + cityOptions.join('')
    $('#inputCity').html(htmlStr)
}

$('#inputCity').change(e => {
    const city = e.currentTarget.value
    initSelectDistrict(city)
    $('#outputCity').text(city)
})

function initSelectDistrict(city) {
    var districtOptions = cityStruct.find(o => o.CityName === city).AreaList
        .map(o => `<option value="${o.AreaName}" >${o.AreaName}</option>`)
    var htmlStr = '<option selected hidden disabled value="0">--請選擇地區--</option>' + districtOptions.join('')
    $('#inputDistrict').html(htmlStr)
}
$('#inputDistrict').change(e => {
    const district = e.currentTarget.value
    $('#outputDistrict').text(district)
})

//套用會員地址
$('#useMemberAddress').click(e => {
    if (!!memberCity) {
        inputCity.value = memberCity
        initSelectDistrict(memberCity)
        $('#outputCity').text(memberCity)
    }
    if (!!memberCity && !!memberDistrict) {
        inputDistrict.value = memberDistrict
        $('#outputDistrict').text(memberDistrict)
    }

    inputAddress.value = memberAddress
    $('#outputAddress').text(memberAddress)
})



//優惠卷單選
$('#couponModal').find('input[type=checkbox]').click(couponSingleChoice)
function couponSingleChoice(e) {
    $(e.currentTarget).closest('table').find('input[type=checkbox]').prop('checked', false)
    $(e.currentTarget).prop('checked', true)
}

//取得所有可使用優惠卷
async function GetAllCoupons() {
    const response = await fetch(ROOT + '/api/apibuy/coupons')
    const data = await response.json()
    return data
}
//選擇優惠卷
$('#btnChooseCoupon').click(e => {
    const checkedInput = $('#couponTable').find('input[type=checkbox]:checked')
    const couponId = +checkedInput.closest('tr').attr('data-couponId')
    const couponName = checkedInput.parent().next().text()
    $('#inputCouponId').val(couponId)
    $('#inputCoupons').val(couponName)
    $('#outputCoupon').text(couponName)
    refreshPriceByCoupon(couponId)
    couponModal.hide()
})
//不使用優惠卷
$('#btnDiscardCoupon').click(e => {
    $('#couponTable').find('input[type=checkbox]:checked').prop('checked', false)
    $('#inputCouponId').val('')
    $('#inputCoupons').val('無')
    $('#outputCoupon').text('無')
    refreshPriceByCoupon()
    couponModal.hide()
})

//優惠卷刷新訂單列表價格

refreshPriceByCoupon()
async function refreshPriceByCoupon(couponId) {
    if (!couponId) couponId = ''
    const response = await fetch(ROOT + '/api/apibuy/cartItems/' + couponId)
    const data = await response.json()

    displayItemsPrice(data)
    calculateTotalPrice()
    sideBarTotal
    document.querySelector('#sideBarTotal').customAnimate('flipInY')
}

//顯示商品價格
function displayItemsPrice(data) {
    const items = data.cartItems
    console.log(items)
    const itemHtmls = items.map(item => {
        const originPrice = Math.floor(item.cartItemPrice * item.discountPercentage / 100) * item.quantity
        const finalPrice = !!item.couponDiscount ? Math.floor(originPrice * item.couponDiscount / 100) : originPrice
        const discountStr = item.discountPercentage != 100 ? `<div class="text-secondary" style = "font-size:5px;" >
                                <del>$ <span> ${item.cartItemPrice * item.quantity}</span></del>
                                </div>` : ''
        const couponDiscountStr = !!item.couponDiscount ? `<div class="text-secondary animate__animated animate__backInLeft" style = "font-size:5px;" >
                                <del>$ <span> ${originPrice}</span></del>
                                </div>`: ''
        return ` <tr>
                                <td>${item.cartItemName}x<span>${item.quantity}</span></td>
                                <td>
                                ${discountStr}
                                ${couponDiscountStr}
                                <div class="text-danger text-nowrap">
                                $ <span class="price-sum">${finalPrice}</span>
                                </div>
                                </td>
                                </tr>`
    })
    const fixedAmountDiscount = !!data.couponDiscountAmount ? ` <tr>
                                    <td>優惠卷折抵</td>
                                    <td>
                                    <div class="text-danger text-nowrap animate__animated animate__backInDown">
                                            $ <span class="price-sum">-${data.couponDiscountAmount}</span>
                                    </div>
                                    </td>
                                    </tr>` : ''
    $('#listBody').html(itemHtmls.join('') + fixedAmountDiscount)
}






