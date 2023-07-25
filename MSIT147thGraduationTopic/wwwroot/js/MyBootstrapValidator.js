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
    constructor(form) {
        this.form = form
    }
    validateFunction(validfunc) {
        this.validfunc = validfunc
    }
    startValidate() {
        $(this.form).find('input').on('keyup', () => {
            $(this.form).find('input').removeClass('is-invalid is-valid')
            this.validfunc()
        })

        return (this.validfunc)()
    }

    endtValidate() {
        $(this.form).find('input').removeClass('is-invalid is-valid').off('keyup')
    }

}