
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
    console.log('test')
    const response = await fetch(ROOT + '/api/apimember/logout')

    if (response.ok) {
        const url = await response.text()
        if (url) {
            alert('¦¨¥\µn¥X')
            window.location.href = url
        }
    }
}