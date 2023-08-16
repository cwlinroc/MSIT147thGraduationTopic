let orderData = null;
getOrders();

//Ajax 得到訂單資料
async function getOrders() {

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

    const sortedOrders = orderData.sort((orderA, orderB) => {
        const timeA = new Date(orderA.purchaseTime);
        const timeB = new Date(orderB.purchaseTime);
        return timeB - timeA;
    });

    const oTable = sortedOrders.map((e, index) => {
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
        const datetimeString = new Date(e.purchaseTime).toLocaleString("zh-TW", dateOptions);

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
                            <td>${datetimeString}</td>
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
                <button id="btnEvaluation_${index}" class="btn btn-primary mx-1 " type="button" data-orderid="${e.orderId}">
                    評價
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

    oTable.forEach((_, index) => {
        $(`#btnEvaluation_${index}`).click(function () {
            var orderId = $(this).data('orderid');
            console.log(orderId)
            window.location.href = '/Evaluation/EIndex/' + orderId;
        });
    })   
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
    const qDateOptions = {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
    };
    const qStartDate = new Date($('#queryStartDate').val())
    const qEndDate = new Date($('#queryEndDate').val())
    const qStartDateString = new Date($('#queryStartDate').val()).toLocaleString("zh-TW", qDateOptions);
    const qEndDateString = new Date($('#queryEndDate').val()).toLocaleString("zh-TW", qDateOptions);
    //$('#startDate').html(qStartDate)
    //$('#endDate').html(qEndDate)

    if (qEndDate < qStartDate) {
        $('.infotext').html('日期錯誤');
        $('.orderTables').html('');
        return;
    }

    const sortedOrders = orderData.sort((orderA, orderB) => {
        const timeA = new Date(orderA.purchaseTime);
        const timeB = new Date(orderB.purchaseTime);
        return timeB - timeA;
    });

    const oTable = sortedOrders.map((e, index) => {
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

        const datetime = new Date(e.purchaseTime);
        const datetimeString = new Date(e.purchaseTime).toLocaleString("zh-TW", dateOptions);

        if (datetime >= qStartDate && datetime <= qEndDate) {
            $('.infotext').html(`您的查詢區間為 ${qStartDateString} 至 ${qEndDateString}`);
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
                            <td>${datetimeString}</td>
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
                <button id="btnEvaluation" class="btn btn-primary mx-1 " type="button" data-orderid="${e.orderId}">
                    評價
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
        } else if ((datetime < qStartDate || datetime > qEndDate) && sortedOrders < 1) {
            $('.infotext').html('查無訂單');
            $('.orderTables').html('');
            return;
        }
    })
    $('.orderTables').html(oTable.join(''));
    
}
