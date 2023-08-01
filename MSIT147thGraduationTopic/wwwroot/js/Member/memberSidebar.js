$('#changeAvatar').click(() => { console.log('123') })

//async function change() {
//    const reader = new FileReader()
//    const { value: file } = await Swal.fire({
//        title: 'Select image',
//        input: 'file',
//        showCancelButton: true,
//        inputAttributes: {
//            'accept': 'image/*',
//        },
//        imageUrl: file.target.result
//    })

//    if (file) {        
//        reader.onload = () => {
//            Swal.fire({
//                title: 'Your uploaded picture'                
//            })
//        }
//        reader.readAsDataURL(file)
//    }
//}
