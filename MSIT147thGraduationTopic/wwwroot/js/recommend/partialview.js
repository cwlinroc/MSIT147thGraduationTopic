//partial view js


console.log("partial js")

const favorSpecContainer = document.querySelector('#favorSpecContainer')
const merchandiseId = +favorSpecContainer.dataset.merchandiseid
console.log(merchandiseId)



showFavorSpecs()
async function showFavorSpecs() {
    favorSpecContainer.innerHTML = ''
    const data = await getFavorSpecs()
    console.log(data)
    const htmlStr = getRecommendItemsHtml(data)
    favorSpecContainer.innerHTML = htmlStr
}


async function getFavorSpecs() {
    const response = await fetch(`${ROOT}/api/apirecommendpartial/favorspecs/${merchandiseId}`)
    return await response.json()
}


function getRecommendItemsHtml(data) {
    return data.map(value => {
        let imageUrl = 'specPicture/default.png'
        if (!!value.merchandiseImageName) imageUrl = 'merchandisePicture/' + value.merchandiseImageName
        if (!!value.specImageName) imageUrl = 'specPicture/' + value.specImageName
        const canceledPrice = value.discountPercentage == 100 ? '' :
            `<del class="text-secondary mt-auto" style="font-size:3px">$<span>${value.price}</span></del>`
        const score = (Math.round(value.score * 10) / 10).toFixed(1);
        const href = `${ROOT}/Mall/Viewpage/?MerchandiseId=${value.merchandiseId}&SpecId=${value.specId}`
        return `
        <div class="p-2 position-relative">
                <figure class="partial-image-container">
                    <img src="${ROOT}/uploads/${imageUrl}" alt="...">
                </figure>
                <div class="px-1 my-0" style="height:50px;overflow:hidden">
                    <a class="stretched-link link-dark" href="${href}">${value.name}</a>
                </div>
                <div class="my-0 d-flex px-1">
                    ${canceledPrice}
                    <i class="fa-solid fa-star text-warning me-1 ms-auto my-auto"></i>
                    <span>${score}</span>
                </div>
                <div class="text-danger px-1">                    
                    NTD$<span>${Math.round(value.price * value.discountPercentage / 100)}</span>
                </div>
            </div>
        `
    }).join('')

}








