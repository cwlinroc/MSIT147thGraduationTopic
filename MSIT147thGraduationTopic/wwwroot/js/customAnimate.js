HTMLElement.prototype.customAnimate = function (animation) {
    console.log('test custom')
    const prefix = 'animate__'
    const node = this
    const animationName = `${prefix}${animation}`
    return new Promise((resolve,reject)=>{
        node.classList.add(`${prefix}animated`, animationName);

        // When the animation ends, we clean the classes and resolve the Promise
        function handleAnimationEnd(event) {
            event.stopPropagation();
            node.classList.remove(`${prefix}animated`, animationName);
            resolve('Animation ended');
        }
    
        node.addEventListener('animationend', handleAnimationEnd, { once: true });
    })
}


// const animateCSS = (element, animation, prefix = 'animate__') =>
//     // We create a Promise and return it
//     new Promise((resolve, reject) => {
//         const animationName = `${prefix}${animation}`;
//         const node = document.querySelector(element);

//         node.classList.add(`${prefix}animated`, animationName);

//         // When the animation ends, we clean the classes and resolve the Promise
//         function handleAnimationEnd(event) {
//             event.stopPropagation();
//             node.classList.remove(`${prefix}animated`, animationName);
//             resolve('Animation ended');
//         }

//         node.addEventListener('animationend', handleAnimationEnd, { once: true });
//     });

