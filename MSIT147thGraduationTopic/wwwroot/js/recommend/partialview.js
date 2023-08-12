//partial view js


const favorSpecContainer = document.querySelector('#favorSpecContainer')
const popularSpecContainer = document.querySelector('#popularSpecContainer')
const merchandiseId = +favorSpecContainer.dataset.merchandiseid


if (!!favorSpecContainer) showRecommendSpecs(favorSpecContainer,'favorspecs')
if (!!popularSpecContainer) showRecommendSpecs(popularSpecContainer, 'popularspecs')

//推薦商品
async function showRecommendSpecs(container,route) {
    container.innerHTML = ''
    const data = await getRecommendSpecs(route)

    const htmlStr = getRecommendItemsHtml(data)
    container.innerHTML = htmlStr
    bindHoverEvents()
}
//ajax取得推薦商品
async function getRecommendSpecs(route) {
    const response = await fetch(`${ROOT}/api/apirecommendpartial/${route}/${merchandiseId}`)
    return await response.json()
}

//將商品data轉換成html
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
        <div class="p-2 position-relative recommend-item">
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

//hover事件
function bindHoverEvents() {
    $('.recommend-item').hover(
        e => {
            $(e.currentTarget).find('figure').addClass('border-danger')
            e.currentTarget.customAnimate('headShake')
        }
        , e => $(e.currentTarget).find('figure').removeClass('border-danger'))
}







