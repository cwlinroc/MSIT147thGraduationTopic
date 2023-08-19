//權限錯誤跳轉
if (!(ROLE == '管理員' || ROLE == '經理' || ROLE == '員工')) {
    window.location.href = ROOT + '/home/index'
}


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


//登出後台
$('#btnLogOut').click(LogOut)
async function LogOut() {

    const response = await fetch(ROOT + '/api/apimember/logout')

    if (response.ok) {
        const url = await response.text()
        if (url) {
            await Swal.fire(
                '成功登出!',
                '將導入商城首頁',
                'success')
            window.location.href = url
        }
    }
}

//修改頭像
$('#changeSelfAvatar').click(async e => {

    const { value: file } = await Swal.fire({
        title: 'Select image',
        input: 'file',
        inputAttributes: {
            'accept': 'image/*',
            'aria-label': 'Upload your profile picture'
        }
    })
    const formdata = new FormData()
    formdata.append('image', file, file.name)

    const response = await fetch(`${ROOT}/api/apiemployee/selfavatar`, {
        method: 'POST',
        body: formdata,
    })
    if (!response.ok) return

    const result = await response.json()
    if (result >= 0) {
        Swal.fire('成功上傳圖片', '確定')
        displaySelfAvatar()
    }
})

//更新頭像

displaySelfAvatar()
async function displaySelfAvatar() {
    const response = await fetch(`${ROOT}/api/apiemployee/selfavatar`)
    let avatarName = await response.text()
    if (!avatarName) avatarName = '_employeeDefault.jpg'
    const avatarUrl = `${ROOT}/uploads/employeeAvatar/${avatarName}`
    $('#navbarAvatar').attr('src', avatarUrl)
}

//更新密碼


//使用密碼再次確認
async function confirmWithPassword() {
    const { value: password } = await Swal.fire({
        title: '請輸入密碼確認',
        input: 'password',
        inputLabel: '因為是demo所以預先輸入密碼(demoAdmin99)',
        inputValue: "demoAdmin99",
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

    if (!result) {
        await Swal.fire(
            '錯誤!',
            '輸入密碼錯誤!',
            'error')
    }
    return result
}

//權限(角色)驗證
async function validateRole(role) {
    const roles = ['員工', '經理', '管理員']
    const requiredPermission = roles.indexOf(role)
    const accessPermission = roles.indexOf(ROLE)
    const isValid = accessPermission >= requiredPermission
    if (!isValid) {
        Swal.fire(
            '權限不足!',
            '無法執行此功能',
            'error')
    }
    return isValid
}

//檢視員工資料驗證
$('#linkEmployeeList').click(async event => {
    if (!await validateRole('經理')) event.preventDefault();
})
//linkRecommendRating
$('#linkRecommendRating').click(async event => {
    if (!await validateRole('經理')) event.preventDefault();
})



//讀取動畫
const loadingBox = document.querySelector('.loading-box')
const showLoadingBox = () => loadingBox.style.display = "block";
const hideLoadingBox = () => loadingBox.style.display = "none"


