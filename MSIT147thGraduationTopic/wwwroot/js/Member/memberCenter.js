let memberData = null;
getMember();

//Ajax 得到會員資料
async function getMember() {
    const response = await fetch(`${ROOT}/api/apiMember/self`);

    if (!response.ok) return;
    const data = await response.json();

    memberData = data
    console.log(data);
    displayMember();
}

//列出會員資料
function displayMember() {
    $('#nickName').val(memberData.nickName)
    $('#email').val(memberData.email)
    $('#phone').val(memberData.phone)
    $('#city').val(memberData.city)
    $('#district').val(memberData.district)
    $('#address').val(memberData.address)
    $('#avatar').val(memberData.avatar)
}

//function checkPasswordMatch() {
//    const password = $('#password').val();
//    const confirmPassword = $('#confirmPassword').val();

//    if (password !== confirmPassword) {
//        $('#confirmPassword').addClass('is-invalid');
//    } else {
//        $('#confirmPassword').removeClass('is-invalid');
//    }
//}

//$('#confirmPassword').blur(function () {
//    checkPasswordMatch();
//});

const selCity = document.querySelector('#city');
const selDistrict = document.querySelector('#district');

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


$('.passwordEditedChk').click(function () {
    if ($('.passwordEditedChk').is(':checked')) {
        $('#password').prop('readonly', false);
        $('#confirmPassword').prop('readonly', false);
    } else {
        $('#password').prop('readonly', true);
        $('#confirmPassword').prop('readonly', true);
    }
});


const myValid = new MyBootsrapValidator(document.querySelector('.needs-validation'))

function editValidator() {
    myValid.endtValidate()
    myValid.validateFunction(() => {
        const password = document.querySelector('#password')
        const confirmPassword = document.querySelector('#confirmPassword')
        const phone = document.querySelector('#phone')
        const email = document.querySelector('#email')

        let passwordValid = false
        const passwordHasValue = !!password.value
        const passwordPatternValid = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{6,32}$/.test(password.value)
        const confirmPasswordValid = confirmPassword.value === password.value

        if ($('.passwordEditedChk').prop('checked')) {
            password.setValidate(() => passwordHasValue, '請輸入密碼')
                .setValidate(() => passwordPatternValid, '密碼格式錯誤')
            confirmPassword.setValidate(() => confirmPasswordValid, '與密碼不符')
            passwordValid = passwordHasValue && passwordPatternValid && confirmPasswordValid
        }
        else {
            password.setValidate(() => true)
            passwordValid = true
        }

        const phoneHasValue = !!phone.value
        let phonePatternValid = /^09\d{2}-\d{3}-\d{3}$/.test(phone.value)

        phone.setValidate(() => phoneHasValue, '請輸入手機號碼')
            .setValidate(() => phonePatternValid, '手機號碼格式錯誤')

        const emailHasValue = !!email.value
        let emailPatternValid = /^\w+@\w+/.test(email.value)

        email.setValidate(() => emailHasValue, '請輸入Email')
            .setValidate(() => emailPatternValid, 'Email格式錯誤')

        return passwordValid && phoneHasValue && phonePatternValid
            && emailHasValue && emailPatternValid
    })
    return myValid.startValidate()
}


$('#btnSubmit').click(async event => {
    event.preventDefault()
    event.stopPropagation()
    const check = await editValidator()
    if (!check) return

    const result = await Swal.fire({
        icon: 'question',
        title: '確定要修改嗎?',
        showCancelButton: true,
        allowOutsideClick: false,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: '確定',
        cancelButtonText: '取消'
    })
    if (!result.isConfirmed) return
    submitEvent()
})

async function submitEvent() {
    const formData = new FormData(document.editForm)
    const response = await fetch(`${ROOT}/api/ApiMember/memberCenter`, {
        method: 'PUT',
        body: formData
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
}

//const forms = document.querySelectorAll('.needs-validation')

//Array.from(forms).forEach(form => {
//    form.addEventListener('submit', async event => {
//        event.preventDefault()
//        event.stopPropagation()

//        await Swal.fire({
//            icon: 'question',
//            title: '確定要修改嗎?',
//            showCancelButton: true,
//            allowOutsideClick: false,
//        })


//        if (!form.checkValidity()) {
//            await Swal.fire({
//                icon: 'error',
//                title: '修改失敗!',
//                text: '資料有錯誤,請修改',
//                allowOutsideClick: false,
//            })
//            return
//        }

//        const formData = new FormData(form)

//        const response = await fetch(`${ROOT} /api/ApiMember/memberCenter`, {
//            body: formData,
//            method: 'put'
//        })

//        if (!response.ok) {
//            console.log('輸入失敗')
//            return
//        }

//        const memberId = await response.json()

//        if (memberId <= 0) {
//            console.log('沒有成功寫入資料庫')
//            return
//        }

//        await Swal.fire({
//            icon: 'success',
//            title: '修改成功!',
//            allowOutsideClick: false
//        })

//        window.location.reload();

//    }, false)
//})