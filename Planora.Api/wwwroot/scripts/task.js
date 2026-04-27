MicroModal.init({
    disableScroll: true,          // lås baggrunds-scroll mens modal er åben
    awaitCloseAnimation: true,    // vent på CSS-animationen før DOM opdateres
})

let create = true
const confirmTitle = document.getElementById('modal-action-title')
const confirmMessage = document.getElementById('modal-action-content')
const confirmBtn = document.getElementById('confirm-action-btn')

confirmBtn.addEventListener('click', async () => {
    if(create) {
        await createTask()
    }
    else {
        await delete_task_handler()
    }
    MicroModal.close('modal-action')
})

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
    const assignedUserIds = assigned_users.getValue()

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
        deadline,
        assignedUserIds
    };

    try {
        const response = await post("/api/Task", task);

        if (!response.ok) throw new Error("Failed to create task");

        const createdTask = await response.json();
        const taskId = createdTask.taskId;


        const selectedCategory = categoryId
            ? categoriesMap[String(categoryId)]
            : null;

        const categoryName =
            selectedCategory?.name 

        const assignResponse = await put(
            `/api/task/${taskId}/category`,
            categoryName
        );

        if (!assignResponse.ok) throw new Error("Failed to assign category");

        console.log("Assigned category:", categoryName);

        for (const userid of assignedUserIds) {
            const response = await post(`/api/Task/${taskId}/user`, userid);
            if (!response.ok) throw new Error(`Failed to assign task to user`);
        }

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

newTaskBtn.addEventListener("click", async () => {
    create = true
    confirmTitle.textContent = `Create task`

    const title = titleInput.value.trim()
    const content = contentInput.value.trim()
    const categorySelect = categoryInput.options[categoryInput.selectedIndex]
    const categoryName = categorySelect?.text ?? ''
    const categoryColor = categorySelect?.dataset.color ?? null
    const hasDeadline = !noDeadlineCheckbox.checked
    const deadline = dateInput.value

    confirmMessage.replaceChildren()

    // Titel
    confirmMessage.append(`This will create task "${title}" `)

    // Content (kun hvis udfyldt)
    if (content) {
        confirmMessage.append(`with content "${content}" `)
    }

    // Kategori med farveprik
    if (categoryName) {
        confirmMessage.append(`in category `)
        if (categoryColor) {
            const swatch = document.createElement('span')
            swatch.className = 'color-swatch'
            swatch.style.background = categoryColor
            confirmMessage.append(swatch)
        }
        confirmMessage.append(` "${categoryName}" `)
    }

    // Deadline
    if (hasDeadline && deadline) {
        const formatted = new Date(deadline).toLocaleDateString('da-DK', {
            day: 'numeric',
            month: 'long',
            year: 'numeric'
        })
        confirmMessage.append(`with deadline ${formatted}.`)
    } else {
        confirmMessage.append(`with no deadline.`)
    }
    confirmBtn.textContent = `Create`
    MicroModal.show('modal-action')
});

loadCategories();


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