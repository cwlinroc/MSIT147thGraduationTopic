
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