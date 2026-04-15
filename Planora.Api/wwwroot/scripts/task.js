document.addEventListener("DOMContentLoaded", () => {

    const newTaskBtn = document.querySelector(".new-task-btn");
    const titleInput = document.querySelector(".task-name");
    const contentInput = document.querySelector(".task-content");
    const categoryInput = document.querySelector(".task-category");

    let categoriesMap = {};


    async function loadCategories() {
        try {
            const response = await fetch("/api/Category");

            if (!response.ok) {
                throw new Error("Failed to load categories");
            }

            const categories = await response.json();

            console.log("Loaded categories:", categories);

            categoryInput.innerHTML =
                `<option value="" disabled selected hidden>Category</option>`;

            categories.forEach(c => {

                const id = c.categoryId ?? c.CategoryId;
                const name = c.name ?? c.Name;
                const color = c.hexColor ?? c.HexColor;

                const option = document.createElement("option");

                option.value = id;
                option.textContent = name;

                categoryInput.appendChild(option);

                categoriesMap[id] = {
                    categoryId: id,
                    name,
                    hexColor: color
                };
            });

        } catch (err) {
            console.error("Error loading categories:", err);

            categoryInput.innerHTML =
                `<option value="" disabled selected hidden>No categories</option>`;
        }
    }

    categoryInput.addEventListener("change", () => {
        const selected = categoriesMap[categoryInput.value];

        if (selected?.hexColor) {
            categoryInput.style.fontWeight = "600";
            categoryInput.style.background = selected.hexColor;
            categoryInput.style.color = "#fff";
        }
    });

    async function createTask() {

        const title = titleInput.value.trim();
        const content = contentInput.value.trim();
        const categoryId = categoryInput.value || null;

        if (!title) {
            titleInput.focus();
            return;
        }

        const task = {
            taskId: null,
            title,
            content,
            categoryId: null
        };

        try {
            const response = await fetch("/api/Task", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(task)
            });

            if (!response.ok) throw new Error("Failed to create task");

            const createdTask = await response.json();
            const taskId = createdTask.taskId;

            const DEFAULT_CATEGORY_NAME = "Default";

            const selectedCategory = categoryId
                ? categoriesMap[categoryId]
                : null;

            const categoryName =
                selectedCategory?.name ?? DEFAULT_CATEGORY_NAME;

            await fetch(`/api/task/${taskId}/assign/${categoryName}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                }
            });

            console.log("Assigned category:", categoryName);

            titleInput.value = "";
            contentInput.value = "";
            categoryInput.selectedIndex = 0;

            categoryInput.style.color = "";
            categoryInput.style.background = "";

            updateButtonText();

        } catch (err) {
            console.error("Error:", err);
        }
    }
    newTaskBtn.addEventListener("click", createTask);

    loadCategories();
});