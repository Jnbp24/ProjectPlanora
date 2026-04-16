const API = "https://localhost:7127/api/Category"
// --- Category management ---
const nameInput = document.getElementById('category-name');
const colorInput = document.getElementById('category-color');
const addBtn = document.getElementById('add-category-btn');
const listEl = document.getElementById('category-list');
const emptyState = document.getElementById('category-empty');

addBtn.addEventListener('click', async () => await createCategory());
nameInput.addEventListener('keydown', async e => {
    if (e.key === 'Enter') {
        await createCategory()
    }
});

render()

async function createCategory() {
    const name = nameInput.value.trim();
    const hexColor = colorInput.value;

    if (!name) {
        nameInput.focus();
        nameInput.classList.add('shake');
        setTimeout(() => nameInput.classList.remove('shake'), 400);
        return;
    }

    // Matches CategoryDTO(string? CategoryId, string Name, string HexColor)
    // CategoryId is not included — generated server-side
    const category = { name, hexColor };

    await apiFetch(API, {
        method: "POST",
        body: JSON.stringify(category)
    })
    await render();

    nameInput.value = '';
    nameInput.focus();
}

async function render() {
    const categories = await getAllCategories()
    listEl.querySelectorAll('.category-card').forEach(el => el.remove());

    if (categories.length === 0) {
        emptyState.style.display = '';
        return;
    }

    emptyState.style.display = 'none';

    categories.forEach((cat, i) => {
        const card = document.createElement('div');
        card.id = cat.categoryId
        card.className = 'category-card';
        card.style.animationDelay = `${i * 40}ms`;

        card.innerHTML = `
            <span class="category-color-dot" style="background:${cat.hexColor}"></span>
            <span class="category-card-name">${escapeHtml(cat.name)}</span>
            <button class="category-delete-btn" aria-label="Delete category">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none"
                        stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                    <line x1="18" y1="6" x2="6" y2="18"/>
                    <line x1="6" y1="6" x2="18" y2="18"/>
                </svg>
            </button>
        `;

        card.querySelector('.category-delete-btn').addEventListener('click', async () => await deleteCategory(cat.categoryId));
        listEl.appendChild(card);
    });
}

function escapeHtml(str) {
    const d = document.createElement('div');
    d.textContent = str;
    return d.innerHTML;
}


async function getAllCategories() {
    return await apiFetch(API)
}

async function deleteCategory(id) {
    await apiFetch(API + `/${id}`, {
        method: "DELETE"
    })
    await render();
}

async function apiFetch(url, options = {}) {
    const token = sessionStorage.getItem("token");

    const response = await fetch(url, {
        ...options,
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`,
            ...options.headers
        }
    });

    if (response.status === 401) {
        // Token missing or expired — send to login
        sessionStorage.removeItem("token");
        window.location.href = "/login";
        return;
    }

    if (response.status === 403) {
        // Authenticated but wrong role — show error, stay on page
        alert("You do not have permission to do this");
        return;
    }

    // Returner null hvis der ikke er noget body
    if (response.status === 204) {
        return null
    }

    return response.json();
}


