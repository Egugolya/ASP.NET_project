const form = document.querySelector("#login-form");
const result = document.querySelector("#result");
const loadMessagesButton = document.querySelector("#load-messages");
const messagesList = document.querySelector("#messages");
const orderForm = document.querySelector("#order-form");
const ordersResult = document.querySelector("#orders-result");
const loadOrdersButton = document.querySelector("#load-orders");
const ordersContainer = document.querySelector("#orders");

loadMessagesButton.disabled = !localStorage.getItem("token");

form.addEventListener("submit", async (event) => {
    event.preventDefault();
    const values = new FormData(form);
    const credentials = {
        email: values.get("email").trim(),
        password: values.get("password")
    };

    try {
        const response = await fetch("/api/sender/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(credentials)
        });
        const responseBody = await response.text();
        if (response.status === 401) {
            result.textContent = "Невірний email або пароль.";
            return;
        }
        if (!response.ok) {
            result.textContent = `HTTP ${response.status} ${response.statusText}\n${responseBody}`;
            return;
        }

        const loginResult = JSON.parse(responseBody);
        if (!loginResult.token) {
            result.textContent = "Сервер не повернув токен.";
            return;
        }

        localStorage.setItem("token", loginResult.token);
        loadMessagesButton.disabled = false;
        result.textContent = "Вхід успішний. Токен збережено.";
    } catch (error) {
        result.textContent = `Помилка запиту: ${error.message}`;
    }
});

loadMessagesButton.addEventListener("click", async () => {
    const token = localStorage.getItem("token");
    if (!token) {
        result.textContent = "Спочатку увійдіть.";
        return;
    }
    messagesList.replaceChildren();

    try {
        const response = await fetch("/messages", {
            headers: { "Authorization": `Bearer ${token}` }
        });
        const responseBody = await response.text();
        if (!response.ok) {
            result.textContent = `HTTP ${response.status} ${response.statusText}\n${responseBody}`;
            return;
        }

        const messages = JSON.parse(responseBody);
        for (const message of messages) {
            const item = document.createElement("li");
            const dateTime = message.DateTime ?? message.dateTime ?? "";
            item.textContent = `${message.from} → ${message.to}: ${message.content} ${dateTime}`;
            messagesList.append(item);
        }
    } catch (error) {
        result.textContent = `Помилка запиту: ${error.message}`;
    }
});

orderForm.addEventListener("submit", async (event) => {
    event.preventDefault();
    ordersResult.textContent = "";

    const values = new FormData(orderForm);
    const order = {
        orderDate: values.get("orderDate"),
        productsQuantity: Number(values.get("productsQuantity")),
        productsCost: Number(values.get("productsCost")),
        comment: values.get("comment") || null
    };

    try {
        const response = await fetch("/api/orders", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(order)
        });
        const responseBody = await response.text();
        if (!response.ok) {
            ordersResult.textContent = `Не вдалося створити замовлення: HTTP ${response.status}\n${responseBody}`;
            return;
        }

        orderForm.reset();
        ordersResult.textContent = "Замовлення створено.";
        await loadOrders();
    } catch (error) {
        ordersResult.textContent = `Помилка запиту: ${error.message}`;
    }
});

async function loadOrders() {
    loadOrdersButton.disabled = true;
    ordersResult.textContent = "Завантаження замовлень...";
    ordersContainer.replaceChildren();

    try {
        const response = await fetch("/api/orders", { cache: "no-store" });
        const responseBody = await response.text();
        if (!response.ok) {
            ordersResult.textContent = `Не вдалося завантажити замовлення: HTTP ${response.status}\n${responseBody}`;
            return;
        }

        const orders = JSON.parse(responseBody);
        if (orders.length === 0) {
            ordersResult.textContent = "Замовлень поки немає.";
            return;
        }

        for (const order of orders) {
            ordersContainer.append(createOrderElement(order));
        }
    } catch (error) {
        ordersResult.textContent = `Помилка запиту: ${error.message}`;
    } finally {
        loadOrdersButton.disabled = false;
    }
}

function createOrderElement(order) {
    const orderNumber = order.orderNumber ?? order.OrderNumber;
    const quantity = order.productsQuantity ?? order.ProductsQuantity;
    const isCompleted = (order.status ?? order.Status) === "виконано";
    const section = document.createElement("section");
    const heading = document.createElement("h3");
    heading.textContent = `Замовлення №${orderNumber}`;

    const details = document.createElement("p");
    details.textContent = `Дата: ${order.orderDate ?? order.OrderDate}; товарів: ${quantity}; вартість: ${order.productsCost ?? order.ProductsCost}; створено: ${order.orderDateTime ?? order.OrderDateTime}; статус: ${order.status ?? order.Status}; коментар: ${order.comment ?? order.Comment ?? "—"}`;

    const quantityForm = document.createElement("form");
    const quantityLabel = document.createElement("label");
    quantityLabel.textContent = "Кількість товарів: ";
    const quantityInput = document.createElement("input");
    quantityInput.type = "number";
    quantityInput.min = "1";
    quantityInput.step = "1";
    quantityInput.value = quantity;
    quantityInput.required = true;
    const quantityButton = document.createElement("button");
    quantityButton.type = "submit";
    quantityButton.textContent = "Оновити кількість";
    quantityLabel.append(quantityInput);
    quantityForm.append(quantityLabel, quantityButton);
    quantityForm.addEventListener("submit", async (event) => {
        event.preventDefault();
        await updateOrder(orderNumber, "quantity", { productsQuantity: Number(quantityInput.value) });
    });

    const statusButton = document.createElement("button");
    statusButton.type = "button";
    statusButton.textContent = isCompleted ? "Позначити невиконаним" : "Позначити виконаним";
    statusButton.addEventListener("click", async () => {
        await updateOrder(orderNumber, "status", { isCompleted: !isCompleted });
    });

    section.append(heading, details, quantityForm, statusButton, document.createElement("hr"));
    return section;
}

async function updateOrder(orderNumber, operation, body) {
    ordersResult.textContent = "";
    try {
        const response = await fetch(`/api/orders/${orderNumber}/${operation}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        });
        const responseBody = await response.text();
        if (!response.ok) {
            ordersResult.textContent = `Не вдалося оновити замовлення: HTTP ${response.status}\n${responseBody}`;
            return;
        }

        ordersResult.textContent = "Замовлення оновлено.";
        await loadOrders();
    } catch (error) {
        ordersResult.textContent = `Помилка запиту: ${error.message}`;
    }
}

loadOrdersButton.addEventListener("click", loadOrders);
loadOrders();
