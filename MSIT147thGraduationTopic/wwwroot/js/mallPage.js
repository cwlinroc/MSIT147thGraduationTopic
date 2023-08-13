//權限錯誤跳轉
//if (ROLE == '管理員' || ROLE == '經理' || ROLE == '員工') {
//    window.location.href = ROOT + '/employeebackstage/welcome'
//}


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

$('#demoMember88').click(() => DemoLogin('demoMember88'));
$('#demoMember99').click(() => DemoLogin('demoMember99'));
$('#demoEmployee').click(() => DemoLogin('demoEmployee99'));
$('#demoManager').click(() => DemoLogin('demoManager99'));
$('#demoAdmin').click(() => DemoLogin('demoAdmin99'));

function DemoLogin(demoAccount) {
    $('#loginAccount').val(demoAccount);
    $('#loginPassword').val(demoAccount);
}

//記住我
$(function () {
    if (localStorage.chkRemember && localStorage.chkRemember != '') {
        $('#chkRemember').attr('checked', 'checked');
        $('#loginAccount').val(localStorage.loginAccount);
        $('#loginPassword').val(localStorage.loginPassword);
    } else {
        $('#chkRemember').removeAttr('checked');
        $('#loginAccount').val('');
        $('#loginPassword').val('');
    }

    $('#chkRemember').click(function () {

        if ($('#chkRemember').is(':checked')) {
            localStorage.loginAccount = $('#loginAccount').val();
            localStorage.loginPassword = $('#loginPassword').val();
            localStorage.chkRemember = $('#chkRemember').val();
        } else {
            localStorage.loginAccount = '';
            localStorage.loginPassword = '';
            localStorage.chkRemember = '';
        }
    });
});

//Ajax 登入
async function LogIn() {
    //驗證
    const account = $('#loginAccount').val()
    const password = $('#loginPassword').val()    

    const response = await fetch(ROOT + '/api/apimember/login', {
        body: JSON.stringify({ 'Account': account, 'Password': password }),
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
        return;
    } else if (url === 'Member/NoRole') {
        Swal.fire({
            icon: 'error',
            title: '沒有授權',
            text: '您未開通或已遭停權',
            allowOutsideClick: false
        })
        return;
    }
    

    Swal.fire({
        icon: 'success',
        title: '登入成功!',
        allowOutsideClick: false
    }).then(() => {
        if (url === 'reload') {
            window.location.reload()
        }
        else {
            window.location.href = url
        }
    })
}

async function ForgetPwd() {
    const account = $('#loginAccount').val()   

    const response = await fetch(ROOT + '/api/apisendmail/forgetPwd', {
        body: JSON.stringify({ 'Account': account}),
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
            title: '帳號不存在!',
            allowOutsideClick: false
        })
        return
    }

    Swal.fire({
        icon: 'success',
        title: '請至信箱收信!',
        allowOutsideClick: false
    })
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
                    window.location.href = ROOT + '/home/index'
                }
            })
        }
    }
}

//導覽列購物車數量
if (ROLE === '會員') refreshNavbarCart()
async function refreshNavbarCart() {
    const response = await fetch('/api/apicart/cartcount')
    const cartItemNumber = await response.json()
    const numberBadge = document.querySelector('#cartCount')
    if (!+cartItemNumber) {
        numberBadge.style.display = 'none'
    }
    else {
        numberBadge.style.display = 'block'
        numberBadge.querySelector('div').textContent = cartItemNumber
        numberBadge.closest('a').customAnimate('fadeIn')
    }

}















