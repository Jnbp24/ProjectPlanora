document.addEventListener("DOMContentLoaded", loadTasks);

async function loadTasks() {
    const token = sessionStorage.getItem("token");

    const res = await fetch("api/task/extended", {
        headers: {
            "Authorization": `Bearer ${token}`
        }
    });

    if (!res.ok) return;

    const tasks = await res.json();
    const list = document.getElementById("task-list");
    const empty = document.getElementById("task-empty");
    const template = document.getElementById("task-card-template");

    if (tasks.length > 0) empty.style.display = "none";

    tasks.forEach(task => {
        const card = template.content.cloneNode(true).querySelector(".task-card");

        card.style.background = task.category?.hexColor ?? "#6b7280";
        card.querySelector(".task-card-title").textContent = task.title;
        card.querySelector(".task-card-deadline").textContent = task.deadline ? `Deadline: ${new Date(task.deadline).toLocaleDateString()}` : "No deadline";

        list.appendChild(card);
    });
}