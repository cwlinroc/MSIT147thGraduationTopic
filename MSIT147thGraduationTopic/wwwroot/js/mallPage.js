$(".custom-merchandise").hover(
    e => $(e.currentTarget).addClass("shadow-lg"),
    e => $(e.currentTarget).removeClass("shadow-lg")
)

$("#backToTop").hide()

$("#backToTop").click(e => $(window).scrollTop(0))

$(window).scroll(e => {
    if ($(window).scrollTop() < 300) {
        $("#backToTop").hide()
    }
    else {
        $("#backToTop").show()
    }
})


//登入
$('#btnLogIn').click(LogIn)
$('#btnLogOut').click(LogOut)
//Ajax 
async function LogIn() {
    //驗證

    const account = $('#loginAccount').val()
    const password = $('#loginPassword').val()
    const chkRemember = $('#chkRemember').prop('checked')

    const response = await fetch('@Url.Content("~/api/apimember/login")', {
        body: JSON.stringify({ 'Account': account, 'Password': password, 'chkRemember': chkRemember }),
        method: 'POST'
    })

    if (response.ok) {
        const url = await response.text()
        console.log(url)
        if (url) {
            alert('成功登入')
            window.location.href = url
        }
    }
}

async function LogOut() {

    const response = await fetch('@Url.Content("~/api/apimember/logout")')

    if (response.ok) {
        const url = await response.text()
        console.log(url)
        if (url) {
            alert('成功登出')
            window.location.href = url
        }
    }
}
