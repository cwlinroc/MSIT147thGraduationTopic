//權限錯誤跳轉
//if (!(ROLE == '管理員' || ROLE == '經理' || ROLE == '員工')) {
//    window.location.href = ROOT + '/home/index'
//}


$(document).ready(function () {
    $('#sidebarToggle').on('click', function () {
        $('.sidebar').toggleClass('active');
    });
});

$(document).ready(function () {
    $('#sidebarCollapse').on('click', function () {
        $('.sidebar').removeClass('active');
    });
});



$('#btnLogOut').click(LogOut)

async function LogOut() {
    
    const response = await fetch(ROOT + '/api/apimember/logout')

    if (response.ok) {
        const url = await response.text()
        if (url) {
            alert('成功登出')
            window.location.href = url
        }
    }
}

//使用密碼再次確認
async function confirmWithPassword() {
    const { value: password } = await Swal.fire({
        title: '請輸入密碼確認',
        input: 'password',
        inputLabel: 'Your password',
        inputValue: "",
        showCancelButton: true,
        confirmButtonText: '確認',
        cancelButtonText: `離開`,
        inputValidator: (value) => {
            if (!value) {
                return '請輸入密碼'
            }
        }
    })
    if (!password) return false

    const response = await fetch(`${ROOT}/api/apiemployee/confirm`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ 'password': password })
    });

    if (!response.ok) {
        console.log('fail')
        return false
    }

    const result = await response.json()
    return result
}