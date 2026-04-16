const API = "https://localhost:7127/api/Category"
document.addEventListener('DOMContentLoaded', () => {
    // --- Tab switching ---
    const navItems = document.querySelectorAll('.nav-item[data-tab]');
    const tabs = document.querySelectorAll('.tab-content');

    navItems.forEach(item => {
        item.querySelector('a').addEventListener('click', e => {
            e.preventDefault();
            const target = item.dataset.tab;

            document.querySelectorAll('.nav-item').forEach(n => n.classList.remove('active'));
            item.classList.add('active');

            tabs.forEach(t => t.style.display = 'none');
            const targetTab = document.getElementById('tab-' + target);
            if (targetTab) targetTab.style.display = '';
        });
    });

    // --- Category management ---
    const nameInput = document.getElementById('category-name');
    const colorInput = document.getElementById('category-color');
    const addBtn = document.getElementById('add-category-btn');
    const listEl = document.getElementById('category-list');
    const emptyState = document.getElementById('category-empty');

    colorInput.addEventListener('input', () => {
        hexPreview.textContent = colorInput.value;
    });

    addBtn.addEventListener('click', createCategory);
    nameInput.addEventListener('keydown', e => {
        if (e.key === 'Enter') createCategory();
    });

    function createCategory() {
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

        categories.push(category);
        save();
        render();

        nameInput.value = '';
        nameInput.focus();
    }

    function deleteCategory(index) {
        categories.splice(index, 1);
        save();
        render();
    }

    function save() {
        localStorage.setItem('planora_categories', JSON.stringify(categories));
    }

    async function render() {
        let categories = await get(API, "Failed to load categories")
        listEl.querySelectorAll('.category-card').forEach(el => el.remove());

        if (categories.length === 0) {
            emptyState.style.display = '';
            return;
        }
        emptyState.style.display = 'none';

        categories.forEach((cat, i) => {
            const card = document.createElement('div');
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

            card.querySelector('.category-delete-btn').addEventListener('click', () => deleteCategory(i));
            listEl.appendChild(card);
        });
    }

    function escapeHtml(str) {
        const d = document.createElement('div');
        d.textContent = str;
        return d.innerHTML;
    }

    render();
});


async function get(url) {
    const token = sessionStorage.getItem("token");

    const response = await fetch(url, {
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        }
    });

   if (response.status === 401) {
        sessionStorage.removeItem("token");
        window.location.href = "/login";
        return;
    }

    if (response.status === 403) {
        alert("You do not have permission to do this");
        return;
    }

    return await response.json();
}


