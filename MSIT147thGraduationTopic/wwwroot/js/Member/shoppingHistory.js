let orderData = null;
getOrdersByMemberId();

//Ajax 得到訂單資料
async function getOrdersByMemberId() {

    const response = await fetch(`${ROOT}/api/apiMember/ShoppingHistory`);

    if (!response.ok) return;
    const data = await response.json();

    orderData = data
    console.log(data);
    displayOrders();
}

function displayOrders() {
    if (!orderData || orderData.length === 0) {
        $('.infotext').html('查無訂單');
        $('.orderTables').html('');
        return;
    }

    const oTable = orderData.map((e, index) => {
        //console.log(e.listOfSpecs)
        let specs = e.listOfSpecs.map(s => {
            return `<tr>
                        <td>${s.merchandiseName}</td>
                        <td>${s.quantity}</td>
                        <td>${s.discount}</td>
                        <td>${s.price}</td>
                    </tr>`
        })
        const dateOptions = {
            year: "numeric",
            month: "2-digit",
            day: "2-digit",
            hour: "numeric",
            minute: "numeric"
        };
        const datetime = new Date(e.purchaseTime).toLocaleString("zh-TW", dateOptions);

        return `<table class="table table-bordered px-4 py-2 mb-1">
                    <thead>
                        <tr>
                            <th scope="col">訂單日期</th>
                            <th scope="col">訂單編號</th>
                            <th scope="col">總金額</th>
                            <th scope="col">付款方式</th>
                        </tr>
                    </thead>
                    <tbody class="table-group-divider">
                        <tr>
                            <td>${datetime}</td>
                            <td>${e.orderId}</td>
                            <td>${e.paymentAmount}</td>
                            <td>${e.paymentMethodName}</td>
                        </tr>
                    </tbody>
                </table>
                <p>
                <button class="btn btn-primary" type="button" data-bs-toggle="collapse"
                    data-bs-target="#orderdetail${index}">
                    訂單明細
                </button>
                </p>
                <div class="collapse" id="orderdetail${index}">
                <div class="card card-body mb-3">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th scope="col">商品名稱</th>
                                <th scope="col">數量</th>
                                <th scope="col">折扣</th>
                                <th scope="col">價格</th>
                            </tr>
                        </thead>
                        <tbody class="table-group-divider">
                            ${specs.join('')}
                        </tbody>
                    </table>
                </div>
            </div>`;
    })
    $('.orderTables').html(oTable.join(''));
}

$("#queryStartDate,#queryEndDate").change(() => {
    if ($('#queryStartDate').val() && $('#queryEndDate').val()) {
        $('#btnQuery').prop('disabled', false);
    } else {
        $('#btnQuery').prop('disabled', true);
    }
})

$('#btnQuery').click(displayOrderBetweenDate)

function displayOrderBetweenDate() {
    const oTable = orderData.map((e, index) => {
        //console.log(e.listOfSpecs)
        let specs = e.listOfSpecs.map(s => {
            return `<tr>
                        <td>${s.merchandiseName}</td>
                        <td>${s.quantity}</td>
                        <td>${s.discount}</td>
                        <td>${s.price}</td>
                    </tr>`
        })

        const dateOptions = {
            year: "numeric",
            month: "2-digit",
            day: "2-digit",
            hour: "numeric",
            minute: "numeric"
        };
        const qDateOptions = {
            year: "numeric",
            month: "2-digit",
            day: "2-digit",            
        };
        const qStartDate = new Date($('#queryStartDate').val()).toLocaleString("zh-TW", qDateOptions);
        const qEndDate = new Date($('#queryEndDate').val()).toLocaleString("zh-TW", qDateOptions);
        const datetime = new Date(e.purchaseTime).toLocaleString("zh-TW", dateOptions);       

        if (qStartDate && qEndDate && datetime >= qStartDate && datetime <= qEndDate) {
            $('#startDate').html(qStartDate)
            $('#endDate').html(qEndDate)

            return `<table class="table table-bordered px-4 py-2 mb-1">
                    <thead>
                        <tr>
                            <th scope="col">訂單日期</th>
                            <th scope="col">訂單編號</th>
                            <th scope="col">總金額</th>
                            <th scope="col">付款方式</th>
                        </tr>
                    </thead>
                    <tbody class="table-group-divider">
                        <tr>
                            <td>${datetime}</td>
                            <td>${e.orderId}</td>
                            <td>${e.paymentAmount}</td>
                            <td>${e.paymentMethodName}</td>
                        </tr>
                    </tbody>
                </table>
                <p>
                <button class="btn btn-primary" type="button" data-bs-toggle="collapse"
                    data-bs-target="#orderdetail${index}">
                    訂單明細
                </button>
                </p>
                <div class="collapse" id="orderdetail${index}">
                <div class="card card-body mb-3">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th scope="col">商品名稱</th>
                                <th scope="col">數量</th>
                                <th scope="col">折扣</th>
                                <th scope="col">價格</th>
                            </tr>
                        </thead>
                        <tbody class="table-group-divider">
                            ${specs.join('') }
                        </tbody>
                    </table>
                </div>
            </div>`;
        } else {
            $('.infotext').html('查無訂單');
            $('.orderTables').html('');
            return;
        }
    })
    $('.orderTables').html(oTable.join(''));

}