let orderData = null;
getOrdersByMemberId();

//Ajax 得到訂單資料
async function getOrdersByMemberId(id) {
    let str = ''
    if (id) str = '/' + id

    const response = await fetch(`${ROOT}/api/apiMember/ShoppingHistory${str}`);

    if (!response.ok) return;
    const data = await response.json();

    orderData = data
    console.log(data);
    displayOrder();
}

//列出訂單資料
function displayOrder() {
    orderData.map((e) => {
        $('#purchaseTime').html(e.purchaseTime)
        $('#orderId').html(e.orderId)
        $('#paymentAmount').html(e.paymentAmount)
        $('#paymentMethodName').html(e.paymentMethodName)
        $('#merchandiseName').html(e.merchandiseName)
        $('#quantity').html(e.quantity)
        $('#discount').html(e.discount)
        $('#price').html(e.price)
    })
}

//const qStartDate = $('#queryStartDate').val();
//const qEndDate = $('#queryEndDate').val();

//$('#btnQuery').click(displayOrderBetweenDate)

//async function displayOrderBetweenDate(qStartDate, qEndDate) {
//    if (qStartDate && qEndDate) {
//        $('#startDate').val(qStartDate)
//        $('#endDate').val(qEndDate)
//    }

//}