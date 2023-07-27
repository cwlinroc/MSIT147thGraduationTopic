'use strict'
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


//Buttons
$('#btnLogIn').click(LogIn)
$('#btnLogOut').click(LogOut)



//Ajax 登入
async function LogIn() {
    //驗證

    const account = $('#loginAccount').val()
    const password = $('#loginPassword').val()
    const chkRemember = $('#chkRemember').prop('checked')

    const response = await fetch(ROOT + '/api/apimember/login', {
        body: JSON.stringify({ 'Account': account, 'Password': password, 'chkRemember': chkRemember }),
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
    })

    if (!response.ok) {
        console.log('request failed')
        return
    }

    const url = await response.text()

    if (!url) {
        Swal.fire({
            icon: 'error',
            title: '登入失敗',
            text: '帳號或密碼錯誤',
            allowOutsideClick: false
        })
        return
    }

    Swal.fire({
        icon: 'success',
        title: '登入成功!',
        allowOutsideClick: false
    }).then(result => {
        if (url == 'reload') {
            window.location.reload()
        }
        else {
            window.location.href = url
        }
    })

    if (localStorage.chkRemember && localStorage.chkRemember !== '') {
        $('#chkRemember').attr('checked', 'checked');
        $('#loginAccount').val(localStorage.loginAccount);
    } else {
        $('#chkRemember').removeattr('ckecked');
        $('#loginAccount').val('');
    }

    $('#chkRemember').click(function () {
        if ($('#chkRemember').is(':checked')) {
            localStorage.loginAccount = $('#loginAccount').val();
            localStorage.chkRemember = $('#chkRemember').val();
        } else {
            localStorage.loginAccount = '';
            localStorage.chkRemember = '';
        }
    }    

}

//Ajax 登出
async function LogOut() {

    const response = await fetch(ROOT + '/api/apimember/logout')

    if (response.ok) {
        const url = await response.text()
        if (url) {
            Swal.fire({
                icon: 'success',
                title: '登出成功!',
                allowOutsideClick: false
            }).then(result => {
                if (result.isConfirmed) {
                    window.location.reload()
                }
            })
        }
    }
}
