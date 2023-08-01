//權限錯誤跳轉
//if (ROLE == '管理員' || ROLE == '經理' || ROLE == '員工') {
//    window.location.href = ROOT + '/employeebackstage/welcome'
//}



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
$('#demoMember').click(() => {
    $('#loginAccount').val('demoMember99');
    DemoLogin('demoMember99');
});
$('#demoEmployee').click(() => {
    $('#loginAccount').val('demoEmployee99');
    DemoLogin('demoEmployee99');
});

$('#demoManager').click(() => {
    $('#loginAccount').val('demoManager99');
    DemoLogin('demoManager99');
});

$('#demoAdmin').click(() => {
    $('#loginAccount').val('demoAdmin99');
    DemoLogin('demoAdmin99');
});

function DemoLogin(demoAccount) {
    switch (demoAccount) {
        case 'demoMember99':
            $('#loginPassword').val('demoMember99');
            break;
        case 'demoEmployee99':
            $('#loginPassword').val('demoEmployee99');
            break;
        case 'demoManager99':
            $('#loginPassword').val('demoManager99');
            break;
        case 'demoAdmin99':
            $('#loginPassword').val('demoAdmin99');
            break;
    }
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








