let orderData = null;
GetOrderByMemberId();

//Ajax 得到訂單資料
async function GetOrderByMemberId(id) {
    let str = ''
    if (id) str = '/' + id

    const response = await fetch(`${ROOT}/api/apiMember${str}`);

    if (!response.ok) return;
    const data = await response.json();

    orderData = data
    console.log(data);
    displayOrder();
}

//列出訂單資料
function displayOrder() {
    orderData.map((e) => {
        $('#purchaseTime').val(e.purchaseTime)
        $('#orderId').val(e.orderId)
        $('#paymentAmount').val(e.paymentAmount)
        $('#paymentMethodName').val(e.paymentMethodName)
        $('#merchandiseName').val(e.merchandiseName)
        $('#quantity').val(e.quantity)
        $('#price').val(e.price)
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