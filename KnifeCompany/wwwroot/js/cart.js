function register() {
    alert('Check your email');
    return false;
}

function calc_price(price, count) {
    var total = price * count;
    total.toLocaleString(undefined, { maximumFractionDigits: 2, minimumFractionDigits: 2 });
    document.getElementById("calc_price").innerHTML = total;
}