let vantaEffect = null;

window.initVantaBackground = (elementId, options) => {
    if (vantaEffect) {
        //console.log("VantaBackground already enabled, skipping..");
        return;
    }
    vantaEffect = VANTA.NET({
        el: "#" + elementId,
        minHeight: 200.00,
        minWidth: 200.00,
        scale: 1.00,
        scaleMobile: 1.00,
        color: 0x096af2,
        backgroundColor: 0x000000,
        ...options
    });
    //console.log("VantaBackground enabled!");
};

window.destroyVantaBackground = () => {
    if (vantaEffect) {
        vantaEffect.destroy();
        vantaEffect = null;
        //console.log("VantaBackground disabled!");
    }
};

function updateRedactedStyle(element, isHidden) {
    if (isHidden) {
        element.classList.add('redacted');
        element.classList.remove('normal');
    } else {
        element.classList.remove('redacted');
        element.classList.add('normal');
    }
    element.style.opacity = '1';
}



window.scrollElementIntoView = (element) => {
    if (element) {
        element.scrollIntoView();
    }
};

window.confetti_stars = () => {
    const defaults = {
        spread: 360,
        ticks: 50,
        gravity: 0,
        decay: 0.94,
        startVelocity: 30,
        shapes: ["star"],
        colors: ["FFE400", "FFBD00", "E89400", "FFCA6C", "FDFFB8"],
    };

    function shoot() {
        confetti({
            ...defaults,
            particleCount: 40,
            scalar: 1.2,
            shapes: ["star"],
        });

        confetti({
            ...defaults,
            particleCount: 10,
            scalar: 0.75,
            shapes: ["circle"],
        });
    }

    setTimeout(shoot, 0);
    setTimeout(shoot, 100);
    setTimeout(shoot, 200);
}

window.confetti_fireworks = () => {
    const duration = 5 * 1000,
    animationEnd = Date.now() + duration,
    defaults = { startVelocity: 30, spread: 360, ticks: 60, zIndex: 0 };

    function randomInRange(min, max) {
    return Math.random() * (max - min) + min;
}

const interval = setInterval(function () {
    const timeLeft = animationEnd - Date.now();

    if (timeLeft <= 0) {
        return clearInterval(interval);
    }

    const particleCount = 150 * (timeLeft / duration);

    // since particles fall down, start a bit higher than random
    confetti(
        Object.assign({}, defaults, {
            particleCount,
            origin: { x: randomInRange(0.1, 0.3), y: Math.random() - 0.2 },
        })
    );
    confetti(
        Object.assign({}, defaults, {
            particleCount,
            origin: { x: randomInRange(0.7, 0.9), y: Math.random() - 0.2 },
        })
    );
}, 250);
}