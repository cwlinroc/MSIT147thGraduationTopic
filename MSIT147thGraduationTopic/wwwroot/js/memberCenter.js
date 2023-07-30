let memberData = null;
getMembers();

//Ajax 得到會員資料
async function getMembers(query) {
    let str = '';
    if (query) str = '/' + query;

    const response = await fetch('@Url.Content("~/api/apiMember")' + str);

    if (!response.ok) return;
    const data = await response.json();

    memberData = data
    console.log(data);
    displayMembers();
}

//列出會員資料
function displayMembers() {
    const datas = memberData.map((element) => {
        const imageName = (element.avatarName) ? element.avatarName : 'avatar_icon.png'
        return 
    })    
    
}

function checkPasswordMatch() {
    const password = $('#password').val();
    const confirmPassword = $('#confirmPassword').val();

    if (password !== confirmPassword) {
        $('#confirmPassword').addClass('is-invalid');
    } else {
        $('#confirmPassword').removeClass('is-invalid');
    }
}

$('#confirmPassword').blur(function () {
    checkPasswordMatch();
});

const selCity = document.querySelector('#selectCity');
const selDistrict = document.querySelector('#selectDistrict');

LoadCities();

async function LoadCities() {
    try {
        const response = await fetch('/Member/Cities');
        const datas = await response.json();

        var cities = datas.map(city => {
            return (`<option value="${city}">${city}</option>`);
        });

        selCity.innerHTML = cities.join("");
        LoadDistricts();
    } catch (error) {
        console.error(error);
    }
}

async function LoadDistricts() {
    try {
        const city = selCity.options[selCity.selectedIndex].value;
        const response = await fetch(`/Member/Districts?city=${city}`);
        const datas = await response.json();

        var districts = datas.map(district => {
            return (`<option value="${district}">${district}</option>`);
        });

        selDistrict.innerHTML = districts.join("");
    } catch (error) {
        console.error(error);
    }
}

selCity.addEventListener('change', () => {
    LoadDistricts();
});

//取得全部需要驗證表單
const forms = document.querySelectorAll('.needs-validation')

Array.from(forms).forEach(form => {
    form.addEventListener('submit', async event => {
        event.preventDefault()
        event.stopPropagation()

        if (!form.checkValidity()) {
            await Swal.fire({
                icon: 'error',
                title: '註冊失敗!',
                text: '資料有錯誤,請修改',
                allowOutsideClick: false,
            })
            form.classList.add('was-validated')
            return
        }

        const formData = new FormData(form)

        const response = await fetch(`${ROOT} /api/ApiMember`, {
            body: formData,
            method: 'POST'
        })

        if (!response.ok) {
            console.log('輸入失敗')
            return
        }

        const memberId = await response.json()

        if (memberId <= 0) {
            console.log('沒有成功寫入資料庫')
            return
        }

        await Swal.fire({
            icon: 'success',
            title: '修改成功!',
            allowOutsideClick: false
        })

        window.location.reload();

    }, false)
})