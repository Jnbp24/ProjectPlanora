document.addEventListener("DOMContentLoaded", () => {

    if (!is_tovholder()) {
        document.getElementById("delete_task_btn").classList.remove("restricted")
    }

    const newTaskBtn = document.querySelector(".new-task-btn");
    const titleInput = document.querySelector(".task-name");
    const contentInput = document.querySelector(".task-content");
    const categoryInput = document.querySelector(".task-category");
    const dateInput = document.querySelector(".task-date");
    const noDeadlineCheckbox = document.querySelector("#no-deadline");

    let categoriesMap = {};

    async function loadCategories() {
        try {
            const categories = await get("/api/Category", "Failed to load categories");

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

    noDeadlineCheckbox.addEventListener("change", () => {

        if (noDeadlineCheckbox.checked) {
            dateInput.value = "";
            dateInput.style.setProperty("display", "none");
        } else {
            dateInput.style.setProperty("display", "inline-block");
        }
    });

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

        const deadline = noDeadlineCheckbox.checked
            ? null
            : (dateInput.value || null);

        if (!title) {
            titleInput.focus();
            return;
        }

        const task = {
            taskId: null,
            title,
            content,
            categoryId,
            deadline
        };

        try {
            const response = await post("/api/Task", task);

            if (!response.ok) throw new Error("Failed to create task");

            const createdTask = await response.json();
            const taskId = createdTask.taskId;

            const DEFAULT_CATEGORY_NAME = "Default";

            const selectedCategory = categoryId
                ? categoriesMap[String(categoryId)]
                : null;

            const categoryName =
                selectedCategory?.name ?? DEFAULT_CATEGORY_NAME;

            const assignResponse = await put(
                `/api/task/${taskId}/category`,
                categoryName
            );

            if (!assignResponse.ok) throw new Error("Failed to assign category");

            console.log("Assigned category:", categoryName);

            titleInput.value = "";
            contentInput.value = "";
            categoryInput.selectedIndex = 0;
            dateInput.value = "";
            dateInput.style.display = "";

            noDeadlineCheckbox.checked = false;

            categoryInput.style.color = "";
            categoryInput.style.background = "";
            categoryInput.style.fontWeight = "";

        } catch (err) {
            console.error("Error:", err);
        }
    }

    newTaskBtn.addEventListener("click", createTask);

    loadCategories();
});

async function get(url, error_message) {
    const token = sessionStorage.getItem("token");

    const response = await fetch(url, {
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(error_message);
    }

    return await response.json();
}

async function post(url, data) {
    const token = sessionStorage.getItem("token");

    const response = await fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(data)
    });

    return response;
}

async function put(url, data = null) {
    const token = sessionStorage.getItem("token");

    const response = await fetch(url, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: data ? JSON.stringify(data) : null
    });

    return response;
}