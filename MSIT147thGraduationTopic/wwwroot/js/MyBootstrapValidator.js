HTMLElement.prototype.setValidate = function (isValid, message) {
    if (this.classList.contains('is-invalid')) return this

    const addClass = (isValid(this)) ? 'is-valid' : 'is-invalid'
    this.classList.remove('is-valid')
    this.classList.add(addClass)

    if (message) this.parentElement.querySelector('.invalid-feedback').textContent = message

    return this
}


class MyBootsrapValidator {
    form
    validfunc
    keyupValid
    addedKeyUp = false
    constructor(form) {
        this.form = form
    }
    validateFunction(validfunc) {
        this.validfunc = validfunc
    }
    keyupValidateFunction(keyupValid) {
        this.keyupValid = keyupValid
    }
    startValidate() {
        if (!this.addedKeyUp) {
            if (!this.keyupValid) this.keyupValid = this.validfunc
            $(this.form).find('input').on('keyup', () => {
                $(this.form).find('input').removeClass('is-invalid is-valid')
                this.keyupValid()
            })
            $(this.form).find('input[type=radio]').on('click', () => {
                $(this.form).find('input').removeClass('is-invalid is-valid')
                this.keyupValid()
            })
            $(this.form).find('select').on('change', () => {
                $(this.form).find('select').removeClass('is-invalid is-valid')
                this.keyupValid()
            })
            this.addedKeyUp = true
        }
        return (this.validfunc)()
    }

    endtValidate() {
        $(this.form).find('input').removeClass('is-invalid is-valid').off('keyup').off('click')
        //$(this.form).find('input').removeClass('is-invalid is-valid').off('keyup','click')
        $(this.form).find('select').removeClass('is-invalid is-valid')
        this.addedKeyUp = false
    }

}


