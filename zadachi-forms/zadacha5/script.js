const form = document.getElementById('pizza-choice');

form.addEventListener('submit', (e) => {
    const pizzaCount = document.getElementById('count');

    if (pizzaCount.value > 10 || pizzaCount.value < 1) {
        e.preventDefault();
        alert("Броят на пиците трябва да е между 1 и 10!");
        return;
    }

    const name = document.getElementById('name');

    const pizzaType = document.querySelector('input[name="pizza"]:checked');

    const option = document.getElementById('pizza-options');

    if (!document.getElementById('result')) {
        const div = document.createElement('div');
        div.id = 'result';

        div.innerHTML = `<p>Здравей ${name.value}! Общата цена на поръчката е ${(Number(pizzaType.value) + Number(option.value)) * pizzaCount.value} лв`;

        form.appendChild(div);
    }
    else{
        const div = document.getElementById('result');
        div.innerHTML = '';
        div.innerHTML = `<p>Здравей ${name.value}! Общата цена на поръчката е ${(Number(pizzaType.value) + Number(option.value)) * pizzaCount.value} лв`;
    }

    e.preventDefault();
});