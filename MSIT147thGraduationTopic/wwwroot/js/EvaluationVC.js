
$("#loadMore").click(function () {
    //按下時獲取下一頁
    var currentPage = parseInt($(this).attr("data-page")) + 1;  //parseInt 函式會把第一個參數變成字串、解析它、再回傳整數或是 NaN
    loadMoreComments(currentPage);
});
function loadMoreComments(page) {
    $.ajax({
        url: "/ApiEvaluation/ShowMoreEvaliation",
        method: "GET",
        data: {id: merchandiseId, evaluationPageCounts: page},
        success: function (data) {
            //console.log("Received data:", data)
            if (data.length > 0) {
                let newCommentsContainer = $('#commentContainer');
                for (let i = 0; i < data.length; i++) {
                    console.log(data[i].merchandiseName);
                    var newComment = 
                        (`<div class="clickloadMore" style="border-block-color:#FFDDAA;border-width: 2px">
                            <div class="mt-4 d-flex align-items-center">
                                <div class="d-flex rounded-3 px-1 " style="background-color:#FFDDAA">
                                    <div class="mx-1 mt-2">
                                        <h6 id="MerchandiseName_@(i)">${data[i].merchandiseName} </h6>
                                    </div>
                                    <div class="mt-2">
                                        <h6 id="SpecName_@(i)"> ${data[i].specName}</h6>
                                    </div>
                                </div>
                                <div class="d-flex ms-auto float-end">
                                    <div id="score_@(i)" name="Score" class="float-end mx-1 mt-3" itype="starDiv">
                                        <input id="inputscore_@(i)" name="comments[@(i)].Score" value="@(${data[i].score})" hidden>
                                        <img id="star@(i)_1" src="${data[i].score >= 1 ? "/images/一星.png" : "/images/空星.png"}" style="height:40px" />
                                        <img id="star@(i)_2" src="${data[i].score >= 2 ? "/images/二星.png" : "/images/空星.png"}" style="height:40px" />
                                        <img id="star@(i)_3" src="${data[i].score >= 3 ? "/images/三星.png" : "/images/空星.png"}" style="height:40px" />
                                        <img id="star@(i)_4" src="${data[i].score >= 4 ? "/images/四星.png" : "/images/空星.png"}" style="height:40px" />
                                        <img id="star@(i)_5" src="${data[i].score == 5 ? "/images/五星.png" : "/images/空星.png"}" style="height:40px" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-floating mx-2 mt-2 mb-4">
                                <textarea id="commentText_@(i)" name="comments[@(i)].Comment" class="form-control" readonly="readonly"></textarea>                    
                                <label for="floatingTextarea2" style="color:black; font-size: 20px;">${data[i].comment == null ? "" : data[i].comment}</label>
                            </div>
                        </div>`)
                    newCommentsContainer.append(newComment);
                }

                // 更新 "加载更多" 按钮的 data-page 属性
                $("#loadMore").attr("data-page", page);
            } else {
                $("#loadMore").hide();
            }
        },
        error: function () {
            console.log("请求失败");
        }
    });
}     