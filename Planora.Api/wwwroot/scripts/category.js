const API = "https://localhost:7127/api/Category"; // ret til din URL

function getValues() {
    return {
        categoryId: document.getElementById("categoryId").value.trim(),
        name: document.getElementById("name").value,
        hexColor: document.getElementById("hexColor").value
    };
}

function showResult(data) {
    document.getElementById("result").textContent = JSON.stringify(data, null, 2);
}

async function create() {
    const { name, hexColor } = getValues();
    try {
        const res = await fetch(API, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ name, hexColor })
        });
        showResult(await res.json());
    } catch (e) {
        showResult("Error: " + e.message);
    }
}

async function getAll() {
    try {
        const res = await fetch(API);
        showResult(await res.json());
    } catch (e) {
        showResult("Error: " + e.message);
    }
}

async function getById() {
    const { categoryId } = getValues();
    if (!categoryId) return showResult("Indtast et CategoryId");
    try {
        const res = await fetch(`${API}/${categoryId}`);
        showResult(await res.json());
    } catch (e) {
        showResult("Error: " + e.message);
    }
}

async function update() {
    const { categoryId, name, hexColor } = getValues();
    if (!categoryId) return showResult("Indtast et CategoryId");
    try {
        const res = await fetch(`${API}/${categoryId}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ categoryId, name, hexColor })
        });
        if (res.status === 204) return showResult("Updated successfully (204)");
        showResult(await res.json());
    } catch (e) {
        showResult("Error: " + e.message);
    }
}

async function remove() {
    const { categoryId } = getValues();
    if (!categoryId) return showResult("Indtast et CategoryId");
    try {
        const res = await fetch(`${API}/${categoryId}`, {
            method: "DELETE"
        });
        if (res.status === 204) return showResult("Deleted successfully (204)");
        showResult(await res.json());
    } catch (e) {
        showResult("Error: " + e.message);
    }
}